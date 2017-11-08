using System;
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
    private Dictionary<string, string> resources;

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
        resources = new Dictionary<string, string>();

        LoadJson("en-US");
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Gets a cell type string.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetCellType(int key)
    {
        string k = key.ToString();
        string output = missing;

        if (cells.ContainsKey(k))
        {
            output = cells[k];
        }
        else
        {
            Debug.LogWarning("Cell string not found: " + key);
        }

        return output;
    }

    /// <summary>
    /// Gets a building type string.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetBuildingType(int key)
    {
        string k = key.ToString();
        string output = missing;

        if (buildings.ContainsKey(k))
        {
            output = buildings[k];
        }
        else
        {
            Debug.LogWarning("Building string not found: " + key);
        }

        return output;
    }

    /// <summary>
    /// Gets a resource type string.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetResourceType(int key)
    {
        string k = key.ToString();
        string output = missing;

        if (resources.ContainsKey(k))
        {
            output = resources[k];
        }
        else
        {
            Debug.LogWarning("Resource string not found: " + key);
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

            // Resources
            foreach (LocalizationItem item in data.Resources)
            {
                resources.Add(item.k, item.v);
            }
        }
        else
        {
            Debug.LogError("Unable to load localized strings: " + filePath);
        }
    }
}
