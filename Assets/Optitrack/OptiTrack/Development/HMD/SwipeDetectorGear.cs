using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SwipeDetectorGear : MonoBehaviour
{
    public static SwipeDetectorGear Instance;

    public class SwipeEvent : UnityEvent { }

    private const int mMessageWidth = 200;
    private const int mMessageHeight = 64;

    private readonly Vector2 mXAxis = new Vector2(1, 0);
    private readonly Vector2 mYAxis = new Vector2(0, 1);

    private readonly string[] mMessage = {
        "",
        "Swipe Left",
        "Swipe Right",
        "Swipe Top",
        "Swipe Bottom"
    };

    private int mMessageIndex = 0;

    // The angle range for detecting swipe
    private const float mAngleRange = 30;

    // To recognize as swipe user should at lease swipe for this many pixels
    private const float mMinSwipeDist = 20.0f;

    // To recognize as a swipe the velocity of the swipe
    // should be at least mMinVelocity
    // Reduce or increase to control the swipe speed
    private const float mMinVelocity = 650.0f;

    private Vector2 mStartPosition;
    private float mSwipeStartTime;

    private SwipeEvent OnSwipeTopEvent;
    private SwipeEvent OnSwipeDownEvent;
    private SwipeEvent OnSwipeLeftEvent;
    private SwipeEvent OnSwipeRightEvent;

    void Awake()
    {
        if(!Instance) Instance = this;

        OnSwipeTopEvent = new SwipeEvent();
        OnSwipeDownEvent = new SwipeEvent();
        OnSwipeLeftEvent = new SwipeEvent();
        OnSwipeRightEvent = new SwipeEvent();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        // Mouse button down, possible chance for a swipe
        if (Input.GetMouseButtonDown(0))
        {
            // Record start time and position
            mStartPosition = new Vector2(Input.mousePosition.x,
                                         Input.mousePosition.y);
            mSwipeStartTime = Time.time;
        }

        // Mouse button up, possible chance for a swipe
        if (Input.GetMouseButtonUp(0))
        {
            float deltaTime = Time.time - mSwipeStartTime;

            Vector2 endPosition = new Vector2(Input.mousePosition.x,
                                               Input.mousePosition.y);
            Vector2 swipeVector = endPosition - mStartPosition;

            float velocity = swipeVector.magnitude / deltaTime;

            if (velocity > mMinVelocity &&
                swipeVector.magnitude > mMinSwipeDist)
            {
                // if the swipe has enough velocity and enough distance

                swipeVector.Normalize();

                float angleOfSwipe = Vector2.Dot(swipeVector, mXAxis);
                angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;

                // Detect left and right swipe
                if (angleOfSwipe < mAngleRange)
                {
                    OnSwipeRight();
                }
                else if ((180.0f - angleOfSwipe) < mAngleRange)
                {
                    OnSwipeLeft();
                }
                else
                {
                    // Detect top and bottom swipe
                    angleOfSwipe = Vector2.Dot(swipeVector, mYAxis);
                    angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;
                    if (angleOfSwipe < mAngleRange)
                    {
                        OnSwipeTop();
                    }
                    else if ((180.0f - angleOfSwipe) < mAngleRange)
                    {
                        OnSwipeBottom();
                    }
                    else
                    {
                        mMessageIndex = 0;
                    }
                }
            }
        }
    }

    public void AddSwipeDownListener(UnityAction action)
    {
        OnSwipeDownEvent.AddListener(action);
    }

    public void AddSwipeTopListener(UnityAction action)
    {
        OnSwipeTopEvent.AddListener(action);
    }

    public void AddSwipeLeftListener(UnityAction action)
    {
        OnSwipeLeftEvent.AddListener(action);
    }

    public void AddSwipeRightListener(UnityAction action)
    {
        OnSwipeRightEvent.AddListener(action);
    }

    void OnGUI()
    {
        // Display the appropriate message
        GUI.Label(new Rect((Screen.width - mMessageWidth) / 2,
                           (Screen.height - mMessageHeight) / 2,
                            mMessageWidth, mMessageHeight),
                  mMessage[mMessageIndex]);
    }

    private void OnSwipeLeft()
    {
        //add some functionality in the future?

        OnSwipeLeftEvent.Invoke();
    }

    private void OnSwipeRight()
    {
        //add some functionality in the future?
        OnSwipeRightEvent.Invoke();
    }

    private void OnSwipeTop()
    {
        //add some functionality in the future?
        OnSwipeTopEvent.Invoke();
    }

    private void OnSwipeBottom()
    {
        //add some functionality in the future?

        OnSwipeDownEvent.Invoke();
    }
}