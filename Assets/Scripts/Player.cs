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
            return baseDamage + baseDamage / 100 * 200 * level;
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
    public float holdTimeMax = 5F;

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

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("swing") || animator.GetCurrentAnimatorStateInfo(0).IsName("1HAttack"))
        {
            c = Color.blue;
            c.a = .2F;
            baseDamage = 10;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttackCharge"))
        {
            c = Color.red;
            c.a = .3F;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttack"))
        {
            c = Color.red;
            c.a = Mathf.Clamp01(.3F + 1F * (holdTime / holdTimeMax));
            baseDamage = 40 * Mathf.Max(1, Mathf.CeilToInt(holdTime));
        }
        else
        {
            trail.Emit = false;
        }
        //TRAIL COLOUR code
        trail._colors[0] = c;

        animator.SetBool("AttackHold", Input.GetMouseButton(0));

        if (animator.GetBool("AttackHold") && animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttackCharge"))
        {
            holdTime = Mathf.Clamp(Time.deltaTime + holdTime, 0F, holdTimeMax);
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("swing"))
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

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("swing") && noOfClicks == 1)
        {//If the first animation is still playing and only 1 click has happened, return to idle
            animator.SetTrigger("Attack");
            canClick = true;
            noOfClicks = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("swing") && noOfClicks >= 2)
        {//If the first animation is still playing and at least 2 clicks have happened, continue the combo          
            animator.SetTrigger("Attack");
            canClick = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("1HAttack") && noOfClicks == 2)
        {  //If the second animation is still playing and only 2 clicks have happened, return to idle         
            animator.SetTrigger("Attack");
            canClick = true;
            noOfClicks = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("1HAttack") && noOfClicks >= 3)
        {  //If the second animation is still playing and at least 3 clicks have happened, continue the combo         
            animator.SetTrigger("Attack");
            canClick = true;
        }
        else //if (animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttack"))
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
//EXP
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * blinkDistance);
    }
}
