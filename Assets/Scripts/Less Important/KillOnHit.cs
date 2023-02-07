using CrawfisSoftware.AssetManagement;
using UnityEngine;

public class KillOnHit : MonoBehaviour
{
    /*
     * Prevents the usage of resources by destroying
     * all movers hitting this wall. 
     */
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.TryGetComponent<ReleaseOnDestroy>(out ReleaseOnDestroy releaseScript))
            releaseScript.Release();
        else
            Destroy(other.gameObject);
    }
}
