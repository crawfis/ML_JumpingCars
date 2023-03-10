using UnityEngine;

namespace CrawfisSoftware.Jumper
{

    /*
     * Simple Enemy script. It just moves forward with a speed you determine
     */
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Rigidbody Rigidbody;


        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Rigidbody.velocity = Vector3.back * speed;
        }
    }
}