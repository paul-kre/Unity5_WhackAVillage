using ExtensionMethods;
using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    public LayerMask BlurLayers;


    void OnCollisionEnter(Collision collision)
    {
        if (BlurLayers.ContainsLayer(collision.collider.gameObject.layer))
        {
            GameManager.Instance.Blur();
            Destroy(collision.collider.gameObject);
        }
    }
}