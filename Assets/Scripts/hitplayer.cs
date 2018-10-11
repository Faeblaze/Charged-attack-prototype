using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private GameObject hitMarkerPrefab;

    [SerializeField]
    private Color[] markerCoulurs;

    [SerializeField]
    private float markerKillTime;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            HitNow();
        }
    }

    public void HitNow()
    {
        GameObject newMarker = Instantiate(hitMarkerPrefab, playerObject.transform.position, Quaternion.identity);
        newMarker.SetActive(true);
        newMarker.GetComponent<DamageMarkerController>().SetTextAndMove(Random.Range(0, 101).ToString(),
        markerCoulurs[Random.Range(0, markerCoulurs.Length)]);
        Destroy(newMarker.gameObject, markerKillTime);
    }
}
