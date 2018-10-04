using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour {

    private Player player;
    private float hitCooldown;

    // Use this for initialization
    void Start ()
    {
        player = (Player)Object.FindObjectOfType(typeof(Player));
	}

    // Update is called once per frame
    void Update()
    {
        hitCooldown = Mathf.Max(0, hitCooldown - Time.deltaTime);
    }

    void OnTriggerEnter (Collider other)
    {
        if (!player.animator.GetCurrentAnimatorStateInfo(0).IsName("swing") && !player.animator.GetCurrentAnimatorStateInfo(0).IsName("1HAttack") && !player.animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttack") || hitCooldown > 0F)
            return;

        if (other.CompareTag("Enemy"))
        {
            Enemy_Class enemy = other.GetComponentInChildren<Enemy_Class>();

            hitCooldown = .5F;

            Debug.Log("Herpty derp I have collided");

            enemy.TakeDamage(player.Damage);
            //enemy.Health -= Player Damage
        }
    }
}
