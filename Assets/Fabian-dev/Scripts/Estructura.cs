using UnityEngine;

public class Estructura : MonoBehaviour
{
    [Range(0f, 1f)]

    public int MonedasRequeridas = 10;
    public float cantidadDeTransparencia = 0.3f;
    private bool activado;
    public bool EstaActivado { get { return activado; } }


    void Start()
    {
        activado = false;
        CambiarTransparencia(cantidadDeTransparencia);
    }
    public void Activado()
    {
        activado = true;
        CambiarTransparencia(1.0f);
    }

    public void CambiarTransparencia(float value)
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (Material material in renderer.materials)
            {
                // Cambiar color con alfa
                Color color = material.color;
                color.a = value;
                material.color = color;

                // Configurar blending
                material.SetFloat("_Mode",value<1.0f? 3:0);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);

                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.EnableKeyword("_ALPHABLEND_ON");

                material.renderQueue = 3000;
            }
        }
    }
}
