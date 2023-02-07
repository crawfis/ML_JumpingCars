using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Always returns the exact same Sprite.
    /// </summary>
    public class SingleInstanceSpriteProvider : MonoBehaviour, IAssetManagerAsync<Sprite>
    {
        [SerializeField] private Sprite _asset;

        /// <inheritdoc/>
        public IEnumerable<string> AvailableAssets()
        {
            yield return _asset.name;
        }

        /// <inheritdoc/>
        public Task<Sprite> GetAsync(string name)
        {
            return Task.FromResult(_asset);
        }

        /// <inheritdoc/>
        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task ReleaseAllAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task ReleaseAsync(Sprite asset)
        {
            return Task.CompletedTask;
        }
    }
}