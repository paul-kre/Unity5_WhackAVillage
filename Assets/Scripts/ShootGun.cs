using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGun : MonoBehaviour {

    [SerializeField]
    private GameObject gunAmmoPrefab;
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float power;

    [SerializeField]
    private float shotDelay;

    private bool reloading = false;

    [SerializeField]
    private SteamVR_TrackedController gunTracker;


    // Use this for initialization
    void Start () {
        gunTracker = this.GetComponent<SteamVR_TrackedController>();

        gunTracker.TriggerClicked += Trigger;
    }
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Y))
        {
            Shoot();
        }
	}

    void Trigger(object sender, ClickedEventArgs e)
    {
        if(!reloading)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        reloading = true;
        GameObject geschoss = Instantiate<GameObject>(gunAmmoPrefab, spawnPoint);
        geschoss.transform.parent = null;
        geschoss.SetActive(true);
        geschoss.GetComponent<Rigidbody>().isKinematic = false;
        geschoss.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * power,  ForceMode.Impulse);
        geschoss.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 1, 1), ForceMode.Impulse);
        StartCoroutine(ShootDelay(shotDelay));

    }

    private IEnumerator ShootDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        reloading = false;

    }
}
