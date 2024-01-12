using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_TriggerObjectType
{
    Misc = 0,
    Coin,
    Goal,
    Obstacle,
    Checkpoint
}

public class TriggerObject : MonoBehaviour
{
    [SerializeField]
    ScriptableEvent triggerEvent;

    [SerializeField]
    bool setInactiveWhenTrigger = true;

    [SerializeField]
    E_TriggerObjectType triggerObjType;

    [SerializeField]
    ScriptableInt playerCollisionLayer;

    public E_TriggerObjectType GetTriggerObjType { get => triggerObjType; }

    private void Start()
    {
        LevelManager.Instance.SubscribeTriggerObject(this);
    }


    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer != playerCollisionLayer.DefaultValue)
            return;
        Debug.Log("Raise Trigger Event! " + this.gameObject.name);
        triggerEvent?.RaiseEvent();
        if (setInactiveWhenTrigger)
            gameObject.SetActive(false);

    }

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.gameObject.layer != playerCollisionLayer.DefaultValue)
            return;

        if (this.enabled == false)
            return;

        triggerEvent?.RaiseEvent();
        if (setInactiveWhenTrigger)
            gameObject.SetActive(false);
    }

}
