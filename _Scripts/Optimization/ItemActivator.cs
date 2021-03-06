using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemActivator : MonoBehaviour {

    // --------------------------------------------------
    // Variables:

    public static ItemActivator instance;
    [SerializeField] private int distanceFromPlayer;

    private GameObject player;

    private List<ActivatorItem> activatorItems;

    public List<ActivatorItem> addList;

	// --------------------------------------------------

    void Awake()
    {
        instance = this;
    }

	void Start()
	{
        //player = GameObject.Find("BloquinhoBranco");
        if(BloquinhoBrancoController.instance != null)
            player = BloquinhoBrancoController.instance.gameObject;
        
        activatorItems = new List<ActivatorItem>();
        addList = new List<ActivatorItem>();

        AddToList();
    }

    void AddToList()
    {
        if (addList.Count > 0)
        {
            foreach(ActivatorItem item in addList)
            {
                if (item.item != null)
                {
                    activatorItems.Add(item);
                }
            }

            addList.Clear();
        }

        StartCoroutine("CheckActivation");
    }

	IEnumerator CheckActivation()
    {
        List<ActivatorItem> removeList = new List<ActivatorItem>();

        if (activatorItems.Count > 0)
        {
            foreach (ActivatorItem item in activatorItems)
            {
                if(item.item != null)
                {
                    if (Vector3.Distance(player.transform.position, item.item.transform.position) > distanceFromPlayer)
                    {
                        if (item.item == null)
                        {
                            removeList.Add(item);
                        }
                        else
                        {
                            item.item.SetActive(false);
                        }
                    }
                    else
                    {
                        if (item.item == null)
                        {
                            removeList.Add(item);
                        }
                        else
                        {
                            item.item.SetActive(true);
                        }
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
        }

        yield return new WaitForSeconds(0.01f);

        if (removeList.Count > 0)
        {
            foreach (ActivatorItem item in removeList)
            {
                activatorItems.Remove(item);
            }
        }

        yield return new WaitForSeconds(0.01f);

        AddToList();
    }
}

public class ActivatorItem
{
    public GameObject item;
}
