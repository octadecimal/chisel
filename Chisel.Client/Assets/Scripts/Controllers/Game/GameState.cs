using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Chisel
{
    public class GameState : MonoBehaviour
    {
        /// <summary>
        /// Loads the game.
        /// </summary>
        public GameModel Load()
        {
            GameModel gameModel;

            string path = Application.persistentDataPath + "/save.bin";

            if (File.Exists(path))
            {
                Debug.Log("Loading: " + path);

                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);

                gameModel = (GameModel)formatter.Deserialize(file);

                file.Close();
            }
            else
            {
                Debug.Log("Creating new game: " + path);

                gameModel = new GameModel();
                gameModel.CreateNewGame();

                Save(gameModel);
            }

            return gameModel;
        }

        /// <summary>
        /// Saves the game.
        /// </summary>
        /// <param name="gameModel"></param>
        public void Save(GameModel gameModel)
        {
            gameModel.Time = DateTime.Now.Ticks;

            string path = Application.persistentDataPath + "/save.bin";

            Debug.Log("Saving: " + path);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(path);
            formatter.Serialize(file, gameModel);
            file.Close();
        }

        /// <summary>
        /// Deletes the saved game.
        /// </summary>
        public void DeleteSave()
        {
            string path = Application.persistentDataPath + "/save.bin";
            Debug.Log("Deleting save: " + path);
            File.Delete(path);
        }
    }
}
