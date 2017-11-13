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
        /// Input manager.
        /// </summary>
        public InputManager InputManger { get; private set; }

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
            InputManger = GetComponent<InputManager>();

            // Assert
            Debug.Assert(State != null, "State not found.");
            Debug.Assert(Grid != null, "Grid not found.");
            Debug.Assert(TaskManager != null, "TaskManager not found.");

            // Notifications
            NotificationManager.Subscribe(GameNotifications.Invalidated, OnInvalidate);
            NotificationManager.Subscribe(ToolbarNotifications.Explore, OnToolbarExplore);
            NotificationManager.Subscribe(CellNotifications.Created, OnCellCreated);
        }

        /// <summary>
        /// Start routine.
        /// </summary>
        private void Start()
        {
            // Load state
            State.DeleteSave();
            Model = State.Load();

            // Configure
            TaskManager.Configure(GetComponent<Game>());
            InputManger.Configure(Model);
            Grid.Configure(Model);

            // Restore
            Grid.Restore();
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
        /// Game: Invalidated
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


        // TOOLBAR NOTIFICATIONS
        // ========================================================================

        /// <summary>
        /// Toolbar: Explore
        /// </summary>
        /// <param name="data"></param>
        private void OnToolbarExplore(object data)
        {
            Cell cell = (Cell)data;

            TaskManager.AddTask(new ExploreTask(cell.Id));
        }


        // TOOLBAR NOTIFICATIONS
        // ========================================================================

        /// <summary>
        /// Cell: Created
        /// </summary>
        /// <param name="data"></param>
        private void OnCellCreated(object data)
        {
            Cell cell = (Cell)data;

            Model.AddCell(cell);
        }
    }
}