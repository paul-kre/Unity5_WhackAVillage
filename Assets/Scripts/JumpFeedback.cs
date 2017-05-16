using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpFeedback : MonoBehaviour
{
    [SerializeField]
    private Collider[] Colliders;

    [SerializeField]
    [Range(-2, 2)]
    private float Offset = 0f;

    [SerializeField]
    [Range(0, 1)]
    private float UpSmoothness = 0.5f;

    [SerializeField]
    [Range(0, 1)]
    private float DownSmoothness = 0.5f;


    [SerializeField]
    [NonEditable]
    private float DesiredHeight = 0;



    void Awake()
    {
    }


    void OnEnable()
    {
        StartCoroutine(UpdateAll());
    }

    IEnumerator UpdateAll()
    {
        while (true)
        {
            DesiredHeight = 0;

            foreach (var Col in Colliders)
            {
                DesiredHeight = Mathf.Min(DesiredHeight, Mathf.Min(Col.transform.position.y + Offset, 0));
            }

            if (transform.position.y < DesiredHeight) transform.position = new Vector3(0, Mathf.Lerp(transform.position.y, DesiredHeight, UpSmoothness), 0);
            else transform.position = new Vector3(0, Mathf.Lerp(transform.position.y, DesiredHeight, DownSmoothness), 0);

            yield return null;
        }
    }
}