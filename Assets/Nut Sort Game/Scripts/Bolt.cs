using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public float MoveHieght = 0.02f;
    public float StartHight= -0.095f;
    public float DistanceToMove = 0.015f;
    public int MaxBolt = 4;
    public float OffsetTimeMove = 0.3f;
    public Stack<Nut> NutOutOder = new Stack<Nut>();
    public List<Nut> NutInBolt = new List<Nut>();
    public void CustomBolt()
    {
        float Difference = MaxBolt / 6;
        transform.localScale= new Vector3(transform.localScale.x, transform.localScale.y, Difference);
        float Offset = -(0.1f - Difference / 10f);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,Offset);
    }
    void Start()
    {
        foreach (Nut nut in NutInBolt)
        {
            NutOutOder.Push(nut);
        }
    }
    public bool IsFull()
    {
        return NutInBolt.Count == MaxBolt;
    }
    public Nut GetTopNut()
    {
        if (NutOutOder.TryPeek(out Nut nut))
            return nut;
        return null;
    }
    public void DetachNut(Nut nut)
    {
        Vector3 targetMove = new Vector3(0, 0, MoveHieght * 6 / MaxBolt);
        nut.GotoPosition(targetMove, true);
    }
    public void InsertNut(Nut nut)
    {
        Vector3 targetMove = new Vector3(0, 0, StartHight + NutOutOder.Count * DistanceToMove * 6 / MaxBolt);
        nut.GotoPosition(targetMove, true);
    }
    public void RemoveTopNut()
    {
        if (NutOutOder.Count == 0) return;
        NutOutOder.Pop();
        NutInBolt.RemoveAt(NutInBolt.Count - 1);
        if (NutOutOder.TryPeek(out Nut nut))
        {
            nut.isHidden = false;
            nut.UpdateState();
        }
    }
    public IEnumerator MoveNutToThisScrew(Nut nut)
    {
        DetachNut(nut);
        Vector3 distance = nut.transform.position - transform.position;

        NutInBolt.Add(nut);

        yield return new WaitForSeconds(Mathf.Abs(distance.magnitude) - Mathf.Abs(OffsetTimeMove * distance.magnitude));
        NutOutOder.Push(nut);
       // uncomment when Sound added SoundManager.Instance.PushSFX();

        InsertNut(nut);
    }
    public void AddNutNoAnimation(Nut nut)
    {
        NutInBolt.Add(nut);
        NutOutOder.Push(nut);
        InsertNutNoAnimation(nut);
    }
    public void InsertNutNoAnimation(Nut nut)
    {
        Vector3 targetMove = new Vector3(0, 0, StartHight + NutOutOder.Count * DistanceToMove * 6/ MaxBolt);
        nut.GotoPosition(targetMove, false);
    }
}
