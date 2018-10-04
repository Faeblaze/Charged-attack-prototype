using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    Transform tr_Player;
    public float f_RotSpeed = 1.0f, f_MoveSpeed = 1.0f;
    public float range = 10F;

    private float damageCooldown;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Enemy_Class enemy;

    private bool inRange = false;

    //Use this for initialization
    void Start()
    {
        enemy = GetComponent<Enemy_Class>();
        tr_Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(tr_Player.transform.position, transform.position) > range)
        {
            animator.SetBool("Moving", false);
            return;
        }

        /* Look at Player*/
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(tr_Player.position.x, transform.position.y, tr_Player.position.z) - transform.position), f_RotSpeed * Time.deltaTime);

        if (inRange)
        {
            animator.SetBool("Moving", false);
            return;
        }

        animator.SetBool("Moving", true);

        /* Move at Player*/
        transform.position += transform.forward * f_MoveSpeed * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(range, 0, range));
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = true;

        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("SwingHeavy"))
            damageCooldown += Time.deltaTime;

        if (damageCooldown >= enemy.DamageSpeed)
        {
            damageCooldown = 0F;

            animator.SetTrigger("Swing");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = false;
    }
}
