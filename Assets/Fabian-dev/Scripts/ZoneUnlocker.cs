using UnityEngine;
using UnityEngine.UI;

public class ZoneUnlocker : MonoBehaviour
{
    [Header("Configuración de Zona")]
    [SerializeField] private GameObject zonaBloqueada;
    [SerializeField] private int costoMonedas = 10;
    [SerializeField] private int costoMaiz = 20;
    [SerializeField] private int costoTomate = 15;

    private bool jugadorEnZona;
    private bool zonaComprada = false;

    // Canvas y botones de prueba
    private Canvas testCanvas;
    private Button btnBuyZone;

    // ===== NUEVO: Elementos UI para mensajes =====
    private GameObject mensajePanel;
    Text tituloText;
    Text cuerpoText;
    Text detalleText;

    public string titulo;
    public string requisitoMaiz;
    public string requisitoTomate;
    public string requisitoMonedas;
    public string consejo;

    public GameObject triggerOcultar;
    public GameObject zonaNueva;



    private void Start()
    {
        CrearBotonesDePrueba();
        CrearPanelMensajes();

        testCanvas.gameObject.SetActive(false);

        if (CashManager.instance == null)
            Debug.LogWarning("No se encontró CashManager.instance en la escena.");
        if (InventoryManager.instance == null)
            Debug.LogWarning("No se encontró InventoryManager.instance en la escena.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        jugadorEnZona = true;

        if (!zonaComprada)
            testCanvas.gameObject.SetActive(true);

        VerificarRequisitos();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        jugadorEnZona = false;
        testCanvas.gameObject.SetActive(false);
        OcultarMensaje();
    }

    // ==========================
    // DESBLOQUEO CON MONEDAS Y CULTIVOS
    // ==========================
    public void ComprarZona()
    {
        if (zonaComprada) return;
        if (CashManager.instance == null || InventoryManager.instance == null) return;

        int monedas = CashManager.instance.GetCoins();
        int maiz = InventoryManager.instance.GetCropAmount(CropType.Corn);
        int tomate = InventoryManager.instance.GetCropAmount(CropType.Tomato);

        if (monedas >= costoMonedas && maiz >= costoMaiz && tomate >= costoTomate)
        {
            CashManager.instance.RemoveCoins(costoMonedas);
            InventoryManager.instance.RemoveCrop(CropType.Corn, costoMaiz);
            InventoryManager.instance.RemoveCrop(CropType.Tomato, costoTomate);

            DesbloquearZona("monedas + cultivos");
        }
        else
        {
            MostrarMensaje(
                "Requisitos insuficientes",
                $"Necesitas {costoMonedas} monedas, {costoMaiz} maíces y {costoTomate} tomates.",
                "Sigue cosechando o recolectando para desbloquear la zona."
            );
        }
    }

    private void DesbloquearZona(string metodo)
    {
        if (zonaBloqueada != null)
        {
            zonaBloqueada.SetActive(true);
            triggerOcultar.SetActive(false);
            zonaNueva.SetActive(true);
        }



        zonaComprada = true;
        if (btnBuyZone != null)
            btnBuyZone.gameObject.SetActive(true);

        testCanvas.gameObject.SetActive(false);

        MostrarMensaje("Zona Desbloqueada", $"Has usado {metodo}.", "¡Felicidades, puedes explorar la nueva zona!");
    }

    private void VerificarRequisitos()
    {
        int monedas = CashManager.instance != null ? CashManager.instance.GetCoins() : 0;
        int maiz = InventoryManager.instance != null ? InventoryManager.instance.GetCropAmount(CropType.Corn) : 0;
        int tomate = InventoryManager.instance != null ? InventoryManager.instance.GetCropAmount(CropType.Tomato) : 0;

        MostrarMensaje($"{titulo}",
          $"{requisitoMonedas}" +" " +$"{requisitoMaiz}" + " "+ $"{requisitoTomate}" ,
          $"{consejo}");

        /*MostrarMensaje($"{titulo}",
            $"Monedas: {monedas}/{costoMonedas} | Maíz: {maiz}/{costoMaiz} | Tomate: {tomate}/{costoTomate}",
            "Reúne todos los recursos para desbloquear la zona.");*/
    }

    // ==========================
    // BOTONES DE PRUEBA
    // ==========================
    private void CrearBotonesDePrueba()
    {
        GameObject canvasObj = new GameObject("ZoneUnlocker_TestCanvas");
        testCanvas = canvasObj.AddComponent<Canvas>();
        testCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Crear panel primero
        CrearPanelMensajes();

        // Crear botón debajo del panel
        GameObject botonObj = new GameObject("BotonComprar");
        botonObj.transform.SetParent(testCanvas.transform, false);

        RectTransform rect = botonObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 45);

        //  Centramos el botón horizontalmente y lo ubicamos justo debajo del panel
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0, -50); // 50 píxeles debajo del panel

        botonObj.AddComponent<CanvasRenderer>();
        Image img = botonObj.AddComponent<Image>();
        img.color = new Color(0.2f, 0.6f, 0.9f, 0.9f);

        btnBuyZone = botonObj.AddComponent<Button>();
        btnBuyZone.onClick.AddListener(ComprarZona);

        GameObject textObj = new GameObject("TextoBoton");
        textObj.transform.SetParent(botonObj.transform, false);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.sizeDelta = rect.sizeDelta;
        textRect.anchoredPosition = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.text = "Comprar Zona";
        text.alignment = TextAnchor.MiddleCenter;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.color = Color.white;
        text.fontSize = 18;
    }


    private Button CrearBoton(string texto, Vector2 posicion, UnityEngine.Events.UnityAction accion)
    {
        GameObject botonObj = new GameObject(texto);
        botonObj.transform.SetParent(testCanvas.transform);

        RectTransform rect = botonObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 45);
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

    // ==========================
    // SISTEMA DE MENSAJES EN PANTALLA
    // ==========================
    private void CrearPanelMensajes()
    {
        mensajePanel = new GameObject("MensajePanel");
        mensajePanel.transform.SetParent(testCanvas.transform);
        RectTransform rect = mensajePanel.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(400, 150);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, 100);

        Image fondo = mensajePanel.AddComponent<Image>();
        fondo.color = new Color(0, 0, 0, 0.6f);

        tituloText = CrearTexto("TituloText", mensajePanel.transform, new Vector2(0, 40), 20, FontStyle.Bold);
        cuerpoText = CrearTexto("CuerpoText", mensajePanel.transform, new Vector2(0, 0), 16, FontStyle.Normal);
        detalleText = CrearTexto("DetalleText", mensajePanel.transform, new Vector2(0, -40), 14, FontStyle.Italic);

        mensajePanel.SetActive(false);
    }

    private Text CrearTexto(string nombre, Transform padre, Vector2 posicion, int tamaño, FontStyle estilo)
    {
        GameObject textoObj = new GameObject(nombre);
        textoObj.transform.SetParent(padre);

        RectTransform rect = textoObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(380, 40);
        rect.anchoredPosition = posicion;

        Text txt = textoObj.AddComponent<Text>();
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.color = Color.white;
        txt.fontSize = tamaño;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.fontStyle = estilo;

        return txt;
    }

    public void MostrarMensaje(string titulo, string cuerpo, string detalle)
    {
        if (mensajePanel == null) return;

        tituloText.text = titulo;
        cuerpoText.text = cuerpo;
        detalleText.text = detalle;

        mensajePanel.SetActive(true);
    }

    public void OcultarMensaje()
    {
        if (mensajePanel != null)
            mensajePanel.SetActive(false);
    }
}
