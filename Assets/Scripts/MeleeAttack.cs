﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MeleeAttack : MonoBehaviour
{
    public float distance;
    public float maxHitDistance = 1.0f;
    public float hitRadius = 1.0f;
    public List<Ingredient> ingredients1 { get; set; }
    public List<Ingredient> ingredients2 { get; set; }
    public List<Ingredient> activeIngredients { get; set; }
    private GameObject weapon;
    public Material material1;
    public Material material2;
    public Bread bread1;
    public int? breadHealth1;
    public Bread bread2;
    public int? breadHealth2;
    public Bread activebread;
	private GuiManager guiManager = null;

    private bool originalHealthSet;
    private int? originalHealth1;
    private int? originalHealth2;
    private GameObject progressBackground;
    private GameObject progressForeground;

    void Start()
    {
        weapon = GameObject.Find("Weapon");
		guiManager = GameObject.FindGameObjectWithTag("GuiManager").GetComponent<GuiManager>();
		if (guiManager == null)
		{
			Debug.LogError("GuiManager is null");
		}
        progressBackground = GameObject.Find("ProgressBackground");
        progressForeground = GameObject.Find("ProgressForeground");
    }

    // Update is called once per frame
    void Update()
    {

        if (breadHealth1 != null && breadHealth2 != null && !originalHealthSet)
        {
            originalHealth1 = breadHealth1;
            originalHealth2 = breadHealth2;
            originalHealthSet = true;
        }

        if (Input.GetKey(KeyCode.E) && (breadHealth1 <= 0 || breadHealth2 <= 0))
        {
            RaycastHit[] allHits;
            allHits = Physics.SphereCastAll(transform.position, hitRadius, transform.forward, maxHitDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            foreach (RaycastHit hit in allHits)
            {
                if (hit.transform.gameObject.name == "Counter")
                {
                    progressBackground.SetActive(true);
                    progressForeground.SetActive(true);
                    progressForeground.GetComponent<Image>().fillAmount = progressForeground.GetComponent<Image>().fillAmount + (Time.deltaTime * 0.4f);

                    if(progressForeground.GetComponent<Image>().fillAmount >= 1)
                    {
                        progressBackground.SetActive(false);
                        progressForeground.SetActive(false);
                        progressForeground.GetComponent<Image>().fillAmount = 0;

                        if (breadHealth1 <= 0)
                        {
                            breadHealth1 = originalHealth1;
                        }
                        if (breadHealth2 <= 0)
                        {
                            breadHealth2 = originalHealth2;
                        }
                        guiManager.SetBread1Hits(breadHealth1);
                        guiManager.SetBread2Hits(breadHealth2);
                    }
                    break;
                }
            }
        }
        else
        {
            progressBackground.SetActive(false);
            progressForeground.SetActive(false);
            progressForeground.GetComponent<Image>().fillAmount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && breadHealth1 > 0)
        {
            activeIngredients = ingredients1;
            activebread = bread1;
            weapon.GetComponentInChildren<Renderer>().material = material1;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && breadHealth2 > 0)
        {
            activeIngredients = ingredients2;
            activebread = bread2;
            weapon.GetComponentInChildren<Renderer>().material = material2;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (activeIngredients != null)
            {
                foreach (Ingredient i in activeIngredients)
                {
                    //Debug.Log(i.getName());
                }
            }

            if (breadHealth1 != null && breadHealth1 <= 0 && breadHealth2 != null && breadHealth2 <= 0)
            {
                Debug.Log("both dead");
                weapon.GetComponentInChildren<Renderer>().enabled = false;
                return;
            }
            else { weapon.GetComponentInChildren<Renderer>().enabled = true; }


            if (activebread != null && activebread == bread1)
            {
                breadHealth1--;
				guiManager.SetBread1Hits (breadHealth1);
                if (breadHealth1 <= 0)
                {
                    Debug.Log("bread 1 dead");
                    if (breadHealth2 > 0)
                    {
                        activeIngredients = ingredients2;
                        activebread = bread2;
                        weapon.GetComponentInChildren<Renderer>().material = material2;
                    }

                }
            }
            if (activebread != null && activebread == bread2)
            {
                breadHealth2--;
				guiManager.SetBread2Hits (breadHealth2);
                if (breadHealth2 <= 0)
                {
                    Debug.Log("bread 2 dead");
                    if (breadHealth1 > 0)
                    {
                        activeIngredients = ingredients1;
                        activebread = bread1;
                        weapon.GetComponentInChildren<Renderer>().material = material1;
                    }
                }
            }

            RaycastHit[] allHits;
            allHits = Physics.SphereCastAll(transform.position, hitRadius, transform.forward, maxHitDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            foreach (RaycastHit hit in allHits)
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<EnemyController>().HitByPlayer(activeIngredients, activebread);
                    break;
                }
            }
        }
    }
}
