using UnityEngine;
using System.Collections;

public class Enemigos : MonoBehaviour
{
    public float distancia = 5f;
    public float velocidad = 2f;
    public float tiempoAntesDeRegreso = 2f;
    
    private Vector3 inicio;
    private Vector3 destino;

    void OnEnable()
    {
        destino = transform.position + transform.up * distancia;
        inicio = transform.position;
        StartCoroutine(CicloUnaVez());
    }

    IEnumerator CicloUnaVez()
    {

        while (transform.position != destino)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(tiempoAntesDeRegreso);

        destino = inicio;
        while (transform.position != destino)
        {
            Regresar();
            
            yield return null;
            
        }
        yield return new WaitForSeconds(tiempoAntesDeRegreso);
        Despawn();
        yield return null;
    }

    void Regresar()
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
        }

    void Despawn()
    {
        Destroy(gameObject);
    }
}