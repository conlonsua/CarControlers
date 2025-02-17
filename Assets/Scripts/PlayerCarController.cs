using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    public enum CarType
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }

    public CarType carType = CarType.FourWheelDrive;

    public enum ControlMode
    {
        Keyboard,
        Button
    };

    public ControlMode control;

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
    public float accenlerationForce = 300f;
    public float breakingForce = 3000f;
    private float presentBreakForce = 0f;
    private float presentAcceleration = 0f;

    [Header("Car Steering")]
    public float wheelsTorque = 150f;
    private float presentTurAngle = 0f;
    public float maxSteeringAngle = 45f; // Tăng góc lái tối đa
    public float steeringSensitivity = 4.5f; // Hệ số nhạy của góc lái

    [Header("Car Sound")]
    public AudioSource audioSource;
    public AudioClip accelerationSound;
    public AudioClip slowAcceleraionSound;
    public AudioClip stopSound;

    // Thêm biến cho điều khiển
    private float vertical = 0f;
    private float horizontal = 0f;

    void Update(){
        GetInput();  // Lấy input từ bàn phím hoặc nút bấm
        MoveCar();   // Di chuyển xe
        CarSteering();  // Lái xe
    }

    // Lấy input từ bàn phím hoặc nút bấm
    void GetInput()
    {
        if (control == ControlMode.Keyboard)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        else if (control == ControlMode.Button)
        {
            // Điều khiển bằng nút bấm
            horizontal = SimpleInput.GetAxis("Horizontal");  // Sử dụng SimpleInput cho ngang
            vertical = SimpleInput.GetAxis("Vertical");  // Sử dụng SimpleInput cho dọc
        }
    }

    void MoveCar(){
        // Cập nhật động cơ
        frontLeftWheelCollider.motorTorque = presentAcceleration;
        frontRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollier.motorTorque = presentAcceleration;

        presentAcceleration = accenlerationForce * vertical;

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

    void CarSteering(){
        // Điều chỉnh góc lái theo tốc độ và độ nhạy
        float speedFactor = Mathf.Clamp(frontLeftWheelCollider.rpm / 1000f, 0.5f, 1f); // Tốc độ càng cao, góc lái càng nhỏ
        float dynamicSteeringAngle = maxSteeringAngle * steeringSensitivity * speedFactor;

        presentTurAngle = dynamicSteeringAngle * horizontal;

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

    public void ApplyBreaks(){
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
}
