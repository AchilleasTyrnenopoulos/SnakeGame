using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float Score { get { return score; } }
    [SerializeField]
    private float score;
    public event Action<int> ChangeScore; 

    [SerializeField]
    private TextMeshProUGUI scoreTxt;

    private void Awake()
    {
        //check if a game manager already exists
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        
        //subscribe method to event
        ChangeScore += UpdateScore;;

        DontDestroyOnLoad(this.gameObject);
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

    private void UpdateScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreTxt.text = "Score: " + score;
    }
}
