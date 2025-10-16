using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("Elementos")]
    [SerializeField] private Transform cropRenderer;
    [SerializeField] private ParticleSystem harvestedParticles;

    public void ScaleUp()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleRoutine(Vector3.one, 0.5f)); // escala hasta 1 en 0.5s
    }

    public void ScaleDown()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleRoutine(Vector3.zero, 0.5f, true)); // escala hasta 0 y luego destruye

        harvestedParticles.transform.parent = null;
        harvestedParticles.gameObject.SetActive(true);
        harvestedParticles.Play();
        Debug.Log("Deberiafuncionar");
    }

    private IEnumerator ScaleRoutine(Vector3 targetScale, float duration, bool destroyOnEnd = false)
    {
        Vector3 initialScale = cropRenderer.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // easing tipo "easeOutBack"
            float easeT = EaseOutBack(t);

            cropRenderer.localScale = Vector3.LerpUnclamped(initialScale, targetScale, easeT);
            yield return null;
        }

        cropRenderer.localScale = targetScale;

     //   if (destroyOnEnd)
       //     DestroyCrop();
    }

    // Funcn para simular LeanTweenType.easeOutBack
    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }

    public void DestroyCrop()
    {
        Destroy(gameObject);
    }
}