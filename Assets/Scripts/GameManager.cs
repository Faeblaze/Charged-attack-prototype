using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum Gamestate
{
    TITLE,
    INGAME,
    PAUSE,
    GAMEOVER
}
public enum Difficulty
{
    EASY,
    MEDIUM,
    HARD,
    HELL,
    GODMODE
}

public class GameManager : MonoBehaviour
{
    // This will be inherited by outside scripts.
    public static GameManager instance;

    public int score = 0;
    public int lives;
    public float timer = 120;

    public int difficultyIncreaseRate = 10;

    public int conditionsMet;
    public int conditionsCount;
    public int enemiesKilled = 0;

    public Difficulty difficulty;
    public Gamestate gameState;

    private void Awake()
    {
        instance = this;

        conditionsMet = 0;
        conditionsCount = 0;
    }

    // Use this for initialization
    void Start()
    {
        difficulty = Difficulty.EASY;
        gameState = Gamestate.TITLE;
    }

    void Update()
    {
        timer -= Time.deltaTime;                   
    }

    void LoadNewScene()
    {
        SceneManager.LoadScene("SceneTwo");
    }

    public void CycleDifficulty()
    {
        difficulty = (Difficulty)(Math.Min(Enum.GetValues(typeof(Difficulty)).Length - 1, (int)difficulty + 1));
    }
}
