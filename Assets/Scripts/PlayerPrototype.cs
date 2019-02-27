using UnityEngine;

namespace Titan.Test
{
    public class PlayerPrototype : MonoBehaviour
    {
        //Monolithic player prototype class
        CharacterController2D controller;

        [SerializeField] float hSpeedMultiplier = 20f;
        public float vSpeedMultiplier = 17.5f;

        float hInput = 0f;
        float vInput = 0f;

        bool onCrouch = false;
        bool onJump = false;
        
        //PROTOTYPE
        bool onLadder = false;
        bool manningTerminal = false;
        bool atTerminal = false;
        Collider2D[] playerColliders;

        public KeyCode actionKey = KeyCode.J;
        public KeyCode jumpKey = KeyCode.K;


        [Tooltip("Temp")]
        public LayerMask whatIsTerminal;


        void Awake()
        {
            controller = GetComponent<CharacterController2D>();

            playerColliders = GetComponents<Collider2D>();      //Get all colliders for use with finding terminals and ladders etc
        }

        void Update()
        {
            hInput = Input.GetAxis("Horizontal") * hSpeedMultiplier;
            vInput = Input.GetAxis("Vertical") * vSpeedMultiplier;

            // if (Input.GetButtonDown("Jump"))
			if (Input.GetKeyDown(KeyCode.Space))
                onJump = true;

            if (Input.GetKeyDown(KeyCode.DownArrow))
                onCrouch = true;
            else if (Input.GetKeyUp(KeyCode.DownArrow))
                onCrouch = false;


            ////GARBAGE QUICK PROTOTYPE CODE
            //Terminal
            //If the player is touching a terminal...
            Vector2 playerBoxCastSize = new Vector2(5, 5);
            if (Physics2D.BoxCast(transform.position, playerBoxCastSize, 0f, Vector2.up, 0f, whatIsTerminal))
            {
                //...and player has pressed action
                // if (Input.get)
            }

            //Ladder

        }

        void FixedUpdate()
        {
            //Move using physics
            controller.Move(hInput * Time.fixedDeltaTime, onCrouch, onJump);
            onJump = false;
        }
    }
}