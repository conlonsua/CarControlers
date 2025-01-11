using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCar : MonoBehaviour
{
    [Header("Car Engine")]
    public float maxSpeed;
    public float currentSpeed;
    public float accenleration = 1f;
    public float turningSpeed = 150f;
    public float breakSpeed = 12f;
    public float movingSpeed ;

    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached;

    private void Start(){
        currentSpeed = movingSpeed;
    }

    private void Update(){
        Drive();
    }

    public void Drive(){
        if(!destinationReached){
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;

            if(destinationDistance >= breakSpeed){
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);

                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, accenleration * Time.deltaTime);

                transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
            }
            else{
                destinationReached = true;
            }
        }
    }

    public void LocateDestination (Vector3 destination){
        this.destination =  destination;
        destinationReached = false;

        currentSpeed = movingSpeed;
    }
}
