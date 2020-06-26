using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RewardWall : MonoBehaviour
{
    [SerializeField]
    Material[] listOfMaterials;
    [SerializeField]
    GameObject baseWallPiece;

    List<GameObject> constructedPieces;
    [SerializeField, Range(0.25f, 1.0f)]
    float scaleOfWallPieces = 0.95f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(baseWallPiece != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B) == true)
        {
            BuildWall(6);
        }
    }

    public void BuildWall(int numPieces)
    {
        if(constructedPieces != null)
        {
            foreach(var wallPiece in constructedPieces)
            {
                Destroy(wallPiece);
            }
        }
        constructedPieces = new List<GameObject>(); 
        float height = baseWallPiece.GetComponent<MeshRenderer>().bounds.extents.y * 2;

        Vector3 position = transform.position;

        for (int i = 0; i < numPieces; i++)
        {
            position.y += height;
            GameObject obj = CreateWallPiece(position, i, scaleOfWallPieces);
            height = obj.GetComponent<MeshRenderer>().bounds.extents.y * 2;
        }
        position.y += height;
        GameObject obj2 = CreateWallPiece(position, 0, scaleOfWallPieces-0.95f);
        obj2.GetComponent<Renderer>().material.color = Color.white;
    }

    void ScaleObjectToMatch(GameObject obj, GameObject parent, float percentage)
    {
        Vector3 scale = parent.transform.localScale;
        scale.x *= percentage;
        scale.z *= percentage;
        obj.transform.localScale = scale;
        obj.transform.parent = transform;
    }

    GameObject CreateWallPiece(Vector3 position, int colorIndex, float scale)
    {
        GameObject obj = Instantiate(baseWallPiece, position, baseWallPiece.transform.rotation);
        Color color = Color.red;
        if (colorIndex % 2 == 1)
            color = Color.blue;
        obj.GetComponent<Renderer>().material.color = color;
        ScaleObjectToMatch(obj, this.gameObject, scaleOfWallPieces);
        obj.transform.parent = transform;
        constructedPieces.Add(obj);
        return obj;
    }
}

