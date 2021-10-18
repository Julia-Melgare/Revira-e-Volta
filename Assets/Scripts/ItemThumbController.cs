using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemThumbController : MonoBehaviour
{
    [SerializeField] private Sprite[] itemThumbs;
    [SerializeField] private GameObject itemThumbCanvas;
    // Start is called before the first frame update
    void Start()
    {
        var active = SceneManager.GetActiveScene();
        Debug.Log("BuildIndex: "+ active.buildIndex);
        switch (active.buildIndex)
        {
            case 3:
                itemThumbCanvas.GetComponent<Image>().sprite = itemThumbs[0];
                break;
            case 4:
                Debug.Log("BuildIndex: " + active.buildIndex);
                itemThumbCanvas.GetComponent<Image>().sprite = itemThumbs[1];
                break;
        }
    }

}
