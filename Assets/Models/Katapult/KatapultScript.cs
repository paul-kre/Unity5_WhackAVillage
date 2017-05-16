using UnityEngine;
using System.Collections;

public class KatapultScript : MonoBehaviour {

	Animator anim;
	int destroy = Animator.StringToHash("Destroy");
	public ParticleSystem particles;
	int spawn = Animator.StringToHash("Spawn");


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			anim.SetTrigger (destroy);
			particles.Play();
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			anim.SetTrigger (spawn);
		}
	}
}
