using CrawfisSoftware.EventManagement;
using UnityEngine;

namespace CrawfisSoftware.Jumper
{
    [RequireComponent(typeof(Rigidbody))]
    internal class RigidBodyMover : MonoBehaviour
    {
        [SerializeField] private float _jumpForce;
        [SerializeField] private EventsPublisher _eventsPublisher;

        private bool _jumpIsReady = true;
        private bool _jumpNow = false;
        private Rigidbody _rBody;
        private Vector3 _startingPosition;

        public bool CanJump {  get {  return _jumpIsReady; } }

        public void Awake()
        {
            _rBody = GetComponent<Rigidbody>();
            _startingPosition = transform.position;
            _eventsPublisher.SubscribeToEvent("JumpRequested", OnJumpRequest);
            _jumpVelocity = new Vector3(0, _jumpForce, 0);
        }

        private Vector3 _jumpVelocity;
        private void OnJumpRequest(object sender, object data)
        {
            if (_jumpIsReady)
            {
                //rBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
                _jumpNow = true;
                _jumpIsReady = false;
                _eventsPublisher.PublishEvent("JumpPerformed", this, _jumpForce);
            }
        }

        private void FixedUpdate()
        {
            if (_jumpNow)
            {
                //rBody.velocity = _jumpVelocity;
                _rBody.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
                _jumpNow = false;
            }
        }

        private void Reset()
        {
            _jumpIsReady = true;

            //Reset Movement and Position
            transform.position = _startingPosition;
            _rBody.velocity = Vector3.zero;

            _eventsPublisher.PublishEvent("JumperReset", this, null);
        }

        private void OnCollisionEnter(Collision collidedObj)
        {
            if (collidedObj.gameObject.CompareTag("Street"))
            {
                _jumpIsReady = true;
                _rBody.velocity = Vector3.zero;
            }

            else if (collidedObj.gameObject.CompareTag("Mover") || collidedObj.gameObject.CompareTag("DoubleMover"))
            {
                _eventsPublisher.PublishEvent("Collision", this, collidedObj.gameObject);
                Reset();
            }
        }

        private void OnTriggerEnter(Collider collidedObj)
        {
            if (collidedObj.gameObject.CompareTag("score"))
            {
                _eventsPublisher.PublishEvent("ObstacleAvoided", this, collidedObj);
            }
        }
    }
}