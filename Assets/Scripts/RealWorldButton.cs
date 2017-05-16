using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RealWorldButton : MonoBehaviour
{
    public enum InteractionMode
    {
        Instant,
        Hold
    }

    [SerializeField]
    private string Action;

    [SerializeField]
    private InteractionMode Mode;

    [Header("Hold Mode")]
    [SerializeField]
    private Text TimeText;
    [SerializeField]
    private Animator ButtonAnimator;
    [SerializeField]
    private Animator CanvasAnimator;
    [SerializeField]
    [Range(0, 5)]
    private float Duration = 2;
    [SerializeField]
    [NonEditable]
    private float RemainingTime = 0;


    private string Description;
    private bool Triggered = false;
    private bool Active = true;

    //    [SerializeField]
    //    private Animator Animator;
    //
    //    [SerializeField]
    //    private ParticleSystem ParticleSystem;

    void Start()
    {
//        Activated();
    }

    private void Awake()
    {
        //        ButtonAnimator = GetComponent<Animator>();
        if (TimeText) Description = TimeText.text;
    }

    public void Activated()
    {
        StopAllCoroutines();
        Active = true;

        RemainingTime = Duration;
        TimeText.text = Description; 
        if (CanvasAnimator) CanvasAnimator.SetBool("Enabled", true);
        if (ButtonAnimator) ButtonAnimator.SetBool("Enabled", true);
    }

    public void Deactivated()
    {
        Active = false;

        if (CanvasAnimator) CanvasAnimator.SetBool("Enabled", false);
        if (ButtonAnimator) ButtonAnimator.SetBool("Enabled", false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!Active) return;
        if (Mode != InteractionMode.Instant) return;
        if (collider.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        GameManager.Instance.SendMessage(Action);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (!Active) return;
        if (Triggered) return;
        if (Mode != InteractionMode.Hold) return;
        if (collider.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        RemainingTime -= Time.deltaTime;
        TimeText.text = RemainingTime.ToString("F1");

        if (RemainingTime > 0) return;

        RemainingTime = Duration;
        Triggered = true;
        TimeText.text = "";

        GameManager.Instance.SendMessage(Action);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (Mode != InteractionMode.Hold) return;
        if (collider.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        RemainingTime = Duration;
        if (!Triggered) TimeText.text = Description;
        else DelayedReset();
        Triggered = false;
    }

    private void DelayedReset()
    {
        Timer(2, delegate
        {
            TimeText.text = Description;
        });
    }

    private void Timer(float time, UnityAction callback)
    {
        StartCoroutine(TimerCR(time, callback));
    }

    IEnumerator TimerCR(float time, UnityAction callback)
    {
        yield return new WaitForSeconds(time);

        callback();
    }
}