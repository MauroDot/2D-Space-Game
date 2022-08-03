using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(buttonAppear());
    }

    private IEnumerator buttonAppear()
    {
        yield return new WaitForSeconds(10);
        transform.GetChild(1).gameObject.SetActive(true);
        StopCoroutine(buttonAppear());
    }

    public void goToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void pauseControl()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            if (paused == false)
            {
                paused = true;
                Time.timeScale = 0;
                Text pauseButton = transform.GetChild(0).GetComponent<Text>();
                pauseButton.text = "UNPAUSE";
            }
            else
            {
                paused = false;
                Time.timeScale = 1;
                Text pauseButton = transform.GetChild(0).GetComponent<Text>();
                pauseButton.text = "PAUSE";
            }
        }
    }
}
