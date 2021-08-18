using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] int playerLife = 20;
    [SerializeField] float runSpeed = 5f;

    [SerializeField] RuntimeAnimatorController downwardsAnim;
    [SerializeField] RuntimeAnimatorController upwardsAnim;
    [SerializeField] RuntimeAnimatorController leftwardsAnim;
    [SerializeField] RuntimeAnimatorController rightwardsAnim;
    Animator animator;
    Vector2 flow;


    Rigidbody2D myRigidBody;

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flow = Vector2.zero;

    }

    void Update()
    {
        ChangeSprite();
        RunV();
        RunH();
    }

    public void SetFlow(Vector2 newFlow)
    {
        flow = newFlow;
    }

    private void RunH()
    {
        //float flow = CrossPlatformInputManager.GetAxis("Horizontal") / 1.41f;
        Vector2 playerVelocity = new Vector2(flow.x * runSpeed, myRigidBody.velocity.x);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
    }

    private void RunV()
    {
        //float flow = CrossPlatformInputManager.GetAxis("Vertical") / 1.41f;
        Vector2 playerVelocity = new Vector2(flow.y * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
    }

    void ChangeSprite()
    {
        float velX = myRigidBody.velocity.x;
        float velY = myRigidBody.velocity.y;
        if (Mathf.Abs(velX) > Mathf.Abs(velY))
        {
            if (velX > 0)
                animator.runtimeAnimatorController = rightwardsAnim;
            if (velX < 0)
                animator.runtimeAnimatorController = leftwardsAnim;
        } else
        {
            if (velY > 0)
                animator.runtimeAnimatorController = upwardsAnim;
            if (velY < 0)
                animator.runtimeAnimatorController = downwardsAnim;
        }
    }
}
