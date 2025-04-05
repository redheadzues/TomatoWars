using System;
using UnityEngine;

namespace Source.Code.IdleNumbers
{
    [Serializable]
    public class UnitySerializableIdleNumber
    {
        [SerializeField] private float _value;
        [SerializeField] private int _degree;
        
        public IdleNumber Value => new (_value, _degree);
    }
}