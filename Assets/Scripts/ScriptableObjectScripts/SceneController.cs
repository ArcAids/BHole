using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class SceneController : ScriptableObject
{
    [SerializeField] List<string> tips;

   // Queue<(int, LoadSceneMode, bool)> LevelLoadQueue;
    LevelLoader listener;

    [System.NonSerialized] protected bool inited=false;

    public virtual void Init()
    {
        if (inited)
            return;
        hideFlags = HideFlags.DontUnloadUnusedAsset;
        inited = true;
    }

    public string GetRandomTip()
    {
        if (tips != null && tips.Count > 0)
            return tips[Random.Range(0, tips.Count - 1)];
        else
            return string.Empty;
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex; 
    }

    public void Load(int sceneIndex, bool additive, bool showLoading=true)
    {
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings || (listener!=null && listener.isLoading && showLoading))
            return;

        LoadSceneMode sceneMode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;

        if (listener != null && showLoading)
        {
            listener.LoadLevel(sceneIndex, sceneMode,FadeOutDone);
        }
        else
            SceneManager.LoadSceneAsync(sceneIndex,sceneMode);
    }

    protected virtual void FadeOutDone()
    {
        Debug.Log("fadeOutDone");
    }

    public void Load(int sceneIndex)
    {
        Load(sceneIndex, false);
    }
    public void Load(string sceneIndex)
    {
        Load(sceneIndex, false);
    }
    public void Load(string sceneName, bool additive)
    {
        Load(SceneManager.GetSceneByName(sceneName).buildIndex, additive);
    }
    public void LoadPreviousScene()
    {
        int level = SceneManager.GetActiveScene().buildIndex - 1;
        if (level >= 0)
            Load(level);
        else
            Debug.Log("Invalid Scene");
    }

    public void LoadNextScene()
    {
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        if (level < SceneManager.sceneCountInBuildSettings)
            Load(level);
        else
            Debug.Log("Invalid Scene");
    }

    public void UnloadScene(int index)
    {
        Debug.Log("Attempting unload: " + index);
        if(SceneManager.GetSceneByBuildIndex(index).isLoaded)
            SceneManager.UnloadSceneAsync(index);
    }

    public void Register(LevelLoader passedEvent)
    {
        listener =passedEvent;
    }

    public void DeRegister(LevelLoader passedEvent)
    {
        if (listener == passedEvent)
            listener = null;
    }
}

public interface IOnLevelAdded
{
    void OnLevelAdded();
}
