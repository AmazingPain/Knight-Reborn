using KnightReborn.Utilities;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 10f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimingMax = 2f;

    [SerializeField] private bool isChasingEnemy = false;
    [SerializeField] private float chasingDistance = 4f;
    [SerializeField] private float chasingSpeed;
    [SerializeField] private float chasingCharge = 2f;
    [SerializeField] private float roamSpeed;

    [SerializeField] private bool isAttackingEnemy = false;
    private float attackingDistance = 2f;


    private NavMeshAgent navMeshAgent;
    private State currentState;
    private float roamingTime;
    private Vector3 roamPosition;
    private Vector3 startingPosition;
    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
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
    }

    private void StateHandler()
    {
        switch (currentState)
        {
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0)
                {
                    Roaming();
                    roamingTime = roamingTimingMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                {
                    ChasingTarget();
                    CheckCurrentState();
                }
                break;
            case State.Attacking:
                {
                    AttackingTarget();
                    CheckCurrentState();
                }
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

    }

    private void ChasingTarget()
    {
        navMeshAgent.SetDestination(PlayerBehavior.Instance.transform.position);
    }

    private bool IsRunning()
    {
        if (navMeshAgent.velocity == Vector3.zero)
            return false;
        else
        {
            return true;
        }
    }

    private void Roaming()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        ChangeFaceDirection(startingPosition, roamPosition);
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
}
