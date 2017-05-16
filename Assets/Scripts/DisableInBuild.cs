using UnityEngine;

public class DisableInBuild : MonoBehaviour
{
    void Awake()
    {
        if(!Application.isEditor) gameObject.SetActive(false);
    }
}