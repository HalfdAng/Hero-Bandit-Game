using UnityEngine;
using UnityEngine.SceneManagement;

public class deathFade : MonoBehaviour
{
    public bool fadeAway = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeAway == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
