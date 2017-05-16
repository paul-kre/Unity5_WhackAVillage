using System.Collections.Generic;
using UnityEngine;

public class ForceApplicator : MonoBehaviour
{
    [SerializeField]
    [Range(1, 15)]
    private int SpeedDepth = 1;


    [SerializeField]
    private List<Vector3> RelativePositions;

    private List<float> TimeSteps;

    private Vector3 PreviousPosition;

    [SerializeField]
    private bool GUIEnabled = false;

    private Vector3 Velocity;

    //    private Collider ForceCollider;

    void Awake()
    {
        //        ForceCollider = GetComponent<Collider>();

        RelativePositions = new List<Vector3>();
        TimeSteps = new List<float>();
    }

    void Start()
    {
        PreviousPosition = transform.position;
    }

    void Update()
    {
        UpdateVelocity();
    }


    void UpdateVelocity()
    {
        RelativePositions.Insert(0, transform.position - PreviousPosition);
        TimeSteps.Insert(0, Time.deltaTime);

        PreviousPosition = transform.position;

        while (RelativePositions.Count > SpeedDepth)
        {
            RelativePositions.RemoveAt(RelativePositions.Count - 1);
            TimeSteps.RemoveAt(TimeSteps.Count - 1);
        }

        var tempVel = new Vector3();
        float accTime = 0;

        for (int index = 0; index < RelativePositions.Count; index++)
        {
            tempVel += RelativePositions[index];
            accTime += TimeSteps[index];
        }

        Velocity = tempVel / accTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Destructable")) return;

        ForceReceiver receiver = collision.gameObject.GetComponent<ForceReceiver>();

        if (receiver != null) receiver.OnHit(Velocity);


//        print("Hit with force: " + Velocity + " magnitude: " + Velocity.magnitude);

//        Debug.

        //        foreach (ContactPoint contact in collision.contacts)
        //        {
        //            Debug.DrawRay(contact.point, contact.normal, Color.white, 1f);
        //        }
    }

    void OnGUI()
    {
        if (!GUIEnabled) return;

        GUI.Label(new Rect(0, 0, 300, 20), "Velocity: " + Velocity);
        GUI.Label(new Rect(0, 20, 300, 20), "Vel Magnitude: " + Velocity.magnitude.ToString("F"));
    }
}
