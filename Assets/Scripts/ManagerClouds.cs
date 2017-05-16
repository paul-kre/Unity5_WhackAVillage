using UnityEngine;
using System.Collections;

public class ManagerClouds : MonoBehaviour
{
    private GameObject cloudA;
    private GameObject cloudB;
    private GameObject cloudC;
    private GameObject cloudD;
    public GameObject prefabCloud;
    
    public float speed = 0.02f;
    public float height = 1.6f;
    public float distance = 3f;
    public int limit=10;
    public int spawnchance_1_in = 1000;

    void Start()
    {
    }

    void Update()
    {
        int rng = Random.Range(1, spawnchance_1_in);

        //1.Wolke
        if (cloudA != null)
        {
            cloudA.transform.position = new Vector3(cloudA.transform.position.x, height, cloudA.transform.position.z - speed);
            if (cloudA.transform.position.z < -limit)
            {
                Destroy(cloudA);
            }
        }
        else
        {
            if (rng == 1)
            {
                print("spawn cloud_a");
                cloudA = (GameObject)Instantiate(prefabCloud, new Vector3(Random.Range(distance, -distance), height, limit), new Quaternion(0, 0, 0, 0));
            }
        }

        //2.Wolke
        if (cloudB != null)
        {
            cloudB.transform.position = new Vector3(cloudB.transform.position.x, height, cloudB.transform.position.z - speed);
            if (cloudB.transform.position.z < -limit)
            {
                Destroy(cloudB);
            }
        }
        else
        {
            if (rng == 2)
            {
                print("spawn cloud_b");
                cloudB = (GameObject)Instantiate(prefabCloud, new Vector3(Random.Range(distance, -distance), height, limit), new Quaternion(0, 0, 0, 0));

            }
        }

        //3.Wolke
        if (cloudC != null)
        {
            cloudC.transform.position = new Vector3(cloudC.transform.position.x, height, cloudC.transform.position.z - speed);
            if (cloudC.transform.position.z < -limit)
            {
                Destroy(cloudC);
            }
        }
        else
        {
            if (rng == 3)
            {
                print("spawn cloud_c");
                cloudC = (GameObject)Instantiate(prefabCloud, new Vector3(Random.Range(distance, -distance), height, limit), new Quaternion(0, 0, 0, 0));

            }
        }

        //4.Wolke
        if (cloudD != null)
        {
            cloudD.transform.position = new Vector3(cloudD.transform.position.x, cloudD.transform.position.y, cloudD.transform.position.z - speed);
            if (cloudD.transform.position.z < -limit)
            {
                Destroy(cloudD);
            }
        }
        else
        {
            if (rng == 4)
            {
                print("spawn cloud_d");
                cloudD = (GameObject)Instantiate(prefabCloud, new Vector3(Random.Range(distance, -distance), height, limit), new Quaternion(0, 0, 0, 0));

            }
        }





    }
}