using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public delegate void ButtonPressed();
    public static event ButtonPressed OnLeftPressed;
    public static event ButtonPressed OnRightPressed;

    public Joystick joystick;


    void Update()
    {
        
        if (joystick.Horizontal >= 0.2f)
        {
            OnRightPressed();
        }
        else if (joystick.Horizontal <= -0.2f)
        {
            OnLeftPressed();
        }

    }
}
