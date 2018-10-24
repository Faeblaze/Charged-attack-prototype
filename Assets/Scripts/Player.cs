using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public MeleeWeaponTrail trail;

    [NonSerialized]
    public float health = 1F;
    public int baseHealth = 100;
    public int baseDamage = 10;

    public Gradient colorShiftTier1;
    public Gradient colorShiftTier2;

    public ParticleSystem swordParticles;

    public float comboCooloff = 5F;

    public int MaxHealth
    {
        get
        {
            return baseHealth + baseHealth / 100 * 150 * level;
        }
    }

    public int Damage
    {
        get
        {
            float dmg = baseDamage + baseDamage / 100F * 200F * level;

            for(int i = 0; i < Mathf.FloorToInt(comboCount / 10F); i++)
            {
                dmg += dmg / 100F * 25F;
            }

            return Mathf.RoundToInt(dmg);
        }
    }

    public float blinkDistance = 10F;

    public int xp;
    public float xpPerLevel = 100;
    public int level;

    float cooldown = 10;
    float dashcd = 10;
    float barriorcd = 10;
    bool supercd = true;
    bool dash = true;
    bool reflect = true;

    private SuperActor superActor;
    private BarrierActor barrierActor;

    [NonSerialized]
    public Animator animator;//You may not need an animator, but if so declare it here 

    int noOfClicks; //Determines Which Animation Will Play
    bool canClick; //Locks ability to click during animation event

    private float holdTime = 0F;
    [Header("Hold Tiers")]
    public float holdTimeTier1 = 5F;
    public float holdTimeTier2 = 10F;

    private int comboCount = 0;
    private float comboTime = 0F;

    void Start()
    {
        superActor = GetComponentInChildren<SuperActor>();
        barrierActor = GetComponentInChildren<BarrierActor>();
        //barrierActor = GetComponentInChildren<BarrierActor>();

        //Initialize appropriate components
        animator = GetComponent<Animator>();

        noOfClicks = 0;
        canClick = true;

        UIManager.instance.super.fillAmount = 0;
        cooldown = 0;
        UIManager.instance.dash.fillAmount = 0;
        dashcd = 0;
        UIManager.instance.reflect.fillAmount = 0;
        barriorcd = 0;
    }

    void Update()
    {
        trail.Emit = true;
        Color c = trail._colors[0];

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("swing") || animator.GetCurrentAnimatorStateInfo(1).IsName("1HAttack"))
        {
            c = Color.blue;
            c.a = .2F;
            baseDamage = 10;
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("2HAttackCharge"))
        {
            if (!swordParticles.isPlaying)
                swordParticles.Play();

            swordParticles.startColor = c;

            if (holdTime <= holdTimeTier1)
                c = colorShiftTier1.Evaluate(holdTime / holdTimeTier1);
            else
                c = colorShiftTier2.Evaluate((holdTime - holdTimeTier1) / (holdTimeTier2 - holdTimeTier1));
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("2HAttack"))
        {
            if (swordParticles.isPlaying)
                swordParticles.Stop();

            baseDamage = Mathf.RoundToInt(40 * Mathf.Clamp(holdTime, 1, holdTimeTier1));
            if (holdTime > holdTimeTier1)
                baseDamage += Mathf.RoundToInt(100 * Mathf.Clamp(holdTime - holdTimeTier1, 0F, holdTimeTier2 - holdTimeTier1));
        }
        else
        {
            trail.Emit = false;
        }
        //TRAIL COLOUR code
        trail._colors[0] = c;

        animator.SetBool("AttackHold", Input.GetMouseButton(0));

        if (animator.GetBool("AttackHold") && animator.GetCurrentAnimatorStateInfo(1).IsName("2HAttackCharge"))
        {
            holdTime = Mathf.Clamp(Time.deltaTime + holdTime, 0F, holdTimeTier2);
            Debug.Log(holdTime);
        }
        else if(animator.GetCurrentAnimatorStateInfo(1).IsName("swing"))
        {
            holdTime = 0F;
        }

        if (health <= 0)
        {
            UIManager.instance.LoseScreen.SetActive(true);
            UIManager.instance.requiresCursor = true;
            Time.timeScale = 0;
        }
        // SceneManager.LoadScene("Wk10");      
        if (Input.GetMouseButtonDown(0)) { ComboStarter(); }
        //flat dmg button 1
        if (Input.GetKeyDown(KeyCode.Alpha1) && supercd)
        {
            SuperAttack();
            supercd = false;
            cooldown = 10;
        }
        if (supercd == false)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                supercd = true;
                UIManager.instance.super.fillAmount = 1;
            }
        }

        UIManager.instance.super.fillAmount = cooldown / 10;
        //move fast blink button 2
        if (Input.GetKeyDown(KeyCode.Alpha2) && dash)
        {
            Evade();
            dash = false;
            dashcd = 10;
        }
        if (dash == false)
        {
            dashcd -= Time.deltaTime;
            if (dashcd <= 0)
            {
                dash = true;
                UIManager.instance.dash.fillAmount = 1;
            }
        }

        UIManager.instance.dash.fillAmount = dashcd / 10;
        //shield button 3
        if (Input.GetKeyDown(KeyCode.Alpha3) && reflect)
        {
            DestructiveBarrier();
            reflect = false;
            barriorcd = 20;
        }
        if (reflect == false)
        {
            barriorcd -= Time.deltaTime;
            if (barriorcd <= 0)
            {
                reflect = true;
                UIManager.instance.reflect.fillAmount = 1;
            }
        }

        UIManager.instance.reflect.fillAmount = barriorcd / 20;

        if(comboTime > 0F)
            comboTime -= Time.deltaTime;

        if (comboTime <= 0F)
            comboCount = 0;
        UIManager.instance.ComboValue = comboCount;
    }

    // RESTART GAME ON BUTTON CLICK IN THE GAME.
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    void ComboStarter()
    {
        if (canClick)
        {
            noOfClicks++;
        }

        if (noOfClicks == 1)
        {
            animator.SetTrigger("Attack");
        }
    }
    //combo code
    public void ComboCheck()
    {
        canClick = false;

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("swing") && noOfClicks == 1)
        {//If the first animation is still playing and only 1 click has happened, return to idle
            animator.SetTrigger("Attack");
            canClick = true;
            noOfClicks = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("swing") && noOfClicks >= 2)
        {//If the first animation is still playing and at least 2 clicks have happened, continue the combo          
            animator.SetTrigger("Attack");
            canClick = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("1HAttack") && noOfClicks == 2)
        {  //If the second animation is still playing and only 2 clicks have happened, return to idle         
            animator.SetTrigger("Attack");
            canClick = true;
            noOfClicks = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("1HAttack") && noOfClicks >= 3)
        {  //If the second animation is still playing and at least 3 clicks have happened, continue the combo         
            animator.SetTrigger("Attack");
            canClick = true;
        }
        else //if (animator.GetCurrentAnimatorStateInfo(1).IsName("2HAttack"))
        { //Since this is the third and last animation, return to idle          
            canClick = false;
            noOfClicks = 0;
        }


    }
    //alpha 1
    void SuperAttack()
    {
        superActor.Activate();
    }
    //alpha2
    void Evade()
    {
        if (Physics.Raycast(transform.position, transform.forward, blinkDistance, 1 << 2))
            transform.Translate(Vector3.forward * blinkDistance);
    }
    //alpha3
    void DestructiveBarrier()
    {
        barrierActor.Activate();
    }


    public void TakeDamage(int damage)
    {
        if (barrierActor.isActive)
            return;
        health -= (float)damage / MaxHealth;
        if (health < 0F)
            health = 0F;
    }
    #region EXP
    public void GrantXP(int xp)
    {
        this.xp += xp;
        while (this.xp >= Mathf.FloorToInt(xpPerLevel))
        {
            this.xp -= Mathf.FloorToInt(xpPerLevel);
            level++;
            xpPerLevel = xpPerLevel / 100 * 120;
            health = 1F;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * blinkDistance);
    }

    internal void OnDealtDamage()
    {
        comboCount++;
        comboCount = Mathf.Min(comboCount, 200);
        comboTime = comboCooloff;
    }
}
