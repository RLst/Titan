using UnityEngine;

namespace Titan
{
	public class StationRoom : Room
	{
		public Collider2D terminal;

		void Start()
		{
			//Get the terminal trigger collider
			terminal = GetComponent<Collider2D>();
		}
	}

}