using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject objectPf;
    [SerializeField] private Transform spawnPos;
    private GameObject objectInScene;
    private Vector2 offset; //[SerializeField]
    [SerializeField] private Vector2 size;

    [Header("Object Settings")]
    [SerializeField] private Vector3 objectScale;

    private bool canSpawn = true;

    void Update()
    {
        if(canSpawn)
        {
            objectInScene = Instantiate(objectPf, spawnPos.position, spawnPos.rotation);

            objectInScene.transform.localScale = objectScale;
            objectInScene.transform.parent = this.transform;
            
            canSpawn = false;
        }

        if(objectInScene == null)
            canSpawn = true;

        DestroyObjectHandler();
    }

    private void DestroyObjectHandler()
    {
        if (objectInScene != null)
        {
            Vector2 sizeRangeSpawner = new Vector2((size.x / 2), size.y / 2);

            float objectSceneDistanceX = Mathf.Abs(ObjectInSceneDistanceX());
            float objectSceneDistanceY = Mathf.Abs(ObjectInSceneDistanceY());

            if (objectSceneDistanceX >= sizeRangeSpawner.x || objectSceneDistanceY >= sizeRangeSpawner.y)
                objectInScene.GetComponent<IDestroyable>().DestroyObject();
        }
    }

    private float ObjectInSceneDistanceX()
    {
        return Vector2.Distance(objectInScene.transform.position, transform.position);
    }
    private float ObjectInSceneDistanceY()
    {
        return transform.position.y - objectInScene.transform.position.y;
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        
        Vector3 newPos = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);
        Gizmos.DrawWireCube(newPos, size);

    }
}
