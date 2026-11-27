using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action EmpezarJuego;
    public static event Action<bool> CambioPausa;

    public GameObject miCanvas;

    [Header("Configuración de Entrada")]
    public InputActionAsset inputActions;
    private InputAction pauseAction;

    private bool estaPausado = false;

    void Awake()
    {
        pauseAction = inputActions.FindActionMap("UI").FindAction("Pause");
    }

    void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.performed += OnPausePressed;
    }

    void OnDisable()
    {
        pauseAction.performed -= OnPausePressed;
        pauseAction.Disable();
    }

    void Start()
    {
        if (miCanvas != null)
            miCanvas.SetActive(false);
        Time.timeScale = 1f;
        EmpezarJuego?.Invoke();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (estaPausado)
            Reanudar();
        else
            Pausar();
    }

    public void Pausar()
    {
        miCanvas.SetActive(true);
        Time.timeScale = 0f;
        estaPausado = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CambioPausa?.Invoke(true);
    }

    public void Reanudar()
    {
        miCanvas.SetActive(false);
        Time.timeScale = 1f;
        estaPausado = false;

        CambioPausa?.Invoke(false);
    }

    public void Menu(string Menú)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Salir()
    {
        Application.Quit();
    }
}