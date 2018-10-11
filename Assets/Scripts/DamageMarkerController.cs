using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageMarkerController : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField]
    private float moveAmt = 0F;
    [SerializeField]
    private float movespeed = 0F;

    private Vector3[] moveDirs;
    private Vector3 myMoveDir;

    private bool canMove = false;

    private Player player;

    private void Awake()
    {

        myText = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        moveDirs = new Vector3[]
        {
            transform.up,
            (transform.up + transform.right),
            (transform.up + -transform.right)
        };
        myMoveDir = moveDirs[Random.Range(0, moveDirs.Length)];
    }

    private void Update()
    {
        if (canMove) transform.position = Vector3.MoveTowards(transform.position, transform.position + myMoveDir, moveAmt * (movespeed * Time.deltaTime));
        transform.LookAt(player.transform);
        transform.Rotate(Vector3.up * 180F);
    }

    public DamageMarkerController SetTextAndMove(string textStr, Color textColour)
    {
        myText.color = textColour;
        myText.text = textStr;
        canMove = true;

        return this;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }
}
