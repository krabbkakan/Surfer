using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Singelton
    private static GameManager _instance;

    public static GameManager Instace
    {
        get
        {

            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }

            return _instance;

        }

    }
    #endregion

    public bool gameOver = true;

    public GameObject playerPrefab;

    private UIManager _uiManager;


    private void Awake()
    {
        _instance = this;
    }


    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

    }

    void Update()
    {

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                gameOver = false;
                _uiManager.HideTitleScreen();
                _uiManager.ResetScore();
                _uiManager.StartCountingPoints();
            }
        }
    }

    public void OnStartPressed()
    {
        if (gameOver)
        {
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            gameOver = false;
            _uiManager.HideTitleScreen();
            _uiManager.ResetScore();
            _uiManager.StartCountingPoints();

        }
    }

    public void SetGameOverToTrue()
    {
        gameOver = true;
    }

}
