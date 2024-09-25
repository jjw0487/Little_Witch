using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public float duration;
    public float maskScale;

    [SerializeField] private Transform tr_Mask;
    [SerializeField] private Transform tr_BG;

    private CancellationTokenSource source;

    private bool isLoaded = false;

    private AsyncOperation operation;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            LoadScene(-1);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadScene(1);
        }
    }
    private void OnDisable()
    {
        source.Cancel();
        source.Dispose();
    }

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        source = new CancellationTokenSource();
    }
    private void SceneLoaded(Scene scene, LoadSceneMode mode) => isLoaded = true;

    public void LoadScene(string SceneName)
    { // 빌드 이름으로 이동
        isLoaded = false;
        operation = SceneManager.LoadSceneAsync(SceneName);
        operation.allowSceneActivation = false;

        LoadSceneAsync(duration).Forget();
    }

    public void LoadScene(int moveTo)
    { // 빌드 인덱스로 이동
        int curSceneIdx = SceneManager.GetActiveScene().buildIndex;

        int targetScene = curSceneIdx += moveTo;

        if (targetScene < 0 || targetScene > 2) return;

        isLoaded = false;

        operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false;

        LoadSceneAsync(duration).Forget();
    }

    private async UniTaskVoid LoadSceneAsync(float duration)
    {
        if (UIManager.inst != null) { UIManager.inst.HideGUI(); }

        SoundManager.sInst.Play("SceneTo");

        tr_BG.gameObject.SetActive(true);
        tr_Mask.gameObject.SetActive(true);

        tr_Mask.transform.localScale = new Vector2(maskScale, maskScale);
        tr_Mask.transform.localPosition = Vector2.zero;
        tr_Mask.DOScale(0f, duration);

        await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: source.Token);

        operation.allowSceneActivation = true;

        await UniTask.WaitUntil(() => isLoaded, cancellationToken: source.Token);

        SoundManager.sInst.Play("SceneFrom");

        tr_Mask.transform.localPosition = Vector2.zero;
        tr_Mask.DOScale(maskScale, duration).OnComplete(() =>
        {
            tr_BG.gameObject.SetActive(false);
            tr_Mask.gameObject.SetActive(false);

            if(UIManager.inst != null) { UIManager.inst.ShowGUI(); }
        });

    }
}
