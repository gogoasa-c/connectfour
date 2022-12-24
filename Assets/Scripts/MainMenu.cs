using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    
    public GameObject rulesHolder;

    public void NewGame() {
        SceneManager.LoadScene("Game");
    }

    public void Quit() {
        Application.Quit();
    }

    public void ShowRules() {
        rulesHolder.SetActive(true);
    }

    public void HideRules() {
        rulesHolder.SetActive(false);
    }
}
