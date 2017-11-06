using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ClickedDelegate(CellController cell);

public class CellController : MonoBehaviour
{
    public event ClickedDelegate OnClicked;

    public int Id;
    public int X;
    public int Z;

    public Texture2D TextureNormal;
    public Texture2D TextureHover;

    public GameObject Element0;
    public GameObject Element1;
    public GameObject Element2;

    public int Type;

    GameObject Mesh;

    private Vector2 uvOffset;

	// Use this for initialization
	void Start ()
    {

    }


    // PUBLIC
    // ========================================================================
    
    public void Update ()
    {
        if(Type == 1)
        { 
            uvOffset.x += 0.0001f;
            uvOffset.y += -0.0001f;

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


    // EVENTS
    // ========================================================================

    private void OnMouseUp()
    {
        OnClicked?.Invoke(this);
    }

    private void OnMouseEnter()
    {
        if(Type != 2)
        { 
            gameObject.transform.Translate(0, 0, 0.01f);
        }

        //Renderer renderer = gameObject.GetComponent<Renderer>();
        //renderer.material.color = Color.red;
    }

    private void OnMouseExit()
    {
        if(Type != 2)
        { 
            gameObject.transform.Translate(0, 0, -0.01f);
        }

        //Renderer renderer = gameObject.GetComponent<Renderer>();
        //renderer.material.color = Color.white;
    }
}
