using UnityEngine;
using System.Collections;
public class DestroyOldObjects : MonoBehaviour
{
    public float wait;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}
