using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchInputs : MonoBehaviour
{
    public static PlayerTouchInputs instance;

    [Header("TouchControls")]
    [SerializeField] Vector2 joystickSize = new Vector2(250, 250);
    [SerializeField] public FloatingJoystick joystickSC;

    private Finger movementFinger;
    private Vector2 movementAmount;


    private void OnEnable()
    {
        instance = this;
        EnableTouchStuff();
    }

    private void OnDisable()
    {
        DisableTouchStuff();
    }

    private void Awake()
    {
        instance = this;
    }

    private void Touch_onFingerMove(Finger movedFinger)
    {
        if(movedFinger == movementFinger)
        {
            Vector2 knobPos;
            float maxMovement = joystickSize.x / 2f;
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            if(Vector2.Distance(currentTouch.screenPosition, joystickSC.rectTransform.anchoredPosition) > maxMovement)
            {
                knobPos = (currentTouch.screenPosition - joystickSC.rectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPos = currentTouch.screenPosition - joystickSC.rectTransform.anchoredPosition;
            }

            joystickSC.knob.anchoredPosition = knobPos;
            movementAmount = knobPos / maxMovement;
        }
    }

    private void Touch_onFingerDown(Finger touchedFinger)
    {
        if(movementFinger == null) //&& touchedFinger.screenPosition.y <= Screen.height / 2f)//Instead of checking if the finger is in left side of the screen we are checking if its below half of the screen to implement movement.
        {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            joystickSC.gameObject.SetActive(true);
            joystickSC.rectTransform.sizeDelta = joystickSize;
            joystickSC.rectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }
    }

    private void Touch_onFingerUp(Finger lostFinger)
    {
        if(lostFinger == movementFinger)
        {
            movementFinger = null;
            joystickSC.knob.anchoredPosition = Vector2.zero;
            joystickSC.gameObject.SetActive(false);
            movementAmount = Vector2.zero;
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPos)
    {
        if(startPos.x < joystickSize.x / 2)
        {
            startPos.x = joystickSize.x / 2;
        }

        if (startPos.y < joystickSize.y / 2)
        {
            startPos.y = joystickSize.y / 2;
        }
        else if (startPos.y > Screen.height - joystickSize.y / 2)
            startPos.y = Screen.height - joystickSize.y / 2;

        return startPos;
    }

    public void EnableTouchStuff()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += Touch_onFingerDown;
        ETouch.Touch.onFingerUp += Touch_onFingerUp;
        ETouch.Touch.onFingerMove += Touch_onFingerMove;
    }

    public void DisableTouchStuff()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown -= Touch_onFingerDown;
        ETouch.Touch.onFingerUp -= Touch_onFingerUp;
        ETouch.Touch.onFingerMove -= Touch_onFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        if(!PlayerSC.instance.isDead)
        {
            PlayerSC.instance.transform.LookAt(PlayerSC.instance.transform.position + new Vector3(movementAmount.x, 0, movementAmount.y), Vector3.up);
            PlayerSC.instance.Movement(movementAmount);
        }
    }
}
