using UnityEngine;

namespace Titan
{
	public class Turret : MonoBehaviour
	{
		//This class might not need to be subclassed and can handle all turrets

		public float health;

		[SerializeField] Transform ordnance;		//The actual gun component of this turret ie. HMG, grenade launcher, sniper, etc
		
	}

}