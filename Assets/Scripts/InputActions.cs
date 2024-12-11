using UnityEngine;

public class InputActions : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;

    public bool Jump;
    public bool Slide;

    public bool pause;

    private void Update()
    {
        _inputSystem.Player.Enable();
        Jump = _inputSystem.Player.Jump.IsPressed();
        Slide = _inputSystem.Player.Slide.IsPressed();
        //paused = _inputSystem.UI.Pause.WasPressedThisFrame();
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
