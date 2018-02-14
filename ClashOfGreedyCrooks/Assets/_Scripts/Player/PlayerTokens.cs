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

    public System.Action WellFed;


    private void Update()
    {
        if (wellFed && WellFed !=null)
        {
            WellFed();
        }
    }

    //Inverted controllers
    public void Drunk()
    {

    }

    //Reduce accuracy
    public void Blindfolded()
    {

    }


}
