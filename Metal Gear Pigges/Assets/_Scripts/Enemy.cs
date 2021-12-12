using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Set in inspector: Enemy")]
    public Transform playerTransform;
    public float ViewDistance = 15f;
    public float stunDuration = 6f;
    public float attackDuration = 1.2f;

    [Header("Set Dinamically: Enemy")]
    public Transform Target;
    protected Vector3 lastPlayerPos;

    protected float attackDone;
    protected float stunDone;

    protected NavMeshAgent agent;
    protected Animator anim;
    protected SpriteRenderer sRend;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        sRend = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
    }
    protected virtual bool IsInView()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position + agent.velocity.normalized, playerTransform.position - transform.position);
        if (hit2D.collider != null)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= ViewDistance && hit2D.transform == playerTransform.transform)
            {
                return true;
            }
        }
        return false;
    }
    protected virtual void MoveToTarget()
    {
        agent.SetDestination(Target.position);
    }
    public virtual void Loose()
    {
        stunDone = Mathf.Infinity;
    }
    public virtual void Attack()
    {
        attackDone = Time.time + attackDuration;
    }
}
