using UnityEngine;

class OculusRecenter : MonoBehaviour 
{
    [SerializeField]
    private KeyCode _keyToRecenter = KeyCode.R;

	void Update () 
    {
        if (Input.GetKeyDown(_keyToRecenter))
        {
            UnityEngine.VR.InputTracking.Recenter();
        }
	}
}
