using UnityEngine;

namespace Titan.Test
{
	public class MoveTitanKinematic : MonoBehaviour
	{
		[SerializeField] float speed = 5f;
		[SerializeField] KeyCode left = KeyCode.Q;
		[SerializeField] KeyCode right = KeyCode.E;
		float input;

		void Update()
		{
			if (Input.GetKey(right))
				input = 1f;
			else if (Input.GetKey(left))
				input = -1f;
			else input = 0;

			//Move the titan kinematically
			transform.Translate(Vector2.right * input * speed * Time.deltaTime);
		}
	}
}