using CrawfisSoftware.EventManagement;
using System;
using UnityEngine;

namespace CrawfisSoftware.Jumper
{
    internal class RigidBodyMover : MonoBehaviour
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private EventsPublisher _eventsPublisher;

        private bool jumpIsReady = true;
        private Rigidbody rBody;
        private Vector3 startingPosition;
        public event Action OnReset;

        public bool CanJump {  get {  return jumpIsReady; } }

        public void Awake()
        {
            rBody = GetComponent<Rigidbody>();
            startingPosition = transform.position;
            _eventsPublisher.SubscribeToEvent("JumpRequested", OnJumpRequest);
        }


        private void OnJumpRequest(object sender, object data)
        {
            if (jumpIsReady)
            {
                rBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
                jumpIsReady = false;
                _eventsPublisher.PublishEvent("JumpPerformed", this, jumpForce);
            }
        }

        private void Reset()
        {
            jumpIsReady = true;

            //Reset Movement and Position
            transform.position = startingPosition;
            rBody.velocity = Vector3.zero;

            _eventsPublisher.PublishEvent("JumperReset", this, null);
        }

        private void OnCollisionEnter(Collision collidedObj)
        {
            if (collidedObj.gameObject.CompareTag("Street"))
            {
                jumpIsReady = true;
                rBody.velocity = Vector3.zero;
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