using CrawfisSoftware.EventManagement;
using Unity.MLAgents;
using UnityEngine;

namespace Assets.Scripts
{
    internal class HighScoreMLRecorder : MonoBehaviour
    {
        private StatsRecorder statsRecorder;
        [SerializeField] private EventsPublisher _eventsPublisher;

        private void Awake()
        {
            statsRecorder = Academy.Instance.StatsRecorder;

            _eventsPublisher.SubscribeToEvent("HighScore", OnNewHighScore);
        }

        private void OnNewHighScore(object sender, object data)
        {
            int highScore = (int)data;
            statsRecorder.Add("High Score", highScore, StatAggregationMethod.MostRecent);
        }
    }
}