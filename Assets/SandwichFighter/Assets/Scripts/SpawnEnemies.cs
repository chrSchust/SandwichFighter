using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour {

    public GameObject enemy;

	// Use this for initialization
	void Start () {
        for(int i = 0; i<5; ++i)
        {
            Spawn();
        }
        InvokeRepeating("Spawn", 0, 10);
    }

    void Spawn()
    {
        /*foreach (Transform child in transform)
        {
            Instantiate(enemy, child.position, Quaternion.identity);
            Debug.Log(Random.Range(0, 3));
        }*/

        Instantiate(enemy, transform.GetChild(Random.Range(0, transform.childCount)).position, Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
