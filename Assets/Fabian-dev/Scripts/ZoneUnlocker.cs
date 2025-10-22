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
    private Button btnAddCoins;
    private Button btnBuyZone;
    private Button btnShowCoins;

    private void OnEnable()
    {
        // Escuchar el evento de inventario
        //InventoryManager.onSpecialCropCollected += CheckZoneUnlockByCrops;
    }

    private void OnDisable()
    {
        //InventoryManager.onSpecialCropCollected -= CheckZoneUnlockByCrops;
    }

    void Start()
    {
        CrearBotonesDePrueba();

        // Ocultar botones al inicio
        testCanvas.gameObject.SetActive(false);

        // Verificar si los managers existen
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
            Debug.Log($" Requisitos insuficientes. Necesitas {costoMonedas} monedas, {costoMaiz} maíces y {costoTomate} tomates.");
        }
    }
   
    // ==========================
    // DESBLOQUEO AUTOMÁTICO (AL RECOGER CULTIVOS)
    // ==========================
    /*
    private void CheckZoneUnlockByCrops(CropType cropType, int cantidad)
    {
        if (zonaComprada || InventoryManager.instance == null) return;

        int maiz = InventoryManager.instance.GetCropAmount(CropType.Corn);
        int tomate = InventoryManager.instance.GetCropAmount(CropType.Tomato);

        if (maiz >= costoMaiz && tomate >= costoTomate)
        {
            InventoryManager.instance.RemoveCrop(CropType.Corn, costoMaiz);
            InventoryManager.instance.RemoveCrop(CropType.Tomato, costoTomate);
            DesbloquearZona("cultivos automáticos");
        }
    }*/

    private void DesbloquearZona(string metodo)
    {
        if (zonaBloqueada != null)
            zonaBloqueada.SetActive(false);

        zonaComprada = true;

        if (btnBuyZone != null)
            btnBuyZone.gameObject.SetActive(false);

        testCanvas.gameObject.SetActive(false);

        Debug.Log($" Zona desbloqueada exitosamente con {metodo}.");
    }

    // ==========================
    // VERIFICAR ESTADO DE RECURSOS
    // ==========================
    private void VerificarRequisitos()
    {
        int monedas = CashManager.instance != null ? CashManager.instance.GetCoins() : 0;
        int maiz = InventoryManager.instance != null ? InventoryManager.instance.GetCropAmount(CropType.Corn) : 0;
        int tomate = InventoryManager.instance != null ? InventoryManager.instance.GetCropAmount(CropType.Tomato) : 0;

        Debug.Log($" Monedas: {monedas}/{costoMonedas} |  Maíz: {maiz}/{costoMaiz} |  Tomate: {tomate}/{costoTomate}");
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

        btnAddCoins = CrearBoton("Agregar +10 monedas", new Vector2(100, -60), AddCoinsTest);
        btnBuyZone = CrearBoton("Comprar Zona", new Vector2(100, -110), ComprarZona);
        btnShowCoins = CrearBoton("Mostrar Estado", new Vector2(100, -160), VerificarRequisitos);
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

    private void AddCoinsTest()
    {
        if (CashManager.instance != null)
        {
            CashManager.instance.AddCoins(10);
            Debug.Log(" Se añadieron 10 monedas.");
        }
    }
}
