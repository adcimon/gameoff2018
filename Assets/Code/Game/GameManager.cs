using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Players")]
    public List<GameObject> players = new List<GameObject>();

    [Header("Start Game")]
    public KeyCode startKey = KeyCode.Space;
    public GameObject startPanel;
    public float fadeDuration = 1;

    [Header("End Game")]
    public string endingMessage = " survived.";
    public GameObject endPanel;
    public Text endText;
    public CameraTransitionEffect cameraTransitionEffect;
    public float cameraTransitionDuration = 3;

    private bool gameStarted = false;
    private List<GameObject> deadPlayers = new List<GameObject>();

    private void Awake()
    {
        // Enable the start panel.
        startPanel.SetActive(true);

        // Set the camera transition effect cutoff to maximum to turn the screen black.
        cameraTransitionEffect.cutoff = 1;
    }

    private void Update()
    {
        if( gameStarted )
        {
            return;
        }

        if( Input.GetKeyDown(startKey) )
        {
            StartGame();
        }
    }

    public void OnDeath( GameObject player )
    {
        if( players.Contains(player) )
        {
            deadPlayers.Add(player);
            players.Remove(player);
        }

        if( players.Count == 1 )
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        gameStarted = true;

        // Fade the start panel
        startPanel.GetComponent<FadeCanvasGroup>().FadeOut(fadeDuration);

        // Play camera transition effect.
        cameraTransitionEffect.Play(cameraTransitionDuration);

        // Enable player actions.
        for ( int i = 0; i < players.Count; i++ )
        {
            GameObject player = players[i];
            player.GetComponent<MovementController>().enabled = true;
            player.GetComponent<ShootController>().enabled = true;
            player.GetComponent<BombController>().enabled = true;
        }
    }

    public void EndGame()
    {
        GameObject winner = players[0];

        // Disable player actions.
        for( int i = 0; i < players.Count; i++ )
        {
            GameObject player = players[i];
            player.GetComponent<MovementController>().enabled = false;
            player.GetComponent<ShootController>().enabled = false;
            player.GetComponent<BombController>().enabled = false;
        }

        // Enable ending panel.
        endPanel.SetActive(true);

        // Set ending text.
        endText.text = winner.name + endingMessage;

        // Play camera transition effect.
        cameraTransitionEffect.Play(cameraTransitionDuration);
    }
}