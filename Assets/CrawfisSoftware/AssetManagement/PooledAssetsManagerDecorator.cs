using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CrawfisSoftware.AssetManagement
{
    internal class PooledAssetsManagerDecorator : PoolerBaseAsync<GameObject>
    {
        private readonly IAssetManagerAsync<GameObject> _assetManager;

        public PooledAssetsManagerDecorator(IAssetManagerAsync<GameObject> assetManager)
        {
            _assetManager = assetManager;
        }

        public override IEnumerable<string> AvailableAssets()
        {
            return _assetManager.AvailableAssets();
        }

        public override Task Initialize()
        {
            return _assetManager.Initialize();
        }

        protected override Task<GameObject> CreateNewPoolInstanceAsync(string assetName)
        {
            return _assetManager.GetAsync(assetName);
            //GameObject asset = _assetManager.GetAsync(assetName).GetAwaiter().GetResult();
            //return Task.FromResult(asset);
        }

        protected override Task DestroyPoolInstanceAsync(GameObject poolObject)
        {
            return _assetManager.ReleaseAsync(poolObject);
        }

        protected override void ReinitializePoolInstance(GameObject poolObject)
        {
            poolObject.SetActive(true);
        }

        protected override void ReturnPoolInstance(GameObject poolObject)
        {
            poolObject.SetActive(false);
        }
    }
}