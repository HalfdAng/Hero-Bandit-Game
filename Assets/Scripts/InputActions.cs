using UnityEngine;

public class InputActions : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;

    public bool Jump;
    public bool Slide;

    private void Update()
    {
        Jump = _inputSystem.Player.Jump.IsPressed();
        Slide = _inputSystem.Player.Slide.IsPressed();
    }

    private void Awake()
    {
        _inputSystem = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        _inputSystem.Enable();
    }

    private void OnDisable()
    {
        _inputSystem.Disable();
    }
}
