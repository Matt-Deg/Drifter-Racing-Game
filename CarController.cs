/// <summary>
/// This script controls the behavior of a car in a Unity game.
/// It handles player input for steering, acceleration, and braking,
/// applies these inputs to the car's physics, and updates the visual
/// representation of the car's wheels.
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Constants for input axis names
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    // Input variables
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    // Motor parameters
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    // Wheel colliders for physics simulation
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    // Wheel transforms for visual representation
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;

    /// <summary>
    /// Update the car's physics and visual representation.
    /// </summary>
    private void FixedUpdate()
    {
        GetInput(); // Get input from the player
        HandleMotor(); // Apply acceleration and braking to the car
        HandleSteering(); // Apply steering angle to the car
        UpdateWheels(); // Update the visual representation of the wheels
    }

    /// <summary>
    /// Get input from the player for steering, acceleration, and braking.
    /// </summary>
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL); // Get horizontal input (e.g., left/right arrow keys)
        verticalInput = Input.GetAxis(VERTICAL); // Get vertical input (e.g., up/down arrow keys)
        isBreaking = Input.GetKey(KeyCode.Space); // Check if the brake key (spacebar) is pressed
    }

    /// <summary>
    /// Apply acceleration and braking to the car based on player input.
    /// </summary>
    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce; // Apply motor torque to the front left wheel
        frontRightWheelCollider.motorTorque = verticalInput * motorForce; // Apply motor torque to the front right wheel
        currentbreakForce = isBreaking ? breakForce : 0f; // Set the current braking force based on whether the brake key is pressed
        ApplyBreaking(); // Apply braking force to all wheels
    }

    /// <summary>
    /// Apply braking force to all wheels based on player input.
    /// </summary>
    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce; // Apply braking force to the front right wheel
        frontLeftWheelCollider.brakeTorque = currentbreakForce; // Apply braking force to the front left wheel
        rearLeftWheelCollider.brakeTorque = currentbreakForce; // Apply braking force to the rear left wheel
        rearRightWheelCollider.brakeTorque = currentbreakForce; // Apply braking force to the rear right wheel
    }

    /// <summary>
    /// Apply steering angle to the car based on player input.
    /// </summary>
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput; // Calculate the current steering angle based on horizontal input
        frontLeftWheelCollider.steerAngle = currentSteerAngle; // Apply the steering angle to the front left wheel
        frontRightWheelCollider.steerAngle = currentSteerAngle; // Apply the steering angle to the front right wheel
        rearLeftWheelCollider.steerAngle = currentSteerAngle; // Apply the steering angle to the rear left wheel
        rearRightWheelCollider.steerAngle = currentSteerAngle; // Apply the steering angle to the rear right wheel
    }

    /// <summary>
    /// Update the visual representation of the car's wheels.
    /// </summary>
    private void UpdateWheels()
    {
        UpdateFrontWheel(frontLeftWheelCollider, frontLeftWheelTransform); // Update the visual representation of the front left wheel
        UpdateFrontWheel(frontRightWheelCollider, frontRightWheeTransform); // Update the visual representation of the front right wheel
        UpdateRearWheel(rearRightWheelCollider); // Update the visual representation of the rear right wheel
        UpdateRearWheel(rearLeftWheelCollider); // Update the visual representation of the rear left wheel
    }

    /// <summary>
    /// Update the visual representation of a front wheel.
    /// </summary>
    private void UpdateFrontWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot); // Get the position and rotation of the wheel in world space
        wheelTransform.rotation = rot; // Apply the rotation to the wheel's transform
        wheelTransform.position = pos; // Apply the position to the wheel's transform
    }

    /// <summary>
    /// Update the visual representation of a rear wheel.
    /// </summary>
    private void UpdateRearWheel(WheelCollider wheelCollider)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot); // Get the position and rotation of the wheel in world space
        // Note: This method updates the visual representation of the rear wheels but does not apply it to any transforms.
    }
}
