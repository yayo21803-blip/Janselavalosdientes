using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastDestroy : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private float distanciaRaycast = 20f;
    [SerializeField] private Transform puntoDisparo;

    [Header("Efecto del rayo")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem burbujas;
    [SerializeField] private float duracionRayo = 0.5f;
    [SerializeField] private float duracionBurbujas = 0.5f;

    private InputAction disparar;
    private float tiempoRestante = 0f;

    void Awake()
    {
        burbujas.Stop();
        disparar = new InputAction(type: InputActionType.Button);

        disparar.AddBinding("<Mouse>/leftButton");
        disparar.AddBinding("<Gamepad>/rightTrigger");
    }

    void OnEnable()
    {
        disparar.Enable();
    }

    void OnDisable()
    {
        disparar.Disable();
    }

    void OnDestroy()
    {
        disparar.Dispose();
    }

    void Update()
    {
        if (disparar.WasPressedThisFrame())
        {
            DispararRaycast();
        }

        // Mantener visible el rayo durante un tiempo
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void DispararRaycast()
    {
        Ray rayo = new Ray(
            puntoDisparo.position,
            puntoDisparo.forward
        );

        Vector3 puntoFinal = puntoDisparo.position +
                             puntoDisparo.forward * distanciaRaycast;

        if (Physics.Raycast(rayo, out RaycastHit hit, distanciaRaycast))
        {
            puntoFinal = hit.point;

            Debug.Log("Impacto: " + hit.collider.gameObject.name);

            Destroy(hit.collider.gameObject);
        }

        // Dibujar el rayo
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, puntoDisparo.position);
        lineRenderer.SetPosition(1, puntoFinal);

        // Activar las burbujas
        burbujas.transform.position = puntoDisparo.position;
        burbujas.transform.rotation = puntoDisparo.rotation;
        burbujas.Play();
        StartCoroutine(DetenerBurbujas());

        tiempoRestante = duracionRayo;
    }
    IEnumerator DetenerBurbujas()
{
    yield return new WaitForSeconds(duracionBurbujas);

    burbujas.Stop(
        true,
        ParticleSystemStopBehavior.StopEmitting
    );
}
}
