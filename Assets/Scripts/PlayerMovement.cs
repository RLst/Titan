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

            if (Input.GetButtonDown("Jump"))
                onJump = true;

            if (Input.GetKeyDown(KeyCode.LeftControl))
                onCrouch = true;
            else if (Input.GetKeyUp(KeyCode.LeftControl))
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