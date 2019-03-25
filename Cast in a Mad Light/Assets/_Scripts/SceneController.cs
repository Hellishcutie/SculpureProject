using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public InteractionDetection[] lightBulbs;
    public float transitionThresholdTime = 1f;
    public int numberLightsForTrigger = 3;

    public GameObject scene2;
    public AuraAPI.Aura aura;
    public MeshRenderer floorMesh;

    public float sceneActivationTime = 2f;

    [Header("Music")]
    public MusicController musicController;
    public AudioClip secondMusicTrack;

    private bool transitionHappened;

    // Start is called before the first frame update
    void Start()
    {
        //lightBulbs = FindObjectsOfType<InteractionDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        int numberTouched = 0;
        int numberTouchedBeforeThreshold = 0;
        foreach (InteractionDetection bulb in lightBulbs)
        {
            if(bulb.hasBeenTouched == true)
            {
                numberTouched++;
                float timeSinceLastTouch = Time.time - bulb.lastTouchTime;
                if (timeSinceLastTouch < transitionThresholdTime)
                {
                    numberTouchedBeforeThreshold++;
                }
            }
        }
        //if(numberTouched >=3)
        if(numberTouchedBeforeThreshold >= numberLightsForTrigger)
        {
            DoSceneTransition();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            DoSceneTransition();
        }

    }

    void DoSceneTransition()
    {
        if(transitionHappened == false)
        {
            transitionHappened = true;
            StartCoroutine(CoSceneTransition(1f));
            musicController.CrossFadeTracks(secondMusicTrack, 1f);
        }

    }

    private IEnumerator CoSceneTransition(float duration)
    {
        float startValue = aura.frustum.settings.density;
    
        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float lerp = timer / duration;
            aura.frustum.settings.density = Mathf.Lerp(startValue, 0, lerp);
            floorMesh.material.SetFloat("_Glossiness", Mathf.Lerp(0, 0.95f, lerp));
            yield return null;
        }

        yield return new WaitForSeconds(sceneActivationTime);

        scene2.SetActive(true);
    }
}
