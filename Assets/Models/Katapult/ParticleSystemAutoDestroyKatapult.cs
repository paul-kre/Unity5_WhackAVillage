using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroyKatapult : MonoBehaviour 
{
	public void Start() 
	{

	}

	public void Update() 
	{
		if(Input.GetKeyDown (KeyCode.Q))
		{
			Destroy (gameObject, GetComponent<ParticleSystem> ().main.duration);
		}
	}
}