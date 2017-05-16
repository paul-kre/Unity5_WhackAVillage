using System.Collections;
using UnityEngine;

//[ExecuteInEditMode]
public class CameraFacingBillboard : MonoBehaviour
{
    //public Camera m_Camera;

    //    [SerializeField]
    //    [Range(0, 1)]
    //    private float RotationFactor = 0.5f;

    //    private Quaternion StartingRotation;

    void Start()
    {
        StartCoroutine(UpdateAll());
    }


    private IEnumerator UpdateAll()
    {
        while (true)
        {
            transform.LookAt(Camera.main.transform.position, Vector3.up);
            yield return new WaitForSeconds(1.4f);
        }
    }
}
