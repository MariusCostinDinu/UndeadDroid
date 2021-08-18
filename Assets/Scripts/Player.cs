using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int playerLife = 100;
    public float runSpeed = 5f;


    Rigidbody2D myRigidBody;
    public HealthBar healthBar;
    public GameObject gameWonObject;
    

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((transform.position.x >= 6.3f && transform.position.x <= 8.7f) && (transform.position.y >= 5.3f && transform.position.y <= 7.7f))
            gameWonObject.SetActive(true);
        else
            gameWonObject.SetActive(false);

        FlipSprite();
        RunV();
        RunH();
        healthBar.SetHealth(playerLife);
    }

    private void RunH()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal") / 1.41f;
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.x);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
    }

    private void RunV()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical") / 1.41f;
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
}
