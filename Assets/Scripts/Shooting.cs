using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Shooting : MonoBehaviour
{
    struct BulletStats
    {
        public GameObject bullet;
        public int shootingPeriod;
        public float speed;

        public BulletStats(GameObject bullet, int shootingPeriod, float speed)
        {
            this.bullet = bullet;
            this.shootingPeriod = shootingPeriod;
            this.speed = speed;
        }
    }

    public ProceduralTilemap pt;
    int shootingPeriod = 20;
    float playerOrientation;
    Player player;
    public GameObject bullet_M1911;
    public GameObject bullet_MP40;
    public GameObject bullet_SHOTGUN;
    Dictionary<ProceduralTilemap.ToolType, BulletStats> bulletType;

    void Start()
    {
        player = GetComponent<Player>();
        bulletType = new Dictionary<ProceduralTilemap.ToolType, BulletStats>();
        playerOrientation = 1;

        bulletType[ProceduralTilemap.ToolType.M1911] = new BulletStats(bullet_M1911, 70, 15.0f);
        bulletType[ProceduralTilemap.ToolType.MP40] = new BulletStats(bullet_MP40, 40, 20.0f);
        bulletType[ProceduralTilemap.ToolType.SHOTGUN] = new BulletStats(bullet_SHOTGUN, 200, 15.0f);
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetAxis("Horizontal") != 0)
            playerOrientation = CrossPlatformInputManager.GetAxis("Horizontal");

        if (pt.isTrigger)
            if ((Time.frameCount - pt.triggerStartFrame) % bulletType[pt.currentTool].shootingPeriod == 0)
            {
                GameObject newBullet = Instantiate(bulletType[pt.currentTool].bullet, gameObject.transform.position, Quaternion.identity);
                newBullet.GetComponent<Projectile>().direction = (playerOrientation < 0)
                    ? Projectile.Direction.LEFT
                    : Projectile.Direction.RIGHT;
                newBullet.GetComponent<Projectile>().speed = bulletType[pt.currentTool].speed;
            }
    }
}
