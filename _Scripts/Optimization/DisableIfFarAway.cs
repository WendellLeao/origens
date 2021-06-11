using UnityEngine;
using System.Collections;

public class DisableIfFarAway : MonoBehaviour {

    // --------------------------------------------------
    // Variables:

    private GameObject itemActivatorObject;
    private ItemActivator activationScript;

	// --------------------------------------------------

	void Start()
	{
        //itemActivatorObject = GameObject.Find("ItemActivatorObject");
        if(ItemActivator.instance != null)
            itemActivatorObject = ItemActivator.instance.gameObject;

        activationScript = itemActivatorObject.GetComponent<ItemActivator>();

        StartCoroutine("AddToList");
    }

    IEnumerator AddToList()
    {
        yield return new WaitForSeconds(0.1f);

        activationScript.addList.Add(new ActivatorItem { item = this.gameObject });
    }
}
