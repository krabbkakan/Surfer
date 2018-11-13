using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singelton
    private static PlayerController _instance;

    public static PlayerController Instace
    {
        get
        {

            if (_instance == null)
            {
                GameObject go = new GameObject("PlayerController");
                go.AddComponent<PlayerController>();
            }

            return _instance;

        }

    }
    #endregion


    [SerializeField]
    private Transform surfer;

    private UIManager _uiManager;

    private GameManager _gameManager;

    private OverhangController _overhangController;

    //[SerializeField]
    private Transform overhangTransform;

    [SerializeField]
    private float _forwardSpeed = 2.0f;

    private float _topSpeed = 10.0f;
    private float _minSpeed = 1.0f;

    [SerializeField]
    private float zRotationSpeed;

    private bool isInPlayingPos = false;

    [SerializeField]
    private Transform plaingPos;

    [SerializeField]
    Rigidbody2D rb2d;

    [SerializeField]
    private SpriteRenderer surferSprite;

    [SerializeField]
    private Sprite[] surferAngles;
   
    private bool isJumping = false;
    private bool overHangshouldMove = true;
    private float overhangSpeed;

    private bool gameOver = false;

    private void Awake()
    {
        _instance = this;
    }


    private void OnEnable()
    {
        InputController.OnLeftPressed += OnLeftPressed;
        InputController.OnRightPressed += OnRightPressed;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _overhangController = GameObject.Find("OverHang").GetComponent<OverhangController>();
        overhangTransform = GameObject.Find("OverHang").GetComponent<Transform>();


    }

    private void Update()
    {
        if (transform.position.y < -7.0f)
        {
            transform.position = new Vector3(transform.position.x, -7.0f, 0);
        }
    }





    private void FixedUpdate()
    {
        
            MoveForvard(_forwardSpeed);


        //    if (overhangTransform.position.x > 13.0)
        //    {
        //        overHangshouldMove = false;
        //    }

        //    if (overHangshouldMove)
        //    {
        //        if (_forwardSpeed < 3.0)
        //        {
        //            overhangSpeed = 2.0f;
        //            _overhangController.MoveForvard(overhangSpeed);
        //        }
        //        else if (_forwardSpeed < 5.0 && _forwardSpeed > 3.0)
        //        {
        //            overhangSpeed = 1.0f;
        //            _overhangController.MoveForvard(overhangSpeed);
        //        }
        //        else if (_forwardSpeed < 7.0 && _forwardSpeed > 5.0)
        //        {
        //            overhangSpeed = 0.5f;
        //            _overhangController.MoveForvard(overhangSpeed);
        //        }
        //        else if (_forwardSpeed < _topSpeed && _forwardSpeed > 7.0)
        //        {
        //            if (overhangTransform.position.x < 2.0)
        //            {
        //                overhangTransform.position = new Vector3(2.0f, overhangTransform.position.y, 0);
        //            }
        //            else
        //            {
        //                _overhangController.MoveBackwards(1.0f);
        //            }
        //        }
        //    }
        //    else if (!overHangshouldMove)
        //    {
        //        //_overhangController.MoveBackwards(0.5f);

        //        if (overhangTransform.position.x < 8.0)
        //        {
        //            overHangshouldMove = true;
        //        }

        //}
       

        //MOve forward
        surferSprite.transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);

        //Jump when leave wave
        if (surferSprite.transform.position.y >= 1.8f && !isJumping)
        {
            Jump();
            isJumping = true;
        }

        //Gravity back on landing
        if (surferSprite.transform.position.y <= 1.8f && isJumping)
        {
            isJumping = false;
            GravityBack();
            _uiManager.UpdateScore();
        }

        // Overhang follow
        if (overhangTransform.position.x > 13.0)
        {
            overHangshouldMove = false;
        }

        if (overHangshouldMove)
        {
            if (_forwardSpeed < 3.0)
            {
                overhangSpeed = 2.0f;
                _overhangController.MoveForvard(overhangSpeed);
            }
            else if (_forwardSpeed < 5.0 && _forwardSpeed > 3.0)
            {
                overhangSpeed = 1.0f;
                _overhangController.MoveForvard(overhangSpeed);
            }
            else if (_forwardSpeed < 7.0 && _forwardSpeed > 5.0)
            {
                overhangSpeed = 0.5f;
                _overhangController.MoveForvard(overhangSpeed);
            }
            else if (_forwardSpeed < _topSpeed && _forwardSpeed > 7.0)
            {
                if (overhangTransform.position.x < 2.0)
                {
                    overhangTransform.position = new Vector3(2.0f, overhangTransform.position.y, 0);
                }
                else
                {
                    _overhangController.MoveBackwards(2.0f);
                }
            }
        }
        else if (!overHangshouldMove)
        {
            _overhangController.MoveBackwards(5.0f);

            if (overhangTransform.position.x < 4.0)
            {
                overHangshouldMove = true;
            }
        }



        //Constraints forward
        if (transform.position.x > 2.0f)
        {
            transform.position = new Vector3(2.0f, transform.position.y, 0);
        }

        //Constraints where surfer should be destroyed
        if (transform.position.x < -10.0f)
        {
            if(transform.parent.gameObject != null) 
            {
                Debug.Log("Name: " + transform.parent.gameObject.name);
                Destroy(surfer.transform.parent.gameObject);
            }
            _gameManager.SetGameOverToTrue();
            _uiManager.ShowTitleScreen();
            //Stop counting points
            _uiManager.StopCountingPoints();
        }
        else if (transform.position.y < -6.5f) 
        {
            if (surfer.transform.parent.gameObject != null)
            {
                Debug.Log("Name: " + transform.parent.gameObject.name);
                Destroy(surfer.transform.parent.gameObject);
            }
            _gameManager.SetGameOverToTrue();
            _uiManager.ShowTitleScreen();
            //Stop counting points
            _uiManager.StopCountingPoints();
        }


        //Move with arrows
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotateCounterClockwise(5.0f);


            switch (getAngleOfBoard())
            {
                case 0:
                    if(_forwardSpeed < _topSpeed) {
                        IncreaseSpeed(0.05f);
                    }
                    if (isInPlayingPos)
                    {

                        MoveUp(_forwardSpeed);
                    }
                    break;
                case 1:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.06f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveUp(_forwardSpeed);
                    }
                    break;
                case 2:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.07f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveUp(_forwardSpeed);
                    }
                    break;
                case 3:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.08f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveUp(_forwardSpeed);
                    }
                    break;
                case 4:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.09f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveUp(_forwardSpeed);
                    }
                    break;
                case 5:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.12f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveUp(_forwardSpeed);
                    }
                    break;
                case 6:
                    if (isInPlayingPos)
                    {
                        MoveUp(_forwardSpeed);
                    }
                    break;
                case 7:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.2f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveUp(_forwardSpeed);
                    }
                    break;
                case 8:
                    DecreaseSpeed(0.01f);
                    if (isInPlayingPos)
                    {
                        MoveUp(_forwardSpeed);
                    }
                    break;
                default:
                    Debug.Log("No speed increase");
                    break;
            }
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            RotateClockwise(5.0f);

            switch (getAngleOfBoard())
            {
                case 0:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.04f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveDown(_forwardSpeed);
                    }
                    break;
                case 1:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.06f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveDown(_forwardSpeed);
                    }
                    break;
                case 2:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.07f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveDown(_forwardSpeed);
                    }
                    break;
                case 3:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.08f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveDown(_forwardSpeed);
                    }
                    break;
                case 4:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.09f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveDown(_forwardSpeed);
                    }
                    break;
                case 5:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.12f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveDown(_forwardSpeed);
                    }
                    break;
                case 6:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.15f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveDown(_forwardSpeed);
                    }
                    break;
                case 7:
                    if (_forwardSpeed < _topSpeed)
                    {
                        IncreaseSpeed(0.2f);
                    }
                    if (isInPlayingPos)
                    {
                        MoveDown(_forwardSpeed);
                    }
                    break;
                case 8:
                    DecreaseSpeed(0.01f);
                    if (isInPlayingPos)
                    {
                        MoveDown(_forwardSpeed);
                    }
                    break;
                default:
                    Debug.Log("No speed increase");
                    break;
            }
        }
    }

    private void MoveForvard(float speed)
    {

        transform.Translate(Vector3.right * speed * Time.deltaTime);

    }
    private void MoveUp(float speed)
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }

    private void MoveDown(float speed)
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
    }

    private void RotateCounterClockwise(float speed)
    {

        float angle = surfer.transform.eulerAngles.z + speed;
        Debug.Log(angle);
        surfer.transform.eulerAngles = new Vector3(0, 0, angle);

        // sätt in rätt sprite för rätt vinkel
    }

    private void RotateClockwise(float speed)
    {
        float angle = surfer.transform.eulerAngles.z - speed;
        Debug.Log(angle);
        surfer.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void IncreaseSpeed(float speed)
    {
        _forwardSpeed += speed;
    }

    private void DecreaseSpeed(float speed)
    {
        _forwardSpeed -= speed;
    }

    private void OnLeftPressed()
    {
        RotateCounterClockwise(5.0f);


        switch (getAngleOfBoard())
        {
            case 0:
                IncreaseSpeed(0.05f);
                if (isInPlayingPos)
                {

                    MoveUp(_forwardSpeed);
                }
                break;
            case 1:
                IncreaseSpeed(0.06f);
                if (isInPlayingPos)
                {
                    MoveUp(_forwardSpeed);
                }
                break;
            case 2:
                IncreaseSpeed(0.07f);
                if (isInPlayingPos)
                {
                    MoveUp(_forwardSpeed);
                }
                break;
            case 3:
                IncreaseSpeed(0.08f);
                if (isInPlayingPos)
                {
                    MoveUp(_forwardSpeed);
                }
                break;
            case 4:
                IncreaseSpeed(0.09f);
                if (isInPlayingPos)
                {
                    MoveUp(_forwardSpeed);
                }
                break;
            case 5:
                IncreaseSpeed(0.12f);
                if (isInPlayingPos)
                {
                    MoveUp(_forwardSpeed);
                }
                break;
            case 6:
                if (isInPlayingPos)
                {
                    MoveUp(_forwardSpeed);
                }
                break;
            case 7:
                IncreaseSpeed(0.2f);
                if (isInPlayingPos)
                {
                    MoveUp(_forwardSpeed);
                }
                break;
            case 8:
                DecreaseSpeed(0.01f);
                if (isInPlayingPos)
                {
                    MoveUp(_forwardSpeed);
                }
                break;
            default:
                Debug.Log("No speed increase");
                break;
        }

    }

    private void OnRightPressed()
    {
        RotateClockwise(5.0f);

        switch (getAngleOfBoard())
        {
            case 0:
                IncreaseSpeed(0.05f);
                if (isInPlayingPos)
                {
                    MoveDown(_forwardSpeed);
                }
                break;
            case 1:
                IncreaseSpeed(0.06f);
                if (isInPlayingPos)
                {
                    MoveDown(_forwardSpeed);
                }
                break;
            case 2:
                IncreaseSpeed(0.07f);
                if (isInPlayingPos)
                {
                    MoveDown(_forwardSpeed);
                }
                break;
            case 3:
                IncreaseSpeed(0.08f);
                if (isInPlayingPos)
                {
                    MoveDown(_forwardSpeed);
                }
                break;
            case 4:
                IncreaseSpeed(0.09f);
                if (isInPlayingPos)
                {
                    MoveDown(_forwardSpeed);
                }
                break;
            case 5:
                IncreaseSpeed(0.12f);
                if (isInPlayingPos)
                {
                    MoveDown(_forwardSpeed);
                }
                break;
            case 6:
                IncreaseSpeed(0.15f);
                if (isInPlayingPos)
                {
                    MoveDown(_forwardSpeed);
                }
                break;
            case 7:
                IncreaseSpeed(0.2f);
                if (isInPlayingPos)
                {
                    MoveDown(_forwardSpeed);
                }
                break;
            case 8:
                DecreaseSpeed(0.01f);
                if (isInPlayingPos)
                {
                    MoveDown(_forwardSpeed);
                }
                break;
            default:
                Debug.Log("No speed increase");
                break;
        }
    }

    private int getAngleOfBoard()
    {
        if (transform.eulerAngles.z < 360 && transform.eulerAngles.z > 350)
        {
            surferSprite.sprite = surferAngles[0];
            Debug.Log("Forward");
            return 0;
        }
        else if (transform.eulerAngles.z <= 350 && transform.eulerAngles.z > 340)
        {
            surferSprite.sprite = surferAngles[0];
            Debug.Log("Down 1");
            return 1;
        }
        else if (transform.eulerAngles.z <= 340 && transform.eulerAngles.z > 330)
        {
            surferSprite.sprite = surferAngles[15];
            Debug.Log("Down 2");
            return 2;
        }
        else if (transform.eulerAngles.z <= 330 && transform.eulerAngles.z > 320)
        {
            surferSprite.sprite = surferAngles[15];
            Debug.Log("Down 3");
            return 3;
        }
        else if (transform.eulerAngles.z <= 320 && transform.eulerAngles.z > 310)
        {
            surferSprite.sprite = surferAngles[14];
            Debug.Log("Down 4");
            return 4;
        }
        else if (transform.eulerAngles.z <= 310 && transform.eulerAngles.z > 300)
        {
            surferSprite.sprite = surferAngles[14];
            Debug.Log("Down 5");
            return 5;
        }
        else if (transform.eulerAngles.z <= 300 && transform.eulerAngles.z > 290)
        {
            surferSprite.sprite = surferAngles[13];
            Debug.Log("Down 6");
            return 6;
        }
        else if (transform.eulerAngles.z <= 290 && transform.eulerAngles.z > 280)
        {
            surferSprite.sprite = surferAngles[13];
            Debug.Log("Down 7");
            return 6;
        }
        else if (transform.eulerAngles.z <= 280 && transform.eulerAngles.z > 270)
        {
            surferSprite.sprite = surferAngles[12];
            Debug.Log("Down 8");
            return 7;
        }
        else if (transform.eulerAngles.z <= 270 && transform.eulerAngles.z > 260)
        {
            surferSprite.sprite = surferAngles[12];
            Debug.Log("Down 8");
            return 6;
        }
        else if (transform.eulerAngles.z <= 260 && transform.eulerAngles.z > 250)
        {
            surferSprite.sprite = surferAngles[11];
            Debug.Log("Down 8");
            return 5;
        }
        else if (transform.eulerAngles.z <= 250 && transform.eulerAngles.z > 240)
        {
            surferSprite.sprite = surferAngles[11];
            Debug.Log("Down 8");
            return 5;
        }
        else if (transform.eulerAngles.z <= 240 && transform.eulerAngles.z > 230)
        {
            surferSprite.sprite = surferAngles[10];
            Debug.Log("Down 8");
            return 4;
        }
        else if (transform.eulerAngles.z <= 230 && transform.eulerAngles.z > 220)
        {
            surferSprite.sprite = surferAngles[10];
            Debug.Log("Down 8");
            return 4;
        }
        else if (transform.eulerAngles.z <= 230 && transform.eulerAngles.z > 220)
        {
            surferSprite.sprite = surferAngles[9];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 220 && transform.eulerAngles.z > 210)
        {
            surferSprite.sprite = surferAngles[9];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 210 && transform.eulerAngles.z > 200)
        {
            surferSprite.sprite = surferAngles[8];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 200 && transform.eulerAngles.z > 190)
        {
            surferSprite.sprite = surferAngles[8];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 190 && transform.eulerAngles.z > 180)
        {
            surferSprite.sprite = surferAngles[7];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 180 && transform.eulerAngles.z > 170)
        {
            surferSprite.sprite = surferAngles[7];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 170 && transform.eulerAngles.z > 160)
        {
            surferSprite.sprite = surferAngles[6];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 160 && transform.eulerAngles.z > 150)
        {
            surferSprite.sprite = surferAngles[6];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 150 && transform.eulerAngles.z > 140)
        {
            surferSprite.sprite = surferAngles[5];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 140 && transform.eulerAngles.z > 130)
        {
            surferSprite.sprite = surferAngles[5];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 130 && transform.eulerAngles.z > 120)
        {
            surferSprite.sprite = surferAngles[5];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 120 && transform.eulerAngles.z > 110)
        {
            surferSprite.sprite = surferAngles[4];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 110 && transform.eulerAngles.z > 100)
        {
            surferSprite.sprite = surferAngles[4];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 100 && transform.eulerAngles.z > 90)
        {
            surferSprite.sprite = surferAngles[4];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 90 && transform.eulerAngles.z > 80)
        {
            surferSprite.sprite = surferAngles[4];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 80 && transform.eulerAngles.z > 70)
        {
            surferSprite.sprite = surferAngles[3];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 70 && transform.eulerAngles.z > 60)
        {
            surferSprite.sprite = surferAngles[3];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 60 && transform.eulerAngles.z > 50)
        {
            surferSprite.sprite = surferAngles[2];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 50 && transform.eulerAngles.z > 40)
        {
            surferSprite.sprite = surferAngles[2];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 40 && transform.eulerAngles.z > 30)
        {
            surferSprite.sprite = surferAngles[2];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 30 && transform.eulerAngles.z > 20)
        {
            surferSprite.sprite = surferAngles[1];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 20 && transform.eulerAngles.z > 10)
        {
            surferSprite.sprite = surferAngles[1];
            Debug.Log("Down 8");
            return 3;
        }
        else if (transform.eulerAngles.z <= 10 && transform.eulerAngles.z > 0)
        {
            surferSprite.sprite = surferAngles[0];
            Debug.Log("Down 8");
            return 3;
        }




        return -1;
    }

    private void Jump() {
        
        float forceUp = 5.0f;

        float gravity = forceUp / 4.0f;

        rb2d.velocity = (new Vector3(0, forceUp));

        StartCoroutine(Addgraviy(gravity));

        // Make rotation faster


        // back  to 0 gravity


    }

    IEnumerator Addgraviy(float gravity)
    {
        yield return new WaitForSeconds(0.2f);
        rb2d.gravityScale = gravity;

    }

    private void GravityBack() 
    {
        rb2d.gravityScale = 0;
        rb2d.velocity = new Vector2(0,0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SlowDown")
        {
            if (_forwardSpeed > 1.5f)
            {
                DecreaseSpeed(1.0f);
            }
            //} else if(other.tag == "Stop") {
            //    isInPlayingPos = true;
            //}
        }

    }
}
