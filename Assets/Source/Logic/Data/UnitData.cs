using System;
using UnityEngine;

namespace Source.Logic.Data
{
    [Serializable]
    public class UnitData
    {
        // TODO: Remove & address image by string reference for serialization & modding purposes
        public Sprite Sprite;
        public string Name;
        public int Health;
        public int Power;
    }
}
