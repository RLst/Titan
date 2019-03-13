using UnityEngine;

namespace Titan
{
	[RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
		[SerializeField] Vector2 trajectory;
		[SerializeField] float speed;
		[SerializeField] float mass;

		new Rigidbody2D rigidbody;

		void Awake()
		{
			rigidbody = GetComponent<Rigidbody2D>();
		}

		void Start()
		{
			rigidbody.velocity = trajectory * speed;
			Destroy(this, 5f);	//Crap
		}

    }

}