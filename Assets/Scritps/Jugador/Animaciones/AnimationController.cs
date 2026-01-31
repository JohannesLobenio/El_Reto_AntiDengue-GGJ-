using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private InputHandler detector;
    public bool estaOcupado = false;

    [Header("Configuración de Rotación")]
    public Transform objetoARotar;
    public List<float> angulosZ = new List<float>();

    void Start()
    {
        anim = GetComponent<Animator>();
        detector = GetComponent<InputHandler>();
        anim.SetBool("Quieto", true);
    }

    void Update()
    {
        // Sincronización en tiempo real del parámetro "Mantenido"
        anim.SetBool("Mantenido", detector.estaManteniendoClick);

        if (estaOcupado) return;

        // Ejecución de acciones por pulsación única
        if (detector.clickInput) StartCoroutine(EjecutarAccion("Click"));
        else if (detector.fInput) StartCoroutine(EjecutarAccion("F"));
        else if (detector.eInput) StartCoroutine(EjecutarAccion("E"));
    }

    IEnumerator EjecutarAccion(string parametro)
    {
        estaOcupado = true;

        // --- PREPARACIÓN DE VALORES ---
        bool puedeRotar = objetoARotar != null && angulosZ.Count > 0;
        Vector3 eulerInicial = Vector3.zero;
        float zOriginal = 0f;
        float zObjetivo = 0f;

        if (puedeRotar)
        {
            eulerInicial = objetoARotar.localEulerAngles;
            zOriginal = eulerInicial.z;
            zObjetivo = angulosZ[Random.Range(0, angulosZ.Count)];
        }

        // --- INICIO DE ANIMACIÓN ---
        anim.SetBool("Quieto", false);
        anim.SetBool(parametro, true);

        yield return null;

        float duracionTotal = anim.GetCurrentAnimatorStateInfo(0).length;

        float tiempoRotarIda = duracionTotal * 0.1f;
        float tiempoEsperaTotal = duracionTotal * 0.7f;
        float tiempoRotarVuelta = duracionTotal * 0.1f;

        float t = 0;

        // --- FASE 1: ROTACIÓN RÁPIDA (10%) ---
        while (t < 1.0f)
        {
            t += Time.deltaTime / tiempoRotarIda;
            if (puedeRotar)
            {
                float zActual = Mathf.LerpAngle(zOriginal, zObjetivo, t);
                objetoARotar.localRotation = Quaternion.Euler(eulerInicial.x, eulerInicial.y, zActual);
            }
            yield return null;
        }

        // --- FASE 2: ESPERA HASTA EL 70% ---
        yield return new WaitForSeconds(tiempoEsperaTotal - tiempoRotarIda);

        // --- RESET DE PARÁMETROS ---
        anim.SetBool(parametro, false);
        anim.SetBool("Quieto", true);

        // --- FASE 3: REGRESO RÁPIDO (10%) ---
        t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime / tiempoRotarVuelta;
            if (puedeRotar)
            {
                float zActual = Mathf.LerpAngle(zObjetivo, zOriginal, t);
                objetoARotar.localRotation = Quaternion.Euler(eulerInicial.x, eulerInicial.y, zActual);
            }
            yield return null;
        }

        if (puedeRotar)
            objetoARotar.localRotation = Quaternion.Euler(eulerInicial.x, eulerInicial.y, zOriginal);

        detector.ResetInput(parametro);
        estaOcupado = false;
    }
}