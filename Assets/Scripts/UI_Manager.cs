using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class UI_Manager : MonoBehaviour
{
    public RawImage coin;
    public RawImage gem;
    public RawImage crystal;

    public GameObject collected;

    public static UI_Manager instance;

    void Start()
    {
    }

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (collected.GetComponent<item_pickup>()._coin == false)
        {
            coin.color = Color.black;
        }
        else if (collected.GetComponent<item_pickup>()._coin == true)
        {
            coin.color = Color.white;
        }

        if (collected.GetComponent<item_pickup>()._gem == false)
        {
            gem.color = Color.black;
        }
        else if (collected.GetComponent<item_pickup>()._gem == true)
        {
            gem.color = Color.white;
        }

        if (collected.GetComponent<item_pickup>()._crystal == false)
        {
            crystal.color = Color.black;
        }
        else if (collected.GetComponent<item_pickup>()._crystal == true)
        {
            crystal.color = Color.white;
        }



    }
}
