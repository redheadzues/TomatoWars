using System.Collections.Generic;
using UnityEngine;

namespace Source.Code
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private List<GameWindow> _windows;

        private Game _game;
     
        private void Awake()
        {
            _game = new(this, _windows);
        }
    }
}