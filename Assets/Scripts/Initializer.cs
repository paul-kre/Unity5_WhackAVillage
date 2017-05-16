using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Initializer : MonoBehaviour
{
    [SerializeField]
    private SphereCollider Collider;

    [SerializeField]
    private Transform[] Prefabs;

    [SerializeField]
    [Range(0, 200)]
    private int Amount;

    [SerializeField]
    [Range(0, 30)]
    private float MinDistance;

    [SerializeField]
    [Range(0, 10)]
    private float MinScale = 0.9f;

    [SerializeField]
    [Range(0, 10)]
    private float MaxScale = 1.1f;

    void Awake()
    {
        InsideCircle(Amount);
    }

    private void InsideCircle(int Amount)
    {
        var i = 0;
        var Spread = Collider.radius * transform.lossyScale.x - MinDistance;

        if (Spread < 0) return;


        while (i < Amount)
        {
            Vector3 randDir = Random.insideUnitSphere;
            Vector3 randomPos = transform.position + MinDistance * randDir.normalized + Spread * randDir;


            var instance = Instantiate(Prefabs[Random.Range(0, Prefabs.Length)], randomPos, Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.up), transform);

            instance.localScale = Vector3.one * Random.Range(MinScale, MaxScale);

            i++;
        }
    }
}