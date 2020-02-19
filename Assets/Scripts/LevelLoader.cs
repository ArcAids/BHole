using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    SceneController data;
    [SerializeField]
    Image loadingBar;
    [SerializeField]
    TMP_Text loadingProgressText;
    [SerializeField]
    TMP_Text loadingHintText;
    [Space]
    [Range(0, 1)]
    [SerializeField]
    float actualLoadingEndsAt = 0.5f;
    [SerializeField]
    float delayAfterActualLoading = 1f;
    [SerializeField]
    int iterationsForLoadingSimulation = 3;

    public bool isLoading=false;
    Animator loadingAnimator;
    int randomNumber;
    static LevelLoader Instance;
    AsyncOperation loading;

    public delegate void OnFadeOutDone();
    OnFadeOutDone onFadeOutDone;

    readonly int fadeInHash = Animator.StringToHash("FadeIn");
    readonly int fadeOutHash = Animator.StringToHash("FadeOut");

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        loadingAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        data.Register(this);
    }
    private void OnDisable()
    {
        data.DeRegister(this);
    }

    public void LoadLevel(int levelNumber, LoadSceneMode sceneMode, OnFadeOutDone onFadeOutDone=null)
    {
        if (isLoading)
            return;
        this.onFadeOutDone = onFadeOutDone;
        StartCoroutine(Loading(levelNumber,sceneMode));
    }

    public void LoadLevel(int levelNumber)
    {
        LoadLevel(levelNumber,LoadSceneMode.Single, null);
    }

    private void StartLoading()
    {
        SetLoadingProgress(0);
        loadingHintText.text = data.GetRandomTip();
        isLoading = true;
        if (loadingAnimator != null)
            loadingAnimator.SetTrigger(fadeInHash);
    }

    IEnumerator Loading(int sceneIndex, LoadSceneMode sceneMode)
    {
        StartLoading();
        yield return new WaitForSecondsRealtime(1f);
        onFadeOutDone?.Invoke();
        loading = SceneManager.LoadSceneAsync(sceneIndex, sceneMode);
        Debug.Log("Loading screen:"+sceneIndex+" Mode:"+sceneMode);
        while (loading.progress < 1)
        {   
            SetLoadingProgress(loading.progress * actualLoadingEndsAt);
            yield return null;
        }

        SetLoadingProgress(actualLoadingEndsAt);
        randomNumber = (int)(actualLoadingEndsAt * 100);

        float delay = delayAfterActualLoading / iterationsForLoadingSimulation;

        for (int i = 0; i < iterationsForLoadingSimulation; i++)
        {
            randomNumber = Random.Range(randomNumber, 100);
            SetLoadingProgress(randomNumber / 100f);
            yield return new WaitForSecondsRealtime(delay);
        }
        SetLoadingProgress(1);
        isLoading = false;
        if (loadingAnimator != null)
            loadingAnimator.SetTrigger(fadeOutHash);
    }

    void SetLoadingProgress(float progress)
    {
        progress = (float)decimal.Round((decimal)progress, 2);
        loadingBar.fillAmount = progress;
        loadingProgressText.text = "Loading " + progress * 100 + "%";
    }

}
