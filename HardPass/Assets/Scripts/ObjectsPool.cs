using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    public List<GameObject> Pool;

    public void ReturnToPool(GameObject poolItem)
    {
        poolItem.GetComponent<Rigidbody>().velocity = new Vector3Int(0, 0, 0);
        poolItem.transform.name = "Object";
        Pool.Add(poolItem);
    }

    public GameObject GetPoolItem()
    {
        Pool[0].transform.name = "Picked";
        GameObject poolItem = Pool[0];
        Pool.RemoveAt(0);
        return poolItem;
    }
}
