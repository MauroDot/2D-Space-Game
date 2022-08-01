using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenAsteroid : MonoBehaviour
{
    private int speed=8;

    void Update()
    {
        transform.Translate(Vector3.down*speed*Time.deltaTime);
        if(transform.position.y<-15) Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            Debug.Log("test");
            Player player = other.transform.GetComponent<Player>();
            player.Freeze();
            Destroy(this.gameObject);
        }
    }
}
