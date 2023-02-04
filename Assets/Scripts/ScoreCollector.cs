using CrawfisSoftware.EventManagement;
using TMPro;
using UnityEngine;

namespace CrawfisSoftware.Jumper
{
    public class ScoreCollector : MonoBehaviour
    {
        [SerializeField] private EventsPublisher _eventsPublisher;
        //public static ScoreCollector Instance;

        [SerializeField] private TextMeshProUGUI display;
        [SerializeField] private int _scoreIncrement = 1;

        private int _score = 0;
        private int highScore = 0;
        void Awake()
        {
            //Instance = this;
            _eventsPublisher.SubscribeToEvent("ObstacleAvoided", OnIncreaseScore);
            _eventsPublisher.SubscribeToEvent("JumperReset", OnJumperReset);
        }

        private void OnIncreaseScore(object sender, object data)
        {
            AddScore(_scoreIncrement);
        }

        private void OnJumperReset(object sender, object data)
        {
            _score = 0;
        }

        public void AddScore(int increment)
        {
            _score += increment;
            if (_score > highScore)
            {
                highScore = _score;
                display.text = _score.ToString();
                _eventsPublisher.PublishEvent("HighScore", this, highScore);
            }
        }
    }
}