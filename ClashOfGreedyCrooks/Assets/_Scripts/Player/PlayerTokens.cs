using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTokens : MonoBehaviour {

    private static PlayerTokens instance;
    public static PlayerTokens GetInstance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    [HideInInspector]
    public float playerSize = 2f;

    public bool wellFed = false;
    public bool drunk = false;
    public bool blindFolded = false;

    public System.Action WellFed;
    public System.Action Drunk;
    public System.Action BlindFolded;


    private void Update()
    {
        if (wellFed && WellFed !=null)
        {
            WellFed();
        }
        if (drunk && Drunk != null)
        {
            Drunk();
        }
        if (blindFolded && BlindFolded != null)
        {
            BlindFolded();
        }
    }
}
