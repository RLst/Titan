using System.Collections.Generic;
using UnityEngine;

namespace Titan
{
	public class Room : MonoBehaviour
	{
		public Transform titan;
		public Transform player;
		
		[SerializeField] float health = 100f;
		[SerializeField] Collider2D floor;
		

		void Awake()
		{
			titan = FindObjectOfType<Test.Titan>().transform;
			player = FindObjectOfType<Test.Player>().transform;
		}

		//Ground player to the titan so that it doesn't slide off
		void OnCollisionEnter2D(Collision2D other)
		{
			if (other.transform == player)
			{
				other.collider.transform.SetParent(titan.transform);
			}
		}
		void OnCollisionExit2D(Collision2D other)
		{
			if (other.transform == player)
			{
				other.collider.transform.SetParent(null);
			}
		}
	}
}