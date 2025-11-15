using UnityEngine;
using Unity.Mathematics;
using System.Collections;
using Random = UnityEngine.Random;
public class Knife : MonoBehaviour
{

    public float forceValuex, forceValueY, forceValueZ;
    public GameObject[] ParticleBlood;
    public CameraControl CameraController;

    private int count;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool stop;
    IEnumerator Stopper()
    {
        stop = true;
        yield return new WaitForSeconds(0.15f);
        stop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vegitable" && other.gameObject.GetComponent<MeshCollider>().isTrigger == true && stop == false)
        {
            if (stop == false)
            {

                int[] randomYDir = new int[2] { 600 , -600 };
                int yDir = randomYDir[Random.Range(0, randomYDir.Length)];
                other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                other.gameObject.GetComponent<Rigidbody>()
                    .AddForce(yDir, forceValueY, forceValueZ, ForceMode.Acceleration);
                other.gameObject.GetComponent<MeshCollider>().isTrigger = false;
                other.gameObject.GetComponent<vegitableMovementForce>().Starter();
                other.gameObject.GetComponent<vegitableMovementForce>().MoveForceToZ = true;
                count += 1;
                CameraController.Count.text = count.ToString();
                if (other.transform.parent.gameObject.name == "carrot"|| other.transform.parent.gameObject.name == "Cocumber"|| other.transform.parent.gameObject.name == "Tomator 2"|| other.transform.parent.gameObject.name == "Chakundar1")
                    Debug.Log("The gameObject is "+other.transform.parent.gameObject.name);
                    // Instantiate(ParticleBlood[0], other.transform.position, quaternion.identity);
              /*  if (other.transform.parent.gameObject.name == "cucumber")
                    Instantiate(ParticleBlood[1], other.transform.position, quaternion.identity);
                if (other.transform.parent.gameObject.name == "Daikon")
                    Instantiate(ParticleBlood[2], other.transform.position, quaternion.identity);*/
                StartCoroutine(Stopper());
            }


        }
    }
}
