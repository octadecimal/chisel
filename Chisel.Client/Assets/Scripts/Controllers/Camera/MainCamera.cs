using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Chisel
{
    public class MainCamera : MonoBehaviour
    {
        public Camera Camera;

        private void Awake()
        {
            // Assert
            Debug.Assert(Camera != null, "Camera not found.");
        }
    }
}
