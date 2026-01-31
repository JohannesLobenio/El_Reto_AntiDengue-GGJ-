using UnityEngine;

public class EnemigoVidaControlador : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float vidaActual = 50f;
    public float cooldownDano = 1f;

    [Header("Estados (Bools)")]
    public bool recibioDano = false;
    public bool haMuerto = false;

    private float tiempoUltimoDano;

    void Update()
    {
        // Reseteamos el flag de daño para que otros scripts (como animaciones o efectos)
        // solo detecten el golpe en el momento exacto en que ocurre.
        if (recibioDano)
        {
            recibioDano = false;
        }
    }

    public void TomarDano(float cantidad)
    {
        // Verificamos si ya murió o si todavía está en el tiempo de enfriamiento (1 seg)
        if (haMuerto || Time.time < tiempoUltimoDano + cooldownDano)
        {
            return;
        }

        // Aplicamos la lógica de daño
        vidaActual -= cantidad;
        recibioDano = true;
        tiempoUltimoDano = Time.time;

        Debug.Log(gameObject.name + " recibió daño. Vida restante: " + vidaActual);

        // Comprobamos si la vida se agotó
        if (vidaActual <= 0)
        {
            MorirEnemigo();
        }
    }

    private void MorirEnemigo()
    {
        vidaActual = 0;
        haMuerto = true;
        Debug.Log("El enemigo ha sido derrotado.");

        // Aquí podrías añadir un Destroy(gameObject, 2f) o desactivar el objeto
    }
}