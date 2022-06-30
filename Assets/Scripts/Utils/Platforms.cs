using UnityEngine;
using DG.Tweening;

public class Platforms : MonoBehaviour
{
    [SerializeField] private bool _isDoor = false;
    [SerializeField] private Transform _finalPos;
    [SerializeField] private float _duration = 1;

    [SerializeField] private bool _isPlatform = true;

    private Transform _transform;
    private Vector3 _initPos;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _initPos = _transform.position;

    }

    private void Start()
    {
        GameManager.GetInstance.onGamePause += OnPause;

        if (_isDoor) return;
        MoveIn();
    }

    public void MoveIn()
    {
        _transform.DOMove(_finalPos.position, _duration)
        .SetUpdate(UpdateType.Fixed)
        .OnComplete(MoveOut);
    }

    public void MoveOut()
    {
        _transform.DOMove(_initPos, _duration)
        .SetUpdate(UpdateType.Fixed)
        .OnComplete(MoveIn);
    }

    public void SimpleMove()
    {
        _transform.DOMove(_finalPos.position, _duration).SetUpdate(UpdateType.Fixed);
    }

    private void OnPause(bool value)
    {
        if (value)
            _transform.DOPause();
        else
            _transform.DOPlay();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && _isPlatform)
            other.transform.parent = _transform;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && _isPlatform)
            other.transform.parent = null;
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePause -= OnPause;
    }
}
