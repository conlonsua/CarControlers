using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    [Header("Wheels Collider")]
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider backLeftWheelCollider;
    public WheelCollider backRightWheelCollier;

    [Header("Wheels Transform")]
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform backLeftWheelTransform;
    public Transform backRightWheelTransform;

    [Header("Car Engine")]
    //lực gia tốc tối đa
    public float accenlerationForce = 300f;
    //lực phanh tối đa
    public float breakingForce = 3000f;
    //Lực phanh hiện tại
    private float presentBreakForce = 0f;
    //lực gia tốc hiện tại
    private float presentAcceleration = 0f;

    [Header("Car Steering")]
    public float maxSteeringAngle = 45f;        // Góc lái tối đa (thay wheelsTorque)
    public float steeringSensitivity = 4.5f;    // Hệ số nhạy của góc lái
    private float presentTurAngle = 0f;

    [Header("Car Sound")]
    public AudioSource audioSource;
    public AudioClip accelerationSound;
    public AudioClip slowAcceleraionSound;
    public AudioClip stopSound;
    
    [Header("Timer")]
    public float raceTime = 0f;
    private bool isRacing = false;

    private void Start() {
        isRacing = true;
    }

    private void Update(){
        if (isRacing) {
            raceTime += Time.deltaTime;
        }
        MoveCar();
        CarSteering();
    }

    private void MoveCar(){
        //hệ thống dẫn động bánh trước
        frontLeftWheelCollider.motorTorque = presentAcceleration;
        frontRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollier.motorTorque = presentAcceleration;

        presentAcceleration = accenlerationForce * SimpleInput.GetAxis("Vertical");

        if(presentAcceleration > 0){
            audioSource.PlayOneShot(accelerationSound, 0.2f);
        }
        else if (presentAcceleration < 0){
            audioSource.PlayOneShot(slowAcceleraionSound, 0.2f);
        }
        else if(presentAcceleration == 0){
            audioSource.PlayOneShot(stopSound, 0.1f);
        }
    }

    private void CarSteering(){
        // Tính góc lái với độ nhạy và giới hạn góc tối đa
        float steeringInput = SimpleInput.GetAxis("Horizontal");
        presentTurAngle = maxSteeringAngle * steeringInput * steeringSensitivity;
        
        // Giới hạn góc lái trong khoảng [-maxSteeringAngle, maxSteeringAngle]
        presentTurAngle = Mathf.Clamp(presentTurAngle, -maxSteeringAngle, maxSteeringAngle);
        
        // Áp dụng góc lái
        frontLeftWheelCollider.steerAngle = presentTurAngle;
        frontRightWheelCollider.steerAngle = presentTurAngle;

        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);
        SteeringWheels(backRightWheelCollier, backRightWheelTransform);

    }


    void SteeringWheels(WheelCollider WC, Transform WT){
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);

        WT.position = position;
        WT.rotation = rotation;
    }

    //hệ thống phanh
    public void ApplyBreaks(){
        // StartCoroutine(carBreaks());
        if (gameObject.activeInHierarchy) {
            StartCoroutine(carBreaks());
        }
    }

    IEnumerator carBreaks(){
        presentBreakForce = breakingForce;

        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        frontRightWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;
        backRightWheelCollier.brakeTorque = presentBreakForce;

        yield return new WaitForSeconds(2f);

        presentBreakForce = 0f;

        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        frontRightWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;
        backRightWheelCollier.brakeTorque = presentBreakForce;
    }

    public void StopRacing() {
        isRacing = false;
    }
}