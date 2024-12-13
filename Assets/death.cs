using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class death : MonoBehaviour
{
    private PlayerController playerController;
    //public CinemachineCamera cinemachineCamera;
    public Animator deathText;
    public Animator deathFade;

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
            deathFade.SetBool("died", true);
            playerController.CharacterActive = false;
            //cinemachineCamera.enabled = false;
            //cinemachineCamera.GetComponent<CinemachineCamera>().enabled = false;
        }
    }
}
