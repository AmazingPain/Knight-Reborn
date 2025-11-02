using UnityEngine;
using UnityEngine.AI;
using KnightReborn.Utilities;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 10f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimingMax = 2f;

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTime;

    private Vector3 roamPosition;
    private Vector3 startingPosition;

    private enum State
    {
        Roaming
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = startingState;
    }

    private void Update()
    {
        switch (state)
            {
            default: 
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0)
                {
                    Roaming();
                    roamingTime = roamingTimingMax;
                }
                break;
           
            }
    }

    private void Roaming()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        ChangeFaceDirection(startingPosition,roamPosition);
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
