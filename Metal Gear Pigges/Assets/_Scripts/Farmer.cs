using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Enemy
{
    public enum eMode { patrol, run, diffusing, stun, attack, idle }

    [Header("Set in inspector")]
    public float DetectionDistance = 3f;
    public float defuseDuration = 6f;
    public int pointInLvl = 4;
    public Sprite looseSprite;

    [Header("Set Dinamically")]
    public int facing = 1;
    public int currentPoint = 1;
    public eMode mode = eMode.patrol;
    private Point dangerousPlase;

    public List<Point> points;
    private Robber player;
    private Transform potentialPlayPos;

    [SerializeField] private LayerMask layerMask;

    private float defuseDone;
    protected override void Awake()
    {
        base.Awake();
        player = playerTransform.gameObject.GetComponent<Robber>();
    }

    private void Start()
    {
        potentialPlayPos = new GameObject("PotenPos").transform;
        Target = points[currentPoint].transform;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void FixedUpdate()
    {
        if (Target == null || playerTransform == null) return;
        if (attackDone != 0) mode = eMode.attack;
        sRend.flipX = agent.velocity.x < 0;
        float distanceToPlayer = Vector3.Distance(playerTransform.transform.position, agent.transform.position);
        switch (mode)
        {
            case eMode.patrol:
                anim.CrossFade("WoodcutterWalk", 0);
                if (distanceToPlayer <= DetectionDistance || IsInView())
                {
                    Target = playerTransform;
                    mode = eMode.run;
                }
                MoveToTarget();
                break;
            case eMode.run:
                anim.CrossFade("WoodcutterRun", 0);
                if (!IsInView())
                {
                    if (distanceToPlayer < DetectionDistance) return;
                    potentialPlayPos.position = lastPlayerPos;
                    Target = potentialPlayPos;
                    if ((potentialPlayPos.position - transform.position).magnitude < 0.5f && lastPlayerPos != Vector3.zero)
                    {
                        return;
                    }
                    lastPlayerPos = Vector3.zero;
                    Target = points[currentPoint].transform;
                    mode = eMode.patrol;
                }
                MoveToTarget();
                break;
            case eMode.diffusing:
                anim.CrossFade("WoodcutterCraft", 0);
                if (distanceToPlayer <= DetectionDistance || IsInView())
                {
                    Target = playerTransform;
                    mode = eMode.run;
                }
                if (Time.time >= defuseDone)
                {
                    dangerousPlase.plant = false;
                    dangerousPlase = null;
                    mode = eMode.patrol;
                    defuseDone = 0;
                }
                break;
            case eMode.idle:
                agent.speed = 0;
                anim.CrossFade("WoodcutterIdle", 0);
                break;
            case eMode.stun:
                if (stunDone >= Time.time)
                {
                    agent.speed = 0;
                    anim.CrossFade("WoodcutterStun", 0);
                    return;
                }
                agent.speed = 3;
                mode = eMode.patrol;
                break;
            case eMode.attack:
                if (attackDone >= Time.time)
                {
                    agent.speed = 0.5f;
                    anim.CrossFade("WoodcutterAttack", 0);
                    return;
                }
                if (player.mode == Robber.eMode.die)
                {
                    mode = eMode.idle;
                } else
                {
                    agent.speed = 3;
                    mode = eMode.patrol;
                }
                attackDone = 0;
                break;
        }
    }
    protected override bool IsInView() 
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position + agent.velocity.normalized, playerTransform.position - transform.position);
        if (hit2D.collider != null)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= ViewDistance && hit2D.transform == playerTransform.transform)
            {
                lastPlayerPos = playerTransform.position;
                agent.speed = 5;
                return true;
            }
        }
        agent.speed = 3;
        return false;
    }
    protected override void MoveToTarget() 
    {
        agent.SetDestination(Target.position);
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        Bomb b = coll.GetComponent<Bomb>();
        if (b != null)
        {
            mode = eMode.stun;
            stunDone = Time.time + stunDuration;
            b.Explousion();
        }
        Point p = coll.GetComponent<Point>();
        if (p == null) return;
        if (p.plant)
        {
            dangerousPlase = p;
            defuseDone = Time.time + defuseDuration;
            mode = eMode.diffusing;
        }
        int previosCurP = currentPoint;
        currentPoint = Random.Range(0, pointInLvl);
        if (currentPoint == previosCurP) currentPoint++;
        currentPoint %= 4;
        Target = points[currentPoint].transform;
    }
    public override void Loose()
    {
        mode = eMode.stun;
        stunDone = Mathf.Infinity;
    }
}
