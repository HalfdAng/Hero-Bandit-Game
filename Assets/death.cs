using UnityEngine;

public class death : MonoBehaviour
{
    private PlayerController playerController;
    public Animator deathText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isDead == true)
        {
            deathText.SetBool("died", true);
        }
    }
}
