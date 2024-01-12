using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableEventListener : MonoBehaviour
{
    [SerializeField]
    private ScriptableEvent myEvent;

    [SerializeField]
    UnityEvent eventResponse;

    private void OnEnable()
    {
        myEvent.Register(this);
    }
    private void OnDisable()
    {
        myEvent.UnRegister(this);
    }

    public void OnEventRaised()
    {
        eventResponse.Invoke();
    }
}
