using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
 using UnityEditor;
 using UnityEditor.SceneManagement;
#endif

[System.Serializable]
public class WayPointSystem : MonoBehaviour
{
    public enum StartMode { OnAwake, OnFirstTrigger, OnlyOnce, OnEachTrigger}

    [Header("General Options")]
    [SerializeField] private StartMode start;
    [HideInInspector] public float timeToStart = 0.5f;
    [HideInInspector] public bool loop;

    [Header("Loop Options")]
    [HideInInspector] public bool rewind;
    [HideInInspector] public bool holdBeforeLoop;
    [HideInInspector] public float cooldown; 

    [Header("Points")]
    [SerializeField] private Transform[] wayPoints;

    [Header("Velocity")]
    [SerializeField] private float speed;
    private int wayPointIndex = 0;
    private bool canRewind = false;
    private bool canStart = false;
    private bool canMove;
    private bool canStartTrigged = false;
    private bool firstLoop = true;
    private float timerToHold;
    private float startTimerToHold;
    private bool canStartTimer = false;

    //ADDED - Lift Method
    private Rigidbody2D player;
    private Vector3 moveDelta;

    void Start()
    {
        //timer = cooldown;
        timerToHold = 1.5f;
        startTimerToHold = timerToHold;

        transform.position = wayPoints[wayPointIndex].transform.position;

        timeToStart = 0.5f;

        if (start == StartMode.OnlyOnce)
        {
            loop = false;
            rewind = false;
            canRewind = false;
            holdBeforeLoop = false;
        }

        if(start == StartMode.OnEachTrigger)
        {
            loop = true;
            holdBeforeLoop = false;
        }
        
        if(start == StartMode.OnAwake)
        {
            canStart = true;
            canMove = true;

            //timerToHold = 0f;
        }
        else
        {
            canMove = false;
        }
    }
    void Update()
    {
        if(wayPoints.Length > 1)
        {
            if(loop)
            {
                CheckArrayPositionWithLoop();

                if(holdBeforeLoop && canStart && !firstLoop)
                    HoldingPlatformHandler();
            }
            else
            {
                CheckArrayPositionWithoutLoop();            
            }
        }

        if(start == StartMode.OnEachTrigger)
        {
            if (IsAtTheFirstPoint() || IsAtTheLastPoint())
                canStartTrigged = true;
            else    
                canStartTrigged = false;
        }

        if(canStartTimer)
            StartCoroutine(TimerToStart(timeToStart));

        if(canStart || start == StartMode.OnAwake)
            MovementHandler();
    }

    private void CheckArrayPositionWithoutLoop()
    {
        canRewind = false;
        
        if(IsAtTheFirstPoint())
        {
            if(!firstLoop)
                canMove = false;
        }

        if(IsAtTheCurrentPoint())
        {
            if(!IsAtTheLastPoint())
                wayPointIndex += 1;
        }

        if(IsAtTheLastPoint())
        {
            firstLoop = false;
            canMove = false;
        }
    }
    private void CheckArrayPositionWithLoop()
    {
        if(IsAtTheFirstPoint())
        {
            canRewind = false;

            if(start != StartMode.OnEachTrigger)
            {
                if(!holdBeforeLoop)
                    canMove = true;
            }
            else
            {
                if(!canStartTrigged)
                    canMove = false;
            }
        }

        if(IsAtTheCurrentPoint())
        {
            if (canRewind)
            {
                wayPointIndex -= 1;
            }
            else
            {
                if(!IsAtTheLastPoint())
                    wayPointIndex += 1;
            }
        }

        if(IsAtTheLastPoint())
        {
            firstLoop = false;
            
            if(start != StartMode.OnEachTrigger)
            {
                if(!holdBeforeLoop)
                    canMove = true;
            }
            else
            {
                if(!canStartTrigged)
                    canMove = false;
            }

            if(rewind)
            {
                RewindHandler();
            }
            else
            {
                if (canMove)    
                {
                    canRewind = false;
                    wayPointIndex = 0;
                }
            }
        }
    }

