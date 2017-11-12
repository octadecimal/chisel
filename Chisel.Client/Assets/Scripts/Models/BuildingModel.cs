using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Chisel
{
    [Serializable]
    public class BuildingModel
    {
        /// <summary>
        /// Building id.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Id of cell this building belongs to.
        /// </summary>
        public readonly int CellId;

        /// <summary>
        /// Building type.
        /// </summary>
        public readonly BuildingType Type;

        /// <summary>
        /// Building resources.
        /// </summary>
        public readonly Dictionary<int, long> Resources;

        /// <summary>
        /// Building (id) input connections.
        /// </summary>
        public List<int> Inputs = new List<int>();

        /// <summary>
        /// Building (id) output connections. -1 denotes no connection.
        /// </summary>
        public int Output = -1;

        /// <summary>
        /// The resource type this building is producing.
        /// </summary>
        public ResourceType Produces;

        /// <summary>
        /// Requirements for production.
        /// </summary>
        public Dictionary<ResourceType, long> Requires;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cellId"></param>
        /// <param name="type"></param>
        public BuildingModel(int id, int cellId, BuildingType type)
        {
            Resources = new Dictionary<int, long>();
            Requires = new Dictionary<ResourceType, long>();

            Id = id;
            CellId = cellId;
            Type = type;
        }

        /// <summary>
        /// Returns true if resources exist of the speficied type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool HasResource(int type, long quantity = 1)
        {
            if (Resources.ContainsKey(type))
            {
                if (Resources[type] >= quantity)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a resource by type with the set quantity.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="quantity"></param>
        public void AddResource(int type, long quantity)
        {
            if (!Resources.ContainsKey(type))
            {
                Resources.Add(type, 0);
            }

            Resources[type] += quantity;

            //Debug.Log("Added resource: id=" + Id + " type=" + type + " quantity=" + quantity + " total=" + Resources[type]);
        }

        /// <summary>
        /// Removes a resource by type with the set quantity.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="quantity"></param>
        public void RemoveResource(int type, long quantity)
        {
            if (Resources.ContainsKey(type))
            {
                if (Resources[type] >= quantity)
                {
                    Resources[type] -= quantity;

                    if (Resources[type] == 0)
                    {
                        Resources.Remove(type);
                    }

                    //Debug.Log("Removed resource: id=" + Id + " type=" + type + " quantity=" + quantity + " total=" + Resources[type]);
                }
                else
                {
                    Debug.Log("Attempted to remove more resources than are available.");
                }
            }
            else
            {
                Debug.Log("Attempted to remove non-existant resource.");
            }
        }
    }
}