using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    float horizontalAxis;
    float verticalAxis;
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float acceleration;
    [SerializeField] Rigidbody2D rb2d;

    [SerializeField] float decelerationRate;

    private Camera mainCamera;
    private Vector2 screenBounds;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }

    private void FixedUpdate()
    {
        CostraintsPlayer();
        horizontalAxis = joystick.Horizontal;
        verticalAxis = joystick.Vertical;

        Vector2 dir = new Vector2(horizontalAxis, verticalAxis);

        float angle = Mathf.Atan2(verticalAxis, horizontalAxis) * Mathf.Rad2Deg;

        // Applicare accelerazione se ci sono input
        if (dir.magnitude > 0)
        {
            rb2d.AddForce(dir * acceleration, ForceMode2D.Force);
        }
        else // Altrimenti applicare decelerazione
        {
            rb2d.velocity = Vector2.Lerp(rb2d.velocity, Vector2.zero, Time.deltaTime * decelerationRate);
        }

        // Impostare Velocità massima
        if (rb2d.velocity.magnitude >= maxSpeed)
        {
            rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
        }

        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void CostraintsPlayer()
    {
        Vector3 newPosition = transform.position;

        if (transform.position.x > screenBounds.x)
        {
            newPosition.x = -screenBounds.x;
        }
        else if (transform.position.x < -screenBounds.x)
        {
            newPosition.x = screenBounds.x;
        }

        if (transform.position.y > screenBounds.y)
        {
            newPosition.y = -screenBounds.y;
        }
        else if (transform.position.y < -screenBounds.y)
        {
            newPosition.y = screenBounds.y;
        }

        transform.position = newPosition;
    }
}



/*
float x = transform.position.x + joystick.Horizontal;
float y = transform.position.y + joystick.Vertical;
transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, y, 0f), movementSpeed);*/