    private void MovementHandler()
    {
        if(canMove)
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex].transform.position, speed * Time.deltaTime);
    }
    private void RewindHandler()
    {
        if (canMove)
        {
            wayPointIndex -= 1;
            canRewind = true;
        }

        if (canRewind)
        {
            if (IsAtTheCurrentPoint())
                wayPointIndex -= 1;
        }
    }
    private void HoldingPlatformHandler()
    {
        if (IsAtTheFirstPoint())
            TimerToMoveAgain();
        else if (IsAtTheLastPoint()) //loop
            TimerToMoveAgain();
        else
            timerToHold = startTimerToHold;
            //timer = cooldown;
    }

    private bool IsAtTheFirstPoint()
    {
        return transform.position == wayPoints[0].position;
    }
    private bool IsAtTheLastPoint()
    {
        return transform.position == wayPoints[wayPoints.Length - 1].position;
    }
    private bool IsAtTheCurrentPoint()
    {
        return transform.position == wayPoints[wayPointIndex].transform.position;
    }

    public float Speed
    {
        get{return speed;}
    }

    public bool IsMovingUp()
    {
        Vector3 dir = Vector3.zero;

        if(canMove)
            dir = (this.transform.position - wayPoints[wayPointIndex].transform.position).normalized;

        return dir.y < 0 && canMove;
    }

    public bool IsMovingDown()
    {
        Vector3 dir = Vector3.zero;

        if(canMove)
            dir = (this.transform.position - wayPoints[wayPointIndex].transform.position).normalized;

        return dir.y > 0 && canMove;
    }

    public bool CanStart
    {
        get{return this.canStart;}
    }
    public bool CanStartTrigged
    {
        get{return this.canStartTrigged;}
    }
    public bool CanStartTimer
    {
        set{this.canStartTimer = value;}
    }
    IEnumerator TimerToStart(float timeToStart)
    {
        yield return new WaitForSeconds(timeToStart);
        canStart = true;
        canMove = true;
        canStartTimer = false;
    }
    private void TimerToMoveAgain()
    {
        if(loop)
        {
            canMove = false;
            timerToHold -= Time.deltaTime;

            if (timerToHold <= 0f)
            {
                canMove = true;
                timerToHold = 0f;
            }
        }
    }

    public Transform[] WayPoints
    {
        get{return wayPoints;}
    }
    public int WayPointIndex
    {
        get{return wayPointIndex;}
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(WayPointSystem))]
    public class MyScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var wayPointSystem = target as WayPointSystem;
 
            // if(wayPointSystem.start != StartMode.OnAwake)
            // {
            //     //wayPointSystem.timeToStart = 0.5f;
            //     //wayPointSystem.timeToStart = EditorGUILayout.FloatField("Time to Start", wayPointSystem.timeToStart);
            // }

            if(wayPointSystem.start != StartMode.OnlyOnce && wayPointSystem.start != StartMode.OnEachTrigger)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Loop Options", EditorStyles.boldLabel);
                wayPointSystem.loop = EditorGUILayout.Toggle("Loop", wayPointSystem.loop);

                if(wayPointSystem.start == StartMode.OnFirstTrigger)
                    wayPointSystem.loop = true;
            }

            if(wayPointSystem.start == StartMode.OnEachTrigger)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Loop Options", EditorStyles.boldLabel);
                wayPointSystem.rewind = EditorGUILayout.Toggle("Rewind", wayPointSystem.rewind);
            }

            if(wayPointSystem.loop && wayPointSystem.start != StartMode.OnlyOnce && wayPointSystem.start != StartMode.OnEachTrigger)
            {
                wayPointSystem.rewind = EditorGUILayout.Toggle("Rewind", wayPointSystem.rewind);
                wayPointSystem.holdBeforeLoop = EditorGUILayout.Toggle("Hold Before Loop", wayPointSystem.holdBeforeLoop);
            }

            //if(wayPointSystem.loop && wayPointSystem.holdBeforeLoop && wayPointSystem.start != StartMode.OnlyOnce && wayPointSystem.start != StartMode.OnEachTrigger)
                //wayPointSystem.cooldown = EditorGUILayout.FloatField("Cooldown", wayPointSystem.cooldown);
                           
            if (GUI.changed)
            {
                EditorUtility.SetDirty(wayPointSystem);
                EditorSceneManager.MarkSceneDirty(wayPointSystem.gameObject.scene);
            }
        }
    }
    #endif
}
