using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public float AxisX { get; private set; }
    public float AxisY { get; private set; }
    public bool IsLeftMouseButtonDown { get; private set; }
    public bool IsDownSpace { get; private set; }
    public bool IsUpSpace { get; private set; }
    public bool IsDownLeftShift { get; private set; }

    private void Update()
    {
        GetInput();
        GetButton();
    }

    private void GetButton()
    {
        IsDownSpace = Input.GetKeyUp  (KeyCode.Space);
        IsDownSpace = Input.GetKeyDown(KeyCode.Space);


        IsLeftMouseButtonDown = Input.GetMouseButtonDown(0);

        IsDownLeftShift = Input.GetKeyDown(KeyCode.LeftShift);
    }

    private void GetInput()
    {
        AxisX = Input.GetAxis("Horizontal");
        AxisY = Input.GetAxis("Vertical");
    }
}
