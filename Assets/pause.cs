using UnityEngine;

public class pause : MonoBehaviour
{
    public GameObject pauseScreen;

    public bool paused;

    public bool NoPause;

    private InputActions input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<InputActions>();
        pauseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (input.paused && paused == false && NoPause == false)
        {
            paused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (input.paused && paused == true && NoPause == false)
        {
            paused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (NoPause == true)
        {
            
        }
    }
}
