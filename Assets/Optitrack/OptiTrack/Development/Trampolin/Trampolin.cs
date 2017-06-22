using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;

public delegate void CalibrationHandler();
[RequireComponent(typeof(AudioSource))]
public class Trampolin : MonoBehaviour
{
    public bool spectator = false;

    public GameObject tPoseDummy;       //Mesh that shows the T-Pose the user should perform
    public Text tPoseText;              //Used to display the instructions

    public float modelHeight = 1.65f;
    public Transform hmd;
    public Transform character;

    //vars in inspector
    [SerializeField]
    [Tooltip("Every frame difference between two positions. Is this difference > jumpThreshold, State will be Jumping")]
    private float _jumpDifferenceThreshold;
    [SerializeField]
    [Tooltip("cm, how much necessary for it being a jump")]
    private float _jumpRecognitionOffset;
    [SerializeField]
    [Tooltip("cm, how much necessary for it being recognized as landing again")]
    private float _landingOffset;
    [SerializeField]
    [Tooltip("cm difference from last frame, how little necessary for it being recognized as floating in the air and not jumping up anymore")]
    private float _floatingDifferenceThreshold;
    [SerializeField]
    [Tooltip("Time to go from Landing to Standing, when not jumping again")]
    private float _landToStandTime;
    [SerializeField]
    [Tooltip("Key for Calibration. Hit when person is Standing in T-Pose in 0,0,0 and correct direction")]
    private KeyCode _keyToCalibrate = KeyCode.C;
    [SerializeField]
    [Tooltip("Key for Resetting the Statistics, e.g. when some has performed some test jumps")]
    private KeyCode _keyToResetStatistics = KeyCode.R;
    [SerializeField]
    [Tooltip("Starts Calibration automatically after x seconds")]
    private float _secondsTillCalibration = 5.0f;
    [SerializeField]
    [Tooltip("Starts Calibration when in TPose after x seconds")]
    private float _timeInTPoseNeeded = 3.0f;
    [SerializeField, Tooltip("Sound for calibration start")]
    AudioClip _initialize;
    [SerializeField, Tooltip("Sound for calibration start")]
    AudioClip _goIntoTPose;
    [SerializeField, Tooltip("Sound for calibration start")]
    AudioClip _startCalibrationClip;
    [SerializeField, Tooltip("Sound for calibration finish")]
    AudioClip _finishCalibrationClip;
    [SerializeField, Tooltip("Voice output for calibration finish")]
    AudioClip _finishCalibrationVoice;
    [SerializeField, Tooltip("Seconds before auto calibration")]
    private float _preDelay = 1.0f; // <------ LÄNGER MACHEN
    AudioSource _audioSource;
    private bool _calibrationStarted = false;

    [SerializeField]
    private Transform _userContainer;

    private Vector3 _userContainerStart;

    

    //all joint transforms
    public Transform Head {get; private set;}
    public Transform RightHand {get; private set;}
    public Transform LeftHand {get; private set;}
    public Transform RightFoot {get; private set;}
    public Transform LeftFoot { get; private set; }
    public Transform RightElbow { get; private set; }
    public Transform LeftElbow { get; private set; }
    public Transform RightKnee { get; private set; }
    public Transform LeftKnee { get; private set; }
    //head, hands, foots in a list
    private List<Transform> _joints;

    public float JumpMultiplier { get; set; }

    //other vars
    private bool _isCalibrated;

    public event CalibrationHandler CalibrationDone;

    //singleton
    private static Trampolin _instance;
    public static Trampolin Instance
    {
        get { return _instance; }
    }

    private Coroutine _staysInTPose;
    private bool _isInTPose;
    private bool scalingEnabled = true;
    

    private void Awake()
    {
        
        if (_instance == null)
        {
//            DontDestroyOnLoad(transform.gameObject);
            _instance = this;
        }
        else if (_instance != this)
        {
            //var tPointer = *trampolin;
            Destroy(this.gameObject);
        }


        JumpMultiplier = 0;
        //_instance = this;
        _audioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {

        if (!spectator)
        {
            //init my vars
            _isCalibrated = false;

            //vpc = new ValuePlotterController(this.gameObject, new Rect(0, 0, 100, 100), Color.black, Color.white, -30, 30);
            StartCoroutine(VoiceGuidance());
            StartCoroutine(WaitAndStartCalibration(_preDelay, _secondsTillCalibration));
        }
        else
        {
            _isCalibrated = true;
            tPoseText.text = "";
            tPoseDummy.SetActive(false);
            GameManager.Instance.CalibrationDone();//geht zum Menü über
        }

    }


    IEnumerator VoiceGuidance()
    {
        PlayAudioClip(_initialize);
        yield return new WaitForSeconds(_initialize.length);
        PlayAudioClip(_goIntoTPose);
    }

    IEnumerator WaitAndStartCalibration(float waitForSeconds, float calibrationDelay)
    {
        tPoseDummy.SetActive(true);
        tPoseText.text = "Nehmen Sie die gezeigte T-Pose ein.";


        //HIER SKALIERUNG EINFÜGEN !!!      <-----------



        yield return new WaitForSeconds(waitForSeconds);
        float factor = hmd.position.y / modelHeight;
        character.localScale = Vector3.one * factor;


        tPoseText.text = "";
        tPoseDummy.SetActive(false);
        GameManager.Instance.CalibrationDone();//geht zum Menü über
    }

    public void Recalibrate()
    {
        scalingEnabled = false;
        StartCoroutine(StartCalibration());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator StartCalibration(float delay = 0)
    {
        Debug.Log("StartCalibration() "+ _calibrationStarted);
        if (!_calibrationStarted)
        {
            //tPoseDummy.SetActive(true);
            _calibrationStarted = true;
            Debug.Log("Start Calibration");
            tPoseText.text = "Halten Sie die T-Pose.";

            PlayAudioClip(_startCalibrationClip);
        
            yield return new WaitForSeconds(_startCalibrationClip.length);
            
            yield return new WaitForSeconds(delay);
            

            _isCalibrated = true;
            InputTracking.Recenter();
            Debug.Log("grea-uuuht success! (Calibrated)");
            //PlayAudioClip(_finishCalibrationClip);
            if (CalibrationDone != null)
            {
                CalibrationDone();
            }
            yield return new WaitForSeconds(_finishCalibrationClip.length);
            //PlayAudioClip(_finishCalibrationVoice);
            
            _calibrationStarted = false;
            tPoseText.text = "";
            GameManager.Instance.CalibrationDone();
            tPoseDummy.SetActive(false);
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(_keyToCalibrate))
        {
            StartCoroutine(StartCalibration(0f));
        }
        
    }

    void PlayAudioClip(AudioClip clip)
    {
        if(clip != null)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }

    public bool IsCalibrated()
    {
        return _isCalibrated;
    }
}
