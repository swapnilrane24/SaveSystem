using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    public static PickUpSpawner instance;

    //ref to size of map
    [SerializeField] private Vector2 mapSize;
    //ref to pickup prefab
    [SerializeField] private GameObject pickUpPrefab;

    private List<GameObject> pickUpList;    //store deactive pickup object list

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        //create new list
        pickUpList = new List<GameObject>();
        //sapwn 5 prefab object
        for (int i = 0; i < 5; i++)
        {
            SpawnPickUp();
        }
    }

    public void SpawnPickUp()
    {
        //create gameobject variable
        GameObject pickUpObj = null;
        //check if we have any gameobject in the list
        if (pickUpList.Count > 0)
        {
            //if yes return the 1st element object
            pickUpObj = pickUpList[0];
            pickUpList.RemoveAt(0); //remove it form the list
            //get random position
            Vector3 pos = new Vector3(Random.Range(-mapSize.x, mapSize.x), 1, Random.Range(-mapSize.y, mapSize.y));
            pickUpObj.transform.position = pos; //set the position
            pickUpObj.SetActive(true); //activate the object
        }
        else //of list is empty
        {
            //create new gameobject
            pickUpObj = Instantiate(pickUpPrefab);
            //get random position
            Vector3 pos = new Vector3(Random.Range(-mapSize.x, mapSize.x), 1, Random.Range(-mapSize.y, mapSize.y));
            pickUpObj.transform.position = pos; //set the position
        }

        //set the object parent
        pickUpObj.transform.SetParent(transform);
    }

    public void DestroyPickUp(GameObject pickUpObj)
    {
        pickUpObj.SetActive(false);//deactive object
        pickUpList.Add(pickUpObj);//add to the list
    }
}
