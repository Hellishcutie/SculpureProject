using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetection : MonoBehaviour
{
    public bool hasBeenTouched = false;

    public bool enter = true;
    public bool stay = true;
    public bool exit = true;
    public float distort = 8;
    public float maxDistort = 20;
    public float distortPerSecond = 1.0f;

    public AnimationCurve deformCurve;
    float maxDeform = 20f;
    public AnimationCurve intensityCurve;
    float maxIntensity = 20f;

    private float stayTime = 0.0f;
    public float minStayTime = 2f;
    public float maxStayTime = 3f;

    private Renderer _renderer;
    private Coroutine fadeCoroutine = null;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enter)
        {
            hasBeenTouched = true;
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }

            _renderer.material.SetFloat("_intensity", distort);
            _renderer.material.SetFloat("_deform", 2);
            Debug.Log(_renderer.sharedMaterial.GetFloat("_intensity") );
            Debug.Log(_renderer.sharedMaterial.GetFloat("_deform"));

            Debug.Log("Entered");
        }
    }


    private void OnTriggerStay(Collider other)
    {
       
        if (stay)
        {
            stayTime = stayTime + Time.fixedDeltaTime;

            if (stayTime > minStayTime)
            {
                //distort += distortPerSecond * Time.fixedDeltaTime;// detaTime;
                stayTime = Mathf.Clamp(stayTime, 0f, maxStayTime);
                float lerp = (stayTime - minStayTime) / (maxStayTime - minStayTime);
                distort = deformCurve.Evaluate(lerp) * maxDistort;

                //distort 
                Debug.Log("Staying");
                _renderer.material.SetFloat("_intensity", intensityCurve.Evaluate(lerp) * maxIntensity);
                _renderer.material.SetFloat("_deform", deformCurve.Evaluate(lerp) * maxDeform);
                Debug.Log(_renderer.sharedMaterial.GetFloat("_intensity"));
                Debug.Log(_renderer.sharedMaterial.GetFloat("_deform"));

                

                //exponential increase testing
                

            }
            else
            {
                //stayTime = stayTime + Time.fixedDeltaTime;
            }

            
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (exit)
        {
            //_renderer.material.SetFloat("_intensity", 0);
            //_renderer.material.SetFloat("_deform", 0);
            Debug.Log(_renderer.sharedMaterial.GetFloat("_intensity"));
            Debug.Log(_renderer.sharedMaterial.GetFloat("_deform"));

            fadeCoroutine = StartCoroutine(CoFadeDistort(2f));


            stayTime = 0.0f;
            Debug.Log("exit");
        }
    }

    private IEnumerator CoFadeDistort(float duration)
    {
        float startDistort = _renderer.material.GetFloat("_deform");
        float currentDistort = startDistort;
        float timer = 0f;
        while (timer <= duration)
        {
            timer += duration;
            float lerp = timer / duration;
            currentDistort = Mathf.Lerp(startDistort, 0, lerp);
            _renderer.material.SetFloat("_intensity", currentDistort);
            _renderer.material.SetFloat("_deform", currentDistort);

            Debug.Log(_renderer.sharedMaterial.GetFloat("_intensity"));
            Debug.Log(_renderer.sharedMaterial.GetFloat("_deform"));
            yield return null;
        }
        fadeCoroutine = null;
    }
}
