using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chisel
{
    public class Game : MonoBehaviour
    {
        /// <summary>
        /// Game model.
        /// </summary>
        public GameModel Model { get; private set; }

        /// <summary>
        /// Game state.
        /// </summary>
        public GameState State { get; private set; }

        /// <summary>
        /// Task manager.
        /// </summary>
        public TaskManager TaskManager { get; private set; }

        /// <summary>
        /// Grid controller.
        /// </summary>
        public Grid Grid;

        /// <summary>
        /// Main camera.
        /// </summary>
        public MainCamera MainCamera;

        /// <summary>
        /// Minimap camera.
        /// </summary>
        public MinimapCamera MinimapCamera;

        // Flags
        private bool invalidated;
        private long invalidationTime;


        // CORE
        // ========================================================================

        /// <summary>
        /// Awake routine.
        /// </summary>
        private void Awake()
        {
            // Get componenets
            State = GetComponent<GameState>();
            TaskManager = GetComponent<TaskManager>();

            // Assert
            Debug.Assert(State != null, "State not found.");
            Debug.Assert(Grid != null, "Grid not found.");
            Debug.Assert(TaskManager != null, "TaskManager not found.");

            // Notifications
            NotificationManager.Subscribe(GameNotifications.Invalidated, OnInvalidate);

            // Load state
            State.DeleteSave();
            Model = State.Load();

            // Configure
            TaskManager.Configure(Model);
        }

        /// <summary>
        /// Update routine.
        /// </summary>
        private void Update()
        {
            // Handle invalidations and save
            if (invalidated)
            {
                if (DateTime.Now.Ticks >= invalidationTime)
                {
                    State.Save(Model);
                    invalidated = false;
                    invalidationTime = 0;
                }
            }
        }


        // GAME NOTIFICATIONS
        // ========================================================================

        /// <summary>
        /// Game: Invalidated.
        /// </summary>
        /// <param name="data"></param>
        private void OnInvalidate(object data)
        {
            if (invalidationTime == 0)
            {
                invalidationTime = TimeUtility.AddSeconds(DateTime.Now.Ticks, 60);
                invalidated = true;
            }
        }
    }
}