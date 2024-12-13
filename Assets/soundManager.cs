using UnityEngine;

public class soundManager : MonoBehaviour
{
    public item_pickup collect;
    public level_end stopMain;
    public ArtifactScript artifact;

    [Header("AudioSources")]

    public AudioSource collectController;
    public AudioSource MainSong;

    [Header("Clips")]

    public AudioClip collectClip;
    public AudioClip mainClip;

    private bool coinPlay = true;
    private bool gemPlay = true;
    private bool crystalPlay = true;

    private bool mainPlay = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (collect.GetComponent<item_pickup>()._coin == true && coinPlay == true)
        {
            collectController.PlayOneShot(collectClip);
            coinPlay = false;
        }
        if (collect.GetComponent<item_pickup>()._gem == true && gemPlay == true)
        {
            collectController.PlayOneShot(collectClip);
            gemPlay = false;
        }
        if (collect.GetComponent<item_pickup>()._crystal == true && crystalPlay == true)
        {
            collectController.PlayOneShot(collectClip);
            crystalPlay = false;
        }


        if (stopMain.GetComponent<level_end>().stopMainSong == true)
        {
            MainSong.Stop();
        }

        if (artifact.GetComponent<ArtifactScript>()._started == true && mainPlay == true)
        {
            MainSong.PlayOneShot(mainClip);
            mainPlay = false;
        }
    }
}
