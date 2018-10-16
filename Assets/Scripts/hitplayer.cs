using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    public GameObject hitMarkerPrefab;

    [SerializeField]
    private Color[] markerCoulurs;

    [SerializeField]
    private float markerKillTime;

    private Player player;

    private void Awake()
    {
        player = playerObject.GetComponent<Player>();
    }

    public void HitNow(int damage, Transform parent)
    {
        GameObject attachConstraint = new GameObject("Attach Constraint Object");
        attachConstraint.AddComponent<Attach>().SetFallbackPosition(parent.position).target = parent;

        GameObject newMarker = Instantiate(hitMarkerPrefab, attachConstraint.transform, false);
        newMarker.GetComponent<DamageMarkerController>().SetTextAndMove(damage.ToString(), markerCoulurs[Random.Range(0, markerCoulurs.Length)]).SetPlayer(player);
        Destroy(newMarker.gameObject, markerKillTime);
    }
}
