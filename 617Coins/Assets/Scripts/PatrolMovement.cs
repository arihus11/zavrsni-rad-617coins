using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolMovement : MonoBehaviour
{
    // lista patrolnih čvorova koje se posjećuju
    public List<Waypoint> patrolPoints;

    public Waypoint playerPoint;

    public bool chase = false;
    public bool reset = false;
    private Waypoint currentWaypoint;
    // vjerojatnost promjene smjera
    public float switchProbability;
    private Vector3 beginningPosition;
    private Quaternion beginningRotation;
    NavMeshAgent navMeshAgent;
    int currentPatrolIndex;
    bool traveling;
    bool patrolForward = true;

    void Awake()
    {
        beginningPosition = this.gameObject.transform.position;
        beginningRotation = this.gameObject.transform.rotation;
    }
    void Start()
    {
        chase = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent komponenta je null za " + gameObject.name);
        }
        else
        {
            if (patrolPoints != null && patrolPoints.Count >= 2)
            {
                currentPatrolIndex = 0;
                SetDestiation();
            }
            else
            {
                Debug.Log("Nedovoljan broj patrolnih točaka za osnovnu funkcionalnost");
            }
        }
    }

    void Update()
    {
        FaceTarget();
        if (reset == true)
        {
            reset = false;
            chase = false;
            this.gameObject.transform.position = beginningPosition;
            this.gameObject.transform.rotation = beginningRotation;
            currentPatrolIndex = 0;
            SetDestiation();
        }
        if (chase == true)
        {
            Vector3 targetVector = playerPoint.transform.position;
            currentWaypoint = playerPoint;
            navMeshAgent.SetDestination(targetVector);
        }
        else if (chase == false)
        {
            if (currentWaypoint != playerPoint)
            {
                if (traveling && navMeshAgent.remainingDistance <= 1.0f)
                {
                    traveling = false;
                    ChangePatrolPoint();
                    SetDestiation();

                }
            }
            else if (currentWaypoint == playerPoint)
            {
                traveling = false;
                ChangePatrolPoint();
                SetDestiation();
            }
        }
    }

    private void ChangePatrolPoint()
    {
        // promijeni smjer patroliranja s vjerojatnošću switchProbabilty
        if (UnityEngine.Random.Range(0f, 1f) <= switchProbability)
        {
            patrolForward = !patrolForward;
        }

        // odaberi točku ovisno o smjeru patroliranja
        if (patrolForward)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
        else
        {
            if (--currentPatrolIndex < 0)
            {
                currentPatrolIndex = patrolPoints.Count - 1;
            }
        }
    }

    private void SetDestiation()
    {
        if (patrolPoints != null)
        {
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
            currentWaypoint = patrolPoints[currentPatrolIndex];
            navMeshAgent.SetDestination(targetVector);
            traveling = true;
        }
    }

    void FaceTarget()
    {
        var turnTowardNavSteeringTarget = navMeshAgent.steeringTarget;

        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
