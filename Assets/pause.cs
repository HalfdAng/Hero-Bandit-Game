using UnityEngine;

public class pause : MonoBehaviour
{
    public GameObject pauseScreen;

    public bool paused;

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
        if (input.pause && paused == false)
        {
            paused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (input.pause && paused == true)
        {
            paused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
