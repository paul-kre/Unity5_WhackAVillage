using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMover : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.DEBUG)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                print("forward");
                transform.position += Vector3.forward * 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                this.transform.position.Set(this.transform.position.x - 0.5f, this.transform.position.y, this.transform.position.z);
                transform.position += Vector3.left * 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                this.transform.position.Set(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.5f);
                transform.position += Vector3.back * 0.5f;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                this.transform.position.Set(this.transform.position.x + 0.5f, this.transform.position.y, this.transform.position.z);
                transform.position += Vector3.right * 0.5f;
            }
        }
    }
}
