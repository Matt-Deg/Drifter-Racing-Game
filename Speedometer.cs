/// <summary>
/// Represents a speedometer GameObject in the scene that displays the current speed.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    // Constants for defining the angles of the speedometer needle
    private const float MAX_SPEED_ANGLE = -20;
    private const float ZERO_SPEED_ANGLE = 230;

    // Transform of the needle GameObject and the speed label template GameObject
    private Transform needleTranform;
    private Transform speedLabelTemplateTransform;

    // Maximum speed of the speedometer and current speed
    private float speedMax;
    private float speed;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Find and assign the transforms of the needle and speed label template GameObjects
        needleTranform = transform.Find("needle");
        speedLabelTemplateTransform = transform.Find("speedLabelTemplate");
        
        // Deactivate the speed label template GameObject
        speedLabelTemplateTransform.gameObject.SetActive(false);

        // Initialize speed and speedMax variables
        speed = 0f;
        speedMax = 180f;

        // Create speed labels around the speedometer dial
        CreateSpeedLabels();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Handle player input to adjust speed
        HandlePlayerInput();

        // Rotate the speedometer needle based on the current speed
        needleTranform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    /// <summary>
    /// Handles player input to adjust the speed.
    /// </summary>
    private void HandlePlayerInput()
    {
        // Check for acceleration input
        if (Input.GetKey(KeyCode.UpArrow))
        {
            float acceleration = 30f;
            speed += acceleration * Time.deltaTime;
        }
        // Check for deceleration input
        else
        {
            float deceleration = 20f;
            speed -= deceleration * Time.deltaTime;
        }

        // Check for braking input
        if (Input.GetKey(KeyCode.DownArrow))
        {
            float brakeSpeed = 100f;
            speed -= brakeSpeed * Time.deltaTime;
        }

        // Clamp the speed within the range of 0 to speedMax
        speed = Mathf.Clamp(speed, 0f, speedMax);
    }

    /// <summary>
    /// Creates speed labels around the speedometer dial.
    /// </summary>
    private void CreateSpeedLabels()
    {
        int labelAmount = 9;
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        // Create speed labels based on the number of labels specified
        for (int i = 0; i <= labelAmount; i++)
        {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount;
            float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(labelSpeedNormalized * speedMax).ToString();
            speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }

        // Set the speedometer needle as the last sibling to ensure it's rendered above other elements
        needleTranform.SetAsLastSibling();
    }

    /// <summary>
    /// Calculates the rotation angle of the speedometer needle based on the current speed.
    /// </summary>
    /// <returns>The rotation angle of the speedometer needle.</returns>
    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        // Normalize the current speed within the range of 0 to 1
        float speedNormalized = speed / speedMax;

        // Calculate the rotation angle of the speedometer needle based on the normalized speed
        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}
