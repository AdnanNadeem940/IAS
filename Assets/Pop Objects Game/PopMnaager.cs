using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopMnaager : MonoBehaviour
{
    public GameObject[] ObJectToPop;
    public LayerMask TargetLayer;
    public int TargetToWin, HitObjectsCount;
    bool CanCastRay;
    public TextMeshProUGUI Remain, Score;
    int StartObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartObject = 0;
        ObJectToPop[StartObject].SetActive(true);
        CanCastRay =true;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)&& CanCastRay)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

            if (Physics.Raycast(ray, out hit, 500f, ~0, QueryTriggerInteraction.Collide))
            {

                if (((1 << hit.collider.gameObject.layer) & TargetLayer) != 0)
                {
                    hit.collider.gameObject.transform.DOLocalRotate(new Vector3(-180, 0, 0), 0.5f, RotateMode.Fast);
                    hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false; ;
                    hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic=false;
                    HitObjectsCount++;
                    CheckCompleteCondtion();
                }
            }
        }
    }
    void CheckCompleteCondtion()
    {
       // Remain.text= HitObjectsCount.ToString();
        if (HitObjectsCount==TargetToWin)
        {
            CanCastRay=false;
            Debug.Log("Level Completed");
            if (StartObject + 1 < ObJectToPop.Length)
            {
                ObJectToPop[StartObject].SetActive(false);
                StartObject += 1;
                ObJectToPop[StartObject].SetActive(true);
            }
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1000);
            Score.text = PlayerPrefs.GetInt("Coins").ToString();
        }
    }
    public void ResetData(int Num)
    {
        CanCastRay = true;
        TargetToWin = Num;
        HitObjectsCount = 0;
    }
}
