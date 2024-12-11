using UnityEngine;

public class InputActions : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;

    public bool Jump;
    public bool Slide;

<<<<<<< Updated upstream
    public bool pause;
=======
    public bool paused;
>>>>>>> Stashed changes

    private void Update()
    {
        _inputSystem.Player.Enable();
        Jump = _inputSystem.Player.Jump.IsPressed();
        Slide = _inputSystem.Player.Slide.IsPressed();
<<<<<<< Updated upstream
        pause = _inputSystem.Player.Pause.WasPressedThisFrame();
        pause = _inputSystem.UI.Paused.WasPressedThisFrame();
=======
        paused = _inputSystem.UI.Pause.WasPressedThisFrame();
>>>>>>> Stashed changes
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
