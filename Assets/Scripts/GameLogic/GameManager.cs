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

    //Timer
    [SerializeField]
    private int gameTime = 0;
    public Text Text_Clock;
    float startTime;
    int EndTime = -1;
    int timeInGame;

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

        SwipeDetectorGear.Instance.AddSwipeTopListener(Recalibrate);
        SwipeDetectorGear.Instance.AddSwipeLeftListener(ExitToMenu);
        SwipeDetectorGear.Instance.AddSwipeRightListener(Restart);
        MainMenu.AddStartGameListener(StartGame);

        // StartCoroutine("Villages");
    }

    void Update()
    {
        keyInteraction();
        /*
        if(EndTime == (int)Time.time && !gamePaused)
        {
            StopGame();
        }*/
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

            
            yield return new WaitForSeconds(UpdateInterval);
        }
    }

    private void ResetGame()
    {
        Debug.Log(villageObjects.Count);
        for (int i= villageObjects.Count-1; i>=0 ;i--)
        {
            villageObjects[i].DestroyBuilding();
        }
        /*        foreach (var building in villageObjects)
        {
            building.DestroyBuilding();
        }*/
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
        MainMenu.Deactivated();

        AmbientSoundManager.Instance.enableIdleSounds();

        WaypointManager.SetActive(true);

        StartCoroutine(Villages());

        UiText.gameObject.SetActive(true);

        gameTime = PlayerPrefs.GetInt("GameTime")*10;       //Here
        startTime = Time.time;
        EndTime = (int) startTime + gameTime;
        StartCoroutine(Clock());
    }

    public void StopGame() 
    {
        Debug.Log("StopGame");
        scoreScript.GetScore(UiText.VillagesDestroyed, UiText.HitsReceived);
        StopCoroutine(Villages());
        UiText.gameObject.SetActive(false);
        WaypointManager.SetActive(false);
        gamePaused = true;
        ResetGame();
        MainMenu.Activated();
        MainMenu.UnDestruct();

    }


    public void Credits()
    {

    }

    public void Blur()
    {
        CameraAnimator.SetTrigger("Blurry");
        UiText.addHit();
    }

    public void ExitToMenu()
    {
        gamePaused = true;
        UiText.resetValues();
        UiText.gameObject.SetActive(false);
        AmbientSoundManager.Instance.disableIdleSounds();

        MainMenu.Activated();

        WaypointManager.SetActive(false);
        ResetGame();
        
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

        if (timeleft >= 0)
        {
            string minutes = Mathf.Floor(timeleft / 60).ToString("00");
            string seconds = (timeleft % 60).ToString("00");

            Text_Clock.text = minutes + ":" + seconds;
            yield return new WaitForSeconds(1);
            StartCoroutine(Clock());
        }
        else
        {
            StopCoroutine("Clock");
            StopGame();
        }
        
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