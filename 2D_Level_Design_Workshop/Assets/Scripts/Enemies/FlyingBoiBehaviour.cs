using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBoiBehaviour : EnemyBase
{
    private bool goingLeft = true;

    private bool isDiving = false;

    [SerializeField]
    private Transform sideTriggerBoxTrans;
    [SerializeField]
    private Vector2 sideTriggerBoxDimensions;
    [SerializeField]
    private LayerMask obstacleLayer;

    [SerializeField]
    private ScriptableFloat flySpeed;

    [SerializeField]
    private float maxDeviationXAxis = 3;

    void Start()
    {
        UpdateSpawnPos();
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x - LevelManager.Instance.PlayerInstance.transform.position.x) <= 4)
            isDiving = true;
        if (isDiving)
        {
            transform.position += -(transform.right + transform.up) * flySpeed.RuntimeValue * Time.deltaTime;
            Debug.Log(gameObject.name + ": isDiving");

            return;
        }

        isActive = CheckForPlayerInSpawnRange();
        if (isActive == false)
        {
            if ((Vector2)transform.position != spawnPos)
            {
                transform.position = spawnPos;
                if (goingLeft == false)
                    transform.Rotate(0, 180f, 0);
                goingLeft = true;

            }

            return;
        }


        if(CheckCollisionTriggerBox(sideTriggerBoxTrans.position, sideTriggerBoxDimensions, obstacleLayer))
        {
            goingLeft = !goingLeft;
            transform.Rotate(0, 180f, 0);
        }

        SideMovement(flySpeed.RuntimeValue);

        if (transform.position.x < spawnPos.x - maxDeviationXAxis)
        {
            transform.position = 
                new Vector3(spawnPos.x - maxDeviationXAxis, transform.position.y, transform.position.z);
            goingLeft = false;
            transform.Rotate(0, 180f, 0);
        }
        else if(transform.position.x > spawnPos.x + maxDeviationXAxis)
        {
            transform.position =
                new Vector3(spawnPos.x + maxDeviationXAxis, transform.position.y, transform.position.z);
            goingLeft = true;
            transform.Rotate(0, 180f, 0);
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(spawnPos - new Vector2(maxDeviationXAxis, 0), 0.5f);
        Gizmos.DrawWireSphere(spawnPos + new Vector2(maxDeviationXAxis, 0), 0.5f);

        Gizmos.DrawWireCube(sideTriggerBoxTrans.position, sideTriggerBoxDimensions);
    }


    }
