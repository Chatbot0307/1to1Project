using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CameraFollow cameraFollow;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        cameraFollow = GetComponent<CameraFollow>();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (cameraFollow != null) cameraFollow.SetShakeState(true);

        Vector3 originalPos = transform.position;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, -10);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (cameraFollow != null)
        {
            transform.position = new Vector3(cameraFollow.transform.position.x, cameraFollow.transform.position.y, -10);
            cameraFollow.SetShakeState(false);
        }
    }
}
