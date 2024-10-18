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
    bool setInactiveWhenTriggerWithPlayer = true;

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
        if (_other.gameObject.layer == playerCollisionLayer.DefaultValue)
        {
            Debug.Log("Raise Trigger Event! OnTriggerEnter2D: " + this.gameObject.name);
            triggerEvent?.RaiseEvent(this.gameObject);
            if (setInactiveWhenTriggerWithPlayer)
            {
                gameObject.SetActive(false);
                return;
            }

        }

        if (setInactiveWhenTrigger)
            gameObject.SetActive(false);

    }

    private void OnCollisionEnter2D(Collision2D _col)
    {
        Debug.Log("Col: " + gameObject.name);
        if (_col.gameObject.layer == playerCollisionLayer.DefaultValue)
        {
            Debug.Log("Raise Trigger Event! OnCollisionEnter2D: " + this.gameObject.name);
            triggerEvent?.RaiseEvent(this.gameObject);
        }

        // if (this.enabled == false)
        //     return;
        
        if (setInactiveWhenTrigger)
            gameObject.SetActive(false);
    }

}
