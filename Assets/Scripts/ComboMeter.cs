using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboMeter : MonoBehaviour
{
    public static int comboValue = 0;
    Text comboScore;
	// Use this for initialization
	void Start () {
        Text comboScore;
	}
	
	// Update is called once per frame
	void Update () {
        comboScore.text = "comboScore:" + comboScore;
	}

}
