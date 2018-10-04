using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    private RectTransform healthBg;
    private RectTransform healthBar;

    private Enemy_Class enemy;

    private void Awake()
    {
        healthBg = GetComponent<RectTransform>();
        healthBar = healthBg.GetChild(0).GetComponent<RectTransform>();

        enemy = transform.parent.parent.GetComponentInChildren<Enemy_Class>();
    }

    void Update()
    {
        healthBar.sizeDelta = Vector2.Lerp(healthBar.sizeDelta, new Vector2(healthBg.sizeDelta.x * enemy.health, healthBar.sizeDelta.y), Time.deltaTime * 20F);
    }

}
