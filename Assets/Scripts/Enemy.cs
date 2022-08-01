using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    [SerializeField]
    private float _fireRate = 10.0f;
    [SerializeField]
    private float _canFire = -1;

    private SpawnManager _spawnManager;
    [SerializeField]
    private int _moveID;
    [SerializeField]
    private float _detectRange = 6f;
    [SerializeField]
    private int _ramSpeed = 5;
    [SerializeField]
    private int _enemyID;

    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private bool _isShieldOn;

    // shot from behind
    [SerializeField]
    GameObject _backFireShot;

    // starting enemy can shoot from behind player

    // Start is called before the first frame update
    void Start()
    {
        _fireRate = 10.0f;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _moveID = Random.Range(1, 3);
        _enemyID = Random.Range(0, 2);
        _detectRange = 6f;

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL!");
        }

        int s = Random.Range(0, 100);

        if (s >= 70)
        {
            _isShieldOn = true;
        }
        else
        {
            _isShieldOn = false;
        }
        _shield.SetActive(_isShieldOn);

        if (_player == null)
        {
            Debug.LogError("Player is NUll!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        EnemyID();
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            float randomX = Random.Range(-2f, 11f);
            transform.position = new Vector3(randomX, 10, 1);
        }
    }
    void EnemyID()
    {
        switch (_enemyID)
        {
            case 0:
                BaseEnemy();
                break;
            case 1:
                RamEnemy();
                break;
            default:
                Debug.Log("Default Value");
                break;
        }
    }
    void BaseEnemy()
    {
        switch (_moveID)
        {
            case 1:
                transform.Translate((Vector3.down + Vector3.left) * Time.deltaTime);
                break;
            case 2:
                transform.Translate((Vector3.down + Vector3.right) * Time.deltaTime);
                break;
            case 3:
                transform.Translate(Vector3.down * Time.deltaTime);
                break;
            default:
                break;
        }

        if (Time.time > _canFire)
        {
            NormalFire();
        }
    }
    public void NormalFire()
    {
        _fireRate = Random.Range(4f, 6f);
        _canFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }
    public void BackFire()
    {
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignBackFire();
        }
    }
    void RamEnemy()
    {
        if (_player != null)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < _detectRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _ramSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            Damage();
        }

        if (other.tag == "Laser" && _backFireShot == true)
        {
            Destroy(other.gameObject);
            Damage();
        }
    }

    void Damage()
    {
        if (_isShieldOn == true)
        {
            _isShieldOn = false;
            _shield.SetActive(false);
        }
        else
        {
            _anim.SetTrigger("EnemyExplode");
            _speed = 0;
            _ramSpeed = 0;
            GetComponent<Collider2D>().enabled = false;
            _audioSource.Play();
            _spawnManager.EnemyDead();
            if (_player != null)
            {
                _player.AddScore(1);
            }
            Destroy(this.gameObject, .8f);
        }

    }
    public void Dodge()
    {
        int rng = Random.Range(0, 2);

        if (rng == 0)
        {
            StartCoroutine(MoveLeft());
        }
        else
        {
            StartCoroutine(MoveRight());
        }
    }

    IEnumerator MoveLeft()
    {
        Vector2 _currentPos = transform.position;
        Vector2 _destination = new Vector2(transform.position.x - 2, transform.position.y);
        float _t = 0f;

        while (_t < 1)
        {
            _t += Time.deltaTime * 3f;
            transform.position = Vector2.Lerp(_currentPos, _destination, _t);
            yield return null;
        }
    }

    IEnumerator MoveRight()
    {
        Vector2 _currentPos = transform.position;
        Vector2 _destination = new Vector2(transform.position.x + 2, transform.position.y);
        float _t = 0f;

        while (_t < 1)
        {
            _t += Time.deltaTime * 3f;
            transform.position = Vector2.Lerp(_currentPos, _destination, _t);
            yield return null;
        }
    }
}
