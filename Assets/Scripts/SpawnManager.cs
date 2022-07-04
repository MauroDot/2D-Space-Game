using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab, _enemy2Prefab, _enemy3Prefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] powerUps; //array braket to make a fixed list for all powerups

    private int _waveNumber;
    private int _enemiesDead;
    private int _maxEnemies;
    private int _enemiesLeftToSpawn;


    private UIManager _uiManager;

    public void Start()
    {
        _uiManager = GameObject.FindObjectOfType<UIManager>();

        foreach (GameObject obj in _common)
        {
            Debug.Log(obj);
        }


        foreach (GameObject obj in _uncommon)
        {
            Debug.Log(obj);
        }


        foreach (GameObject obj in _rarity)
        {
            Debug.Log(obj);
        }

    }

    [SerializeField] private List<GameObject> _common = new List<GameObject>();
    [SerializeField] private List<GameObject> _uncommon = new List<GameObject>();
    [SerializeField] private List<GameObject> _rarity = new List<GameObject>();
    public void StartSpawning(int waveNumber)
    {
        if(waveNumber <= 5)
        {
            _stopSpawning = false;
            _enemiesDead = 0;
            _waveNumber = waveNumber;
            _uiManager.DisplayWaveNumber(_waveNumber);
            _enemiesLeftToSpawn = _waveNumber + 50;
            _maxEnemies = _waveNumber + 50;
            StartCoroutine(SpawnEnemyRoutine()); 
            StartCoroutine(SpawnPowerupRoutine());
        }
        else if(waveNumber >= 5)
        {
            SceneManager.LoadScene(2);
        }
        //else start boss battle
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.8f);
        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-11f, 11f), 8, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab,  posToSpawn, Quaternion.identity);
            newEnemy = Instantiate(_enemy2Prefab, posToSpawn, Quaternion.identity);
            newEnemy = Instantiate(_enemy3Prefab, posToSpawn, Quaternion.identity);
            
            newEnemy.transform.parent = _enemyContainer.transform;

            _enemiesLeftToSpawn--;
            if(_enemiesLeftToSpawn == 0)
            {
                _stopSpawning = true;
            }
            yield return new WaitForSeconds(2.0f);
        }
        StartSpawning(_waveNumber + 1);
        yield return new WaitForSeconds(5f);
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5f);
        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 9);
            List<GameObject> _currentRare = GetRare();
            GameObject _randomPowerup = _currentRare[Random.Range(0, _currentRare.Count)];
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

    private int _rarityCount;
    private int _uncommonCount;
    private int _commonCount;
    // powerup spawn balancing common, uncommon, rarity
    private List<GameObject> GetRare()
    {
        int _currentRare = Random.Range(0, 100);
        if(_currentRare > 0 && _currentRare <= 10)
        {
            _rarityCount++;
            Debug.Log("Rarity = " + _rarityCount);
            return _rarity;
        }
        else if(_currentRare > 10 && _currentRare <= 50)
        {
            _uncommonCount++;
            Debug.Log("Uncommon = " + _uncommonCount);
            return _uncommon;
        }
        else
        {
           _commonCount++;
            Debug.Log("Common = " + _commonCount);
            return _common;
        }
    }

}
