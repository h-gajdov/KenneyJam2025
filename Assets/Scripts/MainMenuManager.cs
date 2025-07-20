using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public GameObject howToPlayPanel;
    public GameObject buttonsPanel;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) CloseAllPanels();
    }

    public void Play() {
        SceneManager.LoadScene(1);
        AudioManager.FindSound("MainMenuSong").source.volume = 0.1f;
    }

    public void HowToPlay() {
        buttonsPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void Back() {
        CloseAllPanels();
    }

    public void Quit() {
        Debug.Log("Quiting...");
        Application.Quit();
    }

    private void CloseAllPanels() {
        howToPlayPanel.SetActive(false);
        buttonsPanel.SetActive(true);
    }
}
