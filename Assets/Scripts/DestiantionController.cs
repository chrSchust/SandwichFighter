using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DestiantionController : MonoBehaviour
{
    private RawImage image;

    // Use this for initialization
    void Start()
    {
        GameObject warningImageGo = GameObject.Find("WarningImage");
        image = warningImageGo.GetComponent<RawImage>();
        Color c = image.color;
        c.a = 0;
        image.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        float minDistance = 100;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float tempMinDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (tempMinDistance < minDistance)
            {
                minDistance = tempMinDistance;
            }
        }
        if (minDistance < 10)
        {
            Color c = image.color;
            float alpha = (1 - (minDistance / 10));
            c.a = alpha;
            image.color = c;
        }
        else
        {
            Color c = image.color;
            c.a = 0;
            image.color = c;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject);
            GameObject GameFlow = GameObject.Find("GameFlow");
            GameFlow.GetComponent<GameFlowController>().checkGameOver();
        }
    }
}
