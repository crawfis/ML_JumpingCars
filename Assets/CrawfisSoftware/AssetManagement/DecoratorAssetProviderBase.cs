using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Abstract class for decorators ontop of the ScriptableAssetProviderBase. Handles the IAssetManagerAsync functions
    /// and contains the real asset provider.
    /// </summary>
    /// <typeparam name="T">The type of asset (currently GameObject or Sprite)</typeparam>
    public abstract class DecoratorAssetProviderBase<T> : ScriptableAssetProviderBase<T>
    {
        /// <summary>
        /// The real asset provider (IAssetManagerAsync)
        /// </summary>
        protected ScriptableAssetProviderBase<T> _assetProvider;

        /// <summary>
        /// Required in lieu of a constructor to set the real asset provider (IAssetManagerAsync).
        /// </summary>
        /// <param name="assetProvider">The real asset provider (IAssetManagerAsync)</param>
        public void SetRealInstance(ScriptableAssetProviderBase<T> assetProvider)
        {
            _assetProvider = assetProvider;
        }

        /// <inheritdoc/>
        public override Task<T> GetAsync(string name)
        {
            return _assetProvider.GetAsync(name);
        }

        /// <inheritdoc/>
        public override IEnumerable<string> AvailableAssets()
        {
            return _assetProvider.AvailableAssets();
        }

        /// <inheritdoc/>
        public override Task Initialize()
        {
            return _assetProvider.Initialize();
        }

        /// <inheritdoc/>
        public override Task ReleaseAllAsync()
        {
            return _assetProvider.ReleaseAllAsync();
        }

        /// <inheritdoc/>
        public override Task ReleaseAsync(T asset)
        {
            return _assetProvider.ReleaseAsync(asset);
        }
    }
}