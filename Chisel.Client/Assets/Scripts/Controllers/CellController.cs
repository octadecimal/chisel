using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour, IEnumerable
{
    public CellModel Model;
    public int Type;

    public BuildingController Building;

    private Vector2 uvOffset;

    // Use this for initialization
    void Start ()
    {

    }


    // ACCESSORS
    // ========================================================================

    public int Id
    {
        get { return Model.Id; }
    }

    public int X
    {
        get { return Model.X; }
    }

    public int Z
    {
        get { return Model.Z; }
    }


    // PUBLIC
    // ========================================================================

    public void AttachModel(CellModel model)
    {
        Model = model;
    }
    
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

    public IEnumerator GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}
