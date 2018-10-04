using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    // This will be inherited by outside scripts.
    public static UIManager instance;

    private Player player;
    private SpawnManager Sawning;

    public RectTransform healthBarBackground;
    public RectTransform healthBar;
    public RectTransform xpBarBackground;
    public RectTransform xpBar;

    public Text levelText;

    private Text healthText;
    private Text xpText;
    public Text counterText;
    public GameObject WinScreen;
    public GameObject LoseScreen;

    public Image super;
    public Image dash;
    public Image reflect;

    public bool requiresCursor = false;

	void Awake () {

        instance = this;

        player = (Player)FindObjectOfType(typeof(Player));

        healthText = healthBarBackground.GetComponentInChildren<Text>();
        xpText = xpBarBackground.GetComponentInChildren<Text>();

        LoseScreen.SetActive(false);
        WinScreen.SetActive(false);
        Time.timeScale = 1;
        requiresCursor = false;
    }
	
	// Update is called once per frame
	void Update () {
        Cursor.lockState = requiresCursor ? CursorLockMode.None : CursorLockMode.Locked;

        healthBar.sizeDelta = Vector2.Lerp(healthBar.sizeDelta, new Vector2(healthBarBackground.sizeDelta.x * player.health, healthBar.sizeDelta.y), Time.deltaTime * 20F);
        healthText.text = Mathf.RoundToInt(player.health * player.MaxHealth) + "/" + player.MaxHealth;

        xpBar.sizeDelta = Vector2.Lerp(xpBar.sizeDelta, new Vector2(xpBarBackground.sizeDelta.x * player.xp / player.xpPerLevel, xpBar.sizeDelta.y), Time.deltaTime * 20F);
        xpText.text = player.xp + "/" + player.xpPerLevel;

        levelText.text = (player.level + 1).ToString();

        counterText.text = "Enemies Killed: " + GameManager.instance.enemiesKilled;
        switch (GameManager.instance.difficulty)
        {
            case Difficulty.EASY:
                counterText.color = Color.green;
                break;
            case Difficulty.MEDIUM:
                counterText.color = Color.yellow;
                break;
            case Difficulty.HARD:
                counterText.color = Color.red;
                break;
            case Difficulty.HELL:
                counterText.color = Color.magenta;
                break;
            case Difficulty.GODMODE:
                counterText.color = Color.black;
                break;
        }

        if (GameManager.instance.conditionsMet >= GameManager.instance.conditionsCount)
        {
            requiresCursor = true;
            WinScreen.SetActive(true);  
            Time.timeScale = 0;
        }      
    }

    public void QuitGame()
    {
        Application.Quit();
    }



}
