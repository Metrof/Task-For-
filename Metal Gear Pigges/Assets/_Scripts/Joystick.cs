using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    Image joystickBg;
    Image joystick;
    Vector2 inputVector;
    void Start()
    {
        joystickBg = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBg.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joystickBg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystickBg.rectTransform.sizeDelta.y);
        }
        inputVector = new Vector2(pos.x * 2, pos.y * 2);
        if (Mathf.Abs(inputVector.x) >= Mathf.Abs(inputVector.y)) inputVector.y = 0;
        if (Mathf.Abs(inputVector.x) <= Mathf.Abs(inputVector.y)) inputVector.x = 0;
        inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        joystick.rectTransform.anchoredPosition = new Vector2(inputVector.x * (joystickBg.rectTransform.sizeDelta.x / 2), inputVector.y * (joystickBg.rectTransform.sizeDelta.y / 2));
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    public Vector3 Direction()
    {
        return new Vector3(Mathf.Round(inputVector.x), Mathf.Round(inputVector.y), 0);
    }
    public float Horizontal()
    {
        if (inputVector.x != 0) 
        {
            if (inputVector.x < 0) inputVector.x = -1f;
            if (inputVector.x > 0) inputVector.x = 1f;
            return inputVector.x;
        } else
        {
            return Input.GetAxis("Horizontal");
        }
    }
    public float Vertical()
    {
        if (inputVector.y != 0)
        {
            if (inputVector.y < 0) inputVector.y = -1f;
            if (inputVector.y > 0) inputVector.y = 1f;
            return inputVector.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}
