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
        switch (active.buildIndex)
        {
            case 3:
                itemThumbCanvas.GetComponent<Image>().sprite = itemThumbs[3];
                break;
            case 4:
                itemThumbCanvas.GetComponent<Image>().sprite = itemThumbs[4];
                break;
        }
    }

}
