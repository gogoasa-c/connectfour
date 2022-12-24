using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIHandler : MonoBehaviour {

    [Header("Victory Screen")]
    public Image winBackdrop;
    public Image winRays;
    public Color originalBackdropColor;
    public Text winText;

    [Header("Buttons")]
    public List<Button> playButtons = new List<Button>();
    public List<Button> gameEndButtons = new List<Button>();
    public GameObject gameEndHolder;

    [Header("Turn Text")]
    public Text redTurn;
    public Text yellowTurn;
    public Color redText;
    public Color yelText;
    public Color offText;

    private void Awake() {
        winBackdrop.enabled = false;
        winRays.enabled = false;
        winText.enabled = false;

        gameEndHolder.SetActive(false);

        redText = redTurn.color;
        yelText = yellowTurn.color;
    }

    public void SwitchTo(int team) {
        if ( team == 1 ) {
            redTurn.color = redText;
            yellowTurn.color = offText;
        } else {
            redTurn.color = offText;
            yellowTurn.color = yelText;
        }
    }

    public void TriggerVictoryDelay(int winningTeam, float after) {
        string teamName = winningTeam == 1 ? "RED" : "YELLOW";

        winText.text = teamName + " HAS WON!";

        foreach ( Button b in playButtons ) {
            b.interactable = false;
        }
        
        gameEndHolder.SetActive(true);

        foreach (Button b in gameEndButtons) {
            b.image.color = new Color(1,1,1,0);
            Text bt = b.GetComponentInChildren<Text>();
            bt.color = new Color(1,1,1,0);
        }

        StartCoroutine(Wait(after, winningTeam));
    }

    public void TriggerVictory (int winningTeam) {
        winBackdrop.enabled = true;
        winRays.enabled = true;
        winText.enabled = true;

        StartCoroutine("FadeIn");

        SimpleSpin spinner = winRays.GetComponent<SimpleSpin>();
        spinner.spinning = true;
    }

    IEnumerator FadeIn() {
        float t = 0;

        Color initialBackdrop = new Color( originalBackdropColor.r, originalBackdropColor.g, originalBackdropColor.b, 0 );
        Color initial = new Color(1,1,1,0);

        while ( t < 1 ) {
            t += Time.deltaTime;
            winBackdrop.color = Color.Lerp(initialBackdrop, originalBackdropColor, t);
            winRays.color = Color.Lerp(initial, Color.white, t);
            winText.color = Color.Lerp(initial, Color.white, t);

            foreach(Button b in gameEndButtons) {
                b.image.color = Color.Lerp(initial, Color.white, t);
                Text bt = b.GetComponentInChildren<Text>();
                bt.color = Color.Lerp(initial, Color.white, t);
            }

            yield return null;
        }
    }

    IEnumerator Wait(float lim, int winningTeam) {
        float t = 0;
        while ( t < lim ) {
            t += Time.deltaTime;
            yield return null;
        }
        TriggerVictory(winningTeam);
    }

    public void NewGame() {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit() {
        Application.Quit();
    }

}
