using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Menu : MonoBehaviour
{
    /// <summary>
    /// Menu item prefab.
    /// </summary>
    public GameObject MenuItemPrefab;

    // Items
    protected List<GameObject> items;
    protected List<GameObject> itemPool;

    // Storage
    protected Vector3 initialPosition;


    // CORE
    // ========================================================================

    /// <summary>
    /// Start routine.
    /// </summary>
    protected virtual void Start()
    {
        // Create
        items = new List<GameObject>();
        itemPool = new List<GameObject>();

        // Assertions
        Debug.Assert(MenuItemPrefab != null, "MenuItemPrefab not found.");

        // Save initial state
        initialPosition = transform.position + Vector3.zero;
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Clears any populated menu items. Items will be pooled
    /// for later usage.
    /// </summary>
    public virtual void Clear()
    {
        transform.position = initialPosition;

        foreach (GameObject item in items.ToList())
        {
            items.Remove(item);
            itemPool.Add(item);
            item.SetActive(false);
        }
    }

    /// <summary>
    /// Shows the number of menu items specified.
    /// </summary>
    /// <param name="count"></param>
    public virtual void ShowItems(int count)
    {
        transform.position = initialPosition;

        for (int i = 0; i < count; i++)
        {
            AddMenuItem();

            if (i > 0)
            {
                transform.Translate(0, 40f, 0);
            }
        }

        UpdateItems();
    }

    /// <summary>
    /// Updates the menu items.
    /// </summary>
    public virtual void UpdateItems()
    {

    }


    // PRIVATE
    // ========================================================================

    protected virtual void AddMenuItem()
    {
        GameObject item;

        if (itemPool.Count > 0)
        {
            item = itemPool.Last();
            itemPool.Remove(item);
        }
        else
        {
            item = Instantiate(MenuItemPrefab, this.transform);
        }

        item.SetActive(true);
        items.Add(item);
    }

}
