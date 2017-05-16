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

        //		StartCoroutine("Villages");
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
        foreach (var building in villageObjects)
        {
            building.DestroyBuilding();
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
        MainMenu.Deactivated();

        AmbientSoundManager.Instance.enableIdleSounds();

        WaypointManager.SetActive(true);

        StartCoroutine(Villages());

        UiText.gameObject.SetActive(true);
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
}