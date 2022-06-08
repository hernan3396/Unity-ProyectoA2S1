using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
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
    [SerializeField] private bool _isRanged; // para decidir si el enemigo es melee o ranged
    private float _newWaypointSpeed;
    private int _acceleration;
    private int _speed;
    #endregion

    #region AI
    private int _currentWaypoint = 0;
    private bool _isMoving = false;
    private NavMeshAgent _agent;
    #endregion

    #region Pause
    private Vector2 _lastVelocity;
    private bool _onPause;
    #endregion
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

        EnemyData enemy = _enemy.GetEnemyData;
        _newWaypointSpeed = enemy.NewWaypointSpeed;
        _acceleration = enemy.Acceleration;
        _speed = enemy.Speed;

        NextWapyoint();
    }

    private void Start()
    {
        GameManager.GetInstance.onGamePause += OnPause;
    }

    private void Update()
    {
        if (_onPause) return;
        // si ve al enemigo frena, sino sigue su ruta
        if (_enemy.EnemyOnSight && _isRanged)
        {
            StopMovement();
            return;
        }

        if (!_enemy.EnemyOnSight)
        {
            ReturnMovement();
            Moving();
            return;
        }
    }

    private void NextWapyoint()
    {
        if (_isMoving) return;

        _isMoving = true;
        if (!_agent.enabled) return; // evita que tire error
        // espero que frene el error que tira cuando muere el objeto
        // e intenta encontrar un nuevo waypoint
        if (_enemy.GetCurrentState == Enemy.States.Dead) return;
        _agent.SetDestination(_waypoints[_currentWaypoint].position);
    }

    private void ReturnMovement()
    {
        _agent.isStopped = false;
    }

    private void StopMovement()
    {
        _agent.isStopped = true;
        _lastVelocity = _agent.velocity;
        _agent.velocity = Vector2.zero;
    }

    private void Moving()
    {
        if (!_isMoving) return;

        if (_agent.remainingDistance <= 0.2f)
        {
            _isMoving = false;
            _currentWaypoint += 1;

            if (_currentWaypoint >= _waypoints.Count)
                _currentWaypoint = 0;

            Invoke("NextWapyoint", _newWaypointSpeed);
        }
    }

    #region Pause
    private void OnPause(bool value)
    {
        _onPause = value;

        if (_onPause)
            StopMovement();
        else
        {
            ReturnMovement();
            _agent.velocity = _lastVelocity;
            // queda medio feo aca pero para no escribir un
            // metodo nuevo que sea practicamente este con un cambio
            // en una linea lo hago aca
        }
    }
    #endregion

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

    private void OnDisable()
    {
        GameManager.GetInstance.onGamePause -= OnPause;
    }
}
