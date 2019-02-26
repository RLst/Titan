using UnityEngine;

namespace Titan.Test
{
    public class PlayerMovement : MonoBehaviour
    {
        CharacterController2D controller;
        [SerializeField] float runSpeed = 40f;
        float horizontalInput = 0f;
        bool onCrouch = false;
        bool onJump = false;

        void Awake()
        {
            controller = GetComponent<CharacterController2D>();
        }

        void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal") * runSpeed;

            // if (Input.GetButtonDown("Jump"))
			if (Input.GetKeyDown(KeyCode.Space))
                onJump = true;

            if (Input.GetKeyDown(KeyCode.DownArrow))
                onCrouch = true;
            else if (Input.GetKeyUp(KeyCode.DownArrow))
                onCrouch = false;

        }

        void FixedUpdate()
        {
            //Move your butthole
            controller.Move(horizontalInput * Time.fixedDeltaTime, onCrouch, onJump);
            onJump = false;
        }
    }
}