using UnityEngine;
using System.Collections;

public class DestiantionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" )
        {
            Destroy(col.gameObject);
            GameObject GameFlow = GameObject.Find("GameFlow");
            GameFlow.GetComponent<GameFlowController>().checkGameOver();
        }
    }
}
