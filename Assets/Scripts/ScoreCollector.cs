using CrawfisSoftware.EventManagement;
using System;
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
        private int _highScore = 0;
        void Awake()
        {
            //Instance = this;
            _eventsPublisher.SubscribeToEvent("ObstacleAvoided", OnIncreaseScore);
            _eventsPublisher.SubscribeToEvent("JumperReset", OnJumperReset);
            _eventsPublisher.SubscribeToEvent("ScoreReset", OnHighScoreReset);
        }

        private void OnHighScoreReset(object sender, object data)
        {
            _score = 0;
            _highScore = 0;
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
            if (_score > _highScore)
            {
                _highScore = _score;
                display.text = _score.ToString();
                _eventsPublisher.PublishEvent("HighScore", this, _highScore);
            }
        }
    }
}