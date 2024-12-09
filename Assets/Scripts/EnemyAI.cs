using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public enum EnemyState
    {
        Patrolling,
        Chasing, 

        Searching
    }

    public EnemyState currentState;

    private NavMeshAgent _AIAgent;
    private Transform _playerTranform;

    //Puntos patrulla
    [SerializeField] Transform[] _patrolPoints;
    
    //Cosas detencion
    [SerializeField] float _visionRange = 20; 

    void Awake()
    {
        _AIAgent = GetComponent<NavMeshAgent>();
        _playerTranform = GameObject.FindWithTag("Player").transform;
    } 

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Patrolling;
        SetRandomPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
            break;
            case EnemyState.Chasing:
                Chase();
            break;
            case EnemyState.Searching:
                Search();
            break;
        }
    }

    void Patrol()
    {
        if(OnRange())
        {
            currentState = EnemyState.Chasing;
        }
        if(_AIAgent.remainingDistance < 0.5f)
        {
            SetRandomPatrolPoint();
        }
        
    } 

    void Chase()
    {
        if(!OnRange())
        {
            currentState = EnemyState.Patrolling;
        }
        _AIAgent.destination = _playerTranform.position;
    }

    void Search()
    {

    }

    bool OnRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTranform.position);

        if(distanceToPlayer < _visionRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetRandomPatrolPoint()
    {
        _AIAgent.destination = _patrolPoints[Random.Range(0, _patrolPoints.Length)].position;
    }

    void OnDrawGizmos()
    {
        foreach (Transform point in _patrolPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _visionRange);
    }

}
