using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloonFacingPlayer : MonoBehaviour {

    public GameObject head;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 direction = this.transform.position - head.transform.position;


        transform.LookAt(new Vector3(head.transform.position.x,this.transform.position.y, head.transform.position.z), Vector3.up);
    }
}
