using UnityEngine;

namespace CrawfisSoftware.Jumper
{
    public class KillOnHit : MonoBehaviour
    {
        /*
         * Prevents the usage of resources by destroying
         * all movers hitting this wall. 
         */
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PooledGameObject releaseScript))
                releaseScript.Release();
            else
                Destroy(other.gameObject);
        }
    }
}