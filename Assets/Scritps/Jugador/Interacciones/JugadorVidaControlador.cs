using UnityEngine;

public class JugadorVidaControlador : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float vidaActual = 100f;
    public float cooldownDano = 1f;

    [Header("Estados (Bools)")]
    public bool recibioDano = false;
    public bool haMuerto = false;

    private float tiempoUltimoDano;

    void Update()
    {
        // Resetear el estado de 'recibioDano' después de un frame o según tu lógica
        // para que otros scripts no crean que sigue recibiendo daño constantemente.
        if (recibioDano)
        {
            recibioDano = false;
        }
    }

    public void TomarDano(float cantidad)
    {
        // Si ya está muerto o no ha pasado el segundo de cooldown, ignoramos el daño
        if (haMuerto || Time.time < tiempoUltimoDano + cooldownDano)
        {
            return;
        }

        // Aplicar daño
        vidaActual -= cantidad;
        recibioDano = true;
        tiempoUltimoDano = Time.time;

        Debug.Log("Vida restante: " + vidaActual);

        // Verificar si la vida llegó a 0 o menos
        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        vidaActual = 0;
        haMuerto = true;
        Debug.Log("El jugador ha muerto.");
        // Aquí podrías desactivar el movimiento o disparar una animación
    }
}