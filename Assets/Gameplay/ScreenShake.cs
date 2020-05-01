using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    public float shakeIntensityLerpSpeed = 1;
    public float positionalShakeAmount = 1;
    public float rotationalShakeAmount = 1;
    public float virtualShakeIntensity;

    private float targetIntensity;
    private float currentIntensity;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        targetIntensity = virtualShakeIntensity;

        if (targetIntensity == 0 && currentIntensity < 0.001f)
        {
            currentIntensity = 0;
        }
        else
        {
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * shakeIntensityLerpSpeed);
        }

        transform.localPosition = RandomVector3() * currentIntensity * 0.002f * positionalShakeAmount;
        transform.localRotation = Quaternion.Lerp(Quaternion.identity, RandomQuaternion(), currentIntensity * 0.0001f * rotationalShakeAmount);
    }

    private Vector3 RandomVector3()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private Quaternion RandomQuaternion()
    {
        return Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
    }

    public void SetShakeImpulse(float intensity, float speed)
    {
        shakeIntensityLerpSpeed = speed;
        currentIntensity += intensity;
        targetIntensity = 0;
    }
}