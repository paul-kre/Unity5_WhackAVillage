using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawn : MonoBehaviour {

    public GameObject BalloonPrefab;
    private GameObject Balloon;
    public int chance = 100;
    public int spawnwinkel=180; //Maximal 180

    private int count = 1;  //Variable zum sichergehen, dass nur einer in der Szene ist
    public float height; //Höhencheck
    private float z;
    private float x;
    public float radius = 2;
    public float speed = 1;
    public float speed2 = 1;

    private Vector3 Direction;
    private float i;
    private float o;


    void Start ()
    {

    }

    // Update is called once per frame
    void Update() {

        this.transform.position = new Vector3(0, 0, 0);
        if (Balloon == null)
        {
            int RNG = Random.Range(0, chance);
            //print(RNG);
            if (RNG == 1)
            {
                count = 0;
            }
        }

        if (count == 0)
        {
            Balloon = Instantiate(BalloonPrefab);
            Balloon.transform.parent = transform;

            var ang = Random.value * spawnwinkel;
            ang = ang + 90 - spawnwinkel / 2;
            z = radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            x = radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            Balloon.transform.position = new Vector3(x,Balloon.transform.position.y,z);
            count = 1;
        }
        /*
        if (Balloon != null && Balloon.transform.position.y < height && count == 1)
        {
            i += Time.deltaTime * speed;
            Balloon.transform.position = Vector3.Lerp(Balloon.transform.position, new Vector3(Balloon.transform.position.x, height + 0.1f, Balloon.transform.position.z), i);
            //Balloon.transform.position = new Vector3(Balloon.transform.position.x, Balloon.transform.position.y + speed, Balloon.transform.position.z);
        }
        else
        {
            
            count = 2;
        }

        if (Balloon!= null && count == 2)
        {
            i += Time.deltaTime * speed2;
            Balloon.transform.position = Vector3.Lerp(Balloon.transform.position, Camera.main.transform.position, i);

        }

    */



    }
}
