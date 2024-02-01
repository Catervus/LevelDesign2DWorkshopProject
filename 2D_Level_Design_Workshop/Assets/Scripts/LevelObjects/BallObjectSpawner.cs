using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject ballObject;

    [SerializeField]
    private ScriptableFloat spawnCD;

    [SerializeField]
    private Transform spawnTrans;

    [SerializeField]
    private ScriptableFloat ballShootForce;

    private float curSpawnCD;

    void Update()
    {
        if(curSpawnCD <= 0)
        {
            SpawnObject();
            curSpawnCD = spawnCD.DefaultValue;
        }

        curSpawnCD -= Time.deltaTime;
    }

    void SpawnObject()
    {
        GameObject go = Instantiate(ballObject, spawnTrans.position, Quaternion.identity);
        go.transform.parent = this.transform;
        Destroy(go, 5);

        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("Warning: Spawned object does not have the necessary Rigidbody.");
            return;
        }

        rb.velocity = transform.up * ballShootForce.RuntimeValue;
    }
}
