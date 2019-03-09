using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    public InteractionDetection[] lightBulbs;

    public GameObject scene2;

    // Start is called before the first frame update
    void Start()
    {
        lightBulbs = FindObjectsOfType<InteractionDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        int numberTouched = 0;
        foreach (InteractionDetection bulb in lightBulbs)
        {
            if(bulb.hasBeenTouched == true)
            {
                numberTouched++;
            }
        }
        if(numberTouched >=2)
        {
            scene2.SetActive(true);
        }


    }
}
