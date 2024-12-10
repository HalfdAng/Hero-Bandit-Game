using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class item_pickup : MonoBehaviour
{
    
    public bool _coin;
    public bool _gem;
    public bool _crystal;

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
    }

    /* public void AddCoin(int points)
    {
        _coins += points;
        UI_Manager.instance.updateScore(_coins); //oppdaterer scoren med verdien til _keys
    } */
}
