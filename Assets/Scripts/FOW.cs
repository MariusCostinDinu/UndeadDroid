using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOW : MonoBehaviour
{
    [SerializeField] public float viewRadius = 5;
    [SerializeField] public float viewAngle = 135;
    [SerializeField] public LayerMask obstacleMask, playerMask;
    Collider2D[] playerInRadius;
    public List<Transform> visiblePlayer = new List<Transform>();
     void FixedUpdate()
    {
        FindVisiblePlayer();
    }

    void FindVisiblePlayer()
    {
        playerInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        for (int i = 0; i < playerInRadius.Length; i++)
        {
            Transform player = playerInRadius[i].transform;
            Vector2 dirPlayer = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);
            if (Vector2.Angle(dirPlayer, transform.right) < viewAngle / 2)
            {
                float distancePlayer = Vector2.Distance(transform.position, player.position);

                if(Physics2D.Raycast(transform.position,dirPlayer,distancePlayer,obstacleMask))
                {
                    visiblePlayer.Add(player);
                }
            }
        }
    }

    public Vector2 DirFromAngle(float angleDeg, bool global)
    {
        if (!global)
        {
            angleDeg += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Cos(angleDeg * Mathf.Deg2Rad));
    }
}
