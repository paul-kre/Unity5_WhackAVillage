using UnityEngine;
using System.Collections;

public class ApplySimpleDifference : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Which transform position difference shall be applied? Note: Calibrate method has to be called before the difference is applied")]
    private Transform _whichToApply;

    private Vector3 _startApply, _myStart;
    [SerializeField]
    private bool locked;

    private void Start()
    {
        locked = true;
    }

    private void Update()
    {
        if (!locked)
        {
            //apply difference from other transform to my transform
            transform.position = _myStart + _whichToApply.position - _startApply;
        }
    }

    //setting start values when calibrating and unlocking
    public void Calibrate()
    {
        _startApply = _whichToApply.position;
        _myStart = transform.position;
        locked = false;
    }
    

}
