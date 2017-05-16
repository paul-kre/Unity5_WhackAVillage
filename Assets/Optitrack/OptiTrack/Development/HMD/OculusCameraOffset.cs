using UnityEngine;

class OculusCameraOffset : MonoBehaviour
{
    //do not apply the position set by oculus
	private void Update() 
    {
        transform.parent.localPosition = -transform.localPosition;
	}
}
