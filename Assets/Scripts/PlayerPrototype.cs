using UnityEngine;

namespace Titan.Test
{
    public class PlayerPrototype : MonoBehaviour
    {
        //Monolithic player prototype class
        CharacterController2D controller;

        [SerializeField] float hSpeedMultiplier = 20f;
        [SerializeField] float vSpeedMultiplier = 17.5f;

        float hInput, vInput;

		bool onAction = false;
        bool onJump = false;
		bool onSpecial = false;
		bool onCancel = false;

		//TODO separate these out into its own input class later
		public KeyCode actionKey = KeyCode.J;
        public KeyCode jumpKey = KeyCode.K;
		public KeyCode specialKey = KeyCode.I;
		public KeyCode cancelKey = KeyCode.L;
        

        void Awake()
        {
            controller = GetComponent<CharacterController2D>();

            // playerColliders = GetComponents<Collider2D>();      //Get all colliders for use with finding terminals and ladders etc
        }

        void Update()
        {
            hInput = Input.GetAxis("Horizontal") * hSpeedMultiplier;
            vInput = Input.GetAxis("Vertical") * vSpeedMultiplier;

            // if (Input.GetButtonDown("Jump"))
			if (Input.GetKeyDown(jumpKey))
                onJump = true;

			if (Input.GetKey(actionKey))
				onAction = true;
			else onAction = false;

			if (Input.GetKey(specialKey))
				onSpecial = true;
			else onSpecial = false;
			
			if (Input.GetKey(cancelKey))
				onCancel =  true;
			else onCancel = false;


            // if (Input.GetKeyDown(KeyCode.DownArrow))
            //     onCrouch = true;
            // else if (Input.GetKeyUp(KeyCode.DownArrow))
            //     onCrouch = false;

            // ////GARBAGE QUICK PROTOTYPE CODE
            // //Terminal
            // //If the player is touching a terminal...
            // Vector2 playerBoxCastSize = new Vector2(5, 5);
            // if (Physics2D.BoxCast(transform.position, playerBoxCastSize, 0f, Vector2.up, 0f, whatIsTerminal))
            // {
            //     //...and player has pressed action
            //     // if (Input.get)
            // }
        }

        void FixedUpdate()
        {
            //Move using physics
            controller.Move(new Vector2(hInput * hSpeedMultiplier, vInput * vSpeedMultiplier) * Time.fixedDeltaTime, onJump, onAction, onSpecial, onCancel);
            onJump = false;
            // controller.Move(hInput * Time.fixedDeltaTime, onCrouch, onJump);

			// if ()
			// controller.Action()

        }
    }
}