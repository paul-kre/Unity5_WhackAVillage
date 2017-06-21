using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TrollHand : MonoBehaviour {
    
    private Animator anim;

    [SerializeField]
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Controller.GetHairTrigger()) anim.SetBool("closed", true);
        else anim.SetBool("closed", false);
    }
}
