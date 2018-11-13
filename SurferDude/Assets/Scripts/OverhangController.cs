using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverhangController : MonoBehaviour {

    private UIManager _uiManager;
    private GameManager _gameManager;


    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    public void MoveForvard(float speed)
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void MoveBackwards(float speed)
    {
       
        transform.Translate(Vector3.left * speed * Time.deltaTime);

    }

    public void GameOver() {

        if (transform.position.x >= 33.0f) {
            transform.position = new Vector3(33.0f, transform.position.y, transform.position.z);
        }

        transform.Translate(Vector3.right * 5 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Surfer") {
            Debug.Log("Våg");

            if(other.gameObject != null) {
                Destroy(other.transform.parent.gameObject);
                GameOver();
                _uiManager.ShowTitleScreen();
                _gameManager.SetGameOverToTrue();
            }



        }
    }

}
