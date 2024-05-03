using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum InputType
{
    TapAndDrag = 0,
    tapAndDragV2 = 1

}
public class InputControlller : MonoBehaviour
{
    public InputType inputType;
    [HideInInspector] public float DistanceX;
    [HideInInspector] public float DistanceY;
    public event Action OnMousePositionChanged;

    private bool isFirsttime = true;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;
    public bool IsInputTakeable = false;
    public event Action OnMouseExited;

    private void OnEnable()
    {
        ResetValues();
    }

    private void ResetValues()
    {
        startPosition = Vector3.zero;
        currentPosition = Vector3.zero;
        DistanceX = 0;
        DistanceY = 0;
        IsInputTakeable = false;
        isFirsttime = true;
    }

    private void Update()
    {
        TakeInput();
    }

    private void TakeInput()
    {
        switch (inputType)
        {
            case InputType.TapAndDrag:

            {
                if (Input.GetMouseButtonDown(0)) //butona butona basti
                {
                    IsInputTakeable = true;
                    startPosition = Input.mousePosition;
                    if (isFirsttime)
                    {
                        GameManager.LevelStarted?.Invoke();
                        isFirsttime = false;
                    }
                }

                if (Input.GetMouseButton(0)) //butona basiliyor
                {
                    currentPosition = Input.mousePosition;
                }

                if (Vector3.Distance(startPosition, currentPosition) > .2f && IsInputTakeable)
                {
                    OnMousePositionChanged?.Invoke();
                    DistanceX = (startPosition.x - currentPosition.x);
                    //print(DistanceY);

                    var yDifference = (startPosition.y - currentPosition.y);
                    var yPersentage = (yDifference) / Screen.height;
                    DistanceY = yDifference;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    startPosition = currentPosition = Vector3.zero;
                    DistanceX = 0;
                    DistanceY = 0;
                    IsInputTakeable = false;
                }

                break;
            }
            case InputType.tapAndDragV2:
            {
                if (IsInputTakeable)
                {
                    var xDifference = Input.mousePosition.x - currentPosition.x;
                    var xPersentage = (100f * xDifference) / Screen.width;
                    DistanceX = xPersentage;
                    var yDifference = Input.mousePosition.y - currentPosition.y;
                    var yPersentage = (100f * yDifference) / Screen.height;
                    DistanceY = yPersentage;
                    OnMousePositionChanged?.Invoke();
                }

                if (Input.GetMouseButton(0))
                {
                    IsInputTakeable = true;
                    currentPosition = Input.mousePosition;
                    if (isFirsttime)
                    {
                        GameManager.LevelStarted?.Invoke();
                        isFirsttime = false;
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    IsInputTakeable = false;
                    DistanceX = 0;
                    DistanceY = 0;
                    OnMouseExited?.Invoke();
                }

                break;
            }
            default:
                break;
        }
    }
}