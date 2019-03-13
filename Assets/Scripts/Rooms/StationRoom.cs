using UnityEngine;

namespace Titan
{
	public class StationRoom : Room
	{
		[SerializeField] Collider2D stationSeat;

		void Start()
		{
			//Make sure seat is a trigger
			stationSeat.isTrigger = true;
		}
	}

}