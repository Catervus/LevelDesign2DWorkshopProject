using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBoiBehaviour : EnemyBase
{

    private bool goingLeft = true;

    [SerializeField]
    private Transform sideTriggerBoxTrans;
    [SerializeField]
    private Vector2 sideTriggerBoxDimensions;

    [SerializeField]
    private Transform groundTriggerBoxTrans;
    [SerializeField]
    private Vector2 groundTriggerBoxDimensions;

    [SerializeField]
    private ScriptableInt groundCollisionLayer;

    [SerializeField]
    private ScriptableFloat walkSpeed;

    [SerializeField]
    private LayerMask obstacleLayer;
    [SerializeField]
    private LayerMask groundLayer;


    void Start()
    {
        UpdateSpawnPos();
    }

    void Update()
    {
        isActive = CheckForPlayerInSpawnRange();
        if (isActive == false)
        {
            if ((Vector2)transform.position != spawnPos)
            {
                transform.position = spawnPos;
                if(goingLeft == false)
                    transform.Rotate(0, 180f, 0);
                goingLeft = true;
                
            }

            return;
        }


        if (CheckCollisionTriggerBox(sideTriggerBoxTrans.position, sideTriggerBoxDimensions, obstacleLayer) || !CheckCollisionTriggerBox(groundTriggerBoxTrans.position, groundTriggerBoxDimensions, groundLayer))
        {
            Debug.Log("Collision Trigger!");
            goingLeft = !goingLeft;
            transform.Rotate(0, 180f, 0);
        }

        SideMovement(walkSpeed.RuntimeValue);
    }

    bool CheckCollisionTriggerBox2()
    {
        bool check = Physics2D.OverlapBox(sideTriggerBoxTrans.position, sideTriggerBoxDimensions, 0, obstacleLayer);
        if (check)
            return true;
        
        {
            return true;
        }
        // return false;


        check = Physics2D.OverlapBox(groundTriggerBoxTrans.position, groundTriggerBoxDimensions, 0, groundLayer);
        Debug.Log("Check Ground: " + check);
        return (check == false);

        // return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(sideTriggerBoxTrans.position, sideTriggerBoxDimensions);
        Gizmos.DrawWireCube(groundTriggerBoxTrans.position, groundTriggerBoxDimensions);

    }


}
