using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Abstract base class for making ScriptableObject files for an IAssetManagerAsync.
    /// </summary>
    /// <typeparam name="T">The type of asset (currently GameObject or Sprite)</typeparam>
    public abstract class ScriptableAssetProviderBase<T> : ScriptableObject, IAssetManagerAsync<T>
    {
        protected List<string> _assetNames = new List<string>();

        public virtual IEnumerable<string> AvailableAssets()
        {
            foreach (string assetName in _assetNames)
            {
                yield return assetName;
            }
        }

        public abstract Task<T> GetAsync(string name);
        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }

        public abstract Task ReleaseAllAsync();
        public abstract Task ReleaseAsync(T instance);

    }
}