using UnityEngine;

public class DetectarVelocidadY : MonoBehaviour
{
    private Rigidbody rigid;
    public bool estaQuietoEnY;

    // Tiempo mínimo que debe pasar antes de permitir otro salto
    [SerializeField] private float cooldownSalto = 0.2f;
    private float tiempoSiguienteDeteccion;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Solo comprobamos la velocidad si el tiempo actual superó el cooldown
        if (Time.time > tiempoSiguienteDeteccion)
        {
            estaQuietoEnY = Mathf.Abs(rigid.linearVelocity.y) < 0.01f;
        }
        else
        {
            // Mientras estemos en cooldown, forzamos que sea false
            estaQuietoEnY = false;
        }
    }

    
    public void IniciarCooldown()
    {
        tiempoSiguienteDeteccion = Time.time + cooldownSalto;
        estaQuietoEnY = false;
    }
}
