using System.Collections;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [Range(0, 10)]
    public float Speed;

    public bool ScaleBased = true;

    void Awake()
    {

    }

    void OnEnable()
    {
        StartCoroutine(UpdateAll());
    }


    IEnumerator UpdateAll()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));

        while (true)
        {
            if (ScaleBased) transform.rotation = Quaternion.Euler(
                 transform.rotation.eulerAngles.x,
                 transform.rotation.eulerAngles.y + Time.deltaTime * Speed / transform.localScale.x,
                 transform.rotation.eulerAngles.z);
            else transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + Time.deltaTime * Speed,
                transform.rotation.eulerAngles.z);

            yield return new WaitForSeconds(0.25f);
        }
    }
}