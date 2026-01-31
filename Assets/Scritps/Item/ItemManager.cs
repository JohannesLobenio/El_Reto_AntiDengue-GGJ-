using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Estado del Inventario")]
    public bool tieneMataMoscas = true;
    public bool tieneRady = false;
    public bool tieneFumigador = false;

    [Header("Referencias a los Objetos")]
    public GameObject objetoMataMoscas;
    public GameObject objetoRady;
    public GameObject objetoFumigador;

    void Start() => ActualizarVisualizacion(0);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && tieneMataMoscas) ActualizarVisualizacion(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && tieneRady) ActualizarVisualizacion(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && tieneFumigador) ActualizarVisualizacion(2);
    }

    public void ActualizarVisualizacion(int indice)
    {
        // Limpiamos antes de ocultar
        LimpiarEstado(objetoMataMoscas);
        LimpiarEstado(objetoRady);
        LimpiarEstado(objetoFumigador);

        if (objetoMataMoscas) objetoMataMoscas.SetActive(false);
        if (objetoRady) objetoRady.SetActive(false);
        if (objetoFumigador) objetoFumigador.SetActive(false);

        GameObject seleccionado = null;
        switch (indice)
        {
            case 0: seleccionado = objetoMataMoscas; break;
            case 1: seleccionado = objetoRady; break;
            case 2: seleccionado = objetoFumigador; break;
        }

        if (seleccionado != null) seleccionado.SetActive(true);
    }

    private void LimpiarEstado(GameObject obj)
    {
        if (obj == null || !obj.activeSelf) return;

        // 1. Reset de Lógica y Rotación Específica en Z
        AnimationController animCtrl = obj.GetComponent<AnimationController>();
        if (animCtrl != null)
        {
            animCtrl.StopAllCoroutines();
            animCtrl.estaOcupado = false;

            if (animCtrl.objetoARotar != null)
            {
                // Obtenemos euler actual, cambiamos solo Z y aplicamos
                Vector3 rotActual = animCtrl.objetoARotar.localEulerAngles;
                animCtrl.objetoARotar.localRotation = Quaternion.Euler(rotActual.x, rotActual.y, 0f);
            }
        }

        // 2. Reset de Animator
        Animator anim = obj.GetComponent<Animator>();
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            anim.SetBool("Quieto", true);
            anim.SetBool("Click", false);
            anim.SetBool("F", false);
            anim.SetBool("E", false);
            anim.SetBool("Mantenido", false);
            anim.Play("Quieto", 0, 0f);
        }
    }

    public void RegistrarRecogida(string nombreItem)
    {
        if (nombreItem == "Rady") tieneRady = true;
        if (nombreItem == "Fumigador") tieneFumigador = true;
    }
}