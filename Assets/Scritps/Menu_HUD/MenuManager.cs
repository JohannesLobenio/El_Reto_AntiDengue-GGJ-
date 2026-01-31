using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(Time.timeScale > 0)
            {
                MainMenu();
            }
            Debug.Log("El juego ya esta pausado");
        }
    }

    public void MainMenu()
    {
        Debug.Log("Juego pausado");
        Time.timeScale = 0f;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        mainMenu.gameObject.SetActive(true);    
    }

    public void Continuar()
    {
        Debug.Log("Juego continuado");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainMenu.gameObject.SetActive(false);
    }

    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Escapar()
    {
        Debug.Log("Se escapo del juego");
        Application.Quit();
    }
}
