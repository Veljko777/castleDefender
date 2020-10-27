using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controlls : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    private float moveSpeed = 3.0f;
    public RectTransform circle;
    public RectTransform outerCircle;
    public float joystickLimit = 1f;
    Vector2 direction = Vector2.zero;

    void Start()
    {
        Input.multiTouchEnabled = true;
    }

    void FixedUpdate()
    {
        DetectLookDirection();
        if (direction.x != 0 && direction.y != 0 &&!GameLogic.isPlayerDead)
        {
            GameObject.Find("Player").GetComponent<PlayerLogic>().playerWalking = true;
            MoveCharacter();
        }
        else
        {
            GameObject.Find("Player").GetComponent<PlayerLogic>().playerWalking = false;
        }
    }

    private void MoveCharacter()
    {
        GameObject.Find("Player").transform.position += new Vector3(direction.x, direction.y, 0) * moveSpeed * Time.deltaTime;
    }

    public void StartFire()
    {
        GameObject.Find("Player").GetComponent<PlayerLogic>().startFire = true;
    }

    public void StopFire()
    {
        GameObject.Find("Player").GetComponent<PlayerLogic>().startFire = false;
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        OnDrag(eventdata);
    }

    public void OnDrag(PointerEventData eventdata)
    {
        Vector2 joyDirection = eventdata.position - RectTransformUtility.WorldToScreenPoint(new Camera(), outerCircle.position);
        direction = (joyDirection.magnitude > outerCircle.sizeDelta.x / 3f) ? joyDirection.normalized : joyDirection / (outerCircle.sizeDelta.x / 3f);
        circle.anchoredPosition = (direction * outerCircle.sizeDelta.x / 3f) * joystickLimit;
    }

    public void OnPointerUp(PointerEventData eventdata)
    {
        direction = Vector2.zero;
        circle.anchoredPosition = Vector2.zero;
    }

    private void DetectLookDirection()
    {
        if (direction.x < 0)
        {
            GameObject.Find("PlayerImage").transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if(direction.x>0)
        {
            GameObject.Find("PlayerImage").transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        //look left
        if (direction.x < 0 && Math.Abs(direction.x) > Math.Abs(direction.y))
        {
            GameObject.Find("PlayerBowDirection").transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        //look right
        else if (direction.x > 0 && Math.Abs(direction.x) > Math.Abs(direction.y))
        {
            GameObject.Find("PlayerBowDirection").transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        //look up
        else if (direction.y > 0 && Math.Abs(direction.y) > Math.Abs(direction.x))
        {
            GameObject.Find("PlayerBowDirection").transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        //look down
        else if (direction.y < 0 && Math.Abs(direction.y) > Math.Abs(direction.x))
        {
            GameObject.Find("PlayerBowDirection").transform.localEulerAngles = new Vector3(0, 0, -90);
        }
    }
}
