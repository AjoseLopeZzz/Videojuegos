using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class NPCDialogue3D : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private string playerTag = "Player";
    [Tooltip("Si lo dejas en null, usará el BoxCollider de este GameObject.")]
    [SerializeField] private BoxCollider triggerZone;

    [Header("UI de interacción")]
    [Tooltip("Objeto que aparece al acercarse (puede ser el mismo botón Hablar).")]
    [SerializeField] private GameObject interactButtonUI;

    [Tooltip("Botón para iniciar el diálogo (OBLIGATORIO).")]
    [SerializeField] private Button talkButton;

    [Header("UI de diálogo")]
    [SerializeField] private GameObject dialogueRoot;      // Panel raíz del diálogo
    [SerializeField] private Image dialogueFrameImage;     // Opcional (marco decorativo)
    [SerializeField] private TextMeshProUGUI dialogueText; // Texto del diálogo (TMP)
    [SerializeField] private Image portraitImage;          // Retrato/personaje (Image)
    [SerializeField] private Button nextButton;            // Avanzar línea
    [SerializeField] private Button closeButton;           // Cerrar diálogo

    [System.Serializable]
    public struct Line
    {
        [TextArea(2, 4)] public string text;
        public Sprite portrait;
    }

    [Header("Contenido")]
    [SerializeField] private Line[] lines;

    private int index = -1;
    private bool playerInside = false;
    private bool dialogueOpen = false;

    // ---------------------------------------------------------------------
    // Setup
    // ---------------------------------------------------------------------
    private void Reset()
    {
        // Asegura trigger y rigidbody kinematic para eventos de trigger confiables
        var col = GetComponent<BoxCollider>();
        col.isTrigger = true;

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void Awake()
    {
        if (triggerZone == null) triggerZone = GetComponent<BoxCollider>();

        // AUTOLINK: si no asignaste InteractButtonUI pero sí TalkButton, úsalo como interact
        if (interactButtonUI == null && talkButton != null)
            interactButtonUI = talkButton.gameObject;

        SafeSetActive(interactButtonUI, false);
        SafeSetActive(dialogueRoot, false);

        // Botón para iniciar (OBLIGATORIO)
        if (talkButton != null)
        {
            talkButton.onClick.RemoveAllListeners();
            talkButton.onClick.AddListener(StartDialogue);
        }
        else
        {
            Debug.LogError("[NPCDialogue3D] Falta asignar TalkButton. Es obligatorio para iniciar el diálogo.");
        }

        // Botones dentro del diálogo
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextLine);
        }
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseDialogue);
        }
    }

    // ---------------------------------------------------------------------
    // Trigger
    // ---------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInside = true;
        if (!dialogueOpen) SafeSetActive(interactButtonUI, true);
        Debug.Log("[NPCDialogue3D] ENTRÓ Player al trigger");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInside = false;
        SafeSetActive(interactButtonUI, false);

        // Opcional: cierra si se aleja
        // if (dialogueOpen) CloseDialogue();

        Debug.Log("[NPCDialogue3D] SALIÓ Player del trigger");
    }

    // ---------------------------------------------------------------------
    // Diálogo (solo UI, sin teclas)
    // ---------------------------------------------------------------------
    private void StartDialogue()
    {
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("[NPCDialogue3D] No hay líneas configuradas.");
            return;
        }

        dialogueOpen = true;
        index = 0;

        SafeSetActive(interactButtonUI, false);
        SafeSetActive(dialogueRoot, true);

        ApplyLine();
    }

    private void NextLine()
    {
        if (!dialogueOpen) return;

        index++;
        if (index >= lines.Length)
        {
            CloseDialogue();
            return;
        }
        ApplyLine();
    }

    private void CloseDialogue()
    {
        dialogueOpen = false;
        index = -1;
        SafeSetActive(dialogueRoot, false);

        // Si el jugador sigue cerca, vuelve a mostrar el botón de hablar
        if (playerInside) SafeSetActive(interactButtonUI, true);
    }

    private void ApplyLine()
    {
        if (index < 0 || index >= lines.Length) return;

        var line = lines[index];

        if (dialogueText != null)
            dialogueText.text = line.text;

        if (portraitImage != null)
        {
            portraitImage.sprite = line.portrait;
            portraitImage.enabled = line.portrait != null;
        }
    }

    // ---------------------------------------------------------------------
    // Util
    // ---------------------------------------------------------------------
    private void SafeSetActive(GameObject go, bool state)
    {
        if (go != null && go.activeSelf != state) go.SetActive(state);
    }

    private void OnDrawGizmosSelected()
    {
        var col = triggerZone != null ? triggerZone : GetComponent<BoxCollider>();
        if (col == null) return;

        Gizmos.color = new Color(0f, 1f, 1f, 0.22f);
        Gizmos.matrix = col.transform.localToWorldMatrix;
        Gizmos.DrawCube(col.center, col.size);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(col.center, col.size);
    }
}
