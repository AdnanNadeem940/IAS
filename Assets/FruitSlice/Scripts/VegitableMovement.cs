using System.Collections;
using UnityEngine;

public class VegitableMovement : MonoBehaviour
{
    [SerializeField] float MovementSpeed;
    void Update()
    {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y, gameObject.transform.localPosition.z - MovementSpeed * Time.deltaTime);
    }
}
