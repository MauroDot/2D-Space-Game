using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab, _enemy2Prefab;
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
            _enemiesLeftToSpawn = _waveNumber + 15;
            _maxEnemies = _waveNumber + 15;
            StartCoroutine(SpawnEnemyRoutine()); 
            StartCoroutine(SpawnPowerupRoutine());
        }
        //else start boss battle
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.8f);
        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 8, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab,  posToSpawn, Quaternion.identity);
            newEnemy = Instantiate(_enemy2Prefab, posToSpawn, Quaternion.identity);
            //_enemyPrefab.IsShieldActive();
            newEnemy.transform.parent = _enemyContainer.transform;

            _enemiesLeftToSpawn--;
            if(_enemiesLeftToSpawn == 0)
            {
                _stopSpawning = true;
            }
            yield return new WaitForSeconds(1f);
        }
        StartSpawning(_waveNumber + 1);
        yield return new WaitForSeconds(3f);
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5f);
        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 9);
            Instantiate(powerUps[randomPowerup], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 5f));
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
