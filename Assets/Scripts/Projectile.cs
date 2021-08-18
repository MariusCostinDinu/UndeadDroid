using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Projectile : MonoBehaviour
{
    public enum Direction
    {
        LEFT,
        RIGHT
    }
    public float speed;
    public Direction direction;
    Rigidbody2D rb;
    GameObject walls;
    Tilemap wallTilemap;
    public TileBase altTile;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walls = GameObject.Find("Walls");
        wallTilemap = walls.GetComponent<Tilemap>();
    }

    void Update()
    {
        rb.velocity = (direction == Direction.LEFT)
            ? speed * Time.deltaTime * Vector3.left * 200
            : speed * Time.deltaTime * Vector3.right * 200
            ;
        /*transform.position += (direction == Direction.LEFT)
            ? speed * Time.deltaTime * Vector3.left
            : speed * Time.deltaTime * Vector3.right
            ;*/
    }

    Vector3Int V3ToV3Int(Vector3 v3int)
    {
        return new Vector3Int((int)v3int.x, (int)v3int.y, 0);
    }

    Vector3 Vec3DividedByVec3(Vector3 a, Vector3 b, bool pozX, bool pozY)
    {
        /*return new Vector3(
            Mathf.Sign(a.x) * (Mathf.Abs(a.x) / b.x),
            Mathf.Sign(a.y) * (Mathf.Abs(a.y) / b.y),
            Mathf.Sign(a.z) * (Mathf.Abs(a.z) / b.z)
        );*/
        int resX = (int)a.x / (int)b.x;
        int resY = (int)a.y / (int)b.y;
        int resZ = (int)a.z / (int)b.z;

        if (!pozX) resX--;
        if (!pozY) resY--;

        return new Vector3(resX, resY, resZ);
    }

    private void FixedUpdate()
    {
        if (wallTilemap.GetTile(
            V3ToV3Int(
                Vec3DividedByVec3(
                    transform.position - walls.transform.position, 
                    walls.transform.localScale,
                    Mathf.Sign(transform.position.x) == 1,
                    Mathf.Sign(transform.position.y) == 1
                )
            )
        ) != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyHealth>().hits++;
            if (collision.gameObject.GetComponent<EnemyHealth>().hits == 3)
                Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
