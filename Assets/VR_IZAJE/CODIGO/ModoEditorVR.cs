using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class ModoEditorVR : MonoBehaviour
{
    [Header("Referencias")]
    public Camera vrCamera;
    public Transform cameraOffset;
    public Transform rightHand;
    public CharacterController characterController;

    [Header("Animacion Manos")]
    public AnimatedHandInput rightHandAnimation;
    public Animator rightHandAnimator;

    [Header("Referencia Mano Izquierda (se apaga)")]
    public GameObject leftHand;

    [Header("Posiciones Editor")]
    public Transform cameraPosition;
    public Transform handPosition;

    [Header("Rotacion de Mano (Mouse)")]
    public float handRotationSpeed = 100f;

    [Header("Rotacion de Camara (WASD)")]
    public float cameraRotationSpeed = 80f;

    [Header("Movimiento Personaje (Flechas)")]
    public float moveSpeed = 3f;

    [Header("Toggle")]
    public KeyCode toggleKey = KeyCode.F1;

    private bool active = false;
    private float cameraYaw;
    private float cameraPitch;
    private float handYaw;
    private float handPitch;

    private Vector3 originalCameraLocalPos;
    private Quaternion originalCameraLocalRot;
    private Vector3 originalHandLocalPos;
    private Quaternion originalHandLocalRot;

    private TrackedPoseDriver trackedPoseDriver;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    void Start()
    {
        if (vrCamera == null) vrCamera = Camera.main;

        if (vrCamera != null)
        {
            originalCameraLocalPos = vrCamera.transform.localPosition;
            originalCameraLocalRot = vrCamera.transform.localRotation;
        }

        if (rightHand != null)
        {
            originalHandLocalPos = rightHand.localPosition;
            originalHandLocalRot = rightHand.localRotation;
            trackedPoseDriver = rightHand.GetComponent<TrackedPoseDriver>();
            rayInteractor = rightHand.GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        }

        if (cameraOffset != null)
            cameraYaw = cameraOffset.eulerAngles.y;
    }

    void Update()
    {
        var kb = Keyboard.current;
        var ms = Mouse.current;
        if (kb == null) return;

        if (kb[Key.F1].wasPressedThisFrame)
        {
            active = !active;
            if (active) ActivateEditorMode();
            else DeactivateEditorMode();
        }

        if (!active) return;

        HandleHandRotation(ms);
        FeedTriggerInput(ms);
        HandleCameraRotation(kb);
        HandleMovement(kb);
    }

    void ActivateEditorMode()
    {
        Cursor.visible = false;

        if (leftHand != null)
            leftHand.SetActive(false);

        if (rightHandAnimation != null)
            rightHandAnimation.enabled = false;

        if (trackedPoseDriver != null)
            trackedPoseDriver.enabled = false;

        if (rayInteractor != null)
            rayInteractor.selectInput.inputSourceMode = XRInputButtonReader.InputSourceMode.ManualValue;

        if (cameraPosition != null && vrCamera != null)
        {
            vrCamera.transform.localPosition = cameraPosition.localPosition;
            vrCamera.transform.localRotation = cameraPosition.localRotation;
        }

        if (handPosition != null && rightHand != null)
        {
            rightHand.localPosition = handPosition.localPosition;
            rightHand.localRotation = handPosition.localRotation;
        }

        handYaw = 0f;
        handPitch = 0f;
    }

    void DeactivateEditorMode()
    {
        Cursor.visible = true;

        if (leftHand != null)
            leftHand.SetActive(true);

        if (rightHandAnimation != null)
            rightHandAnimation.enabled = true;

        if (trackedPoseDriver != null)
            trackedPoseDriver.enabled = true;

        if (vrCamera != null)
        {
            vrCamera.transform.localPosition = originalCameraLocalPos;
            vrCamera.transform.localRotation = originalCameraLocalRot;
        }

        if (rightHand != null)
        {
            rightHand.localPosition = originalHandLocalPos;
            rightHand.localRotation = originalHandLocalRot;
        }

        if (cameraOffset != null)
            cameraOffset.rotation = Quaternion.identity;

        if (rightHandAnimator != null)
        {
            rightHandAnimator.SetFloat("Trigger", 0f);
            rightHandAnimator.SetFloat("Grip", 0f);
        }

        cameraPitch = 0f;
        cameraYaw = 0f;
        handYaw = 0f;
        handPitch = 0f;
    }

    void OnGUI()
    {
        if (!active) return;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 14;
        style.normal.textColor = Color.green;
        GUI.Label(new Rect(10, 10, 600, 30),
            "MODO EDITOR (F1) | ClickIzq: gatillo | Mouse: mano | WASD: camara | Flechas: mover", style);
    }

    void HandleHandRotation(Mouse ms)
    {
        if (rightHand == null || ms == null) return;

        Vector2 delta = ms.delta.ReadValue();
        handYaw += delta.x * handRotationSpeed * Time.deltaTime;
        handPitch -= delta.y * handRotationSpeed * Time.deltaTime;
        handPitch = Mathf.Clamp(handPitch, -80f, 80f);

        rightHand.localRotation = Quaternion.Euler(handPitch, handYaw, 0f);
    }

    void FeedTriggerInput(Mouse ms)
    {
        if (ms == null || rayInteractor == null) return;

        bool triggerPressed = ms.leftButton.isPressed;

        if (rightHandAnimator != null)
            rightHandAnimator.SetFloat("Trigger", triggerPressed ? 1f : 0f);

        rayInteractor.selectInput.QueueManualState(triggerPressed, triggerPressed ? 1f : 0f);
    }

    void HandleCameraRotation(Keyboard kb)
    {
        if (cameraOffset == null) return;

        if (kb[Key.A].isPressed) cameraYaw -= cameraRotationSpeed * Time.deltaTime;
        if (kb[Key.D].isPressed) cameraYaw += cameraRotationSpeed * Time.deltaTime;
        if (kb[Key.W].isPressed) cameraPitch -= cameraRotationSpeed * Time.deltaTime;
        if (kb[Key.S].isPressed) cameraPitch += cameraRotationSpeed * Time.deltaTime;

        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);
        cameraOffset.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
    }

    void HandleMovement(Keyboard kb)
    {
        if (characterController == null) return;

        Vector3 forward = cameraOffset != null ? cameraOffset.forward : Vector3.forward;
        Vector3 right = cameraOffset != null ? cameraOffset.right : Vector3.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();

        Vector3 dir = Vector3.zero;
        if (kb[Key.UpArrow].isPressed) dir += forward;
        if (kb[Key.DownArrow].isPressed) dir -= forward;
        if (kb[Key.RightArrow].isPressed) dir += right;
        if (kb[Key.LeftArrow].isPressed) dir -= right;

        if (dir != Vector3.zero)
            characterController.Move(dir.normalized * moveSpeed * Time.deltaTime);
    }
}
