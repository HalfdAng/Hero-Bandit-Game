using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject _pause;

    [Header("Animations")]
    public Animator background;
    public Animator gameName;
    public Animator startButton;
    public Animator exitButton;
    public Animator timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _pause.GetComponent<pause>().NoPause = true;
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        _pause.GetComponent<pause>().NoPause = false;
        background.SetBool("fade", true);
        gameName.SetBool("fade", true);
        startButton.SetBool("fade", true);
        exitButton.SetBool("fade", true);
        timer.SetBool("start", true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
