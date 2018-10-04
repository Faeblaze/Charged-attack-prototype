using System;
using System.Linq;
using UnityEngine;

public class Enemy_Class : MonoBehaviour
{
    private Player player;
    public string enemyName;
    [NonSerialized]
    public float health = 1F;
    public float height;
    public float weight;
    public char sex;
    public float mySpeed;

    public int grantXp;

    public bool isVictoryCondition = false;
    public bool broadcastExistence = false;
    public bool uniformDifficulty = false;

    public EnemyStats[] enemyStats;

    public int MaxHealth
    {
        get
        {
            if (uniformDifficulty)
                return enemyStats[0].maxHealth;

            return enemyStats[(int)GameManager.instance.difficulty].maxHealth;
        }
    }

    public int Damage
    {
        get
        {
            if (uniformDifficulty)
                return enemyStats[0].damage;

            return enemyStats[(int)GameManager.instance.difficulty].damage;
        }
    }

    public float DamageSpeed
    {
        get
        {
            if (uniformDifficulty)
                return enemyStats[0].damageSpeed;

            return enemyStats[(int)GameManager.instance.difficulty].damageSpeed;
        }
    }

    private Enemy_AI AI;

    private void Start()
    {
        AI = GetComponent<Enemy_AI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (broadcastExistence)
            GameManager.instance.conditionsCount++;
    }

    // Gets called every frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            player.GrantXP(grantXp);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= (float)damage / MaxHealth;
        if(AI)
            AI.animator.SetTrigger("Hit");

        if (health < 0F)
        {
            health = 0F;

            if (isVictoryCondition)
                GameManager.instance.conditionsMet++;
            else
            {
                GameManager.instance.enemiesKilled++;
                if (GameManager.instance.enemiesKilled % GameManager.instance.difficultyIncreaseRate == 0)
                    GameManager.instance.CycleDifficulty();
            }
            //Debug.Log(UIManager.instance.enemiesKilled);
        }
    }

    /**
     * Gets list of difficulties
     * If the enemy stats array is null, create new array of size of number of difficulties
     * If the enemy stats array is larger or smaller than number of difficulties, resize
     * Go through each enemy stats instance and set the difficulty value
     */
    private void OnValidate()
    {
        if (uniformDifficulty)
        {
            if (enemyStats == null)
                enemyStats = new EnemyStats[1];

            if (enemyStats.Length != 1)
                Array.Resize(ref enemyStats, 1);

            if (enemyStats[0] == null)
                enemyStats[0] = new EnemyStats();
            enemyStats[0].difficulty = Difficulty.GODMODE;

            return;
        }

        Difficulty[] difficulties = (Difficulty[])Enum.GetValues(typeof(Difficulty));
        if (enemyStats == null)
            enemyStats = new EnemyStats[difficulties.Length];

        if (enemyStats.Length != difficulties.Length)
            Array.Resize(ref enemyStats, difficulties.Length);

        for(int i = 0; i < difficulties.Length; i++)
        {
            if (enemyStats[i] == null)
                enemyStats[i] = new EnemyStats();
            enemyStats[i].difficulty = difficulties[i];
        }
    }

    [Serializable]
    public class EnemyStats
    {
        public Difficulty difficulty;
        public int maxHealth;
        public int damage;
        public float damageSpeed;
    }
}
