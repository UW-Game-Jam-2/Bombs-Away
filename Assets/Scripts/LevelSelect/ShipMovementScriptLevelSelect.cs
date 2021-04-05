using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipMovementScriptLevelSelect : MonoBehaviour
{

    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float steeringPower;
    [SerializeField] float movementAcceleration;
    [SerializeField] SceneFader sceneFader;

    public static event Action<string> didCollideWithIsland;
    public static event Action didTriggerOpenOcean;


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

        float minimumTurning = rigidBody.velocity.magnitude;

        rigidBody.rotation += steeringAmount * steeringPower * minimumTurning * direction * Time.fixedDeltaTime;

        rigidBody.AddRelativeForce(Vector2.up * speed);

        rigidBody.AddRelativeForce(Vector2.right * minimumTurning * steeringAmount / 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Boundary")
        {
            print(collision.gameObject.name);
            didCollideWithIsland?.Invoke(collision.gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        didTriggerOpenOcean?.Invoke();
    }
}
