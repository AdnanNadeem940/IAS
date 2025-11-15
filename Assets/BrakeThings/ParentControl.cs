using UnityEngine;

public class ParentControl : MonoBehaviour
{
    public GameObject Child1;
 public void SetFlowerNull()
    {
        Child1.transform.SetParent(null);
        Child1.GetComponent<Rigidbody>().isKinematic=false;
    }

   
}
