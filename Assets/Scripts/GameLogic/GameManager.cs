using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class GameEvent : UnityEvent { }

public class GameManager : MonoBehaviour
{

    public bool setShortTime = false;
    public bool Phone = false;
    public bool spectator = false;


    [Serializable]
    public class BuildingSite
    {
        public BuildingSite(Vector3 position)
        {
            Position = position;
        }

        public Vector3 Position;
    }

    public static GameManager Instance;

    [SerializeField]
    [Range(0.1f, 10)]
    private float UpdateInterval = 5;

    public bool DEBUG;

    public UIText UiText;

    [SerializeField]
    private Text Text;
    [SerializeField]
    private MainMenu MainMenu { get { return MainMenu.Instance; } }
    [SerializeField]
    private GameObject Camera;
    [SerializeField]
    private GameObject VillagePrefab;
    [SerializeField]
    private GameObject CatapultPrefab;
    [SerializeField]
    private Transform GameModels;
    [SerializeField]
    private Transform BottomModels;
    [SerializeField]
    private GameObject WaypointManager;
    [SerializeField]
    private Transform BuildingSites;

    public Transform HeadTrackerPosition;

    public int maxNumberVillages;

    //Score
    [SerializeField]
    private Score scoreScript;
    private bool SetScore;

    //Timer
    [SerializeField]
    private int gameTime = 0;
    public int timeToRestartMenue = 5;
    public Text Text_Clock;
    float startTime;
    int EndTime = -1;
    int timeInGame;
    public AudioClip Warning_3;
    public AudioClip Warning_2;
    public AudioClip Warning_1;
    public AudioClip Warning_Runde_vorbei;
    AudioSource timerSound;

    //bool[] spawned = new bool[] { false, false, false, false, false, false };
    List<BuildingSite> villagesToSpawn = new List<BuildingSite>();
    List<BuildingSite> villagesSpawned = new List<BuildingSite>();
    List<Destructible> villageObjects = new List<Destructible>();
    bool gamePaused = false;

    private Animator CameraAnimator;

    void Awake()
    {
        if (!Instance) Instance = this;

        foreach (var Transform in BuildingSites.GetComponentsInChildren<Transform>())
        {
            if(Transform == BuildingSites.transform) continue;
            if(!Transform.gameObject.activeSelf) continue;

            villagesToSpawn.Add(new BuildingSite(Transform.position));
        }
    }

    void Start()
    {
        CameraAnimator = Camera.GetComponent<Animator>();

        //SwipeDetectorGear.Instance.AddSwipeTopListener(Recalibrate);
        SwipeDetectorGear.Instance.AddSwipeLeftListener(ExitToMenu); //Works only in game
        SwipeDetectorGear.Instance.AddSwipeRightListener(Restart);
        MainMenu.AddStartGameListener(StartGame);

        // StartCoroutine("Villages");
        if(Phone)
        {
            PlayerPrefs.SetInt("GameTime", 2);
        }
    }

    void Update()
    {
        if(!Phone)
        {
        keyInteraction();
        }
    }

