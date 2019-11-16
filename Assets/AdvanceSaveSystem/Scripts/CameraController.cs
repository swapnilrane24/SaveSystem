using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject target; //set pare car as target

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        //get the tdistance offset on all axis
        offset = transform.position - target.transform.position;
    }

    //we are using fixedupdate instead of lateupdate because the target object is update in fixed update
    //and fps of lateupdate and fixedupdate is different which causes jettery effect
    //lateupdate can only be used when target object is updating in Update method
    void FixedUpdate()
    {
        //if target in not null
        if (target != null)
            transform.position = target.transform.position + offset;//we add the offset to target pos and set camera transform
    }
}
