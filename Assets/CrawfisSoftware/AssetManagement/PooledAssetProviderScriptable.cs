using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Decorator that uses Unity's Pool API to support object pooling.
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/PooledDecorator", order = 4)]
    public class PooledAssetProviderScriptable : DecoratorAssetProviderBase<GameObject>
    {
        private PooledAssetsManagerDecorator _pooledAssets;

        /// <inheritdoc/>
        public override Task<GameObject> GetAsync(string name)
        {
            return _pooledAssets.GetAsync(name);
        }

        /// <inheritdoc/>
        public override Task Initialize()
        {
            _pooledAssets = new PooledAssetsManagerDecorator(_assetProvider);
            return _assetProvider.Initialize();
        }

        /// <inheritdoc/>
        public override Task ReleaseAllAsync()
        {
            return _pooledAssets.ReleaseAllAsync();
        }

        /// <inheritdoc/>
        public override Task ReleaseAsync(GameObject instance)
        {
            return _pooledAssets.ReleaseAsync(instance);
        }
    }
}
