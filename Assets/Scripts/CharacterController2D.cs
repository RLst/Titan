using UnityEngine;
using UnityEngine.Events;

namespace Titan
{

	public class CharacterController2D : MonoBehaviour
	{
		//Core properties
		[SerializeField] bool hasAirControl = true;							// Whether or not a player can steer while jumping;
		[Range(0, .3f)] [SerializeField] float movementSmoothing = .05f;	// How much to smooth out the movement


		[Header("Physics")]
		[SerializeField] private float m_JumpForce = 400f;					// Amount of force added when the player jumps.
		[Tooltip("Used to automatically set rb gravity scale during ladder climb etc")]
		[SerializeField] float gravityScale = 2f;
		Rigidbody2D rb;
		Collider2D[] cols;
		Vector2 jumpVector;
		bool isFacingRight = true;											// For determining which way the player is currently facing.
		private Vector3 m_Velocity = Vector3.zero;


		[Header("Ground & Ceiling")]
		[SerializeField] private Transform m_GroundCheck;					// A position marking where to check if the player is grounded.
		[SerializeField] private LayerMask m_WhatIsGround;					// A mask determining what is ground to the character
		const float k_groundCheckRadius = 0.2f;								//[needs to be pretty small]. Radius of the overlap circle to determine if grounded
		bool isGrounded;                                                    // Whether or not the player is grounded.


		[Header("Ladder")]
		[SerializeField] Vector2 ladderBoxCastExtents = new Vector2(0.25f, 1f);
		[SerializeField] float ladderUpCast = 0.5f;
		[SerializeField] float ladderDownCast = 0.5f;
		[SerializeField] LayerMask whatIsLadder;
		bool isClimbing = false;
		Transform currentLadder;											//The ladder the player is currently climbing on


		[Header("Mountables")]
		[Tooltip("Including terminals/stations, cockpits, manned turrets")]
		[SerializeField] LayerMask whatAreMountables;


		[Header("Events")]
		[Space]
		public UnityEvent OnLandEvent;

		// [System.Serializable]
		// public class BoolEvent : UnityEvent<bool> { }
		// public BoolEvent OnCrouchEvent;
		// private bool m_wasCrouching = false;

		/* Controls:
		Left stick = Movement/Aim
		Right stick = null
		A = Jump, also dismounts terminal/turret
		B = Cancel, dismounts terminals/turret, building screens etc.
		X = Action/Use/Fire,
		Y = Special/build/secondary fire: 
			- Hold + LS direction for building options?
			- Press to build preset, when grounded
			- If on turret, shoot secondary shot OR change firing mode?
		*/

		private void Awake()
		{
			jumpVector = new Vector2(0, m_JumpForce);
			rb = GetComponent<Rigidbody2D>();
			cols = GetComponents<Collider2D>();

			rb.gravityScale = gravityScale;

			if (OnLandEvent == null)
				OnLandEvent = new UnityEvent();
		}


		private void FixedUpdate()
		{
			bool wasGrounded = isGrounded;
			isGrounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_groundCheckRadius, m_WhatIsGround);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					isGrounded = true;
					if (!wasGrounded)
						OnLandEvent.Invoke();
				}
			}
			// Debugs();
		}

		public void Debugs()
		{
			Debug.Log("Grounded: " + isGrounded);
		}

        public void Move(Vector2 direction, bool onJump, bool onAction, bool onSpecial, bool onCancel)
		{
			//Handles:
			//> Grounded movement ie. run
			//> Jumping
			//> Climbing ladders

			//MOVE character only if grounded OR is air controllable
			if (isGrounded || hasAirControl)
			{
				Vector3 targetVelocity = new Vector2(direction.x, rb.velocity.y);
				rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothing);

				//FACING
				if (direction.x > 0 && !isFacingRight)
				{
					Flip();
				}
				else if (direction.x < 0 && isFacingRight)
				{
					Flip();
				}
			}

			//JUMPING
			if (isGrounded && onJump)
			{
				// Add a vertical force to the player.
				rb.AddForce(jumpVector);

				//Player no longer grounded
				isGrounded = false;
			}

			//LADDER
			//Climbing up
			var upCastInfo = Physics2D.BoxCast(transform.position, ladderBoxCastExtents, 0, Vector2.up, 0, whatIsLadder);
			// var upCastInfo = Physics2D.Raycast(transform.position, Vector2.up, ladderUpCast, whatIsLadder);

			//If the player is pressing up
			if (direction.y > 0) {
				//If the player is within range of a ladder
				if (upCastInfo.collider != null)
				{
					//If player is not on a ladder
					if (isClimbing && currentLadder != null)
					{
						//Set player to be in a ladder climbing state
						onClimbLadder(upCastInfo.collider.transform);
					}
				}
			}
			
			

            if (upCastInfo.collider != null || downCastInfo.collider != null)    //If not null then cast has DEFINITELY hit a ladder
            {
				//...and player is also pressing up or down
				if (direction.y != 0)
				{
					onClimbLadder();
					if (upCastInfo.collider != null)
						currentLadder = upCastInfo.collider.transform;
					else
						currentLadder = downCastInfo.collider.transform;
				}
			}
			else
			{
				offClimbLadder();
			}

			//Climb down
			var downCastInfo = Physics2D.BoxCast(m_GroundCheck.position, ladderBoxCastExtents, 0, Vector2.down, 0, whatIsLadder);
			// var downCastInfo = Physics2D.Raycast(m_GroundCheck.position, Vector2.down, ladderDownCast, whatIsLadder);

			if (isClimbing == true)
			{
				//Character climbs or decends based on vertical input
				//Smoothly snap character to ladder ie. lerp character towards x position of ladder
				// rb.velocity = Vector3.SmoothDamp(transform.position, currentLadder.position, ref m_Velocity, movementSmoothing);
				// Vector3 snapToLadder = new Vector2(Vector2.MoveTowards())
				// rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothing);

				rb.velocity = new Vector2(rb.velocity.x, direction.y);
			}
			else {
				currentLadder = null;
			}
		}

		void onClimbLadder(Transform ladder)
		{
			isClimbing = true;
			currentLadder = ladder;

			//Zero gravity scale ie. the player is now holding onto the ladder
			rb.gravityScale = 0;
			//Disable character collider so that he can go up through the rooms
			foreach (var c in cols)
			{
				c.enabled = false;
			}
		}

		void offClimbLadder()
		{
			isClimbing = false;

			//Reset gravity scale
			rb.gravityScale = gravityScale;
			//Re-enable character colliders
			foreach (var c in cols)
			{
				c.enabled = true;
			}
		}

		//Auxillary functions
		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			isFacingRight = !isFacingRight;

			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

}