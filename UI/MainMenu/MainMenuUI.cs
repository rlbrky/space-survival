using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject settingsButton;

    private void Start()
    {
        settingsMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        playButton.SetActive(false);
        settingsButton.SetActive(false);

        settingsMenu.SetActive(true);
    }

    public void OpenGameScene()
    {
        StartCoroutine(PlayGame());
    }

    public void ReturnToMainMenu()
    {
        playButton.SetActive(true);
        settingsButton.SetActive(true);

        settingsMenu.SetActive(false);
    }

    IEnumerator PlayGame()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("GameScene");
    }
}
