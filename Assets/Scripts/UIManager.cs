using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public Transform indicatorsParent;
    public GameObject pausePanel;
    public GameObject loseScreen;
    public TextMeshProUGUI loseScreText;

    public static UIManager instance;
    public static Transform CanvasTransform {
        get {
            return instance.transform;
        }
    }

    public static Transform IndicatorsParent {
        get {
            return instance.indicatorsParent;
        }
    }

    public static GameObject PausePanel {
        get {
            return instance.pausePanel;
        }
    }

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
            bool active = !pausePanel.activeInHierarchy;
            pausePanel.SetActive(active);
            Time.timeScale = (active) ? 0 : 1;
        }
    }

    public void Resume() {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetLoseScreen(int score) {
        loseScreen.SetActive(true);
        loseScreText.text = $"Score: " + score;
    }

    public void PlayAgain() {
        SceneManager.LoadScene(1);
        AudioManager.FindSound("MainMenuSong").source.volume = 0.1f;
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(0);
        AudioManager.FindSound("MainMenuSong").source.volume = 0.5f;
    }
}
