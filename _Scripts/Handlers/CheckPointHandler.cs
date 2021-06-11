using UnityEngine;

public class CheckPointHandler : MonoBehaviour
{
    public static CheckPointHandler instance;

    [SerializeField] private GameObject checkPointText;
    
    [Header("Positions")]
    private Vector2 lastCheckPointPos;
    private Transform bbPos;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        bbPos = FindObjectOfType<BloquinhoBrancoController>().GetComponent<Transform>();

        lastCheckPointPos = bbPos.position;
    }

    public Vector2 LastCheckPointPos
    {
        get{return this.lastCheckPointPos;}
        set{this.lastCheckPointPos = value;}
    }
}
