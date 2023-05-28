using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyingEnemy : MonoBehaviour
{
    public float waypointReachedDistance = 0.2f;
    public float flightSpeed = 2f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;
    Animator animator;
    Rigidbody2D rb;
    [SerializeField] private UnityEvent onDied;
    private Damageable damageable;
    public bool die = true;
    private Transform nextWaypoint;
    private int waypointNum = 0;
    
    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }
    
    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }
    
    private void OnDiedEvent()
    {
        var handler = this.onDied;
        if (handler != null)
        {
            handler.Invoke();
        }
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCooldown); }
        private set { animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0)); }
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;

        if (AttackCooldown > 0)
            AttackCooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {

        if (damageable.IsAlive && die)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else if(!damageable.IsAlive && die)
        {
            this.OnDiedEvent();
            rb.gravityScale = 2f;
            die = false;
        }
    }

    private void Flight()
    {
        //fly to waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;
        
        //check if we have reach the waypoint
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();
        
        //see if we need to swtich waypoints
        if (distance <= waypointReachedDistance)
        {
            //switch to next waypoint
            waypointNum++;
            if (waypointNum >= waypoints.Count)
            {
                //loop to original waypoint
                waypointNum = 0;
            }
        }

        nextWaypoint = waypoints[waypointNum];
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            //facing right
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            //facing left
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }
}
