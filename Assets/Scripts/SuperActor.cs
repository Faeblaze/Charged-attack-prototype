using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperActor : MonoBehaviour {
    public Vector3 offScale = Vector3.zero;
    public Vector3 onScale = new Vector3(5F, 5F, 5F);

    public float activationTime = 1F;
    public int damageDealt = 10;

    private float progress = -1F;

    private void Awake()
    {
        transform.localScale = offScale;
    }

    private void Update()
    {
        if(progress == -2F)
        {
            progress = -1F;
            transform.localScale = Vector3.zero;

            Collider[] collided = Physics.OverlapSphere(transform.position, onScale.x / 2);
            foreach (Collider collider in collided)
            {
                if (!collider.CompareTag("Enemy"))
                    continue;

                Enemy_Class enemy = collider.GetComponent<Enemy_Class>();

                if (!enemy)
                    continue;

                enemy.TakeDamage(damageDealt);
            }
        }
        if (progress == -1F)
            return;

        progress += Time.deltaTime;
        transform.localScale = Vector3.Lerp(offScale, onScale, progress / activationTime);

        if (progress >= activationTime)
        {
            progress = -2F;
        }
    }

    public void Activate()
    {
        progress = 0F;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, onScale.x / 2);
    }
}
