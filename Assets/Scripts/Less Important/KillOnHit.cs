using CrawfisSoftware.Jumper;
using UnityEngine;

public class KillOnHit : MonoBehaviour
{
    /*
     * Prevents the usage of resources by destroying
     * all movers hitting this wall. 
     */
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.TryGetComponent<PooledGameObject>(out PooledGameObject releaseScript))
            releaseScript.Release();
        else
            Destroy(other.gameObject);
    }
}
