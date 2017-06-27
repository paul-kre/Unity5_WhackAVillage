using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Spectator_Position : MonoBehaviour {

    public GameObject controller;
    public GameObject spectatorCamera;

    public float movespeed_y = 50;
    public float movespeed_x_z = 50;

    private bool modeSetCamera = true;
    private int counter = 0;

    public Text timerText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (OVRInput.Get(OVRInput.Button.Back))
        {
            modeSetCamera = !modeSetCamera;
            controller.SetActive(modeSetCamera); 
        }

        if (modeSetCamera)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                timerText.text = "SchulterTaste";

            }

            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
            {
                if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                {
                    spectatorCamera.transform.position = new Vector3
                        (
                        spectatorCamera.transform.position.x, 
                        spectatorCamera.transform.position.y + Time.deltaTime * movespeed_y * OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote).y,
                        spectatorCamera.transform.position.z
                        );
                }
                else
                {
                    spectatorCamera.transform.position = new Vector3
                    (
                    spectatorCamera.transform.position.x + Time.deltaTime * movespeed_x_z * OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote).x,
                    spectatorCamera.transform.position.y,
                    spectatorCamera.transform.position.z + Time.deltaTime * movespeed_x_z * OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote).y
                    );
                }

                timerText.text = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote) +" "+ OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

            }
        }

    }
}
