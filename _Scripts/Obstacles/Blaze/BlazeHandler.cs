using UnityEngine;

public class BlazeHandler : MonoBehaviour
{
    [SerializeField] private Fire[] fires;
    [SerializeField] private int indexToRestartLoop;
    private int currentIndex = 0;
    private bool canRestartLoop;

    void Start()
    {
        //canRestartLoop = true;
        if(fires.Length > 1)
        {
            //Reset fires
            for (int i = 0; i < fires.Length; i++)
            {
                if(i == 0)
                {
                    fires[i].enabled = true;
                    fires[i].IsDisabled = false;
                }
                else
                {
                    fires[i].enabled = false;
                    fires[i].IsDisabled = true;
                }
            }
        }
        else
        {
            fires[0].enabled = true;
            fires[0].IsDisabled = false;
        }
    }

    void Update()
    {
        if(fires.Length > 1)
        {
            HandleBlaze();
        }
        else
        {
            if(fires[0].IsDisabled)
            {
                fires[0].enabled = true;
                fires[0].IsDisabled = false;
            }
        }
    }

    private void HandleBlaze()
    {
        for (int i = 0; i < fires.Length; i++)
        {
            //If the fire is firing so the next fire can start
            if (fires[i].IsOnFire && !fires[i].IsDisabled)
            {
                int nextIndex = i + 1;
                if (nextIndex < fires.Length)
                {
                    fires[nextIndex].enabled = true;
                    fires[nextIndex].IsDisabled = false;
                }
            }

            //If the last fire is disabled the loop must continue
            if(i == indexToRestartLoop)//fires.Length - 1
            {
                if(fires[i].IsDisabled)
                    canRestartLoop = true;
            }
        }

        if(canRestartLoop)
        {
            fires[0].enabled = true;
            fires[0].IsDisabled = false;

            canRestartLoop = false;
        }
    }
}
