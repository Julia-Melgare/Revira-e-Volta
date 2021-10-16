using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticuleController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Image reticule;

    private Camera main;

    private Color baseColor;
    private void Awake()
    {
        main = Camera.main;
        baseColor = reticule.color;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        bool tryHit = Physics.Raycast(main.transform.position, main.transform.forward, 10f, ~3, QueryTriggerInteraction.UseGlobal);

        if (tryHit)
        {
            reticule.color = Color.white;
        }
        else
        {
            reticule.color = baseColor;
        }

    }
}
