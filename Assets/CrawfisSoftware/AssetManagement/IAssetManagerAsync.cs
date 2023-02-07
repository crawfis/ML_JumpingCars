using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Manages the acquisition and disposal of assets.
    /// </summary>
    /// <typeparam name="T">The asset type (GameObject, Sprite, etc.)</typeparam>
    public interface IAssetManagerAsync<T>
    {
        /// <summary>
        /// Get an instance of the asset, perhaps using the name as a key.
        /// </summary>
        /// <param name="name">An optional name to use when selecting from many possible assets.</param>
        /// <returns>An instance of the Type T</returns>
        Task<T> GetAsync(string name);

        /// <summary>
        /// Dispose or release the current instance. This may be back to a pool or deleted, etc.
        /// </summary>
        /// <param name="instance">The asset to be released.</param>
        /// <returns>A task useful for async / await operations.</returns>
        Task ReleaseAsync(T instance);

        /// <summary>
        /// Dispose or release all current instances. This may be back to a pool or deleted, etc.
        /// </summary>
        /// <returns>A task useful for async / await operations.</returns>
        Task ReleaseAllAsync();

        /// <summary>
        /// Perform any initialization operation. This method should be called first. It typically
        /// sets-up the information for the AvailableAssets method.
        /// </summary>
        /// <returns>A task useful for async / await operations.</returns>
        Task Initialize();

        /// <summary>
        /// Get all of the assets that this manager can provide.
        /// </summary>
        /// <returns>An IEnumerable of names.</returns>
        IEnumerable<string> AvailableAssets();
    }
}