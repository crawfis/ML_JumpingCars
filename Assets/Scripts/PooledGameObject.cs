using UnityEngine;
using UnityEngine.Pool;

namespace CrawfisSoftware.Jumper
{
    public class PooledGameObject : MonoBehaviour
    {
        private IObjectPool<PooledGameObject> objectPool;

        public void SetPoolInstance(IObjectPool<PooledGameObject> pool)
        {
            objectPool = pool;
        }

        public void Release()
        {
            objectPool.Release(this);
        }
    }
}