/**
 * Adapted from johny3212
 * Written by Matt Oskamp
 */
using UnityEngine;
using System.Collections;

public class OptiTrackObject : MonoBehaviour {

    //public string rigidBodyName;
    public int rigidbodyIndex;

    public bool setPosition = true;
    public bool setRotation = true;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        if(OptiTrackManager.Instance != null)
        {
            OptitrackManagement.OptiTrackRigidBody rbData = OptiTrackManager.Instance.GetTrackedRigidbody(rigidbodyIndex);

            if (rbData != null)
            {
                if (setPosition)
                {
                    gameObject.transform.position = rbData.position;
                }
                if (setRotation)
                {
                    gameObject.transform.rotation = rbData.orientation;
                }
            }
        }
       
    }
}
