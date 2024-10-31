using System;
using Source.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Utility
{
    public class ScenariosBehavior : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private GameStateLoader gameStateLoader;
        [SerializeField] private TextAsset scenario1;
        [SerializeField] private TextAsset scenario2;
        
        public void LoadScenario1()
        {
            gameStateLoader.Load(scenario1);
        }

        public void LoadScenario2()
        {
            gameStateLoader.Load(scenario2);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
