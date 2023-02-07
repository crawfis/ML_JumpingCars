using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Uses Addressables to create and release GameObjects.
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/AddressableAssetProvider", order = 2)]
    public class AddressablesAssetProvider : ScriptableAssetProviderBase<GameObject>
    {
        [SerializeField] private AssetReference _prefab;

        private List<GameObject> _allocatedAssets = new List<GameObject>();
        //private readonly List<AsyncOperationHandle<GameObject>> _assetHandles = new List<AsyncOperationHandle<GameObject>>();
        private readonly Dictionary<string, IResourceLocation> _assetMapping = new Dictionary<string, IResourceLocation>();

        private async Task Awake()
        {
            await Initialize();
        }

        /// <inheritdoc/>
        public override async Task<GameObject> GetAsync(string name)
        {
            if (_assetMapping.TryGetValue(name, out IResourceLocation prefab))
            {
                var handle = Addressables.InstantiateAsync(prefab);
                //_assetHandles.Add(handle);
                var task = handle.Task;
                await task;
                GameObject asset = task.Result;
                _allocatedAssets.Add(asset);
                return asset;
            }
            else
            {
                var handle = Addressables.InstantiateAsync(name);
                //_assetHandles.Add(handle);
                var task = handle.Task;
                await task;
                GameObject asset = task.Result;
                _allocatedAssets.Add(asset);
                return asset;
            }
            //return null;
        }

        /// <inheritdoc/>
        public override Task ReleaseAllAsync()
        {
            for (int i = 0; i < _allocatedAssets.Count; i++)
            {
                var asset = _allocatedAssets[i];
                _allocatedAssets[i] = null;
                Addressables.ReleaseInstance(asset);
            }
            _allocatedAssets.Clear();
            //foreach (var handle in this._assetHandles)
            //{
            //    Addressables.Release<GameObject>(handle);
            //}

            //_assetHandles.Clear();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override Task ReleaseAsync(GameObject asset)
        {
            if (_allocatedAssets.Remove(asset))
            {
                Addressables.ReleaseInstance(asset);
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override async Task Initialize()
        {
            if (_assetMapping.Count > 0) return;
            //await _addressableCatalog.WaitForCatalogToBeLoaded();
            // Todo: This does not seem to work.
            foreach (var asset in _assetNames)
            {
                var handle = Addressables.LoadResourceLocationsAsync(asset, typeof(GameObject));
                var resourceLocation = await handle.Task;
                if (resourceLocation != null && resourceLocation.Count > 0)
                {
                    _assetMapping[asset] = resourceLocation[0];
                }
            }
        }
    }
}