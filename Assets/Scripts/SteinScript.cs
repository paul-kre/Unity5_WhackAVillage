using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteinScript : MonoBehaviour {

    private float elapsedTime;
    public float secondsBeforeDestroy;
    private bool parent;

    // Use this for initialization
    void Start () {
        elapsedTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (!parent)
        {
            elapsedTime += Time.deltaTime;
            if (transform.position.y < -1 || elapsedTime >= secondsBeforeDestroy)
            {
                Destroy(gameObject);
            }
        }
	}

    public void setParent(bool parent)
    {
        this.parent = parent;
    }
}
