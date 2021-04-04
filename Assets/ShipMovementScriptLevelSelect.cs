using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementScriptLevelSelect : MonoBehaviour
{

    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float steeringPower;
    [SerializeField] float movementAcceleration;

    private float steeringAmount, speed, direction;
    private Vector2 movement;

    void Update()
    {

        steeringAmount = Input.GetAxis("Horizontal");
        speed = - Input.GetAxis("Vertical") * movementAcceleration;
        direction = Mathf.Sign(Vector2.Dot(rigidBody.velocity, rigidBody.GetRelativeVector(Vector2.up)));
    }

    private void FixedUpdate()
    {
        //rigidBody.MovePosition(rigidBody.position + movement * movementSpeed * Time.fixedDeltaTime);
        rigidBody.rotation += steeringAmount * steeringPower * rigidBody.velocity.magnitude * direction * Time.fixedDeltaTime;

        rigidBody.AddRelativeForce(Vector2.up * speed);

        rigidBody.AddRelativeForce(Vector2.right * rigidBody.velocity.magnitude * steeringAmount / 2);
    }
}
