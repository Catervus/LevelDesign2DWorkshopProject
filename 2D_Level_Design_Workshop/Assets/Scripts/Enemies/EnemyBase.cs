using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected bool isActive = true;
    protected Vector2 spawnPos;

    [SerializeField]
    protected float xRangeThreshold = 14;
    [SerializeField]
    protected float yRangeThreshold = 7;

    protected void UpdateSpawnPos()
    {
        spawnPos = transform.position;
    }

    protected bool CheckForPlayerInRange()
    {
        if (LevelManager.Instance.PlayerInstance == null)
            return false;

        if (CheckForRange(transform.position.x, LevelManager.Instance.PlayerInstance.gameObject.transform.position.x, 14))
            return true;

        return false;

    }

    protected bool CheckForPlayerInSpawnRange()
    {
        if (LevelManager.Instance.PlayerInstance == null)
            return false;

        //float diff = spawnPos.x - LevelManager.Instance.PlayerInstance.gameObject.transform.position.x;
        //if (Mathf.Abs(diff) <= 14)
        //{
        //    return true;
        //}

        if (CheckForRange(spawnPos.x, LevelManager.Instance.PlayerInstance.gameObject.transform.position.x, xRangeThreshold))
        {
            if (CheckForRange(spawnPos.y, LevelManager.Instance.PlayerInstance.gameObject.transform.position.y, yRangeThreshold))
                return true;
        }


        return false;

    }

    protected virtual void SideMovement(float _speed)
    {
        transform.position += -transform.right * _speed * Time.deltaTime;

    }

    protected bool CheckCollisionTriggerBox(Vector3 _triggerboxpos, Vector2 _triggerboxsize, LayerMask _layermask)
    {
        Collider2D check = Physics2D.OverlapBox(_triggerboxpos, _triggerboxsize, 0, _layermask);
        return check;
    }

    protected bool CheckForRange(float _pointcoord01, float _pointcoord02, float _disthreshold)
    {
        float diff = 0;
        diff = _pointcoord01 - _pointcoord02;
        if (Mathf.Abs(diff) <= _disthreshold)
        {
            return true;
        }

        return false;
    }
}
