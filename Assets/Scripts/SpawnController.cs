using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour
{

    public GameObject enemyVegan;
	public GameObject enemyNormal;

    // Use this for initialization
    void Start()
    {
    }

    public void Spawn()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator spawn(Level activeLevel)
    {
        foreach (KeyValuePair<int, int> entry in activeLevel.enemyTypeAmount)
        {
            for (int i = 0; i < entry.Value; i++)
            {
				GameObject enemy = enemyNormal;
				if (entry.Key == Enemy.VEGAN) {
					enemy = enemyVegan;
				}
				GameObject enemyObject = Instantiate(enemy, transform.GetChild(UnityEngine.Random.Range(0, transform.childCount)).position, Quaternion.identity) as GameObject;
				enemyObject.GetComponent<EnemyController>().type = entry.Key;
                yield return new WaitForSeconds(activeLevel.spawnInterval);
            }
        }
    }
}