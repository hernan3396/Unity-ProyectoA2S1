using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // generic pool manager
    [SerializeField] private List<GameObject> _objectsPool;
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private int _objectsAmount = 10;
    private Transform _objectsParent;

    private void Start()
    {
        _objectsParent = transform;
        _objectsPool = new List<GameObject>();

        GameObject go;

        for (int i = 0; i < _objectsAmount; i++)
        {
            go = Instantiate(_objectPrefab);
            go.transform.parent = _objectsParent;
            go.SetActive(false);

            _objectsPool.Add(go);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _objectsAmount; i++)
        {
            if (!_objectsPool[i].activeInHierarchy)
            {
                return _objectsPool[i];
            }
        }
        return null;
    }
}