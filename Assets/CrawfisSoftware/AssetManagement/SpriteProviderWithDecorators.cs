using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// General IAssetManagerAsync for Sprites that provides the underlying access (local, addressables, multiple, single, etc.)
    /// with decorators that provide additional functionality (randomly select, sequentially select, use pooling, etc.)
    /// </summary>
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "CrawfisSoftware/AssetProviders/AddSpriteDecorators", order = 3)]
    public class SpriteProviderWithDecorators : DecoratorAssetProviderBase<Sprite>
    {
        [SerializeField] private ScriptableAssetProviderBase<Sprite> _baseAssetProvider;
        [SerializeField] private List<DecoratorAssetProviderBase<Sprite>> _decorationAssetProviders = new List<DecoratorAssetProviderBase<Sprite>>();

        /// <inheritdoc/>
        public override Task<Sprite> GetAsync(string name)
        {
            return _assetProvider.GetAsync(name);
        }

        /// <inheritdoc/>
        public override Task Initialize()
        {
            AddDecorators();
            return _assetProvider.Initialize();
        }

        /// <summary>
        /// Initializes the decorators and produces the correct instance that propogates through the decorators to the actual provider.
        /// </summary>
        protected void AddDecorators()
        {
            int count = _decorationAssetProviders.Count;
            var lastDecorator = _baseAssetProvider;
            for (int i = 0; i < count; i++)
            {
                var decorator = _decorationAssetProviders[i];
                decorator.SetRealInstance(lastDecorator);
                lastDecorator = decorator;
            }
            _assetProvider = lastDecorator;
        }
    }
}