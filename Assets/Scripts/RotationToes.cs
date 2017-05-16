using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationToes : MonoBehaviour {

    public GameObject foot;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = foot.transform.rotation;
	}
}
