using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class ApplyPosition : MonoBehaviour
{
    public Transform TargetTransform;
    [Range(0,1)]
    public float Smoothness = 0.2f;

    void OnEnable()
    {
        StartCoroutine(UpdateAll());
    }

    IEnumerator UpdateAll()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, TargetTransform.position - new Vector3(0, 0, 0), Smoothness);

            yield return null;
        }
    }
}