using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _homingLaserCountText;

    private GameManager _gameManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private Text _waveDisplay;



    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score:" + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager != null)
        {
            //Debug.Log("GameManager is NULL.");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score:" + playerScore.ToString();

    }

    public void UpdateLives(int currentLives)
    {
        //display img sprite
        //give it a new one based on the currentLives index
        if(currentLives >= 0)
        {
            _LivesImg.sprite = _livesSprite[currentLives];
        }
       

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateAmmoCount(int ammoCount, int maximumAmmo)
    {
        _ammoText.text = ammoCount + " / " + maximumAmmo;

        if (ammoCount == 0)
        {
            _ammoText.color = Color.white;
        }
        else
        {
            _ammoText.color = Color.green;
        }
    }

    public void UpdateHomingLaserCount(int homingLaserCount)
    {
        _homingLaserCountText.text = " " + homingLaserCount.ToString();

        if(homingLaserCount > 0)
        {
            _homingLaserCountText.color = Color.green;
        }
        else
        {
            _homingLaserCountText.color = Color.red;
        }
    }

    public void DisplayWaveNumber(int waveNumber)
    {
        _waveDisplay.text = "Wave " + waveNumber;
        _waveDisplay.gameObject.SetActive(true);
        StartCoroutine(WaveDisplayRoutine());
    }

    IEnumerator WaveDisplayRoutine()
    {
        while(_waveDisplay == true)
        {
            yield return new WaitForSeconds(3f);
            _waveDisplay.gameObject.SetActive(false);
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