    IEnumerator Villages()
    {
        yield return new WaitForSeconds(1.5f);
        while (!gamePaused)
        {
            if (villagesToSpawn.Count > 0)
            {
                int newVillage = (int)(UnityEngine.Random.value * villagesToSpawn.Count);
                var temp = villagesToSpawn[newVillage];
                villagesSpawned.Add(villagesToSpawn[newVillage]);
                villagesToSpawn.Remove(villagesToSpawn[newVillage]);


                if (UnityEngine.Random.Range(0f, 1f) < 0.75) createBuilding(temp, VillagePrefab);
                else createBuilding(temp, CatapultPrefab);

            }
            float waitTime;
            if (!spectator)
            {
                waitTime = UnityEngine.Random.Range(1f, 6f);
            }
            else
            {
                waitTime = UnityEngine.Random.Range(1f, 3f);
            }
            //Debug.Log(waitTime);
            //yield return new WaitForSeconds(UpdateInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void ResetGame()
    {
        //Debug.Log(villageObjects.Count);
        for (int i= villageObjects.Count-1; i>=0 ;i--)
        {
            villageObjects[i].DestroyBuilding();
        }

    }

    public void createBuilding(BuildingSite site, GameObject prefab)
    {
        var Village = Instantiate(prefab, site.Position, Quaternion.LookRotation(site.Position), BottomModels).GetComponent<Destructible>();
        Village.transform.localPosition = new Vector3(Village.transform.localPosition.x, 0, Village.transform.localPosition.z);
        villageObjects.Add(Village);

        Village.AddOnDestroyListener(delegate ()
        {
            villageObjects.Remove(Village);
            UiText.addVillage();

            Timer(2, delegate
            {
                villagesToSpawn.Add(site);
                villagesSpawned.Remove(site);
            });
        });
    }


    public void CalibrationDone()
    {
        MainMenu.Activated();
    }

    public void Recalibrate()
    {
        MainMenu.Deactivated();

        Trampolin.Instance.Recalibrate();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CloseMenu()
    {
        MainMenu.Deactivated();
    }

    public void StartGame()
    {
        SetScore = true;
        MainMenu.Deactivated();
        scoreScript.turnOffScoreBoard();
        UiText.resetValues();
        AmbientSoundManager.Instance.enableIdleSounds();

        WaypointManager.SetActive(true);

        StartCoroutine(Villages());

        UiText.gameObject.SetActive(true);

        gameTime = PlayerPrefs.GetInt("GameTime")*60;       //Here

        if(setShortTime) gameTime = PlayerPrefs.GetInt("GameTime") * 5;
        startTime = Time.time;
        EndTime = (int) startTime + gameTime;
        StartCoroutine(Clock());
        gamePaused = false;
        if (DEBUG) Debug.Log("startGame");

    }

    public void StopGame() 
    {
        if(DEBUG) Debug.Log("StopGame");

        if (SetScore)
        {
            scoreScript.GetScore(UiText.VillagesDestroyed, UiText.HitsReceived);
            StartCoroutine(StartMenueAfterSec(timeToRestartMenue));
        }
        else
        {
            StartCoroutine(StartMenueAfterSec(0));

        }

        StopCoroutine(Villages());
        AmbientSoundManager.Instance.disableIdleSounds();
        UiText.gameObject.SetActive(false);
        WaypointManager.SetActive(false);
        gamePaused = true;
        ResetGame();

        //MainMenu.Activated();
        //MainMenu.UnDestruct();

    }

    public void ExitToMenu()
    {
        SetScore = false;
        gameTime = timeInGame;
    }

    public void Credits()
    {

    }

    public void Blur()
    {
        CameraAnimator.SetTrigger("Blurry");
        UiText.addHit();
    }

    private void Timer(float time, UnityAction action)
    {
        StartCoroutine(TimerCR(time, action));
    }

    IEnumerator TimerCR(float time, UnityAction action)
    {
        yield return new WaitForSeconds(time);

        action();
    }
    IEnumerator Clock()
    {
        timeInGame = (int)(Time.time - startTime);
        int timeleft = gameTime - timeInGame;
        if (!spectator)
        {
            if (timeleft > 0)
            {
                string minutes = Mathf.Floor(timeleft / 60).ToString("00");
                string seconds = (timeleft % 60).ToString("00");

                Text_Clock.text = minutes + ":" + seconds;

                yield return new WaitForSeconds(1);

                if (timeleft <= 4)
                {
                    timerSound = GetComponent<AudioSource>();
                    //timerSound.clip = FinalBeep;
                    //timerSound.Play();
                    switch (timeleft)
                    {
                        case 4:
                            if (DEBUG) Debug.Log("3");
                            //timerSound.clip = Warning_3;
                            //timerSound.Play();
                            break;
                        case 3:
                            if (DEBUG) Debug.Log("2");
                            //timerSound.clip = Warning_2;
                            //timerSound.Play();
                            break;
                        case 2:
                            if (DEBUG) Debug.Log("1");
                            //timerSound.clip = Warning_1;
                            //timerSound.Play();
                            break;
                        case 1:
                            break;
                    }

                }
                StartCoroutine(Clock());
            }
            else
            {
                string minutes = 0.ToString("00");
                string seconds = 0.ToString("00");
                if (DEBUG) Debug.Log("TimeUps");
                //timerSound.clip = Warning_Runde_vorbei;
                //timerSound.Play();

                Text_Clock.text = minutes + ":" + seconds;
                StopCoroutine("Clock");
                StopGame();
            }

        }
    }

    IEnumerator StartMenueAfterSec(int time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("aktivat-ion");
        MainMenu.Activated();
        MainMenu.UnDestruct();
    }

    private void keyInteraction()
    {
        //SetTime 
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Set to 1");
            PlayerPrefs.SetInt("GameTime",1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Set to 2");
            PlayerPrefs.SetInt("GameTime", 2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Set to 3");
            PlayerPrefs.SetInt("GameTime", 3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Set to 4");
            PlayerPrefs.SetInt("GameTime", 4);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Set to 5");
            PlayerPrefs.SetInt("GameTime", 5);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("Set to 6");
            PlayerPrefs.SetInt("GameTime", 6);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("Set to 7");
            PlayerPrefs.SetInt("GameTime", 7);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("Set to 8");
            PlayerPrefs.SetInt("GameTime", 8);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("Set to 9");
            PlayerPrefs.SetInt("GameTime", 9);
        }
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Set to 10");
            PlayerPrefs.SetInt("GameTime", 10);
        }

        //Restart
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Restart");
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            scoreScript.GetScore(10, 11);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            scoreScript.delete();
            scoreScript.turnOffScoreBoard();
        }

    }
}