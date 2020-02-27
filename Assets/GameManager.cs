using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] SceneController loader;
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject backToMainMenuUI;

    bool isloading=false;
    public bool GGunUnlocked { get; set; }
    public bool WGunUnlocked { get; set; }
    public bool PGunUnlocked { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void GameOver()
    {
        GameOverUI.SetActive(true);
        StartCoroutine(loadinaSec(1));
    }

    public void GameOverFromWormHole()
    {
        StartCoroutine(loadinaSec(2));
    }

    public void Escaped()
    {
        StartCoroutine(loadinaSec(1));
    }

    public void Win()
    {
        StartCoroutine(loadinaSec(0));
    }

    IEnumerator loadinaSec(int index)
    {
        isloading = true;
        yield return new WaitForSeconds(5);
        GameOverUI.SetActive(false);
        isloading = false;
        loader.Load(index);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isloading)
        {
            loader.Load(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isloading)
        {
            loader.Load(0);
        }
    }
}
