using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingEffect : MonoBehaviour
{
    public float amplitude;
    void Update()
    {
        float ypose = ((Mathf.Sin(Time.time) + 1)* 0.5f)*amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, ypose, transform.localPosition.z);
    }
}
