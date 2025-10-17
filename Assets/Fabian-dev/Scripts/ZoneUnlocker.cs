using UnityEngine;
using UnityEngine.UI;

public class ZoneUnlocker : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject zonaBloqueada;
    [SerializeField] private int costo = 10;

    private bool jugadorEnZona;
    private bool zonaComprada = false;

    // Canvas y botones de prueba
    private Canvas testCanvas;
    private Button btnAddCoins;
    private Button btnBuyZone;
    private Button btnShowCoins;

    void Start()
    {
        CrearBotonesDePrueba();
        testCanvas.gameObject.SetActive(false); // ocultar al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = true;

            // Solo mostrar si no se ha comprado todavía
            if (!zonaComprada)
                testCanvas.gameObject.SetActive(true);

            int monedasActuales = CashManager.instance != null ? CashManager.instance.GetCoins() : 0;

            if (monedasActuales >= costo)
                Debug.Log("Puedes comprar la zona. Usa el botón 'Comprar Zona'.");
            else
                Debug.Log("No tienes suficientes monedas para desbloquear esta zona.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = false;
            testCanvas.gameObject.SetActive(false);
        }
    }

    public void ComprarZona()
    {
        if (CashManager.instance == null) return;

        int monedasActuales = CashManager.instance.GetCoins();

        if (monedasActuales >= costo)
        {
            CashManager.instance.RemoveCoins(costo);

            if (zonaBloqueada != null)
                zonaBloqueada.SetActive(false);

            zonaComprada = true;

            // Oculta el botón de compra definitivamente
            if (btnBuyZone != null)
                btnBuyZone.gameObject.SetActive(false);

            Debug.Log("Zona desbloqueada con éxito. El botón de compra se ha desactivado.");
        }
        else
        {
            Debug.Log("No tienes suficientes monedas.");
        }
    }

    // ==========================
    //      BOTONES DE PRUEBA
    // ==========================
    private void CrearBotonesDePrueba()
    {
        // Crear un Canvas temporal en pantalla
        GameObject canvasObj = new GameObject("ZoneUnlocker_TestCanvas");
        testCanvas = canvasObj.AddComponent<Canvas>();
        testCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Crear los botones
        btnAddCoins = CrearBoton("Agregar +10", new Vector2(100, -60), AddCoinsTest);
        btnBuyZone = CrearBoton("Comprar Zona", new Vector2(100, -110), ComprarZona);
        btnShowCoins = CrearBoton("Mostrar Monedas", new Vector2(100, -160), ShowCoinsTest);
    }

    private Button CrearBoton(string texto, Vector2 posicion, UnityEngine.Events.UnityAction accion)
    {
        GameObject botonObj = new GameObject(texto);
        botonObj.transform.SetParent(testCanvas.transform);

        RectTransform rect = botonObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(180, 40);
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = posicion;

        botonObj.AddComponent<CanvasRenderer>();
        Image img = botonObj.AddComponent<Image>();
        img.color = new Color(0.2f, 0.6f, 0.9f, 0.9f);

        Button btn = botonObj.AddComponent<Button>();
        btn.onClick.AddListener(accion);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(botonObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.sizeDelta = rect.sizeDelta;
        textRect.anchoredPosition = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.text = texto;
        text.alignment = TextAnchor.MiddleCenter;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.color = Color.white;
        text.fontSize = 18;

        return btn;
    }

    private void AddCoinsTest()
    {
        if (CashManager.instance != null)
        {
            CashManager.instance.AddCoins(10);
            Debug.Log("Se añadieron 10 monedas.");
        }
    }

    private void ShowCoinsTest()
    {
        if (CashManager.instance != null)
            Debug.Log("Monedas actuales: " + CashManager.instance.GetCoins());
    }
}
