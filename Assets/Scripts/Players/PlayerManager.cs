using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Serializable]
    internal class PlayerTemplates
    {
        public int id;
        public GameObject prefabTemplatePlayers;
    }

    [Range(2, 4)]
    [SerializeField] private int playerCounter = 2;

    [SerializeField] private List<PlayerTemplates> templates = new List<PlayerTemplates>();

    private void Start() {
        foreach (var template in templates) {
            if(template.id == playerCounter)
            {
                Instantiate(template.prefabTemplatePlayers, transform);
            }
        }
    }

    //ELIMINAR LUEGO DE PRUEBAS
    public void ResetScene() => SceneManager.LoadScene(0);
}
