using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class item_pickup : MonoBehaviour
{
    public Animator prompt;

    private InputActions _interact;

    public ArtifactScript artifact;

    
    public bool _coin;
    public bool _gem;
    public bool _crystal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _interact = GetComponent<InputActions>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Coin")) //når spilleren kolliderer med et objekt med tag "Coin"
        {
            Destroy(other.gameObject); //ødellege objektet den kolliderte med
            Debug.Log("(:)");
            _coin = true;
        }
        if(other.transform.CompareTag("Gem")) //når spilleren kolliderer med et objekt med tag "Coin"
        {
            Destroy(other.gameObject); //ødellege objektet den kolliderte med
            Debug.Log("(:)");
            _gem = true;
        }
        if(other.transform.CompareTag("Crystal")) //når spilleren kolliderer med et objekt med tag "Coin"
        {
            Destroy(other.gameObject); //ødellege objektet den kolliderte med
            Debug.Log("(:)");
            _crystal = true;
        }


        if(other.transform.CompareTag("artifact"))
        {
            prompt.SetBool("fade", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.CompareTag("artifact"))
        {
            if (_interact.interact == true)
            {
                artifact.PickUp();
                prompt.SetBool("fade", false);
                Debug.Log("aoga");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("artifact"))
        {
            prompt.SetBool("fade", false);
        }
    }

    /* public void AddCoin(int points)
    {
        _coins += points;
        UI_Manager.instance.updateScore(_coins); //oppdaterer scoren med verdien til _keys
    } */
}
