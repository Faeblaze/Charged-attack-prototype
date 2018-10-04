using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour {
    public Enemy_AI AI;

    private float hitCooldown;

    // Update is called once per frame
    void Update () {
        hitCooldown = Mathf.Max(0, hitCooldown - Time.deltaTime);
    }

    void OnTriggerEnter (Collider other)
    {
        if (!AI.animator.GetCurrentAnimatorStateInfo(0).IsName("SwingHeavy") || hitCooldown > 0)
            return;

        if (other.CompareTag("Player"))
        {
            hitCooldown = AI.enemy.DamageSpeed / 4F;

            Player player = other.GetComponentInChildren<Player>();

            Debug.Log("Herpty derp I have collided");

            player.TakeDamage(AI.enemy.Damage);
        }
    }
}
