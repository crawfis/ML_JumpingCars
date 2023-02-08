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
        [SerializeField] private float EpisodeMaxReward = 50;

        private float _cummulativeReward = 0;

        public override void Initialize()
        {
            //EventsPublisher.Instance.SubscribeToEvent("JumpPerformed", OnJumped);
            _eventsPublisher.SubscribeToEvent("ObstacleAvoided", OnSuccessfulJump);
            _eventsPublisher.SubscribeToEvent("Collision", OnCollidedWithObstacle);
            _eventsPublisher.SubscribeToEvent("JumpPerformed", OnJumped);
            _cummulativeReward = 0;
        }

        private void OnJumped(object sender, object data)
        {
            AddReward(-0.0001f);
        }

        private void OnSuccessfulJump(object sender, object data)
        {
            AddReward(0.1f);
            _cummulativeReward += 1;
            //Debug.Log(GetCumulativeReward());
            if (_cummulativeReward > EpisodeMaxReward)
            {
                AddReward(_cummulativeReward);
                _cummulativeReward = 0;
                _eventsPublisher.PublishEvent("ScoreReset", this, null);
                EndEpisode();
            }
        }

        private void OnCollidedWithObstacle(object sender, object data)
        {
            AddReward(-100.0f);
            //Debug.Log(GetCumulativeReward());
            GameObject mover = data as GameObject;
            if (mover != null && mover.TryGetComponent<PooledGameObject>(out PooledGameObject releaseScript))
                releaseScript.Release();
            EndEpisode();
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            int discreteActionJump = actions.DiscreteActions[0];
            if (discreteActionJump == 1)
                _eventsPublisher.PublishEvent("JumpRequested", this, null);
        }
        public override void OnEpisodeBegin() {
            SetReward(0);
        }
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            actionsOut.DiscreteActions.Array[0] = 0;

            if (_rigidBodyMover.CanJump)
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