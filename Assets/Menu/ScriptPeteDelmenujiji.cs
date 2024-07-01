using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPeteDelmenujiji : MonoBehaviour
{
    public GameObject start;
    public float tiempo;
    public SpriteRenderer negro;
    public float tiemponegro;
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        tiemponegro = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKey)
        {
            menu.SetActive(false);
        }


        tiemponegro -= Time.deltaTime;
        tiempo += Time.deltaTime;

        if (tiempo > 0.5f)
        {
            start.SetActive(false);
        }
        if (tiempo > 1)
        {
            tiempo = 0;
        }
        if (tiempo < 0.5f)
        {
            start.SetActive(true);
        }
        negro.color = new Color(1f, 1f, 1f, tiemponegro);

    }
}
