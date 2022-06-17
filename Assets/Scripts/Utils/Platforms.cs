using UnityEngine;
using DG.Tweening;

public class Platforms : MonoBehaviour
{
    [SerializeField] private bool _isDoor = false;
    [SerializeField] private Transform _finalPos;
    [SerializeField] private float _duration = 1;
    private Transform _transform;
    private Vector3 _initPos;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _initPos = _transform.position;
    }

    private void Start()
    {
        if (_isDoor) return;
        MoveIn();
    }

    public void MoveIn()
    {
        _transform.DOMove(_finalPos.position, _duration).OnComplete(MoveOut);
    }

    public void MoveOut()
    {
        _transform.DOMove(_initPos, _duration).OnComplete(MoveIn);
    }

    public void SimpleMove()
    {
        _transform.DOMove(_finalPos.position, _duration);
    }
}
