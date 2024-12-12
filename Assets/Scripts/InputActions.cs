using UnityEngine;

public class InputActions : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;

    public bool Jump;
    public bool Slide;

    public bool paused;

    public bool interact;

    private void Update()
    {
        _inputSystem.Player.Enable();
        Jump = _inputSystem.Player.Jump.IsPressed();
        Slide = _inputSystem.Player.Slide.IsPressed();
        paused = _inputSystem.UI.Paused.WasPressedThisFrame();
        interact = _inputSystem.Player.Interact.IsPressed();
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
