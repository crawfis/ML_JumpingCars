using CrawfisSoftware.AssetManagement;
using CrawfisSoftware.EventManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrawfisSoftware.Jumper
{
    //
    //  Spawns the Mover Objects (Enemies) with an interval you determine.
    // 
    public class PooledSpawner : MonoBehaviour
    {
        [SerializeField] private ScriptableAssetProviderBase<GameObject> _assetManager;
        [SerializeField] private List<string> _spawnableObjectNames;
        [Tooltip("The Spawner waits a random number of seconds between these two interval each time a object was spawned.")]
        [SerializeField] private float minSpawnIntervalInSeconds;
        [SerializeField] private float maxSpawnIntervalInSeconds;
        [SerializeField] private EventsPublisher _eventsPublisher;

        //private List<GameObject> spawnedObjects = new List<GameObject>();
        //private Vector3 _initialSpawnPosition;

        private void Awake()
        {
            //Subscribes to Reset of Player
            _eventsPublisher.SubscribeToEvent("JumperReset", DestroyAllSpawnedObjects);
        }

        private IEnumerator Start()
        {
            yield return _assetManager.Initialize();
            //_initialSpawnPosition = transform.localPosition;
            StartCoroutine(nameof(Spawn));
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                //var spawned = Instantiate(GetRandomSpawnableFromList(), transform.position, transform.rotation, transform);
                //spawnedObjects.Add(spawned);
                //var task = _assetManager.GetAsync(GetRandomSpawnableFromList());
                _ = SpawnAsync();
                yield return new WaitForSeconds(Random.Range(minSpawnIntervalInSeconds, maxSpawnIntervalInSeconds));
            }
        }

        private async Task SpawnAsync()
        {
            GameObject spawned = await _assetManager.GetAsync(GetRandomSpawnableFromList());
            if (spawned == null) return;

            spawned.transform.localPosition = Vector3.zero;
            //spawned.transform.localRotation = Quaternion.identity;
            if (!spawned.TryGetComponent<ReleaseOnDestroy>(out _))
            {
                var releaseScript = spawned.AddComponent<ReleaseOnDestroy>();
                releaseScript.SetAssetManager(_assetManager);
                spawned.gameObject.transform.SetParent(transform, false);
            }
        }

        private void DestroyAllSpawnedObjects(object sender, object data)
        {
            _assetManager.ReleaseAllAsync();
            //for (int i = spawnedObjects.Count - 1; i >= 0; i--)
            //{
            //    Destroy(spawnedObjects[i]);
            //    spawnedObjects.RemoveAt(i);
            //}
        }
        private string GetRandomSpawnableFromList()
        {
            int randomIndex = Random.Range(0, _spawnableObjectNames.Count);
            return _spawnableObjectNames[randomIndex];
        }
    }
}