using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTrans;
    private Vector3 targetPos;

    [SerializeField]
    private bool followOnXAxis = true;
    [SerializeField]
    private bool followOnYAxis = true;

    [SerializeField]
    private Vector2 offset = Vector2.zero;

    private Vector3 startPos;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if(playerTrans == null)
        {
            playerTrans = FindObjectOfType<PlayerController>().transform;
            transform.position=  new Vector3(playerTrans.position.x, playerTrans.position.y, transform.position.z) + (Vector3)offset;
            startPos = transform.position;
        }

        if (followOnXAxis && followOnYAxis)
        {
            targetPos = new Vector3(playerTrans.position.x, playerTrans.position.y, transform.position.z);
        }
        else if(followOnXAxis)
        {
            targetPos = new Vector3(playerTrans.position.x, startPos.y, startPos.z);
        }
        else if(followOnYAxis)
        {
            targetPos = new Vector3(startPos.x, playerTrans.position.y, startPos.z);
        }

        transform.position = targetPos + (Vector3)offset;
    }
}
