using UnityEngine;
using System.Collections;
using System;

public class Shake : MonoBehaviour
{
    public float Duration = 1f;
    public float ShakeRange = 1f;
    public AnimationCurve Amplitude = AnimationCurve.Linear(0, 1, 1, 1);
    public AnimationCurve Frequency = AnimationCurve.Linear(0, 1, 1, 1);

    public void StartShake()
    {
        StartCoroutine(ShakeCR());
    }

    private IEnumerator ShakeCR()
    {
        float timepassed = 0;
        float x = UnityEngine.Random.Range(0, 1000);
        float y = UnityEngine.Random.Range(0, 1000);
        float d = 0; //Use to store sampling speed
        Vector3 originalPos = transform.position;


        while (timepassed < Duration)
        {
            float t = timepassed / Duration;
            float amplitude = Amplitude.Evaluate(t);
            float samplespeed = Frequency.Evaluate(t);
            
            //ScreenshakeMAGIC
            d += timepassed * samplespeed;
            float xOffset = PerlinSample(x + d, y) * amplitude * ShakeRange;
            float yOffset = PerlinSample(x, y + d) * amplitude * ShakeRange;

            //Check and Move the object
            Move(xOffset, yOffset, originalPos);

            //PassedTime and frameskip  
            timepassed += Time.deltaTime;
            yield return null;
        }
    }

    private void Move(float xOffset, float yOffset, Vector3 originalPos)
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.position = new Vector2(originalPos.x + xOffset, originalPos.y + yOffset);
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.position = originalPos + new Vector3(xOffset, yOffset);
            return;
        }

        transform.position = originalPos + new Vector3(xOffset, yOffset);
    }

    private float PerlinSample(float x, float y)
    {
        return (Mathf.PerlinNoise(x, y)*2-1);
    }
}
