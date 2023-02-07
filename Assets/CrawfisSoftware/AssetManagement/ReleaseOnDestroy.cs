using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    internal class ReleaseOnDestroy : MonoBehaviour
    {
        private IAssetManagerAsync<GameObject> _assetProvider;

        public void SetAssetManager(IAssetManagerAsync<GameObject> assetProvider)
        {
            this._assetProvider = assetProvider;
        }

        public void Release()
        {
            _assetProvider.ReleaseAsync(this.gameObject);
        }
        //private void OnDestroy()
        //{
        //    _assetProvider.ReleaseAsync(this.gameObject);
        //}
    }
}