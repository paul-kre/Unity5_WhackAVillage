using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    [SerializeField]
    private Destructible StartGameVillage;

    [SerializeField]
    private Animator StartGameAnimator;

    [SerializeField]
    [Range(0, 10)]
    private float FadeOutTime = 3;

    [SerializeField]
    private bool DisabledOnStart = true;

    [Header("Debug")]
    [SerializeField]
    private bool DebugLog;

    private RealWorldButton[] Buttons;

    private void Awake()
    {
        if (!Instance) Instance = this;

        if (DisabledOnStart)
        {
            gameObject.SetActive(false);
            StartGameVillage.gameObject.SetActive(true);
            StartGameVillage.gameObject.SetActive(false);
        }

        Buttons = GetComponentsInChildren<RealWorldButton>();
    }

    public void Activated()
    {
        if (DebugLog) print("Activated menu");
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartGameVillage.gameObject.SetActive(true);


        StartGameAnimator.SetBool("Enabled", true);
        StartGameVillage.Spawn();
        foreach (var Button in Buttons)
        {
            Button.Activated();
        }
    }

    public void Deactivated()
    {
        if (DebugLog) print("Deactivated menu");

        StartGameAnimator.SetBool("Enabled", false);
        StartGameVillage.DestroyAnimation();
        foreach (var Button in Buttons)
        {
            Button.Deactivated();
        }

        Timer(FadeOutTime, delegate
        {
            gameObject.SetActive(false);
        });
    }

    public void AddStartGameListener(UnityAction Action)
    {
        StartGameVillage.AddOnDestroyListener(Action);
    }

    private void Timer(float time, Destructible.Callback callback)
    {
        StartCoroutine(TimerCR(time, callback));
    }

    IEnumerator TimerCR(float time, Destructible.Callback callback)
    {
        yield return new WaitForSeconds(time);

        callback();

    }

//    void OnValidate()
//    {
//        print("OnValidate");
//        print("DebugLog: " + DebugLog);
//    }
}