using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Utility class for GameObjects that calls ReleaseAsync when the object is being destroyed.
    /// </summary>
    public class AutoRelease : MonoBehaviour
    {
        private IAssetManagerAsync<GameObject> _assetProvider;

        /// <summary>
        /// The IAssetManagerAsync associated with this instance. 
        /// </summary>
        /// <param name="assetProvider">The asset provider.</param>
        public void SetAssetManager(IAssetManagerAsync<GameObject> assetProvider)
        {
            this._assetProvider = assetProvider;
        }

        private void OnDestroy()
        {
            _assetProvider.ReleaseAsync(this.gameObject);
        }
    }
}