using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Spectator_Position : MonoBehaviour {

    public GameObject controller;

    public float movespeed_y = 10;
    public float movespeed_x_z = 10;

    private bool setCamera = true;
    private int counter = 0;

    public Text timerText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    //    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
    //    {
    //        timerText.text = "Knopf";
    //    }
    //    else
    //    {
    //        timerText.text = "Kein Knopfi";
    //    }



    //    if (setCamera)
    //    {
    //        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
    //        {
    //            timerText.text = "Knopf";

    //            StartCoroutine(PlaceCamera());
                
    //            if(OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
    //            {
    //                OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote);
    //                this.transform.position = 
    //                    new Vector3(
    //                    this.transform.position.x,
    //                    this.transform.position.y + Time.deltaTime * movespeed_y * OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote).y,
    //                    this.transform.position.z
    //                    );

    //            }
    //            else
    //            {
    //                timerText.text = "Kein Knopf";

    //            }

    //        }


    //    }
    //    else//Nurwechsel in Modus möglich, kein Bewegen
    //    {
    //        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
    //        {
    //            StartCoroutine(PlaceCamera());
    //        }
    //    }
                

    //}

    //IEnumerator PlaceCamera()
    //{
    //    {
    //        yield return new WaitForSeconds(1);
    //        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && !OVRInput.Get(OVRInput.Touch.PrimaryTouchpad) && !OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
    //        {
    //            counter++;
    //            timerText.text = "Switch Modus: " + (5-counter);
    //            if (counter >= 5)
    //            {
    //                setCamera = !setCamera;
    //                controller.SetActive(!controller.activeInHierarchy);
    //                counter = 0;
    //                timerText.text = "";
    //            }
    //            else
    //            {
    //                StartCoroutine(PlaceCamera());
    //            }
    //        }
    //        else
    //        {
    //            counter = 0;
    //            timerText.text = "";
    //        }
    //    }
    }
}
