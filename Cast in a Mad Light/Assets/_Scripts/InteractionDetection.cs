using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] private Renderer _renderer; // "[SerializeField] private" is the same as "public" for the inspector.
    private Coroutine fadeCoroutine = null;

    // Unity Events allow you to call PUBLIC METHODS and CHANGE PUBLIC VARIABLES. (See the inspector)
    // https://docs.unity3d.com/Manual/UnityEvents.html
    // Example: https://unity3d.com/learn/tutorials/topics/user-interface-ui/ui-button
    public UnityEvent OnFirstTouchEvent;    // Event for the FIRST time the object is touched. 
    public UnityEvent OnAnyTouchEvent;      // Event for ANY time the object is touched.
    public UnityEvent OnEnableEvent;        // Event for when the object is turned on.

    public float jiggleForce = 5f;

    [Header("Scaling Effect")]
    public GameObject scalingChildGameObject;
    public float scaleUpTime = 1f;

    public Color hitColor;
    private Color defaultColor;

    public Light _light;
    public float hitLightIntensity = 2f;
    public float hitLightRange = 10f;

    public float lastTouchTime = -1;

    private void Awake()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        if (_renderer == null)
        {
            _renderer = GetComponentInChildren<Renderer>();
        }

        if(_renderer)
        {
            defaultColor = _renderer.material.GetColor("_Color");
        }

        if(!_light)
        {
            _light = GetComponentInChildren<Light>();
        }
    }

    private void OnEnable()
    {
        OnEnableEvent.Invoke();
        //StartCoroutine(CoJiggle(jiggleWaitTime));
        GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * jiggleForce, ForceMode.Impulse);
        StartCoroutine(CoScaleUp(scaleUpTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false) // Tag check to make sure the object is the players hands.
            return;
        if (enter)
        {
            if(!hasBeenTouched)
            {
                OnFirstTouchEvent.Invoke();  // Invoke the FIRST TOUCH event.
                hasBeenTouched = true;
            }
            lastTouchTime = Time.time;

            OnAnyTouchEvent.Invoke();           // Invoke the ANY TOUCH event.

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }
            fadeCoroutine = StartCoroutine(CoHitDistort(2f));

            //_renderer.material.SetFloat("_intensity", distort);
            //_renderer.material.SetFloat("_deform", 2);

            //Debug.Log(_renderer.sharedMaterial.GetFloat("_intensity") );
            //Debug.Log(_renderer.sharedMaterial.GetFloat("_deform"));
            //Debug.Log("Entered");
        }
    }


    private void OnTriggerStay(Collider other)
    {
        return;
        if (other.CompareTag("Player") == false)
            return;

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
                //Debug.Log(_renderer.sharedMaterial.GetFloat("_intensity"));
                //Debug.Log(_renderer.sharedMaterial.GetFloat("_deform"));

                

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
        return;
        if (other.CompareTag("Player") == false)
            return;
        if (exit)
        {
            //_renderer.material.SetFloat("_intensity", 0);
            //_renderer.material.SetFloat("_deform", 0);
            //Debug.Log(_renderer.sharedMaterial.GetFloat("_intensity"));
            //Debug.Log(_renderer.sharedMaterial.GetFloat("_deform"));


            fadeCoroutine = StartCoroutine(CoFadeDistort(2f));


            stayTime = 0.0f;
            Debug.Log("exit");
        }
    }



    private IEnumerator CoHitDistort(float duration)
    {
        float startDistort = _renderer.material.GetFloat("_deform");
        float currentDistort = startDistort;
        float startLightIntensity = _light.intensity;
        float startLightRange = _light.range;
        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float lerp = timer / duration;
            currentDistort = Mathf.Lerp(maxDeform, 0, lerp);
            _renderer.material.SetFloat("_intensity", currentDistort);
            _renderer.material.SetFloat("_deform", currentDistort);

            _renderer.material.SetColor("_Color", Color.Lerp(hitColor, defaultColor, lerp));

            _light.intensity = Mathf.Lerp(hitLightIntensity, startLightIntensity, lerp);
            _light.range = Mathf.Lerp(hitLightRange, startLightRange, lerp);

            //Debug.Log(_renderer.sharedMaterial.GetFloat("_intensity"));
            //Debug.Log(_renderer.sharedMaterial.GetFloat("_deform"));
            yield return null;
        }
        fadeCoroutine = null;
    }

    private IEnumerator CoScaleUp(float duration)
    {
        Vector3 endScale = scalingChildGameObject.transform.localScale;// _renderer.material.GetFloat("_deform");
        //Vector3 currentDistort = startDistort;
        scalingChildGameObject.transform.localScale = Vector3.zero;
        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float lerp = timer / duration;
            scalingChildGameObject.transform.localScale = Vector3.Lerp(Vector3.zero, endScale, lerp);
            yield return null;
        }
        //fadeCoroutine = null;
    }


    private IEnumerator CoFadeDistort(float duration)
    {
        float startDistort = _renderer.material.GetFloat("_deform");
        float currentDistort = startDistort;
        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            //timer += duration;
            float lerp = timer / duration;
            currentDistort = Mathf.Lerp(startDistort, 0, lerp);
            _renderer.material.SetFloat("_intensity", currentDistort);
            _renderer.material.SetFloat("_deform", currentDistort);

            //Debug.Log(_renderer.sharedMaterial.GetFloat("_intensity"));
            //Debug.Log(_renderer.sharedMaterial.GetFloat("_deform"));
            yield return null;
        }
        fadeCoroutine = null;
    }
}
