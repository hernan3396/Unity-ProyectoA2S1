using UnityEngine;
using StarterAssets;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    private Inputs _input;
    private Camera _cam;
    private Vector3 _aimPosition;
    private Shooting _shooting;

    #region Parameters
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _bulletSpeed = 20;
    private bool _canShoot = true;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] private Transform _shootingPos;
    [SerializeField] private Transform _arm;
    private Transform _transform;
    #endregion

    private void Start()
    {
        _input = GetComponent<Inputs>();
        _transform = GetComponent<Transform>();
        _shooting = GetComponent<Shooting>();
        _cam = GameManager.GetInstance.GetMainCamera;
    }

    private void Update()
    {
        Aim();

        if (_canShoot && _input.IsShooting)
            StartCoroutine("Shooting");
    }

    private void Aim()
    {
        // hacer esto luego
        // if mouse esto
        // esto no es necesario ya que _input.look ya te lo da
        // Ray ray = _cam.ScreenPointToRay(_input.look); // rayo desde la camara

        // if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        //     _aimPosition = new Vector3(hit.point.x, hit.point.y, 0); // le sacas el valor en z para que no tenga profundidad ya que es 2D

        // _arm.right = (_aimPosition - _transform.position).normalized;

        // if control esto
        // _arm.right = _input.look;

        Ray ray = _cam.ScreenPointToRay(_input.look); // rayo desde la camara

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            _aimPosition = new Vector3(hit.point.x, hit.point.y, 0); // le sacas el valor en z para que no tenga profundidad ya que es 2D

        _arm.right = (_aimPosition - _transform.position).normalized;
    }

    private IEnumerator Shooting()
    {
        _canShoot = false;
        _shooting.Shoot(_shootingPos.position, _aimPosition, _bulletSpeed);
        yield return new WaitForSeconds(_fireRate);

        _canShoot = true;
    }
}
