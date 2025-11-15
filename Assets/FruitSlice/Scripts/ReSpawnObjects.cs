using UnityEngine;

public class ReSpawnObjects : MonoBehaviour
{
    public GameObject VegitbleObjectPrefab;

    public Transform SpawnPosition;
    private void Start()
    {
         
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Instantiate(VegitbleObjectPrefab, SpawnPosition.position, SpawnPosition.rotation);
        }
        else
        {

            Destroy(other.gameObject);
        }
    }
}
