
using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class enemyspawn : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private int limiteEnemigos = 10;
    [SerializeField] private float tiempoMinimo = 1f;
    [SerializeField] private float tiempoMaximo = 4f;

    private int enemigosSpawneados = 0;
    private int ultimoIndice = -1;
    private Coroutine rutinaSpawn;

    void Awake()
    {
        GameManager.EmpezarJuego += EmpezarRonda;
    }

    void OnDestroy()
    {
        GameManager.EmpezarJuego -= EmpezarRonda;
    }

    void EmpezarRonda()
    {
        if (rutinaSpawn != null)
        {
            StopCoroutine(rutinaSpawn);
        }

        enemigosSpawneados = 0;
        rutinaSpawn = StartCoroutine(RutinaSpawn());
    }

    IEnumerator RutinaSpawn()
    {
        while (enemigosSpawneados < limiteEnemigos)
        {
            float espera = Random.Range(tiempoMinimo, tiempoMaximo);
            yield return new WaitForSeconds(espera);

            SpawnEnemy();
        }

        Debug.Log("Se alcanzó el límite de enemigos.");
        rutinaSpawn = null;
    }

    int ElegirSP()
    {
        if (spawnPoints.Length <= 1) return 0;

        int nuevoIndice;

        do
        {
            nuevoIndice = Random.Range(0, spawnPoints.Length);
        } while (nuevoIndice == ultimoIndice);

        ultimoIndice = nuevoIndice;
        return nuevoIndice;
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No hay spawn points o prefabs asignados.");
            return;
        }

        // Elegir un enemigo aleatorio
        int indiceEnemigo = Random.Range(0, enemyPrefabs.Length);
        GameObject enemigoSeleccionado = enemyPrefabs[indiceEnemigo];

        // Elegir un punto de aparición
        Transform puntoSeleccionado = spawnPoints[ElegirSP()].transform;

        Instantiate(
            enemigoSeleccionado,
            puntoSeleccionado.position,
            puntoSeleccionado.rotation
        );

        enemigosSpawneados++;

        Debug.Log("Enemigos generados: " + enemigosSpawneados + "/" + limiteEnemigos);
    }
}
