using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject [] powerUps; //array braket to make a fixed list for all powerups

    private int _waveNumber;
    private int _enemiesDead;
    private int _maxEnemies;
    private int _enemiesLeftToSpawn;

    private UIManager _uiManager;

    public void Start()
    {
        _uiManager = GameObject.FindObjectOfType<UIManager>();
    }
    public void StartSpawning(int waveNumber)
    {
        if(waveNumber <= 5)
        {
            _stopSpawning = false;
            _enemiesDead = 0;
            _waveNumber = waveNumber;
            _uiManager.DisplayWaveNumber(_waveNumber);
            _enemiesLeftToSpawn = _waveNumber + 10;
            _maxEnemies = _waveNumber + 10;
            StartCoroutine(SpawnEnemyRoutine()); 
            StartCoroutine(SpawnPowerupRoutine());
        }
        //else start boss battle
    }

    

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.8f);
        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.5f, 8.5f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            _enemiesLeftToSpawn--;
            if(_enemiesLeftToSpawn == 0)
            {
                _stopSpawning = true;
            }
            yield return new WaitForSeconds(1f);
        }
        StartSpawning(_waveNumber + 1);
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5f);
        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 9);
            Instantiate(powerUps[randomPowerup], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 5));
        }
    }

    public void EnemyDead()
    {
        _enemiesDead++;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
