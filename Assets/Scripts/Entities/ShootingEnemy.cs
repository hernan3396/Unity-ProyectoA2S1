using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ShootingEnemy : Enemy
{
    #region Components
    private Enemy _enemy;
    #endregion

    #region Waypoints
    [Header("Waypoints")]
    [SerializeField] private Transform _waypointsContainer;
    private List<Transform> _waypoints = new List<Transform>();
    #endregion

    #region Parameters
    [Header("Parameters")]
    [SerializeField] private float _newWaypointDelay = 1;
    #endregion

    #region Movement
    private int _currentWaypoint = 0;
    private bool _isMoving = false;
    private NavMeshAgent _agent;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        _enemy = GetComponent<Enemy>();
        _agent = GetComponent<NavMeshAgent>();

        _agent.acceleration = _acceleration;
        _agent.speed = _speed;

        // seteamos el camino
        foreach (Transform waypoint in _waypointsContainer)
        {
            if (waypoint.TryGetComponent(out MeshRenderer meshRenderer))
                meshRenderer.enabled = false;

            _waypoints.Add(waypoint);
        }

        // empieza el recorrido
        StartMovement();
    }

    private void Update()
    {
        _enemyOnSight = DetectEnemy();

        if (_enemyOnSight)
        {
            // frenamos el movimiento
            // apuntamos
            // disparamos

            StopMovement();
            Aim();

            if (!_canShoot) return; // espera al firerate
            StartCoroutine(Shoot(_weaponList[0])); // el primer arma
        }

        if (!_enemyOnSight)
            SetNextWaypoint();
    }

    private void StartMovement()
    {
        if (_isMoving) return;

        _isMoving = true;
        if (!_agent.enabled) return; // evita que tire error
        _agent.SetDestination(_waypoints[_currentWaypoint].position);
    }

    protected override void SetNextWaypoint()
    {
        if (!_isMoving) return;
        ResumeMovement();

        if (_agent.remainingDistance <= 0.2f)
        {
            _isMoving = false;
            _currentWaypoint += 1;

            if (_currentWaypoint >= _waypoints.Count)
                _currentWaypoint = 0;

            Invoke("StartMovement", _newWaypointDelay);
        }
    }

    // estan separadas por si hay que 
    // agregarle mas logica luego

    private void ResumeMovement()
    {
        _agent.isStopped = false;
    }

    private void StopMovement()
    {
        _agent.isStopped = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_waypointsContainer.GetChild(0).position, 0.6f);

        Gizmos.color = Color.red;

        Vector3 previousWaypoint = _waypointsContainer.GetChild(0).position;
        int index = 0;

        foreach (Transform waypoint in _waypointsContainer)
        {
            if (index != 0)
            {
                Gizmos.DrawLine(previousWaypoint, waypoint.position);
                previousWaypoint = waypoint.position;
            }

            index++;
        }

        Gizmos.DrawLine(previousWaypoint, _waypointsContainer.GetChild(0).position);
    }
}
