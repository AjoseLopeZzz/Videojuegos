using UnityEngine;

public class FaceCameraBillboard : MonoBehaviour
{
    public bool soloEnY = true; // true: gira solo en Y (vertical)

    Transform cam;

    void Start()
    {
        cam = Camera.main != null ? Camera.main.transform : null;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        if (soloEnY)
        {
            Vector3 toCam = cam.position - transform.position;
            toCam.y = 0f;
            if (toCam.sqrMagnitude > 0.0001f)
                transform.rotation = Quaternion.LookRotation(toCam, Vector3.up);
        }
        else
        {
            transform.forward = (cam.position - transform.position).normalized;
        }
    }
}
