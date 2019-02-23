using UnityEngine;

namespace Titan
{
    public class CameraFollow : MonoBehaviour
    {
        Camera cam;

        // [SerializeField] int minPanZone = 200; //The size of the zone where if the 
        [SerializeField] Transform subject;

        void Start()
        {
            cam = Camera.main;
        }

        void Update()
        {
            cam.transform.position = subject.transform.position;

            //Move camera with the subject
            //Horizontal
            // int subjectAndZoneIntersection  = (int)cam.transform.position.x - cam.pixelWidth / 2;
            // if (subjectAndZoneIntersection < minPanZone)
            // {
            // 	cam.transform.Translate()
            // }
        }
    }
}