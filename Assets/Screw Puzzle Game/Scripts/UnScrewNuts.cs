using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnScrewNuts : MonoBehaviour, IPointerDownHandler
{
    public bool isEmpty;
    public CircleCollider2D NutCollider;
    public Transform backCheckPoint; // assign a small empty GameObject behind the nut
    public float checkRadius = 0.1f;
    public LayerMask hurdleLayer; // assign "Hurdle" layer in Inspector

    public void OnPointerDown(PointerEventData EventData)
    {
       // Debug.Log("Pointer Down is working");

        // block if hurdle is detected behind this nut
        if (IsHurdleBehind())
        {
            Debug.Log("Blocked: Hurdle behind this hole!");
            return;
        }

        if (isEmpty && GameController.instance.SelectedScrew != null &&
            !GameController.instance.SelectedScrew.Screwed &&
            !GameController.instance.IsSpaceAvialable)
        {
            if (GameController.instance.NextNut != this)
            {
                GameController.instance.IsSpaceAvialable = true;
                GameController.instance.NextNut.isEmpty = true;
                GameController.instance.NextNut.NutCollider.isTrigger = true;
                GameController.instance.NextNut = this;

                GameController.instance.SelectedScrew.transform.DOMove(transform.position, 0.15f).OnComplete(() =>
                {
                    GameController.instance.SelectedScrew.Collider.isTrigger = false;
                    GameController.instance.NextNut.isEmpty = false;

                    var screw = GameController.instance.SelectedScrew.transform;
                    screw.SetParent(transform);
                    screw.localPosition = new Vector3(screw.localPosition.x, screw.localPosition.y, -0.25f);

                    GameController.instance.SelectedScrew.transform.DOScale(1f, 0.15f);
                    GameController.instance.SelectedScrew.transform.DORotate(Vector3.zero, 0.15f).OnComplete(() =>
                    {
                        GameController.instance.NextNut.isEmpty = false;
                        GameController.instance.NextNut.NutCollider.isTrigger = true;
                        NutCollider.isTrigger = false;
                        GameController.instance.SelectedScrew = null;
                        GameController.instance.IsSpaceAvialable = false;
                    });
                });
            }
        }
    }

    // Helper function to detect hurdle behind
    private bool IsHurdleBehind()
    {
        if (backCheckPoint == null) return false;

        Collider2D hit = Physics2D.OverlapCircle(backCheckPoint.position, checkRadius, hurdleLayer);
        return hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (backCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(backCheckPoint.position, checkRadius);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Metal"))
            isEmpty = true;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Metal"))
            isEmpty = false;
    }
}
