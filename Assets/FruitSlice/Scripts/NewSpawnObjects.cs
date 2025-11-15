using UnityEngine;
using System.Collections;
using Unity.Mathematics;
public class NewSpawnObjects : MonoBehaviour
{
    public static NewSpawnObjects Instance;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject[] Items;

    public float wait;
    public IEnumerator Starter()
    {
        var item = Instantiate(Items[0], transform.position, quaternion.identity);
        item.SetActive(true);
        yield return new WaitForSeconds(0.25f);
    }
}
