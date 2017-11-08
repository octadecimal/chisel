using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour, IEnumerable
{
    public CellModel Model;
    public int Type;

    public BuildingController Building;

    private Vector2 uvOffset;


    // START
    // ========================================================================

    /// <summary>
    /// Start routine.
    /// </summary>
    void Start ()
    {
        
    }


    // ACCESSORS
    // ========================================================================

    /// <summary>
    /// Cell id.
    /// </summary>
    public int Id
    {
        get { return Model.Id; }
    }

    /// <summary>
    /// Cell grid position along x axis.
    /// </summary>
    public int X
    {
        get { return Model.X; }
    }

    /// <summary>
    /// Cell grid position along z axis.
    /// </summary>
    public int Z
    {
        get { return Model.Z; }
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Attaches a model to this controller.
    /// </summary>
    /// <param name="model"></param>
    public void AttachModel(CellModel model)
    {
        Model = model;
    }
    
    /// <summary>
    /// Update routine.
    /// </summary>
    public void Update ()
    {
        if(Type == 1)
        { 
            uvOffset.x += 0.01f * Time.deltaTime;
            uvOffset.y += -0.01f * Time.deltaTime;

            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material.SetTextureOffset("_MainTex", uvOffset);
        }
    }

    /// <summary>
    /// Plays the cell spawn animation for this cell.
    /// </summary>
    /// <param name="info"></param>
    public void Spawn(CellSpawnInfo info)
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        Color color = meshRenderer.material.color;
        color.a = 0f;
        meshRenderer.material.color = color;

        if (info.Dim) Dim();

        gameObject.transform.Translate(new Vector3(0.0f, -10.0f, 0.0f));
        StartCoroutine("SpawnCoroutine", info);
    }

    /// <summary>
    /// Dims this cell.
    /// </summary>
    public void Dim()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        Color color = meshRenderer.material.color;
        color.r = 0.25f;
        color.g = 0.25f;
        color.b = 0.25f;
        color.a = 0.00f;
        meshRenderer.material.color = color;
    }


    // COROUTINES
    // ========================================================================

    /// <summary>
    /// Spawn animation coroutine.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    IEnumerator SpawnCoroutine(CellSpawnInfo info)
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        Color color = meshRenderer.material.color;

        if(info.Delay > 0)
        {
            yield return new WaitForSeconds(info.Delay * info.Index);
        }

        for (float i = 0; i < 1; i += 0.1f)
        {
            gameObject.transform.Translate(new Vector3(0, 1f, 0));

            color.a = i;
            meshRenderer.material.color = color;

            yield return null;
        }

        color.a = 1;
        meshRenderer.material.color = color;
    }

    /// <summary>
    /// IEnumerator implementation.
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}
