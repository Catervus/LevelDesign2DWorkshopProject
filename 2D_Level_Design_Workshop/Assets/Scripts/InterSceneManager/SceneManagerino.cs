using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerino : MonoBehaviour
{
    private static SceneManagerino instance;
    public static SceneManagerino Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(GetActiveSceneIndex());
    }

    public void LoadScene(Scene _scene)
    {
        SceneManager.LoadScene(_scene.buildIndex);
    }
    public void LoadScene(int _sceneindex)
    {
        SceneManager.LoadScene(_sceneindex);
    }
    public void LoadScene(string _scenename)
    {
        SceneManager.LoadScene(_scenename);
    }
    public void LoadNextScene()
    {
        int nextsceneindex = GetActiveSceneIndex() + 1;
        if (nextsceneindex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("There is no next scene to load.");
            return;
        }
        SceneManager.LoadScene(GetActiveSceneIndex() + 1);
    }

    public int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
    }

}
