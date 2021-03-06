using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //rotation speed variable
    [SerializeField]
    private float _rotateSpeed = 300f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //how to rotate an object
        transform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            int waveNumber = 1;
            _uiManager.DisplayWaveNumber(waveNumber);
            _spawnManager.StartSpawning(waveNumber);
            Destroy(this.gameObject, 0.12f);
        }
    }

}
