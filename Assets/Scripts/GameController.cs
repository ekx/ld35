using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    public CanvasGroup Title;
    public Text Score;
    public Animator FlashEffect;
    public CanvasGroup InstructionsScreen;
    public GameObject GameOverScreen;
    public AudioClip GameOverClip;
    public AudioClip RestartClip;
    public AudioSource LoopSource;

    public float TitleFadeOutTime = 5f;
    public float InstructionsFadeOutTime = 10f;
    public float GameOverFlashSpeed = 0.5f;

    [HideInInspector]
    public float TimeScale { get; set; }

    public void Start()
    {
        TimeScale = 1f;
    }

    public void Update()
    {
        // Title Fade Out
        if (Time.time < TitleFadeOutTime)
        {
            Title.alpha = Mathf.Lerp(1f, 0f, Time.time / TitleFadeOutTime);
        }

        // Instructions Fade Out
        if (Time.time < InstructionsFadeOutTime)
        {
            InstructionsScreen.alpha = Mathf.Lerp(1f, 0f, Time.time / InstructionsFadeOutTime);
        }

        // Score
        if (!gameOver)
        {
            score = (int)(ObstacleController.Instance.DifficultyTime * 10);
            Score.text = string.Format("{0} m", score);
        }
    }

    public void OnGameOver()
    {
        gameOver = true;
        FlashEffect.SetTrigger("Flash");

        LoopSource.Stop();
        AudioSource.PlayClipAtPoint(GameOverClip, Camera.main.transform.position);
    }

    public void OnScreenWhite()
    {
        TimeScale = 0f;
        ObstacleController.Instance.DestroyObstacles();
        GameOverScreen.SetActive(true);
    }

    public void OnRestart()
    {
        score = 0;
        gameOver = false;
        TimeScale = 1f;
        
        GameOverScreen.SetActive(false);
        ObstacleController.Instance.ResetDifficulty();
        AudioSource.PlayClipAtPoint(RestartClip, Camera.main.transform.position);
        LoopSource.PlayDelayed(0.1f);
    }

    private int score = 0;
    private bool gameOver = false;
}