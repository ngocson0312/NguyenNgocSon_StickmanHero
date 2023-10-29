using UnityEngine.EventSystems;
using UnityEngine;

public class ThumbStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform backGround;
    [SerializeField] RectTransform handle;
    public Vector2 direction;
    private Vector2 centre;
    int pointerID = -99;
    [SerializeField] float deadZone = 0.1f;
    private float maxX;
    private float minX;
    private void Awake()
    {
        maxX = backGround.position.x + backGround.rect.width / 2;
        minX = backGround.position.x - backGround.rect.width / 2;
        centre = backGround.position;
        handle.localPosition = Vector3.zero;
        direction = Vector2.zero;
        backGround.gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pointerID = eventData.pointerId;
        backGround.position = eventData.position;
        centre = backGround.position;
        maxX = backGround.position.x + backGround.rect.width / 2;
        minX = backGround.position.x - backGround.rect.width / 2;
        backGround.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pointerID != eventData.pointerId) return;
        HandleInput(eventData.position);
    }

    void HandleInput(Vector2 eventPosition)
    {
        eventPosition.x = Mathf.Clamp(eventPosition.x, minX, maxX);
        eventPosition.y = backGround.position.y;
        handle.position = eventPosition;
        direction = (Vector2)handle.position - centre;
        if (Mathf.Abs(direction.x) / (backGround.rect.width / 2) <= deadZone)
        {
            direction = Vector2.zero;
            handle.localPosition = Vector2.zero;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.localPosition = Vector3.zero;
        direction = Vector2.zero;
        backGround.gameObject.SetActive(false);
    }
}
