using UnityEngine;
using UnityEngine.Playables;

public class Cutscene1Script : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject CutsceneCamera;

    public void StartCutscene()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        MainCamera.GetComponent<AudioListener>().enabled = false;

        CutsceneCamera.GetComponent<Camera>().enabled = true;
        CutsceneCamera.GetComponent<AudioListener>().enabled = true;

        gameObject.GetComponent<PlayableDirector>().Play();
    }
}
