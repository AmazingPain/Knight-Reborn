using KnightReborn.Utilities;
using System;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 10f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimingMax = 2f;
    [SerializeField] private float roamSpeed;

    [SerializeField] private bool isChasingEnemy = false;
    [SerializeField] private float chasingDistance = 4f;
    [SerializeField] private float chasingSpeed;
    [SerializeField] private float chasingCharge = 2f;

    [SerializeField] private bool isAttackingEnemy = false;
    [SerializeField] private float attackingDistance = 2f;
    [SerializeField] private float attackRate = 2f;
    private float nextAttackTime = 0f;


    private NavMeshAgent navMeshAgent;
    private State currentState;
    private float roamingTime;
    private Vector3 roamPosition;
    private Vector3 startingPosition;

    private float nextCheckDirectionTime = 0f;
    private float checkDirectionDuration = 0.1f;
    private Vector3 lastPosition;



    public event EventHandler OnEnemyAttack;
    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }


    public bool IsRunning()
    {
        if (navMeshAgent.velocity == Vector3.zero)
            return false;
        else
        {
            return true;
        }
    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        currentState = startingState;
        roamSpeed = navMeshAgent.speed;
        chasingSpeed = navMeshAgent.speed * chasingCharge;
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }

    public float GetRoamingAnimationSpeed()
    {
        return navMeshAgent.speed / roamSpeed;
    }

    public void SetDeathState()
    {
        navMeshAgent.ResetPath();
        currentState = State.Death;
    }

    private void StateHandler()
    {
        switch (currentState)
        {
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0)

                    Roaming();
                roamingTime = roamingTimingMax;
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                break;

        }
    }
    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerBehavior.Instance.transform.position);
        State newState = State.Roaming;

        if (isChasingEnemy)
        {
            if (distanceToPlayer <= chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (isAttackingEnemy)
        {
            if (distanceToPlayer <= attackingDistance)
            {
                newState = State.Attacking;
            }
        }

        if (newState != currentState)
        {
            if (newState == State.Chasing)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                roamingTime = 0f;
                navMeshAgent.speed = roamSpeed;
            }
            else if (newState == State.Attacking)
            {
                navMeshAgent.ResetPath();
            }

            currentState = newState;
        }

    }

    private void AttackingTarget()
    {
        if (Time.time > nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            nextAttackTime = Time.time + attackRate;
        }
    }

    private void ChasingTarget()
    {
        navMeshAgent.SetDestination(PlayerBehavior.Instance.transform.position);
    }

   



    private void Roaming()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utilities.GetRandomDirection() * UnityEngine.Random.Range(roamingDistanceMax, roamingDistanceMin);
    }

    private void ChangeFaceDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    private void MovementDirectionHandler()

    {
        if (Time.time > nextCheckDirectionTime)
        {
            if (IsRunning())
            {
                ChangeFaceDirection(lastPosition, transform.position);
            }
            else if (currentState == State.Attacking)
            {
                ChangeFaceDirection(transform.position, PlayerBehavior.Instance.transform.position);
            }

            lastPosition = transform.position;
            nextCheckDirectionTime = Time.time + checkDirectionDuration;
        }
    }
}
