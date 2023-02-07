using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Instantiates and destroys a given prefab using the name of the prefab.
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/AssetSpriteProvider", order = 1)]
    public class SpriteAssetProvider : ScriptableAssetProviderBase<Sprite>
    {
        [SerializeField] private List<Sprite> _assetPrefabs = new List<Sprite>();

        private readonly List<Sprite> _allocatedAssets = new List<Sprite>();
        private readonly Dictionary<string, Sprite> _assetMapping = new Dictionary<string, Sprite>();

        private async Task Awake()
        {
            await Initialize();
        }

        /// <inheritdoc/>
        public override Task<Sprite> GetAsync(string name)
        {
            if (_assetMapping.TryGetValue(name, out Sprite prefab))
            {
                var asset = Instantiate(prefab);
                _allocatedAssets.Add(asset);
                return Task.FromResult(asset);
            }
            return null;
        }

        /// <inheritdoc/>
        public override Task ReleaseAllAsync()
        {
            for (int i = 0; i < _allocatedAssets.Count; i++)
            {
                var asset = _allocatedAssets[i];
                _allocatedAssets[i] = null;
                Destroy(asset);
            }
            _allocatedAssets.Clear();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override Task ReleaseAsync(Sprite asset)
        {
            if (_allocatedAssets.Remove(asset))
            {
                Destroy(asset);
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override async Task Initialize()
        {
            _assetNames.Clear();
            foreach (var asset in _assetPrefabs)
            {
                _assetMapping[asset.name] = asset;
                _assetNames.Add(asset.name);
            }
            await Task.CompletedTask;
        }
    }
}