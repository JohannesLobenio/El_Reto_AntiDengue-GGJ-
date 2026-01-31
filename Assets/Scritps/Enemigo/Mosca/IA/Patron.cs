using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ControladorMosca controlador;
    [SerializeField] private Rigidbody rigid;

    [Header("Configuración de Detección")]
    [SerializeField] private float radioDeteccion = 15f;
    [SerializeField] private LayerMask capaZonaVuelo;
    [SerializeField] private LayerMask capaSpot;

    [Header("IA y Probabilidades")]
    public Vector3 puntoDestino;
    [SerializeField] private float intervaloDecision = 2f;
    [SerializeField] private float distanciaLlegada = 0.5f;
    [SerializeField] private float alturaAscenso = 2f; // Cuánto sube antes de ir al spot

    [Header("Ajustes de Movimiento")]
    [SerializeField] private float velocidadVuelo = 5f;

    private List<Transform> spotsEncontrados = new List<Transform>();
    [HideInInspector] public bool jugadorDetectado;
    public bool preparandoVuelo = false; // Nuevo estado para controlar la pausa/ascenso
    private bool viajandoAlSpot = false;

    void Start()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rigid.useGravity = false; //

        // Iniciamos el ciclo de decisiones mediante una Corrutina
        StartCoroutine(CicloDecision());
    }

    void Update()
    {
        SistemaDeteccion();
        VerificarLlegada();
    }

    private void FixedUpdate()
    {
        if (viajandoAlSpot && !preparandoVuelo)
        {
            MoverHaciaPunto();
        }
    }

    private IEnumerator CicloDecision()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloDecision);

            // Solo decide si ve al jugador y no está haciendo ya algo
            if (controlador != null && controlador.viendoAlPlayer && !viajandoAlSpot && !preparandoVuelo)
            {
                int suerte = Random.Range(1, 4);
                if (suerte == 3 && spotsEncontrados.Count > 0)
                {
                    int indiceAleatorio = Random.Range(0, spotsEncontrados.Count);
                    Transform spotTransform = spotsEncontrados[indiceAleatorio];
                    puntoDestino = GenerarPuntoAleatorioEnCollider(spotTransform.GetComponent<Collider>());

                    // Iniciamos la secuencia solicitada
                    StartCoroutine(SecuenciaAntesDeVolar());
                }
            }
        }
    }

    private IEnumerator SecuenciaAntesDeVolar()
    {
        preparandoVuelo = true;
        rigid.linearVelocity = Vector3.zero;

        // 1. Esperar 2 segundos
        yield return new WaitForSeconds(2f);

        // 2. Subir un poco hacia arriba
        Vector3 posicionAscenso = transform.position + Vector3.up * alturaAscenso;
        float tiempoAscenso = 0.5f;
        float elapsed = 0;

        while (elapsed < tiempoAscenso)
        {
            transform.position = Vector3.Lerp(transform.position, posicionAscenso, elapsed / tiempoAscenso);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3. Dejar de mirar al player y mirar al spot
        // (El flag preparandoVuelo bloqueará la rotación del ControladorMosca)
        Vector3 direccionAlSpot = (puntoDestino - transform.position).normalized;
        if (direccionAlSpot != Vector3.zero)
        {
            transform.forward = direccionAlSpot;
        }

        preparandoVuelo = false;
        viajandoAlSpot = true;
    }

    private void MoverHaciaPunto()
    {
        Vector3 direccion = (puntoDestino - transform.position).normalized;
        rigid.linearVelocity = direccion * velocidadVuelo;

        if (direccion != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, direccion, Time.fixedDeltaTime * 10f);
        }
    }

    private Vector3 GenerarPuntoAleatorioEnCollider(Collider col)
    {
        Bounds bounds = col.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private void VerificarLlegada()
    {
        if (viajandoAlSpot)
        {
            float distancia = Vector3.Distance(transform.position, puntoDestino);
            if (distancia <= distanciaLlegada)
            {
                viajandoAlSpot = false;
                rigid.linearVelocity = Vector3.zero;
            }
        }
    }

    private void SistemaDeteccion()
    {
        Collider[] playerCol = Physics.OverlapSphere(transform.position, radioDeteccion, 1 << LayerMask.NameToLayer("Player"));
        jugadorDetectado = playerCol.Length > 0;

        Collider[] zonasCol = Physics.OverlapSphere(transform.position, radioDeteccion, capaZonaVuelo);
        spotsEncontrados.Clear();
        foreach (var col in zonasCol)
        {
            foreach (Transform hijo in col.transform)
            {
                if (((1 << hijo.gameObject.layer) & capaSpot) != 0)
                {
                    spotsEncontrados.Add(hijo);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = jugadorDetectado ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        if (viajandoAlSpot || preparandoVuelo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, puntoDestino);
            Gizmos.DrawSphere(puntoDestino, 0.3f);
        }
    }
}