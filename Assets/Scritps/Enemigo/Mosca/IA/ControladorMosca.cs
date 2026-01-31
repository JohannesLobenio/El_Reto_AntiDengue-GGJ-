using UnityEngine;

public class ControladorMosca : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Patron scriptPatron;

    [Header("Flags de Estado")]
    public bool viendoAlPlayer;
    public bool seEstaMoviendo;

    private Transform playerTransform;

    void Start()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        if (scriptPatron == null) scriptPatron = GetComponent<Patron>();
    }

    void Update()
    {
        if (scriptPatron != null)
        {
            viendoAlPlayer = scriptPatron.jugadorDetectado;
        }

        seEstaMoviendo = rigid.linearVelocity.magnitude > 0.1f;

        // MODIFICACIÓN: Si NO está preparando el vuelo, mira al jugador
        if (viendoAlPlayer && playerTransform != null && !scriptPatron.preparandoVuelo)
        {
            Vector3 direccion = (playerTransform.position - transform.position).normalized;
            direccion.y = 0;
            if (direccion != Vector3.zero)
            {
                transform.forward = Vector3.Slerp(transform.forward, direccion, Time.deltaTime * 5f);
            }
        }
    }
}