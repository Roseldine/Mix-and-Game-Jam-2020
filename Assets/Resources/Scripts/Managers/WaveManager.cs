
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Transform[] _spawners;
    public GameObject[] _enemyPrefabs;

    public int _minEnemies;
    public int _maxEnemies;

    public float _cooldownMin = 1f;
    public float _cooldownMax = 3f;
    public float _cooldown;

    private void Start()
    {
        StartCoroutine(SpawnClock());
    }


    public void Spawn()
    {
        int _randNum = Random.Range(_minEnemies, _maxEnemies + 1);
        
        for (int i = 0; i < _randNum; i++)
        {
            var _enemy = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
            Instantiate(_enemy, _spawners[Random.Range(0, _spawners.Length)].position, Quaternion.identity);
        }
    }

    IEnumerator SpawnClock()
    {
        _cooldown = Random.Range(_cooldownMin, _cooldownMax);
        Spawn();

        yield return new WaitForSeconds(_cooldown);
        if (Trophy.Instance._health > 0)
            StartCoroutine(SpawnClock());
    }
}
