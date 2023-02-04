using CrawfisSoftware.EventManagement;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace CrawfisSoftware.Jumper
{
    public class JumperML : Agent
    {
        [SerializeField] private RigidBodyMover _rigidBodyMover;
        [SerializeField] private KeyCode jumpKey;
        [SerializeField] private EventsPublisher _eventsPublisher;

        public override void Initialize()
        {
            //EventsPublisher.Instance.SubscribeToEvent("JumpPerformed", OnJumped);
            _eventsPublisher.SubscribeToEvent("ObstacleAvoided", OnSuccessfulJump);
            _eventsPublisher.SubscribeToEvent("Collision", OnCollidedWithObstacle);
        }

        private void OnSuccessfulJump(object sender, object data)
        {
            AddReward(0.1f);
        }

        private void OnCollidedWithObstacle(object sender, object data)
        {
            AddReward(-1.0f);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            int discreteActionJump = actions.DiscreteActions[0];
            if (discreteActionJump == 1)
                _eventsPublisher.PublishEvent("JumpRequested", this, null);
        }
        //public override void OnEpisodeBegin() { }
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            actionsOut.DiscreteActions.Array[0] = 0;

            //if (_rigidBodyMover.CanJump)
            if (Input.GetKey(jumpKey))
                actionsOut.DiscreteActions.Array[0] = 1;
        }

        private void FixedUpdate()
        {
            if (_rigidBodyMover.CanJump)
                RequestDecision();
        }
    }
}