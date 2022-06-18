using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject _thruster;
    [SerializeField]
    private float _thrustRemaining = 100;
    [SerializeField]
    private bool _thrustersActive = false;
    [SerializeField]
    float _speedBoostAmount;
    [SerializeField]
    float _slowSpeed;
    [SerializeField]
    private float _speed = 9f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _playerHomingLaserPrefab;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private GameObject _boosterShotPrefab;
    [SerializeField]
    private GameObject _flankCannonPrefab;
    [SerializeField]
    private GameObject _isHealPickupActive;
    [SerializeField]
    private Vector3 LaserOffset = new Vector3(0, 1.08f, 0);
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private int _homingLaserCount = 0;
    [SerializeField]
    private bool _isPlayerHomingLaserActive = false;

    [SerializeField]
    private bool _isTripleshotActive = false;
    [SerializeField]
    private bool _isSpeedOverloadActive = false;
    [SerializeField]
    private bool _isShieldBoostActive = false;
    [SerializeField]
    private bool _isBoosterShotActive = false;
    [SerializeField]
    private bool _isFlankCannonAvtive = false;
    [SerializeField]
    private bool _isSlowSpeedActive = false;
    

    //variable reference to the shield visualizer
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _shieldPower;
    SpriteRenderer _shieldColor;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField] 
    private AudioClip _laserEffectClip;
    private AudioSource _audioSource;
    [SerializeField]
    private Slider _playerThrusterBarSlider;

    private CameraShake _cameraShake;

    [SerializeField]
    private int _ammoCount = 100;
    [SerializeField]
    private int _maximumAmmoCount;

    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -2.71f, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        _maximumAmmoCount = _ammoCount;
        _shieldColor = _shieldVisualizer.GetComponent<SpriteRenderer>();

        

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AuidioSource on Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserEffectClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
        ActivateThrusters();
        ThrustersRecharge();
        ThrusterDrain();
        BoosterExaughsted();
        

        if (Input.GetKeyDown(KeyCode.M))
        {
           if (_isPlayerHomingLaserActive == true && _homingLaserCount > 0)
           {
                FireHominglaser();
           }
        }

        if(_homingLaserCount == 0)
        {
            _isPlayerHomingLaserActive = false;
        }

        if(Input.GetKey(KeyCode.Space) && Time.time > _canFire && _ammoCount != 0)
        {
            FireLaser();

            _audioSource.Play();
            _ammoCount -= 1;
            _uiManager.UpdateAmmoCount(_ammoCount, _maximumAmmoCount);
        }
    }
    void calculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(_isSpeedOverloadActive == true)
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * _speedMultiplier * Time.deltaTime);
        }
        else if(_isSlowSpeedActive == true)
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * 0.2f * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -6.00f, 4), 0f);

        if (transform.position.x > 15.34f)
        {
            transform.position = new Vector3(-15.34f, transform.position.y, 0);
        }
        else if (transform.position.x < -15.34f)
        {
            transform.position = new Vector3(15.34f, transform.position.y, 0);
        }
    }

    public void ActivateThrusters()
    {
        if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.LeftShift) && _thrustRemaining > 0)
        {
            _thrustersActive = true;
            _isSpeedOverloadActive = true;
            _thruster.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.J) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            _thrustersActive = false;
            _isSpeedOverloadActive = false;
            _thruster.SetActive(false);
        }
    }

    public void ThrusterDrain()
    {
        if (_thrustersActive == true && _thrustRemaining >= 0)
        {
            _thrustRemaining -= Time.deltaTime * 50;
        }

        ChangeThruster(_thrustRemaining);
    }

    private void ThrustersRecharge()
    {
        if (_thrustersActive == false && _thrustRemaining <= 100)
        {
            _thrustRemaining += Time.deltaTime * 20;
        }
    }

    private void BoosterExaughsted()
    {
        if (_thrustRemaining < 5)
        {
            _thrustersActive = false;
            _isSpeedOverloadActive = false;
            _thruster.SetActive(false);
        }
    }

    public void ChangeThruster(float value)
    {
        _playerThrusterBarSlider.value = value / 100;
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleshotActive == true)
        {
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
        }
        else if (_isBoosterShotActive == true)
        {
            Instantiate(_boosterShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_isFlankCannonAvtive == true)
        {
            Instantiate(_flankCannonPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + LaserOffset, Quaternion.identity);
        }

        _audioSource.Play();
    }
    public void Damage()
    {
        if(_isShieldBoostActive == true)
        {
            _shieldPower--;

            if(_shieldPower == 0)
            {
                _isShieldBoostActive = false;
                _shieldVisualizer.SetActive(false);
                return;
            }
            else if (_shieldPower ==1)
            {
                _shieldColor.color = Color.red;
                return;
            }
            else if(_shieldPower ==2)
            {
                _shieldColor.color = Color.green;
                return;
            }
        }

        _lives --;

        if(_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject, .28f);
        }

        StartCoroutine(_cameraShake.Shake(0.5f, 0.32f));
    }

    //create method for TripleShotActive
    public void TripleShotActive()
    {
        _isTripleshotActive = true;
        StartCoroutine(TripleShotPowerdownRoutine());
    }

    IEnumerator TripleShotPowerdownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleshotActive = false;
    }

    public void BoosterShotActive()
    {
        _isBoosterShotActive = true;
        StartCoroutine(BoosterShotPowerDownRoutine());
    }

    IEnumerator BoosterShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isBoosterShotActive = false;
    }

    public void FlankCannonsActive()
    {
        _isFlankCannonAvtive = true;
        StartCoroutine(FlankCannonsPowerDownRoutine());
    }

    IEnumerator FlankCannonsPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isFlankCannonAvtive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedOverloadActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerdownRoutine());
    }

    IEnumerator SpeedBoostPowerdownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedOverloadActive = false;
        _speed /= _speedMultiplier;
    }

    public void SlowSpeedActive()
    {
        _isSlowSpeedActive = true;
        _thrustersActive = false;
        StartCoroutine(SlowSpeedPowerdownRoutine());    
    }

    IEnumerator SlowSpeedPowerdownRoutine()
    {
        yield return new WaitForSeconds(15f);
        _thrustersActive = true;
        _isSlowSpeedActive = false;
        _isSpeedOverloadActive = false;
    }

    public void ShieldsActive()
    {
        if(_shieldPower < 3)
        {
            _shieldPower++;
            _isShieldBoostActive = true;
            _shieldVisualizer.SetActive(true);
        }
        if(_shieldPower == 1)
        {
            _shieldColor.color = Color.red;
        }
        else if(_shieldPower == 2)
        {
            _shieldColor.color = Color.green;
        }
        else if (_shieldPower == 3)
        {
            _shieldColor.color = Color.cyan;
        }
    }

    public void HomingLaserActive()
    {
        _isPlayerHomingLaserActive = true;
    }

    //method to add 1 to score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void HealPickupActive()
    {
        if(_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);

            if( _lives == 2)
            {
                _leftEngine.SetActive(false);
            }
            else if ( _lives == 3)
            {
                _rightEngine.SetActive(false);
            }
        }
    }

    public void RefillAmmo()
    {
        _ammoCount = _maximumAmmoCount;
        _uiManager.UpdateAmmoCount(_ammoCount, _maximumAmmoCount);
    }

    private void FireHominglaser()
    {
        Instantiate(_playerHomingLaserPrefab, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        _homingLaserCount--;
        _uiManager.UpdateHomingLaserCount(_homingLaserCount);
    }

    public void PlayerHomingLaser()
    {
        _homingLaserCount = _homingLaserCount + 4;
        if(_homingLaserCount > 24)
        {
            _homingLaserCount = 24;
        }

        _uiManager.UpdateHomingLaserCount(_homingLaserCount);
        _isPlayerHomingLaserActive = true;
    }
}
