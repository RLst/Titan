using UnityEngine;
using UnityEngine.Events;

namespace Titan
{

	public class CharacterController2D : MonoBehaviour
	{
		//Core properties
		[SerializeField] private bool m_AirControl = true;                          // Whether or not a player can steer while jumping;
		[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement


		[Header("Physics")]
		[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
		[Tooltip("Used to automatically set rb gravity scale during ladder climb etc")]
		[SerializeField] float gravityScale = 2f;
		private Rigidbody2D rb;
		Collider2D[] cols;
		private Vector2 m_JumpVector;
		private bool m_FacingRight = true;                                          // For determining which way the player is currently facing.
		private Vector3 m_Velocity = Vector3.zero;


		[Header("Ground & Ceiling")]
		[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
		[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
		const float k_GroundedRadius = 0.2f;                                        //[needs to be pretty small]. Radius of the overlap circle to determine if grounded
		private bool m_Grounded;                                                    // Whether or not the player is grounded.


		[Header("Ladder")]
		[SerializeField] float ladderDetectDist = 5f;
		[SerializeField] LayerMask whatIsLadder;
		bool isClimbing = false;


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
			m_JumpVector = new Vector2(0, m_JumpForce);
			rb = GetComponent<Rigidbody2D>();
			cols = GetComponents<Collider2D>();

			rb.gravityScale = gravityScale;

			if (OnLandEvent == null)
				OnLandEvent = new UnityEvent();
		}


		private void FixedUpdate()
		{
			bool wasGrounded = m_Grounded;
			m_Grounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					m_Grounded = true;
					if (!wasGrounded)
						OnLandEvent.Invoke();
				}
			}

			// Debugs();
		}

		public void Debugs()
		{
			Debug.Log("Grounded: " + m_Grounded);
		}

		public void Action(Vector2 direction)
		{
			//Player firing conditions:
			/* ABLE:
			* isGrounded
			* isJumping
			
			UNABLE:
			* isManningTerminal
			* isOnLadder
			*/


			// IS Grounded, Jumping
			// ISNOT onATerminal,

			//If player is near a mountable then mount it


			//If player is n


		}

		public void Special(bool action)
		{
			//Handles:
			//> 
		}

		public void Move(Vector2 direction, bool onJump, bool onAction, bool onSpecial, bool onCancel)
		{
			//Handles:
			//> Grounded movement ie. run
			//> Jumping
			//> Climbing ladders

			//MOVE character only if grounded OR is air controllable
			if (m_Grounded || m_AirControl)
			{
				if (!isClimbing) {

					Vector3 targetVelocity = new Vector2(direction.x * 10f, rb.velocity.y);
					rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

					//FACING
					if (direction.x > 0 && !m_FacingRight)
					{
						Flip();
					}
					else if (direction.x < 0 && m_FacingRight)
					{
						Flip();
					}
				}
			}

			//JUMPING
			if (m_Grounded && onJump)
			{
				// Add a vertical force to the player.
				m_Grounded = false;
				rb.AddForce(m_JumpVector);
			}

			//LADDER
			var castHitInfo = Physics2D.BoxCast(transform.position, new Vector2(0.25f, 0.50f), 0, Vector2.zero, 0, whatIsLadder);
			// RaycastHit2D rayHitInfo = Physics2D.Raycast(transform.position, Vector2.up, ladderDetectDist, whatIsLadder);
			if (castHitInfo.collider != null)    //If the ray has hit a ladder
			{
				//If player is pressing up
				if (direction.y > 0)
				{
					onClimbLadder();
				}
			}
			else
			{
				offClimbLadder();
			}

			if (isClimbing == true)
			{
				//Move the character
				// m_Rigidbody2D.velocity = (new Vector2(0, ))
				rb.velocity = new Vector2(rb.velocity.x, direction.y);

			}
		}

		void onClimbLadder()
		{
			isClimbing = true;

			//Zero gravity scale ie. the player is now holding onto the ladder
			rb.gravityScale = 0;
			//Disable character collider so that he can go through the rooms
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
			m_FacingRight = !m_FacingRight;

			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

}