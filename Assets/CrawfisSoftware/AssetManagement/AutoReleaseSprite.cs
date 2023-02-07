using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Utility class for Sprites that calls ReleaseAsync, overriding the default destruction process..
    /// </summary>
    public class AutoReleaseSprite : MonoBehaviour
    {
        private IAssetManagerAsync<Sprite> _assetProvider;
        private Sprite _sprite;

        /// <summary>
        /// The IAssetManagerAsync associated with this instance. 
        /// </summary>
        /// <param name="assetProvider">The asset provider.</param>
        public void SetAssetManager(IAssetManagerAsync<Sprite> assetProvider)
        {
            this._assetProvider = assetProvider;
        }

        private void OnDestroy()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            _sprite = spriteRenderer.sprite;
            spriteRenderer.sprite = null;
            _assetProvider.ReleaseAsync(_sprite);
        }
    }
}