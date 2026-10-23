using UnityEngine;
using UnityEngine.EventSystems;

// Ekrandaki sanal joystick. Value: -1..1 arası yön (x = sağ/sol, y = ileri/geri)
public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;
    [Range(0f, 1f)] public float deadZone = 0.1f;

    public Vector2 Value { get; private set; }

    private Canvas canvas;

    private void Awake()
    {
        if (background == null)
            background = (RectTransform)transform;
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Camera cam = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, cam, out Vector2 local))
            return;

        Vector2 radius = background.rect.size * 0.5f;
        Vector2 dir = Vector2.ClampMagnitude(new Vector2(local.x / radius.x, local.y / radius.y), 1f);

        Value = dir.magnitude < deadZone ? Vector2.zero : dir;
        if (handle != null)
            handle.anchoredPosition = dir * radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetStick();
    }

    private void OnDisable()
    {
        ResetStick();
    }

    private void ResetStick()
    {
        Value = Vector2.zero;
        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}
