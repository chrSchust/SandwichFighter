﻿using UnityEngine;
using System.Collections;
using System;

public class White : Bread {
    public int getBaseDamage()
    {
        // return 10;
		return 15;
    }

    public string getName()
    {
        return "Weißbrot";
    }
}
