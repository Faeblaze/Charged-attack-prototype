using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour {

    private Player player;
    private float hitCooldown;
    private hitplayer hitplayer;

    // Use this for initialization
    void Start ()
    {
        player = GetComponentInParent<Player>();
        hitplayer = player.GetComponent<hitplayer>();
    }

    // Update is called once per frame
    void Update()
    {
        hitCooldown = Mathf.Max(0, hitCooldown - Time.deltaTime);
    }


    //ENENY takes dmg... colider

    void OnTriggerEnter (Collider other)
    {
        if (!player.animator.GetCurrentAnimatorStateInfo(0).IsName("swing") && !player.animator.GetCurrentAnimatorStateInfo(0).IsName("1HAttack") && !player.animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttack") || hitCooldown > 0F)
            return;

        if (other.CompareTag("Enemy"))
        {
            Enemy_Class enemy = other.GetComponentInChildren<Enemy_Class>();

            hitCooldown = .5F;

            hitplayer.HitNow(player.Damage, enemy.transform);
            enemy.TakeDamage(player.Damage);
            //enemy.Health -= Player Damage
        }


    }
}
