using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robber : MonoBehaviour
{
    public enum eMode { idle, move, planting, die }
    [Header("Set in inspector")]
    public int speed = 3;
    public float timeForPlanting = 3;
    public GameObject bombPreffab;
    public Joystick joystick;
    public BombButton bombButt;
    public Game gameInsp;

    [Header("Set Dinamically")]
    public int dirHeld = -1;
    public eMode mode = eMode.idle;
    private Point dangerousrPlase;
    public float gridMult = 1.5f;

    public int facing = 1;

    private bool plantOpportunity = false;
    private float timePlantingDone;
    public Vector2 roomPos { get { return transform.position; } set { transform.position = value; } }
    private Vector3[] direction = new Vector3[] { Vector3.right, Vector3.up, Vector3.left, Vector3.down };

    private SpriteRenderer sRend;
    private Rigidbody2D rigid;
    private Image bombImage;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sRend = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        bombImage = bombButt.GetComponent<Image>();
    }

    private void Update()
    {
        Vector3 vel = Vector3.zero;
        dirHeld = -1;
        for (int i = 0; i < 4; i++)
        {
            if (direction[i] == joystick.Direction()) dirHeld = i;
        }

        if (mode != eMode.planting && mode != eMode.die)
        {
            if (dirHeld == -1)
            {
                mode = eMode.idle;
            }
            else
            {
                facing = dirHeld;
                mode = eMode.move;
            }
        }

        switch (mode)
        {
            case eMode.idle:
                anim.CrossFade("RobberIdle", 0);
                break;
            case eMode.move:
                anim.CrossFade("RobberRun", 0);
                if (dirHeld == 2)
                {
                    sRend.flipX = true;
                }
                else if (dirHeld == 0)
                {
                    sRend.flipX = false;
                }
                vel = joystick.Direction();
                break;
            case eMode.planting:
                anim.CrossFade("RobberPlant", 0);
                if (dirHeld != -1)
                {
                    mode = eMode.move;
                    bombButt.Reload();
                }
                if (Time.time >= timePlantingDone)
                {
                    dangerousrPlase.plant = true;
                    dangerousrPlase = null;
                    mode = eMode.idle;
                    bombButt.Reload();
                }
                break;
            case eMode.die:
                break;
        }
        rigid.velocity = vel * speed;
    }
    public void PlantBomb()
    {
        if (plantOpportunity)
        {
            gameInsp.SendMessage("Planting");
            timePlantingDone = Time.time + timeForPlanting;
            mode = eMode.planting;
            return;
        }
        GameObject bomb = Instantiate(bombPreffab);
        bomb.transform.position = transform.position + direction[facing];
        bombButt.Reload();
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        string tagObj = coll.tag;
        switch (tagObj)
        {
            case "Bomb":
                Bomb b = coll.GetComponent<Bomb>();
                b.Explousion();
                mode = eMode.die;
                gameInsp.SendMessage("Loose");
                anim.Play("RobberDie");
                Invoke("StopAnim", 3);
                break;
            case "Enemy":
                mode = eMode.die;
                Enemy e = coll.GetComponent<Enemy>();
                e.Attack();
                gameInsp.SendMessage("Loose");
                anim.Play("RobberDie");
                Invoke("StopAnim", 3);
                break;
            case "Point":
                Point p = coll.GetComponent<Point>();
                if (p.plant) return;
                dangerousrPlase = p;
                plantOpportunity = true;
                bombImage.color = Color.red;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (plantOpportunity)
        {
            dangerousrPlase = null;
            plantOpportunity = false;
            bombImage.color = Color.white;
        }
    }

    public int GetFacing()
    {
        return facing;
    }

    public int GetSpeed()
    {
        return speed;
    }

    void StopAnim()
    {
        anim.speed = 0;
    }
    public Vector2 GetRoomPosOnGrid(float mult = -1)
    {
        if (mult == -1)
        {
            mult = gridMult;
        }
        Vector2 rPos = transform.position;
        rPos /= mult;
        rPos.x = Mathf.Round(rPos.x);
        rPos.y = Mathf.Round(rPos.y);
        rPos *= mult;
        return rPos;
    }
}
