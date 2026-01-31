using UnityEngine;

public class CamaraFP : MonoBehaviour
{
    public float sensibilidad = 100f;
    public Transform cuerpoJugador; //objeto Jugador

    float rotacionX = 0f;

    void Start()
    {
        // Bloquea el mouse en el centro de la pantalla y lo oculta
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obtener el movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        //Lógica para mirar arriba y abajo (Eje X)
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f); // Limita la mirada

        // Aplicar rotación a la cámara
        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        // Lógica para girar el cuerpo (Eje Y)
        cuerpoJugador.Rotate(Vector3.up * mouseX);
    }
}
