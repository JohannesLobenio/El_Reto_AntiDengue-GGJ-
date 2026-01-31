using UnityEngine;

public class MovimientoControlador : MonoBehaviour
{
    [SerializeField] Rigidbody rigid; //Fisicas del Jugador
    [SerializeField] private Detectar_Inpunts inputs_WASD; //Inpunts (WASD)
    [SerializeField] private DetectarVelocidadY flagY; //Esta en aire Si o No
    [SerializeField] private float velocidad; //Velocidad del Jugador
                     private float velocidadInicial; //Velocidad para el Sprint
    [SerializeField] private float saltoFuerza; //Fuerza del salto
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Debug.Log("Controlador de movimiento iniciado");
        velocidadInicial = velocidad; //Guardar la velocidad original
        //Obtener el rigid del Jugador.
        if (rigid == null)
        {
            rigid = GetComponent<Rigidbody>();
        }
        
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {

        //Logica Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            velocidad = velocidadInicial * 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            velocidad = velocidadInicial;
        }
        //Logica Salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (flagY.estaQuietoEnY)
            {
                Debug.Log("Salto con una fuerza de: " + saltoFuerza);
                rigid.AddForce(Vector3.up * saltoFuerza, ForceMode.Impulse);

                // Cooldown para el salto
                flagY.IniciarCooldown();
            }
        }

    }

    private void FixedUpdate()
    {
        float h = inputs_WASD.movimientoX;
        float v = inputs_WASD.movimientoZ;
        Vector3 direccion = transform.right * h + transform.forward * v;

        if (direccion.magnitude > 0)
        {
            if (flagY.estaQuietoEnY)
            {
                // En suelo: Control total e instantáneo
                rigid.linearVelocity = new Vector3(0, rigid.linearVelocity.y, 0);
                rigid.AddForce(direccion * velocidad, ForceMode.VelocityChange);
            }
            else
            {
                // En aire: Aplicamos menos fuerza para evitar que "planee"
                // Multiplicamos por un factor pequeño (ej: 0.2f)
                rigid.AddForce(direccion * (velocidad * 0.2f), ForceMode.VelocityChange);
            }
        }
        else if (flagY.estaQuietoEnY)
        {
            rigid.linearVelocity = new Vector3(0, rigid.linearVelocity.y, 0);
        }

        //(Evita el patinaje y la aceleración infinita)
        LimitarVelocidadHorizontal();
    }

    private void LimitarVelocidadHorizontal()
    {
        Vector3 velH = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);

        // Si la velocidad en el suelo supera tu variable 'velocidad', la cortamos
        if (velH.magnitude > velocidad)
        {
            Vector3 velLimitada = velH.normalized * velocidad;
            rigid.linearVelocity = new Vector3(velLimitada.x, rigid.linearVelocity.y, velLimitada.z);
        }
    }
}
