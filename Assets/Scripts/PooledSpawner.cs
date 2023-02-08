using CrawfisSoftware.EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace CrawfisSoftware.Jumper
{
    //
    //  Spawns the Mover Objects (Enemies) with an interval you determine. Using Pooling
    // 
    public class PooledSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _spawnableObjects;
        [Tooltip("The Spawner waits a random number of seconds between these two interval each time a object was spawned.")]
        [SerializeField] private float minSpawnIntervalInSeconds;
        [SerializeField] private float maxSpawnIntervalInSeconds;
        [SerializeField] private EventsPublisher _eventsPublisher;

        private IObjectPool<PooledGameObject> objectPool;

        private void Awake()
        {
            objectPool = new ObjectPool<PooledGameObject>(OnNewInstanceForPool, OnRemovedFromPool, OnReleasedBackToPool);
        }

        private IEnumerator Start()
        {
            yield return StartCoroutine(nameof(Spawn));
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                objectPool.Get();
                yield return new WaitForSeconds(Random.Range(minSpawnIntervalInSeconds, maxSpawnIntervalInSeconds));
            }
        }

        private GameObject GetRandomSpawnableFromList()
        {
            int randomIndex = Random.Range(0, _spawnableObjects.Count);
            return _spawnableObjects[randomIndex];
        }

        private PooledGameObject OnNewInstanceForPool()
        {
            GameObject spawned = Instantiate(GetRandomSpawnableFromList());
            spawned.transform.localPosition = Vector3.zero;
            //spawned.transform.localRotation = Quaternion.identity;
            if (!spawned.TryGetComponent<PooledGameObject>(out PooledGameObject releaseScript))
            {
                releaseScript = spawned.AddComponent<PooledGameObject>();
                releaseScript.SetPoolInstance(objectPool);
                spawned.gameObject.transform.SetParent(transform, false);
                spawned.gameObject.transform.localPosition = Vector3.zero;
            }
            return releaseScript;
        }

        public void OnRemovedFromPool(PooledGameObject pooledInstance)
        {
            pooledInstance.gameObject.SetActive(true);
            pooledInstance.gameObject.transform.localPosition = Vector3.zero;
        }

        public void OnReleasedBackToPool(PooledGameObject pooledInstance)
        {
            pooledInstance.gameObject.SetActive(false);
        }
    }
}