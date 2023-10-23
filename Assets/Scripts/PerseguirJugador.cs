using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ENEMIGOTORRETA : MonoBehaviour

{
    public GameObject _projectile;
    public float shotForce = 1500;
    public float shotRate = 0.5f;
    private float shotRateTime = 0;

    public float rangoDeVision = 10f;
    public float velocidadRotacion = 3f;
    public float velocidadMovimiento = 5f;
    public int vida = 1;
    public Transform puntoDisparo;

    private Transform jugador;
    private bool jugadorDetectado = false;

    public Rigidbody _rb;
    public bool _drop;
    public int _value;
    public GameObject _loot;
    public string _name;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        
        _rb = GetComponent<Rigidbody>();

        _value = Random.Range(0, 100);
        if (_value >= 15)
        {
            _drop = true;
        }
    }

    private void Update()
    {
        if (jugadorDetectado)
        {
            RotarHaciaJugador();
            MoverHaciaJugador();
            Disparar();
        }
        else if (Vector3.Distance(transform.position, jugador.position) <= rangoDeVision)
        {
            jugadorDetectado = true;
        }
        if (vida <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void RotarHaciaJugador()
    {
        Vector3 direccionJugador = jugador.position - transform.position;
        direccionJugador.y = 0f;
        Quaternion rotacionDeseada = Quaternion.LookRotation(direccionJugador);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, velocidadRotacion * Time.deltaTime);
    }

    private void MoverHaciaJugador()
    {
        transform.position += transform.forward * velocidadMovimiento * Time.deltaTime;
    }

    private void Disparar()
    {
        if (Time.time > shotRateTime)
        {
            GameObject newProjectile;
            newProjectile = Instantiate(_projectile, puntoDisparo.position, puntoDisparo.rotation);
            newProjectile.GetComponent<Rigidbody>().AddForce(puntoDisparo.forward * shotForce);
            shotRateTime = Time.time + shotRate;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("kill");
            Destroy(other.gameObject);
            vida--;
        }
    }

    private void OnDestroy()
    {
        if (_drop == true && SceneManager.GetActiveScene().ToString() == "UnityEngine.SceneManagement.Scene")
        {
            Debug.Log("Coin");
            Instantiate(_loot, this.transform.position, Quaternion.identity);
        }
    }
}

