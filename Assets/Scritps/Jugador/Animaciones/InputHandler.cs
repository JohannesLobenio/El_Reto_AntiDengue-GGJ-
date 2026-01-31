using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Flags de Input (Pulsación única)")]
    public bool clickInput;
    public bool fInput;
    public bool eInput;

    [Header("Estado en Tiempo Real (Mantenido)")]
    public bool estaManteniendoClick; // Nuevo bool para registro en tiempo real

    void Update()
    {
        // Registro de pulsaciones únicas (Banderas para el controlador)
        if (Input.GetMouseButtonDown(0)) clickInput = true;
        if (Input.GetKeyDown(KeyCode.F)) fInput = true;
        if (Input.GetKeyDown(KeyCode.E)) eInput = true;

        // Registro en tiempo real: true mientras el botón esté presionado
        estaManteniendoClick = Input.GetMouseButton(0);
    }

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