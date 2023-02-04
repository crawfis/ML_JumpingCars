using CrawfisSoftware.EventManagement;
using UnityEngine;

namespace CrawfisSoftware.Jumper
{
    public class JumperController : MonoBehaviour
    {
        [SerializeField] private KeyCode jumpKey;
        [SerializeField] private EventsPublisher _eventsPublisher;

        private void Update()
        {
            if (Input.GetKey(jumpKey))
                _eventsPublisher.PublishEvent("JumpRequested", this, null);
        }
    }
}