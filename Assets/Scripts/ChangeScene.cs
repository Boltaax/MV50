using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private Fade fadeScript;
    private string targetScene;
    private bool isWaitingForFade = false;

    void Start()
    {
        fadeScript = FindObjectOfType<Fade>();
    }

    public void LoadScene(string sceneName)
    {
        if (fadeScript == null) return;

        fadeScript.fadeOut = true;
        targetScene = sceneName;
        isWaitingForFade = true;
    }

    void Update()
    {
        if (isWaitingForFade && fadeScript.transparence >= 1.0f)
        {
            isWaitingForFade = false;
            fadeScript.fadeOut=false;
            if (targetScene == "HouseHub") PersistantDataScript.instance.dayStarted = false;
            SceneManager.LoadScene(targetScene);
        }
    }
}
