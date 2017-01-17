using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour
{

    public GameObject enemyVegan;
	public GameObject enemyNormal;
    public GameObject enemyFat;

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

        while (activeLevel.enemyTypeAmount.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, activeLevel.enemyTypeAmount.Count);
            KeyValuePair<int, int> entry = activeLevel.enemyTypeAmount[index];

            GameObject enemy = enemyNormal;
            if (entry.Key == Enemy.VEGAN)
            {
                enemy = enemyVegan;
            }
            if (entry.Key == Enemy.FAT)
            {
                enemy = enemyFat;
            }
            GameObject enemyObject = Instantiate(enemy, transform.GetChild(UnityEngine.Random.Range(0, transform.childCount)).position, Quaternion.identity) as GameObject;
            enemyObject.GetComponent<EnemyController>().type = entry.Key;

            activeLevel.enemyTypeAmount[index] = new KeyValuePair<int, int>(entry.Key, entry.Value-1);
            if(activeLevel.enemyTypeAmount[index].Value <= 0)
            {
                activeLevel.enemyTypeAmount.RemoveAt(index);
            }
            yield return new WaitForSeconds(activeLevel.spawnInterval);
        }

        /*foreach (KeyValuePair<int, int> entry in activeLevel.enemyTypeAmount)
        {
            for (int i = 0; i < entry.Value; i++)
            {
				GameObject enemy = enemyNormal;
				if (entry.Key == Enemy.VEGAN) {
					enemy = enemyVegan;
				}
                if (entry.Key == Enemy.FAT)
                {
                    enemy = enemyFat;
                }
                GameObject enemyObject = Instantiate(enemy, transform.GetChild(UnityEngine.Random.Range(0, transform.childCount)).position, Quaternion.identity) as GameObject;
				enemyObject.GetComponent<EnemyController>().type = entry.Key;
                yield return new WaitForSeconds(activeLevel.spawnInterval);
            }
        }*/
    }
}