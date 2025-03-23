using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExpUI_Manager : MonoBehaviour
{
    public static ExpUI_Manager instance { get; private set; }

    public Slider waveExpUI;
    public Slider characterExpUI;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI keyText;

    [Header("DeathUI")]
    [SerializeField] private GameObject deathUI;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseUI;


    [Header("PowerUpCards")]
    public List<GameObject> powerUpCards;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        deathUI.SetActive(false);
        pauseUI.SetActive(false);
        foreach (var card in powerUpCards)
        {
            card.SetActive(false);
        }
    }

    public void ActivateDeathUI()
    {
        deathUI.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RetryButtonEvent()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void PauseButtonEvent()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeButtonEvent()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1;
    }
}