using UnityEngine;
using UnityEngine.InputSystem;

public class playerPause : MonoBehaviour
{
    private InputActions input;
    public InputSystem_Actions inputActions;

    public Animator pauseText;

    private bool paused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new InputSystem_Actions();
        input = GetComponent<InputActions>();
        inputActions.Player.Enable();
    }

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.paused && paused == false)
        {
            pauseText.SetBool("paused", true);
            paused = true;
            inputActions.Player.Disable();
            inputActions.UI.Enable();
        }
        else if (input.paused && paused == true)
        {
            pauseText.SetBool("paused", false);
            paused = false;
            inputActions.Player.Enable();
            inputActions.UI.Disable();
        }
    }
}
