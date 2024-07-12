using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Vector3 spawnPoint;
    [SerializeField]
    int coinScore = 0;

    // ~ Coin List ~
    private List<TriggerObject> coinList = new List<TriggerObject>();

    [SerializeField]
    TMP_Text coinScoreText;
    [SerializeField]
    TMP_Text levelDoneCoinScoreText;

    private static LevelManager instance;
    public static LevelManager Instance { get => instance; }

    public static Vector3 playerSpawnPos;
    public static bool checkPointHit;

    public GameObject PlayerInstance { get; set; }

    private static Vector2 playerCheckpointPos = Vector2.zero;

    public Vector2 GetPlayerSpawnPos { get => playerCheckpointPos;}

    private static int currentLevel = 0;

    private void Awake()
    {
        // if(instance == null)
        // {
        //     instance = this;
        // }
        // else
        // {
        //     Destroy(this.gameObject);
        // }


        instance = this;
        InitLevel();
    }

    public void TurnOffPlayer()
    {
        if (PlayerInstance == null)
            return;

        PlayerInstance.SetActive(false);
    }

    public void SubscribeTriggerObject(TriggerObject _to)
    {
        if(_to.GetTriggerObjType == E_TriggerObjectType.Coin)
        {
            if (!coinList.Contains(_to))
            {
                coinList.Add(_to);
            }
            UpdateUICoinScore();
        }
        
    }
    public void InitLevel()
    {
        coinList = new List<TriggerObject>();
    }

    public void UpdatePlayerSpawnPos()
    {
        playerCheckpointPos = new Vector2((int)PlayerInstance.transform.position.x + 0.5f, (int)PlayerInstance.transform.position.y + 0.5f);
    }

    public void ResetLevel()
    {
        playerCheckpointPos = Vector2.zero;
        playerSpawnPos = Vector3.zero;
        checkPointHit = false;
    }

    public void LevelDone()
    {
        playerCheckpointPos = Vector2.zero;
        Debug.Log("Level Done!");
        UpdateLevelDoneUICoinScore();
    }

    public void CollectCoin()
    {
        coinScore++;
        UpdateUICoinScore();
    }

    private void UpdateUICoinScore()
    {
        if (coinScoreText == null)
            return;

        coinScoreText.text = coinScore + " / " + coinList.Count;
    }

    private void UpdateLevelDoneUICoinScore()
    {
        if (levelDoneCoinScoreText == null)
            return;

        levelDoneCoinScoreText.text = coinScore + " / " + coinList.Count;
    }


    private void LoadNextLevel()

    {
    }
}
