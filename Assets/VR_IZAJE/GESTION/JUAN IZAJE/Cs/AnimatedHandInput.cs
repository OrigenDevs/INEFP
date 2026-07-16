using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatedHandInput : MonoBehaviour
{
    public InputActionProperty triggerValue;
    public InputActionProperty gripValue;

    public Animator animatorHand;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float trigger = triggerValue.action.ReadValue<float>();
        float grip = gripValue.action.ReadValue<float>();

        animatorHand.SetFloat("Trigger", trigger);
        animatorHand.SetFloat("Grip", grip);

        
    }
}
