using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject[] Enemies;
    public Vector3 spawnValues;
    public float spawnWait;
    public float spawnMostWait;
    public float spawnLeastWait;
    public int startWait;
    public bool stop;

    private Enemy_Class portalInst;

    int randEnemy;
        
	// Use this for initialization
	void Start ()
    {
        portalInst = GetComponentInChildren<Enemy_Class>();
        StartCoroutine(waitSpawner());		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!portalInst)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
        spawnWait = Random.Range(spawnLeastWait, spawnMostWait);		
	}

    IEnumerator waitSpawner ()
    {
        yield return new WaitForSeconds(startWait);
        
        while (!stop)
        {
            randEnemy = Random.Range(0, Enemies.Length);
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));
            Instantiate(Enemies[randEnemy], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            yield return new WaitForSeconds(spawnWait);
        }
    }

    // adds the visual box around the area in which enemies can spawn
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawWireCube(transform.position - new Vector3(0, transform.position.y - 1, 0), spawnValues - new Vector3(0, spawnValues.y, 0));
    }
}
