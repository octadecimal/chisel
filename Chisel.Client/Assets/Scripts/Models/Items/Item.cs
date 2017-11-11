using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chisel
{
    public class Item : ScriptableObject
    {


        public Image icon;

        public Dictionary<Item, long> Consumes;

    }
}