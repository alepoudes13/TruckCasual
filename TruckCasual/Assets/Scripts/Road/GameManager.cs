using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector]public bool gameOver = true;
    public int score = 0;
    int totalScore = 0;
    [SerializeField] GameObject startScreen;
    [SerializeField] Button startButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button shopButton;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI controlGuideText;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Score"))
            totalScore = PlayerPrefs.GetInt("Score");
        else totalScore = 0;
        gameOver = true;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        totalScoreText.text = "Total: " + totalScore;
        if(!gameOver)
            score++;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        score = 0;
        gameOver = false;
        ToggleMenuState(false);
    }

    public void ShowRestart()
    {
        totalScore += score;
        PlayerPrefs.SetInt("Score", totalScore);
        gameOver = true;
        restartButton.gameObject.SetActive(true);
    }

    void ToggleMenuState(bool mode)
    {
        startScreen.gameObject.SetActive(mode);
        startButton.gameObject.SetActive(mode);
        shopButton.gameObject.SetActive(mode);
        controlGuideText.gameObject.SetActive(mode);
    }

    public void RestartGame()
    {
        gameOver = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("Shop");
    }
}
