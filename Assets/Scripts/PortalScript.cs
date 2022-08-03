using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    [SerializeField] private int sceneNumber;
    private void Update() {
        transform.Rotate(Vector3.forward*55*Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag== "Player"){
            SceneManager.LoadScene(sceneNumber);
        }
    }
}
