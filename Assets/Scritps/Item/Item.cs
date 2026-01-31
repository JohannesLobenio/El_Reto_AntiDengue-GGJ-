using UnityEngine;

public class Item : MonoBehaviour
{
    public enum TipoDeItem { Rady, Fumigador }
    public TipoDeItem tipo;

    private InputHandler inputScript;
    private ItemManager managerScript;
    private bool estaElJugador = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Buscamos en el objeto o sus padres por si el collider es un hijo
            inputScript = other.GetComponentInParent<InputHandler>();
            managerScript = other.GetComponentInParent<ItemManager>();

            if (inputScript != null && managerScript != null)
            {
                estaElJugador = true;
                Debug.Log($"<color=green>Presiona E para recoger {tipo}</color>");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            estaElJugador = false;
            inputScript = null;
            managerScript = null;
        }
    }

    void Update()
    {
        // Comunicación con InputHandler y ItemManager
        if (estaElJugador && inputScript != null && inputScript.eInput)
        {
            if (managerScript != null)
            {
                managerScript.RegistrarRecogida(tipo.ToString());
                inputScript.ResetInput("E");

                Debug.Log($"<color=cyan>{tipo} recogido correctamente.</color>");
                gameObject.SetActive(false);
            }
        }
    }
}