using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Given the underlying asset names, selects them in sequental order.
    /// This will then call the underlying IAssetManagerAsync to create the chosen asset.
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/SequentialSpriteDecorator", order = 6)]
    public class SequentialSpriteDecorator : DecoratorAssetProviderBase<Sprite>
    {
        private List<string> _assetPrefabs = new List<string>();
        private int _currentIndex = 0;

        /// <inheritdoc/>
        public override async Task<Sprite> GetAsync(string name)
        {
            if (_assetPrefabs.Count <= 0) return null;
            if (_currentIndex >= _assetPrefabs.Count) _currentIndex = 0;
            var asset = await _assetProvider.GetAsync(_assetPrefabs[_currentIndex++]);
            return asset;
        }

        /// <inheritdoc/>
        public override async Task Initialize()
        {
            await _assetProvider.Initialize();
            _assetPrefabs = _assetProvider.AvailableAssets().ToList();
        }
    }
}