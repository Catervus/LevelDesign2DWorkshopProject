using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private ScriptableEvent entityDeathEvent;

    [SerializeField]
    private ScriptableInt entityHealthPoints;

    [SerializeField]
    private ScriptableInt deathTriggerLayer;


    void Update()
    {

    }

    public void TakeDamage(int _dmgamount)
    {
        entityHealthPoints.RuntimeValue -= _dmgamount;

        if (entityHealthPoints.RuntimeValue <= 0)
            EntityDeath();

    }

    private void EntityDeath()
    {
        entityDeathEvent?.RaiseEvent();
    }


    private void OnTriggerEnter(Collider _other)
    {
        if(_other.gameObject.layer == deathTriggerLayer.RuntimeValue)
            TakeDamage(entityHealthPoints.DefaultValue);
    }

}
