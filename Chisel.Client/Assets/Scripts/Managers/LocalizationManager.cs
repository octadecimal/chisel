using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    /// <summary>
    /// Singleton reference.
    /// </summary>
    public static LocalizationManager instance;

    // Localization data
    private LocalizationData data;

    // Dictionary lookups
    private Dictionary<string, string> cells;
    private Dictionary<string, string> buildings;

    // Default text for missing strings
    private string missing = "[MISSING STRING]";


    // CORE
    // ========================================================================

    /// <summary>
    /// Awake routine.
    /// </summary>
    private void Awake()
    {
        instance = this;

        cells = new Dictionary<string, string>();
        buildings = new Dictionary<string, string>();

        LoadJson("en-US");
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Gets a cell string.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetCell(int key)
    {
        string k = key.ToString();
        string output = missing;

        if (cells.ContainsKey(k))
        {
            output = cells[k];
        }

        return output;
    }

    /// <summary>
    /// Gets a building string.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetBuilding(int key)
    {
        string k = key.ToString();
        string output = missing;

        if (buildings.ContainsKey(k))
        {
            output = buildings[k];
        }

        return output;
    }



    // PRIVATE
    // ========================================================================

    /// <summary>
    /// Loads the locale-specific json.
    /// </summary>
    /// <param name="locale"></param>
    private void LoadJson(string locale)
    {
        string filePath = Application.streamingAssetsPath + "/Strings/" + locale + ".json";

        if(File.Exists(filePath))
        {
            // Read and parse
            string raw = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<LocalizationData>(raw);

            // Cells
            foreach (LocalizationItem item in data.Cells)
            {
                cells.Add(item.k, item.v);
            }

            // Buildings
            foreach (LocalizationItem item in data.Buildings)
            {
                buildings.Add(item.k, item.v);
            }
        }
        else
        {
            Debug.LogError("Unable to load localized strings: " + filePath);
        }
    }
}
