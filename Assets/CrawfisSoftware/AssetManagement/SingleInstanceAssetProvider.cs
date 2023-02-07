using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Always returns the exact same GameObject.
    /// </summary>
    public class SingleInstanceAssetProvider : MonoBehaviour, IAssetManagerAsync<GameObject>
    {
        [SerializeField] private GameObject _asset;

        /// <inheritdoc/>
        public IEnumerable<string> AvailableAssets()
        {
            yield return _asset.name;
        }

        /// <inheritdoc/>
        public Task<GameObject> GetAsync(string name)
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
        public Task ReleaseAsync(GameObject asset)
        {
            return Task.CompletedTask;
        }
    }
}