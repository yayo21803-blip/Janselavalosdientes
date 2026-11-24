using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action EmpezarJuego;
    public GameObject miCanvas;
    void Start()
    {
        miCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Empezar()
    {
        EmpezarJuego?.Invoke();
        miCanvas.SetActive(false);
    }
}
