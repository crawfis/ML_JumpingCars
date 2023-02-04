using CrawfisSoftware.EventManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrawfisSoftware.Jumper
{
    //
    //  Spawns the Mover Objects (Enemies) with an interval you determine.
    // 
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> spawnableObjects;

        [Tooltip("The Spawner waits a random number of seconds between these two interval each time a object was spawned.")]
        [SerializeField] private float minSpawnIntervalInSeconds;
        [SerializeField] private float maxSpawnIntervalInSeconds;
        [SerializeField] private EventsPublisher _eventsPublisher;

        private List<GameObject> spawnedObjects = new List<GameObject>();

        private void Awake()
        {
            //Subscribes to Reset of Player
            _eventsPublisher.SubscribeToEvent("JumperReset", DestroyAllSpawnedObjects);
            StartCoroutine(nameof(Spawn));
        }

        private IEnumerator Spawn()
        {
            var spawned = Instantiate(GetRandomSpawnableFromList(), transform.position, transform.rotation, transform);
            spawnedObjects.Add(spawned);

            yield return new WaitForSeconds(Random.Range(minSpawnIntervalInSeconds, maxSpawnIntervalInSeconds));
            StartCoroutine(nameof(Spawn));
        }
        private void DestroyAllSpawnedObjects(object sender, object data)
        {
            for (int i = spawnedObjects.Count - 1; i >= 0; i--)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
        private GameObject GetRandomSpawnableFromList()
        {
            int randomIndex = Random.Range(0, spawnableObjects.Count);
            return spawnableObjects[randomIndex];
        }
    }
}