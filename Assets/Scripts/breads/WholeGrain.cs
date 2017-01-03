using UnityEngine;
using System.Collections;
using System;

public class WholeGrain : Bread {
    public int getBaseDamage()
    {
        // return 15;
		return 10;
    }

    public string getName()
    {
        return "Vollkornbrot";
    }

}
