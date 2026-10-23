using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

// Birinci şahıs mobil oyuncu.
// Telefon: sol alttaki joystick ile yürü, ekranın boş bir yerinde parmağı sürükleyerek etrafa bak.
// Editör/PC: WASD veya ok tuşları ile yürü, sol/sağ fare tuşu basılıyken sürükleyerek bak.
[RequireComponent(typeof(CharacterController))]
public class MobilePlayerController : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform cameraPivot;
    public VirtualJoystick moveJoystick;

    [Header("Hareket")]
    public float moveSpeed = 3f;
    public float gravity = -9.81f;

    [Header("Bakış")]
    [Tooltip("1 = ekran yüksekliği kadar sürükleme 180 derece döndürür")]
    public float lookSensitivity = 1f;
    public float minPitch = -70f;
    public float maxPitch = 80f;

    private CharacterController controller;
    private float yaw;
    private float pitch;
    private float verticalVelocity;
    private int lookTouchId = -1;
    private bool mouseLooking;

    private static readonly List<RaycastResult> uiHits = new List<RaycastResult>();

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        yaw = transform.eulerAngles.y;
        if (cameraPivot != null)
            pitch = Mathf.DeltaAngle(0f, cameraPivot.localEulerAngles.x);
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        lookTouchId = -1;
        mouseLooking = false;
    }

    private void Update()
    {
        Look(ReadLookDelta());
        Move(ReadMoveInput());
    }

    private Vector2 ReadMoveInput()
    {
        Vector2 input = moveJoystick != null ? moveJoystick.Value : Vector2.zero;

        Keyboard kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) input.y += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) input.y -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input.x += 1f;
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) input.x -= 1f;
        }

        return Vector2.ClampMagnitude(input, 1f);
    }

    private Vector2 ReadLookDelta()
    {
        Vector2 delta = Vector2.zero;

        foreach (Touch touch in Touch.activeTouches)
        {
            if (touch.phase == TouchPhase.Began && lookTouchId == -1 && !IsOverInteractiveUI(touch.screenPosition))
                lookTouchId = touch.touchId;

            if (touch.touchId != lookTouchId)
                continue;

            if (touch.phase == TouchPhase.Moved)
                delta += touch.delta;
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                lookTouchId = -1;
        }

        Mouse mouse = Mouse.current;
        if (lookTouchId == -1 && mouse != null)
        {
            bool pressedThisFrame = mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame;
            if (pressedThisFrame && !IsOverInteractiveUI(mouse.position.ReadValue()))
                mouseLooking = true;
            if (!mouse.leftButton.isPressed && !mouse.rightButton.isPressed)
                mouseLooking = false;
            if (mouseLooking)
                delta += mouse.delta.ReadValue();
        }

        return delta;
    }

    private void Look(Vector2 pixelDelta)
    {
        if (pixelDelta == Vector2.zero)
            return;

        float degreesPerPixel = 180f / Mathf.Max(1, Screen.height) * lookSensitivity;
        yaw += pixelDelta.x * degreesPerPixel;
        pitch = Mathf.Clamp(pitch - pixelDelta.y * degreesPerPixel, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void Move(Vector2 input)
    {
        Vector3 move = (transform.right * input.x + transform.forward * input.y) * moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    // Joystick, butonlar veya ekran-üstü UI üzerinde başlayan dokunuşlar kamerayı döndürmez.
    // Dünyadaki (world space) konuşma balonları ise bakışı engellemez, sadece butonları engeller.
    private static bool IsOverInteractiveUI(Vector2 screenPosition)
    {
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem == null)
            return false;

        var pointer = new PointerEventData(eventSystem) { position = screenPosition };
        uiHits.Clear();
        eventSystem.RaycastAll(pointer, uiHits);

        foreach (RaycastResult hit in uiHits)
        {
            if (hit.gameObject == null)
                continue;
            Canvas canvas = hit.gameObject.GetComponentInParent<Canvas>();
            if (canvas != null && canvas.rootCanvas.renderMode != RenderMode.WorldSpace)
                return true;
            if (hit.gameObject.GetComponentInParent<Selectable>() != null)
                return true;
        }
        return false;
    }
}
