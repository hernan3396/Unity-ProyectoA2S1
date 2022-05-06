using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class WaypointsMovement : MonoBehaviour
{
    #region Components
    private Enemy _enemy; // esta para recibir la info si el enemigo esta a la vista
    // realmente habria que mover las cosas a scripts mas ordenados
    #endregion

    #region Waypoints
    [Header("Waypoints")]
    [SerializeField] private Transform _waypointsContainer;
    private List<Transform> _waypoints = new List<Transform>();
    #endregion

    #region Parameters
    [Header("Parameters")]
    [SerializeField] private float _newWaypointSpeed = 1;
    #endregion

    private int _currentWaypoint = 0;
    private bool _isMoving = false;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _agent = GetComponent<NavMeshAgent>();

        foreach (Transform waypoint in _waypointsContainer)
        {
            if (waypoint.TryGetComponent(out MeshRenderer meshRenderer))
                meshRenderer.enabled = false;

            _waypoints.Add(waypoint);
        }

        StartMovement();
    }

    private void Update()
    {
        // si ve al enemigo frena, sino sigue su ruta
        // esto esta horrible aca puesto pero funciona
        // cambiar luego
        if (_enemy.EnemyOnSight)
        {
            _agent.isStopped = true;
            return;
        }

        _agent.isStopped = false;

        if (!_isMoving) return;

        if (_agent.remainingDistance <= 0.2f)
        {
            _isMoving = false;
            _currentWaypoint += 1;

            if (_currentWaypoint >= _waypoints.Count)
                _currentWaypoint = 0;

            Invoke("StartMovement", _newWaypointSpeed);
        }
    }

    private void StartMovement()
    {
        if (_isMoving) return;

        _isMoving = true;
        if (!_agent.enabled) return; // evita que tire error
        _agent.SetDestination(_waypoints[_currentWaypoint].position);
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
