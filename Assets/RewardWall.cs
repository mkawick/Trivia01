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
            GameObject obj = Instantiate(baseWallPiece, position, baseWallPiece.transform.rotation);
            Color color = Color.red;
            if (i % 2 == 1)
                color = Color.blue;
            obj.GetComponent<Renderer>().material.color = color;
            ScaleObjectToMatch(obj, this.gameObject, scaleOfWallPieces);
            obj.transform.parent = transform;
            constructedPieces.Add(obj);
        }
        position.y += height;
        GameObject obj2 = Instantiate(baseWallPiece, position, baseWallPiece.transform.rotation);
        obj2.GetComponent<Renderer>().material.color = Color.white;
        ScaleObjectToMatch(obj2, this.gameObject, scaleOfWallPieces - 0.05f);
        obj2.transform.parent = transform;
        constructedPieces.Add(obj2);
    }

    void ScaleObjectToMatch(GameObject obj, GameObject parent, float percentage)
    {
        Vector3 scale = parent.transform.localScale;
        scale.x *= percentage;
        scale.z *= percentage;
        obj.transform.localScale = scale;
        obj.transform.parent = transform;
    }
}

