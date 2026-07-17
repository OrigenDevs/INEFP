using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ModoEditorVR : MonoBehaviour
{
    [Header("Referencias")]
    public Camera vrCamera;
    public Transform cameraOffset;
    public Transform rightHand;
    public XRDirectInteractor rightHandInteractor;
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

    [Header("Raycast Interaccion")]
    public float raycastDistance = 10f;

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

    private bool triggerWasPressed;
    private bool triggerPressed;
    private bool gripWasPressed;
    private bool gripPressed;

    private RaycastHit lastHit;
    private bool hitObject;

    private SimpleGrab grabbedObject;
    private Button3D hoveredButton;
    private RotacionArrastre rotatingObject;
    private CambioMaterialHover hoveredMaterialChanger;

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
        HandleClickInput(ms);
        HandleCameraRotation(kb);
        HandleMovement(kb);
    }

    void ActivateEditorMode()
    {
        if (leftHand != null)
            leftHand.SetActive(false);

        if (rightHandAnimation != null)
            rightHandAnimation.enabled = false;

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

        if (rightHandInteractor != null)
            rightHandInteractor.selectInput.inputSourceMode = XRInputButtonReader.InputSourceMode.ManualValue;
    }

    void DeactivateEditorMode()
    {
        if (leftHand != null)
            leftHand.SetActive(true);

        if (rightHandAnimation != null)
            rightHandAnimation.enabled = true;

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

        if (rightHandInteractor != null)
        {
            rightHandInteractor.selectInput.manualPerformed = false;
            rightHandInteractor.activateInput.manualPerformed = false;
        }

        if (rightHandAnimator != null)
        {
            rightHandAnimator.SetFloat("Trigger", 0f);
            rightHandAnimator.SetFloat("Grip", 0f);
        }

        if (grabbedObject != null)
            grabbedObject = null;
        if (rotatingObject != null)
        {
            rotatingObject.TerminarRotacion();
            rotatingObject = null;
        }
        if (hoveredButton != null)
        {
            hoveredButton.OnHoverEnd();
            hoveredButton = null;
        }
        if (hoveredMaterialChanger != null)
        {
            hoveredMaterialChanger.OnHoverEnd();
            hoveredMaterialChanger = null;
        }

        cameraPitch = 0f;
        cameraYaw = 0f;
        handYaw = 0f;
        handPitch = 0f;
        triggerWasPressed = false;
        gripWasPressed = false;
    }

    void OnGUI()
    {
        if (!active) return;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 14;
        style.normal.textColor = Color.green;
        GUI.Label(new Rect(10, 10, 600, 30),
            "MODO EDITOR (F1) | ClickIzq: agarrar/gatillo | ClickDer: grip | Mouse: mano | WASD: camara | Flechas: mover", style);
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

    void HandleClickInput(Mouse ms)
    {
        if (ms == null) return;

        triggerPressed = ms.leftButton.isPressed;
        gripPressed = ms.rightButton.isPressed;

        if (rightHandInteractor != null)
        {
            rightHandInteractor.selectInput.manualPerformed = triggerPressed;
            rightHandInteractor.activateInput.manualPerformed = gripPressed;
        }

        if (rightHandAnimator != null)
        {
            rightHandAnimator.SetFloat("Trigger", triggerPressed ? 1f : 0f);
            rightHandAnimator.SetFloat("Grip", gripPressed ? 1f : 0f);
        }

        DoRaycast();

        bool triggerJustPressed = triggerPressed && !triggerWasPressed;
        bool triggerJustReleased = !triggerPressed && triggerWasPressed;
        triggerWasPressed = triggerPressed;

        bool gripJustPressed = gripPressed && !gripWasPressed;
        bool gripJustReleased = !gripPressed && gripWasPressed;
        gripWasPressed = gripPressed;

        HandleHover();

        if (triggerJustPressed)
            HandleTriggerDown();
        else if (triggerJustReleased)
            HandleTriggerUp();

        if (gripJustPressed)
            HandleGripDown();
        else if (gripJustReleased)
            HandleGripUp();
    }

    void DoRaycast()
    {
        if (vrCamera == null) return;
        Vector3 origin = vrCamera.transform.position;
        Vector3 direction = vrCamera.transform.forward;
        hitObject = Physics.Raycast(origin, direction, out lastHit, raycastDistance);
    }

    void HandleHover()
    {
        CambioMaterialHover cmh = null;
        if (hitObject && lastHit.collider != null)
            cmh = CambioMaterialHover.BuscarPorCollider(lastHit.collider);

        if (cmh != hoveredMaterialChanger)
        {
            if (hoveredMaterialChanger != null) hoveredMaterialChanger.OnHoverEnd();
            if (cmh != null) cmh.OnHoverStart();
            hoveredMaterialChanger = cmh;
        }

        Button3D btn = null;
        if (hitObject && lastHit.collider != null && grabbedObject == null)
            btn = lastHit.collider.GetComponentInParent<Button3D>();

        if (btn != hoveredButton)
        {
            if (hoveredButton != null) hoveredButton.OnHoverEnd();
            if (btn != null) btn.OnHoverStart();
            hoveredButton = btn;
        }
    }

    void HandleTriggerDown()
    {
        if (!hitObject || lastHit.collider == null) return;

        SimpleGrab sg = lastHit.collider.GetComponentInParent<SimpleGrab>();
        if (sg != null)
        {
            grabbedObject = sg;
            grabbedObject.Agarrar(rightHand);
        }
        else
        {
            RotacionArrastre ra = lastHit.collider.GetComponentInParent<RotacionArrastre>();
            if (ra != null)
            {
                rotatingObject = ra;
                rotatingObject.EmpezarRotacion(lastHit.point);
            }
            else if (hoveredButton != null)
            {
                hoveredButton.OnPress();
            }
            else
            {
                SistemaAutopartes sap = lastHit.collider.GetComponentInParent<SistemaAutopartes>();
                if (sap != null)
                    sap.Cambiar();
            }
        }
    }

    void HandleTriggerUp()
    {
        if (grabbedObject != null)
        {
            grabbedObject.Soltar();
            grabbedObject = null;
        }

        if (rotatingObject != null)
        {
            rotatingObject.TerminarRotacion();
            rotatingObject = null;
        }

        if (hoveredButton != null)
        {
            hoveredButton.OnRelease();
        }
    }

    void HandleGripDown()
    {
    }

    void HandleGripUp()
    {
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
