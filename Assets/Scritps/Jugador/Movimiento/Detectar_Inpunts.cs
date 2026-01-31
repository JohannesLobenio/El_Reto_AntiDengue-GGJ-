using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Detectar_Inpunts : MonoBehaviour
{
    //Inputs
    private float X;
    private float Z;

    //Variables para el movimiento;
    public float movimientoX;
    public float movimientoZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Depuracion para verificar que el script inicio correctamente
        Debug.Log("Detectanto Inpunts (WASD)");
    }

    // Update is called once per frame
    void Update()
    {
        //Detecta los Inputs.
        X = Input.GetAxisRaw("Horizontal");
        Z = Input.GetAxisRaw("Vertical");

        //Aproxima los Inpunts a numeros enteros.
        movimientoX = X;
        movimientoZ = Z;
    }

    
}
