/**
 * Adapted from johny3212
 * Written by Matt Oskamp
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using OptitrackManagement;


public class OptiTrackManager : MonoBehaviour
{
    
	private static OptiTrackManager instance;

    private DirectMulticastSocketClient dmsClient;

    Dictionary<int, OptiTrackRigidBody> rigidbodies;
    
    public enum Sources
    {
        live,
        record
    }

    public Sources currentSource = Sources.live;
    
	public static OptiTrackManager Instance
	{
		get { return instance; } 
	}    

	void Awake()
	{
		instance = this;
        rigidbodies = new Dictionary<int, OptiTrackRigidBody>();
	}

    
	~OptiTrackManager ()
	{      
		Debug.Log("OptitrackManager: Destruct");
		dmsClient.Close();
	}

    IEnumerator Benchmarker()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log(dmsClient.benchmark);
            dmsClient.benchmark = 0;
        }
    }

    public List<OptiTrackRigidBody> GetAllRigidBodyList()
    {
        List<OptiTrackRigidBody> rbList = new List<OptiTrackRigidBody>();

        foreach(OptiTrackRigidBody rb in rigidbodies.Values)
        {
            rbList.Add(rb);
        }

        return rbList;
    }

    public List<Vector3> GetAllUnlabeledMarkerList()
    {
        return unlabeledMarkerPositions;
    }

    public OptiTrackRigidBody GetTrackedRigidbody(int id)
    {
        OptiTrackRigidBody rb;

        if (rigidbodies.TryGetValue(id, out rb))
        {
            return rb;
        }
        else
        {
            return null;
        }
    }

    public void ReceiveRigidbodyData(int source, Vector3 pos, Quaternion rot, int id)
    {
      if(source == (int)currentSource)
       {
            OptiTrackRigidBody rb;

            if(rigidbodies.TryGetValue(id, out rb))
            {
                rb.position = pos;
                rb.orientation = rot;
            }
            else
            {
                rb = new OptiTrackRigidBody();
                rb.position = pos;
                rb.orientation = rot;
                rb.ID = id;

                rigidbodies.Add(id, rb);
            }
       }
    }

    List<Vector3> unlabeledMarkerPositions = new List<Vector3>();

    public void ReceiveUnlabeledMarkerData(int source, Vector3 pos)
    {
        if (source == (int)currentSource)
        {
            unlabeledMarkerPositions.Add(pos);
        }
    }

    public void ClearUnlabeledMarkerPositions()
    {
        unlabeledMarkerPositions.Clear();
    }

    void Start () 
	{
        dmsClient = new DirectMulticastSocketClient(this);
        dmsClient.StartClient();
    
        StartCoroutine(Benchmarker());
        
		Application.runInBackground = true;
	}

    // Update is called once per frame
    void Update()
    {
    }
    
    void OnApplicationQuit()
    {
        dmsClient.Close();

    }

    

    private void OnDrawGizmos()
    {
        if(dmsClient != null && dmsClient.IsInit())
        {
            Gizmos.color = Color.yellow;
            for(int i = 0;i< unlabeledMarkerPositions.Count;i++)
            {
                Gizmos.DrawSphere(unlabeledMarkerPositions[i], 0.03f);
            }
        }
    }
    
    
}