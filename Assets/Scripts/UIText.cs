using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{

    public Text VillageCountText, HitCounText;
    private int VillagesDestroyed;
    private int HitsReceived;
    private bool newValues;

	// Use this for initialization
	void Start ()
	{
	    VillagesDestroyed = 0;
	    HitsReceived = 0;
	    newValues = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (newValues)
	    {
	        VillageCountText.text = "" + VillagesDestroyed;
	        HitCounText.text = "" + HitsReceived;
	        newValues = false;
	    }
	}

    public void addVillage()
    {
        VillagesDestroyed++;
        newValues = true;
    }

    public void addHit()
    {
        HitsReceived++;
        newValues = true;
    }

    public void resetValues()
    {
        VillageCountText.text = "" + 0;
        HitCounText.text = "" + 0;
        VillagesDestroyed = 0;
        HitsReceived = 0;
        newValues = true;
    }
}
