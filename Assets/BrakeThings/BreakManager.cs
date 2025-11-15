using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BreakManager : MonoBehaviour
{
    public GameObject[] ObjectsToSpawn;
    public Transform[] spawnPlaces;

    public float minWait = 0.3f;

    public float maxWait = 1f;

    public float forceMagnitude = 5f;

    public GameObject OrbeBrakeParticle, FlowerBrakeParticle;

   // public AudioSource AudioToPlay;
    //public static int Counter;
   // public Text CounterTxt;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -6, 0);
        //StartCoroutine(SpawnBulbs());
        InvokeRepeating(nameof(StartSpawning), 0.1f, 1.5f);
     //   Counter = 0;
    }

    void StartSpawning()
    {
        Transform t = spawnPlaces[Random.Range(0, spawnPlaces.Length)];

        GameObject go = null;
        var rand = Random.Range(0, 100);

        if (rand <= 15)
        {
            go = ObjectsToSpawn[Random.Range(0, ObjectsToSpawn.Length)];
        }
        else
        {
            go = ObjectsToSpawn[Random.Range(0, ObjectsToSpawn.Length)];
        }

        GameObject bulb =
            Instantiate(go,
                t.transform.position,
                go.transform.rotation);

       // bulb.transform.rotation = Quaternion.Euler(0, 0, 0);

        bulb
            .GetComponent<Rigidbody>()
            .AddForce(t.transform.up * forceMagnitude, ForceMode.Acceleration);

        Destroy(bulb, 5f);
    }
    IEnumerator SpawnBulbs()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            Transform t = spawnPlaces[Random.Range(0, spawnPlaces.Length)];

            GameObject go = null;
            var rand = Random.Range(0, 100);

            if (rand <= 15)
            {
                go = ObjectsToSpawn[Random.Range(0, ObjectsToSpawn.Length)];
            }
            else
            {
                go = ObjectsToSpawn[Random.Range(0, ObjectsToSpawn.Length)];
            }

            GameObject bulb =
                Instantiate(go,
                t.transform.position,
                t.transform.rotation);

            bulb.transform.rotation = Quaternion.Euler(0, 0, 0);

            bulb
                .GetComponent<Rigidbody>()
                .AddForce(t.transform.up * forceMagnitude, ForceMode.Impulse);

            Destroy(bulb, 5f);
        }
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("Metal"))
                {
                    //AudioToPlay.Play();
                    var particles = Instantiate(OrbeBrakeParticle, hit.collider.gameObject.transform.position, Quaternion.identity);
                    hit.collider.gameObject.GetComponent<ParentControl>().SetFlowerNull();
                    Destroy(hit.collider.gameObject);
                    Destroy(particles, 1f);
                    //Counter += 1;
                    //CounterTxt.text = Counter.ToString();
                }
                else if (hit.collider.gameObject.CompareTag("Flower"))
                {
                    //AudioToPlay.Play();
                    var particles = Instantiate(FlowerBrakeParticle, hit.collider.gameObject.transform.position, Quaternion.identity);
                    hit.collider.gameObject.GetComponent<ParentControl>().SetFlowerNull();
                    Destroy(hit.collider.gameObject);
                    Destroy(particles, 1f);
                    //Counter += 1;
                    //CounterTxt.text = Counter.ToString();
                }
            }
        }
    }
}
