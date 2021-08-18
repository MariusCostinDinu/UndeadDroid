using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public float speed;
    public float checkRadius;
    public float attackRadius;
    public float startTimeBtwAttacks;
    private float timeBtwAttacks;
    public Text healthText;

    public bool shouldRotate;

    GameObject player;

    public LayerMask whatIsPlayer;

    private Transform target;
    private Animator anim;
    public Vector3 dir;


    private bool isInChaseRange;
    private bool isInAttackRange;

    private void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        timeBtwAttacks = startTimeBtwAttacks;
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        anim.SetBool("isRunning", isInChaseRange);

        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

        //Debug.Log("Chase Range => " + isInChaseRange);
        //Debug.Log("attack Range => " + isInAttackRange);
        //Debug.Log(timeBtwAttacks);
        if (isInAttackRange)
        {
            if (timeBtwAttacks <= 0)
            {
                player.GetComponent<Player>().playerLife -= 10;
                //Debug.Log(player.GetComponent<Player>().playerLife);
                timeBtwAttacks = startTimeBtwAttacks;
            }
            else
            {
                timeBtwAttacks -= Time.deltaTime;
            }
        }

        dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        if (shouldRotate)
        {
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
        }

        //healthText.text = "Health:" + player.GetComponent<Player>().playerLife.ToString();

        if (player.GetComponent<Player>().playerLife < 10)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
