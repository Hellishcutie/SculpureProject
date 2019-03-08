using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetection : MonoBehaviour
{
    public bool enter = true;
    public bool stay = true;
    public bool exit = true;
    public float distort = 8;
    public float maxDistort = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (enter)
        {
            GetComponent<Renderer>().material.SetFloat("_intensity", distort);
            GetComponent<Renderer>().material.SetFloat("_deform", 2);
            Debug.Log(gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_intensity"));
            Debug.Log(gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_deform"));

            Debug.Log("Entered");
        }
    }
    private float stayCount = 0.0f;

    private void OnTriggerStay(Collider other)
    {
       
        if (stay)
        {
            if (stayCount > 2f)
            {
                //distort += distortPerSecond * Time.detaTime;
                Debug.Log("Staying");
                GetComponent<Renderer>().material.SetFloat("_intensity", maxDistort);
                GetComponent<Renderer>().material.SetFloat("_deform", 8);
                Debug.Log(gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_intensity"));
                Debug.Log(gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_deform"));

                

                //exponential increase testing
                

            }
            else
            {
                stayCount = stayCount + Time.deltaTime;
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (exit)
        {
            GetComponent<Renderer>().material.SetFloat("_intensity", 0);
            GetComponent<Renderer>().material.SetFloat("_deform", 0);
            Debug.Log(gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_intensity"));
            Debug.Log(gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_deform"));

            stayCount = 0.0f;
            Debug.Log("exit");
        }
    }
}
