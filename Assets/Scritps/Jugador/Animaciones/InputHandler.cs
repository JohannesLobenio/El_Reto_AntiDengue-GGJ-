using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Flags de Input")]
    public bool clickInput;
    public bool fInput;
    public bool eInput;

    void Update()
    {
        // Registramos el input. 
        // Nota: Solo se activan si el controlador ya ha procesado el anterior (están en false).
        if (Input.GetMouseButtonDown(0)) clickInput = true;
        if (Input.GetKeyDown(KeyCode.F)) fInput = true;
        if (Input.GetKeyDown(KeyCode.E)) eInput = true;
    }

    // Método para que el controlador limpie los estados después de usarlos
    public void ResetInput(string inputName)
    {
        switch (inputName)
        {
            case "Click": clickInput = false; break;
            case "F": fInput = false; break;
            case "E": eInput = false; break;
        }
    }
}
