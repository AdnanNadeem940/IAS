using DG.Tweening;
using System.Collections;
using UnityEngine;
public class Nut : MonoBehaviour
{
    public ScrewColors ColorofTheScrew;
    public ScrewColorData ScrewData;
    public Color color;
    private Renderer render;
    Animator animator;
    public bool isHidden;
    public GameObject hiddenObj;

    void Awake()
    {
        render = GetComponent<Renderer>();
        animator = GetComponent<Animator>();
        color = ScrewData.GetColor(ColorofTheScrew);
        UpdateState();
    }
#if UNITY_EDITOR

    private void OnValidate()
    {
        if (ScrewData != null)
        {
            color = ScrewData.GetColor(ColorofTheScrew);
        }

    }

#endif
    public void UpdateState()
    {
        if (isHidden)
        {
            render.sharedMaterial.color = Color.black;
            hiddenObj.SetActive(true);
        }
        else
        {
            hiddenObj.SetActive(false);
            Debug.Log(color);
            render.sharedMaterial.color = color;
        }
    }
    public void ChangeColor(ScrewColors newColorType)
    {
        ColorofTheScrew = newColorType;
        color = ScrewData.GetColor(ColorofTheScrew  );
        render.material.color = color;
    }

    public void GotoPosition(Vector3 position, bool haveAnim)
    {
        if (haveAnim)
            StartCoroutine(MoveSmooth(position));
        else
            transform.localPosition = position;
    }
    public IEnumerator MoveSmooth(Vector3 targetPosition)
    {
        animator.SetBool("CanRotate", true);
        Vector3 start = transform.localPosition;
        Vector3 direction = targetPosition - start;
        float distance = direction.magnitude;

        float speed = 0.5f;
        float duration = distance / speed;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(start, targetPosition, elapsed / duration);
            yield return null;
        }

        transform.localPosition = targetPosition;
        transform.localRotation = Quaternion.identity;
        animator.SetBool("CanRotate", false);
    }
}
