using UnityEngine;
using UnityEngine.Events;

public class EventHnader : MonoBehaviour
{
    public UnityEvent Enable, _Start, Disable;
    private void OnEnable()
    {
        Enable.Invoke();
    }
    void Start()
    {
        _Start.Invoke();
    }

    // Update is called once per frame
    void OnDisable()
    {
        Disable.Invoke();
    }
}
