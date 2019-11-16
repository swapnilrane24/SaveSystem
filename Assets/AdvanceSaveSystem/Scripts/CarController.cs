using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    //move and rotation speed
    [SerializeField] private float speed, rotationSpeed;
    [SerializeField] private bool playerCar = false;    //check for deciding car
    [SerializeField] private GameObject explosion;  //ref to prefab

    private Rigidbody rb;   
    private float translation, rotation;
    private GameObject target;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (UIManager.instance.GameOver)
        {
            if(rb.isKinematic == false) rb.isKinematic = true;
            return;
        }
        if (playerCar)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
            {
                rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
                translation = Input.GetAxis("Vertical") * speed;
            }
        }
        else
        {
            Vector3 targetDirection = transform.position - target.transform.position;
            targetDirection.Normalize();

            rotation = Vector3.Cross(targetDirection, transform.forward).y;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PickUp":  //if its pickup
                //incrase the score
                UIManager.instance.IncreaseScore();
                //destory pickup
                PickUpSpawner.instance.DestroyPickUp(other.gameObject);
                //spawn new pickup
                PickUpSpawner.instance.SpawnPickUp();
                break;
            case "Enemy":   //if it enemy
                //call GameOverMethod of UIManager
                UIManager.instance.GameOverMethod();
                //spawn the explosion
                Instantiate(explosion, transform.position, Quaternion.Euler(new Vector3(-90,0,0)));
                Destroy(other.gameObject);  //destroy player car
                Destroy(gameObject);    //destroy enemy car
                break;
        }
    }

    private void FixedUpdate()
    {
        if (UIManager.instance.GameOver) return;
        if (playerCar)
        {
            // Add velocity along the object's z-axis
            rb.velocity = transform.forward * translation;
            // Rotate around our y-axis
            transform.Rotate(Vector3.up * rotation);
        }
        else
        {
            // Rotate around our y-axis
            rb.angularVelocity = rotationSpeed * rotation * new Vector3(0, 1, 0);
            rb.velocity = transform.forward * speed; //add velocity in forward direction of object
        }
    }
}
