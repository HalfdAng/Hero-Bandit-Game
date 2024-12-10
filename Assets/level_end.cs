using UnityEngine;
using UnityEngine.UI;

public class level_end : MonoBehaviour
{
    public GameObject player;

    public Animator fade;
    public Animator coin;
    public Animator gem;
    public Animator crystal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().enabled = false;
            Debug.Log("ay");

            fade.GetComponent<Animator>().SetBool("fade", true);
            coin.GetComponent<Animator>().SetBool("fade", true);
            gem.GetComponent<Animator>().SetBool("fade", true);
            crystal.GetComponent<Animator>().SetBool("fade", true);
        }
    }
}
