using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrewController : MonoBehaviour, IPointerDownHandler
{
    public CircleCollider2D Collider;
    public bool Screwed = true;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameController.instance.IsSpaceAvialable)
        {
            Debug.Log("OnPointerDown: " + gameObject.name);
            UnScrewNuts currentNut = GetComponentInParent<UnScrewNuts>();
            GameController.instance.NextNut = currentNut;

            if (GameController.instance.SelectedScrew != null && GameController.instance.SelectedScrew != this)
            {
                GameController.instance.SelectedScrew.SetStatusScrew(true, 0.2f);
            }
            if (GameController.instance.SelectedScrew == this && !Screwed)
            {
                SetStatusScrew(true, 0.2f);
            }
            else
            {
                SetStatusScrew(false, 0.2f);
            }
        }

    }
    public void SetStatusScrew(bool status, float time)
    {
        transform.DOKill();

        if (status)
        {
            Screwed = true;
            transform.DOScale(1f, time);
            transform.DORotate(new Vector3(0f, 0f, 0f), time).OnComplete(() =>
            {
                if (GameController.instance.SelectedScrew == this)
                {
                   GameController.instance.NextNut.isEmpty = false;
                    GameController.instance.NextNut.NutCollider.isTrigger = true;
                    Collider.isTrigger = false;
                    GameController.instance.SelectedScrew = null;
                }
            });
        }
        else
        {
            GameController.instance.SelectedScrew = this;
            // GameManager.Instance.currentScrewNut.empty = true;
            GameController.instance.NextNut.NutCollider.isTrigger = false;
            Collider.isTrigger = true;
            transform.DOScale(1.3f, time);
            transform.DORotate(new Vector3(0f, 0f, 180f), time).OnComplete(() =>
            {
                Screwed = false;
            });


        }
    }

    public void MoveToScrewNut(Transform trans)
    {
        transform.DOMove(trans.position, 0.15f).OnComplete(() =>
        {
            GameController.instance.SelectedScrew.Collider.isTrigger = false;
            GameController.instance.NextNut.isEmpty = false;
            GameController.instance.SelectedScrew.transform.SetParent(GameController.instance.NextNut.transform);
            GameController.instance.SelectedScrew.SetStatusScrew(true, 0.2f);
        });
    }
}
