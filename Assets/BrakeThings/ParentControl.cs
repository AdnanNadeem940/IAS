using UnityEngine;

public class ParentControl : MonoBehaviour
{
    public GameObject Child1, ParticleSystem;

 public void SetFlowerNull()
    {
        Child1.transform.SetParent(null);
        Child1.GetComponent<Rigidbody>().isKinematic=false;
        if(ParticleSystem!=null)
        {
            ParticleSystem.SetActive(true);
            Destroy(ParticleSystem,1.5f);
        }
        Destroy(Child1, 15f);
    }

   
}
