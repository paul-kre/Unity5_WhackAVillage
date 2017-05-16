using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationRandomizer : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;

    [SerializeField]
    [Range(0, 10)]
    private float MinSpeed = 0.9f;

    [SerializeField]
    [Range(0, 10)]
    private float MaxSpeed = 1.1f;

    void Start()
    {
        Animator.speed = Random.Range(MinSpeed, MaxSpeed);
    }
}