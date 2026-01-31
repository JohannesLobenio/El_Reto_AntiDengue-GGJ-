using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private InputHandler detector;
    private bool estaOcupado = false;

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
        if (estaOcupado) return;

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

        yield return null; // Sincronizar con Animator

        float duracionTotal = anim.GetCurrentAnimatorStateInfo(0).length;

        // Definición de tiempos según tus necesidades
        float tiempoRotarIda = duracionTotal * 0.1f;    // Rotación muy rápida (10%)
        float tiempoEsperaTotal = duracionTotal * 0.7f; // Punto de corte para lógica (70%)
        float tiempoRotarVuelta = duracionTotal * 0.1f; // Regreso igual de rápido (10%)

        float t = 0;

        // --- FASE 1: ROTACIÓN RÁPIDA (10% de la duración) ---
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

        // --- FASE 2: ESPERA RESTANTE HASTA EL 70% ---
        // Como ya gastamos el 10% rotando, esperamos el 60% restante
        yield return new WaitForSeconds(tiempoEsperaTotal - tiempoRotarIda);

        // --- RESET DE PARÁMETROS ANIMATOR ---
        anim.SetBool(parametro, false);
        anim.SetBool("Quieto", true);

        // --- FASE 3: REGRESO RÁPIDO A Z ORIGINAL (10% de la duración) ---
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

        // Asegurar posición final exacta
        if (puedeRotar)
            objetoARotar.localRotation = Quaternion.Euler(eulerInicial.x, eulerInicial.y, zOriginal);

        detector.ResetInput(parametro);
        estaOcupado = false;
    }
}