using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition S; // A private Singleton

    [Header("Inscribed")]
    public TextMeshProUGUI uitLevel;  // The UIText_Level Text
    public TextMeshProUGUI uitShots;  // The UIText_Shots Text
    public Vector3 castlePos;  // The place to put castles
    public GameObject[] castles;  // An array of the castles
    public GameObject gameOverPanel;

    [Header("Dynamic")]
    public int level;  // The current level
    public int levelMax;  // The number of levels
    public int shotsTaken;
    public GameObject castle;  // The current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";  // FollowCam mode

    void Start() {
        S = this;  // Define the Singleton

        if (gameOverPanel != null){
            gameOverPanel.SetActive(false);
        }

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;

        StartLevel();
    }

    void StartLevel() {
        // Get rid of the old castle if one exists
        if (castle != null) {
            Destroy(castle);
        }

        // Destroy old projectiles if they exist (the method is not yet written)
        Projectile.DESTROY_PROJECTILES();  // This will be underlined in red

        // Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        // Reset the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI() {
        // Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update() {
        UpdateGUI();

        // Check for level end
        if (mode == GameMode.playing && Goal.goalMet) {
            // Change mode to stop checking for level end
            mode = GameMode.levelEnd;
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);

            // Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel() {
        level++;
        if (level == levelMax) {
            ShowGameOver();
        }else{
            StartLevel();

        }
        
    }

    // Static method that allows code anywhere to increment shotsTaken
    static public void SHOT_FIRED() {
        S.shotsTaken++;
    }

    // Static method that allows code anywhere to get a reference to S.castle
    static public GameObject GET_CASTLE() {
        return S.castle;
    }
    void ShowGameOver() {
        gameOverPanel.SetActive(true);  // Activate the Game Over panel
    }

    // Method to be called when the "Play Again" button is pressed
    public void PlayAgain() {
        gameOverPanel.SetActive(false);  // Hide the Game Over panel
        level = 0;  // Reset the level to the beginning
        shotsTaken = 0;  // Reset the shots taken
        Projectile.DESTROY_PROJECTILES();
        StartLevel();  // Restart the game
    }
}

