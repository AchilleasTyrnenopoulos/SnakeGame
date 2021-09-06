using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float Score { get { return score; } }
    [SerializeField]
    private float score;
    public event Action<int> ChangeScore;

    public event Action Death;

    public bool paused = false;

    [SerializeField]
    private TextMeshProUGUI scoreTxt;
    [SerializeField]
    private GameObject scoreTxtGO;

    [SerializeField]
    private GameObject pausedPanel;

    [SerializeField]
    private GameObject playAgainBtn;
    [SerializeField]
    private GameObject continueBtn;
    [SerializeField]
    private GameObject mainMenuBtn;

    [SerializeField]
    private TextMeshProUGUI deathScoreTxt;

    private void Awake()
    {
        //check if a game manager already exists
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //subscribe method to event
        ChangeScore += UpdateScore;
        Death += EnableDeathPanel;

        //DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        ChangeScore -= UpdateScore;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnChangeScore(int value)
    {
        ChangeScore.Invoke(value);
    }

    public void OnDeath()
    {
        Death.Invoke();
    }

    private void PauseGame()
    {
        paused = true;

        Time.timeScale = 0f;
        
        pausedPanel.SetActive(true);
        continueBtn.SetActive(true);
        mainMenuBtn.SetActive(true);
    }

    public void TogglePause()
    {
        if (paused)
            ContinueBtn();
        else
            PauseGame();
    }

    public void PlayAgainBtn()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void ContinueBtn()
    {
        pausedPanel.SetActive(false);
        continueBtn.SetActive(false);
        mainMenuBtn.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    private void UpdateScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreTxt.text = "Score " + score;
    }

    private void EnableDeathPanel()
    {
        pausedPanel.SetActive(true);
        playAgainBtn.SetActive(true);
        scoreTxtGO.SetActive(false);
        deathScoreTxt.text = "Score " + score;
    }
}
