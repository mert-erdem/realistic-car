using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CarController : MonoBehaviour
{
    [SerializeField] private List<AxleInfo> axles;

    [Header("Core Specs")]
    [SerializeField] [Range(10f, 1000f)] private float maxForwardMotorTorque;
    [SerializeField] [Range(10f, 250f)] private float maxBackwardMotorTorque;
    [SerializeField] [Range(10f, 360f)] [Tooltip("20~60")] private float maxSteeringAngle;

    [SerializeField] [Tooltip("Must be bigger than maxMotorTorque")] private float breakForce;

    //inputs
    private float motor, steering;

    private void FixedUpdate()
    {
        motor = Input.GetAxis("Vertical");
        steering = Input.GetAxis("Horizontal");

        ApplyInputsToAxles();
    }

    private void ApplyInputsToAxles()
    {
        foreach (AxleInfo axle in axles)
        {
            if (axle.canSteering)
                Steer(axle);

            if (axle.hasMotor)
                Accelerate(axle);

            if (Input.GetKey(KeyCode.Space))
                Break(axle);
        }
    }

    private void Steer(AxleInfo axle)
    {
        axle.leftWheel.steerAngle = maxSteeringAngle * steering;
        axle.rightWheel.steerAngle = maxSteeringAngle * steering;
    }

    private void Accelerate(AxleInfo axle)
    {
        axle.leftWheel.brakeTorque = 0;
        axle.rightWheel.brakeTorque = 0;

        axle.leftWheel.motorTorque = maxForwardMotorTorque * motor;
        axle.rightWheel.motorTorque = maxForwardMotorTorque * motor;
    }

    private void Break(AxleInfo axle)
    {
        axle.leftWheel.brakeTorque = breakForce;
        axle.rightWheel.brakeTorque = breakForce;
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool hasMotor;
    public bool canSteering;
}
