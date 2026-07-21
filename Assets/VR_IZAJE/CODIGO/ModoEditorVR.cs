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

    [Header("Avanzar / Saltar Dialogos")]
    public Key advanceKey = Key.E;
    public Key skipKey = Key.R;

    private bool active = false;
    private float cameraYaw;
    private float cameraPitch;
    private float handYaw;
    private float handPitch;

    private Vector3 originalCameraLocalPos;
    private Quaternion originalCameraLocalRot;
    private Vector3 originalHandLocalPos;
    private Quaternion originalHandLocalRot;
    private Transform originalHandParent;

    private TrackedPoseDriver trackedPoseDriver;
    private TrackedPoseDriver cameraTrackedPoseDriver;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    void Start()
    {
        if (vrCamera == null) vrCamera = Camera.main;

        if (vrCamera != null)
        {
            originalCameraLocalPos = vrCamera.transform.localPosition;
            originalCameraLocalRot = vrCamera.transform.localRotation;
            cameraTrackedPoseDriver = vrCamera.GetComponent<TrackedPoseDriver>();
        }

        if (rightHand != null)
        {
            originalHandLocalPos = rightHand.localPosition;
            originalHandLocalRot = rightHand.localRotation;
            originalHandParent = rightHand.parent;
            trackedPoseDriver = rightHand.GetComponent<TrackedPoseDriver>();
            rayInteractor = rightHand.GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        }
    }

    void Update()
    {
        var kb = Keyboard.current;
        var ms = Mouse.current;
        if (kb == null) return;

        if (kb[Key.F1].wasPressedThisFrame || kb[Key.Escape].wasPressedThisFrame && active)
        {
            active = !active;
            if (active) ActivateEditorMode();
            else DeactivateEditorMode();
        }

        if (!active) return;

        HandleCameraRotation(kb);
        HandleHandRotation(ms);
        FeedTriggerInput(ms);
        HandleMovement(kb);
        HandleDialogKeys(kb);
    }

    void ActivateEditorMode()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (leftHand != null)
            leftHand.SetActive(false);

        if (rightHandAnimation != null)
            rightHandAnimation.enabled = false;

        if (trackedPoseDriver != null)
            trackedPoseDriver.enabled = false;

        if (cameraTrackedPoseDriver != null)
            cameraTrackedPoseDriver.enabled = false;

        if (rayInteractor != null)
            rayInteractor.selectInput.inputSourceMode = XRInputButtonReader.InputSourceMode.ManualValue;

        if (cameraPosition != null && vrCamera != null)
        {
            vrCamera.transform.localPosition = cameraPosition.localPosition;
            vrCamera.transform.localRotation = cameraPosition.localRotation;
        }

        if (handPosition != null && rightHand != null)
        {
            rightHand.SetParent(vrCamera.transform);
            rightHand.localPosition = handPosition.localPosition;
            rightHand.localRotation = handPosition.localRotation;
        }

        if (cameraOffset != null)
            cameraOffset.localRotation = Quaternion.identity;

        cameraYaw = 0f;
        cameraPitch = 0f;
        handYaw = 0f;
        handPitch = 0f;
    }

    void DeactivateEditorMode()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (leftHand != null)
            leftHand.SetActive(true);

        if (rightHandAnimation != null)
            rightHandAnimation.enabled = true;

        if (trackedPoseDriver != null)
            trackedPoseDriver.enabled = true;

        if (cameraTrackedPoseDriver != null)
            cameraTrackedPoseDriver.enabled = true;

        if (vrCamera != null)
        {
            vrCamera.transform.localPosition = originalCameraLocalPos;
            vrCamera.transform.localRotation = originalCameraLocalRot;
        }

        if (rightHand != null)
        {
            rightHand.SetParent(originalHandParent);
            rightHand.localPosition = originalHandLocalPos;
            rightHand.localRotation = originalHandLocalRot;
        }

        if (cameraOffset != null)
            cameraOffset.localRotation = Quaternion.identity;

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
        GUI.Label(new Rect(10, 10, 800, 30),
            "EDITOR (F1) | Click: trigger | Mouse: mano | WASD: cam | Flechas: mover | E: avanzar | R: saltar", style);
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

    void HandleDialogKeys(Keyboard kb)
    {
        if (kb[advanceKey].wasPressedThisFrame)
        {
            var dp = FindFirstObjectByType<DialogPlayer>();
            if (dp != null) dp.Avanzar();
        }
        if (kb[skipKey].wasPressedThisFrame)
        {
            var dp = FindFirstObjectByType<DialogPlayer>();
            if (dp != null) dp.Saltar();
        }
    }

    void HandleCameraRotation(Keyboard kb)
    {
        if (vrCamera == null) return;

        if (kb[Key.A].isPressed) cameraYaw -= cameraRotationSpeed * Time.deltaTime;
        if (kb[Key.D].isPressed) cameraYaw += cameraRotationSpeed * Time.deltaTime;
        if (kb[Key.W].isPressed) cameraPitch -= cameraRotationSpeed * Time.deltaTime;
        if (kb[Key.S].isPressed) cameraPitch += cameraRotationSpeed * Time.deltaTime;

        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);

        vrCamera.transform.rotation = Quaternion.AngleAxis(cameraYaw, Vector3.up) * Quaternion.AngleAxis(cameraPitch, Vector3.right);
    }

    void HandleMovement(Keyboard kb)
    {
        if (characterController == null) return;

        Vector3 forward = Quaternion.AngleAxis(cameraYaw, Vector3.up) * Vector3.forward;
        Vector3 right = Quaternion.AngleAxis(cameraYaw, Vector3.up) * Vector3.right;
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
