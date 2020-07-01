using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RewardWall : MonoBehaviour
{
    [SerializeField]
    GameObject baseWallPiece = null;
    [SerializeField]
    GameObject wallPiece = null;
    GameObject brickPlaceholder = null;

    [SerializeField]
    bool allowCreateWall = true;

    List<GameObject> constructedPieces;
    [SerializeField, Range(0.25f, 1.0f)]
    float scaleOfWallPieces = 0.95f;

    private void Awake()
    {
        brickPlaceholder = new GameObject();
        brickPlaceholder.transform.parent = this.transform;
        brickPlaceholder.transform.rotation = this.transform.rotation;
        brickPlaceholder.transform.localScale = new Vector3(1, 0.5f, 3.5f); // off by 90
    }
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
        if (allowCreateWall == true)
        {
            if (constructedPieces != null)
            {
                foreach (var wallPiece in constructedPieces)
                {
                    Destroy(wallPiece);
                }
            }
            constructedPieces = new List<GameObject>();
            float height = baseWallPiece.GetComponent<MeshRenderer>().bounds.extents.y;

            Vector3 position = GetTopOfStand();
            GameObject obj = CreateWallPiece(wallPiece, position, 0, scaleOfWallPieces);
            height = obj.GetComponent<MeshRenderer>().bounds.extents.y * 2;
            obj.transform.position += new Vector3(0, height / 2, 0);
            position = obj.transform.position;

            for (int i = 1; i < numPieces; i++)
            {
                position.y += height;
                obj = CreateWallPiece(wallPiece, position, i, scaleOfWallPieces);
            }
            position.y += height;
            GameObject obj2 = CreateWallPiece(baseWallPiece, position, 0, scaleOfWallPieces - 0.05f);
            obj2.GetComponent<MeshRenderer>().material.color = Color.white;

        }
    }

    Vector3 GetTopOfStand()
    {
        Vector3 position = transform.position;
        position.y += 3;
        RaycastHit hit;
        Physics.Raycast(position, Vector3.down, out hit, 8);
        position = hit.point;
        return position;
    }
    void ScaleObjectToMatch(GameObject obj, Transform grandParent, Transform parent, float percentage)
    {
        //var dimensions = grandParent.gameObject.GetComponent<MeshRenderer>().bounds.extents;
        Vector3 scale = parent.transform.localScale;
        scale.x *= percentage;
        scale.z *= percentage;
        obj.transform.localScale = scale;
        obj.transform.parent = parent;
        //obj.transform
    }

    GameObject CreateWallPiece(GameObject piece, Vector3 position, int colorIndex, float scale)
    {
        float testHeight = piece.GetComponent<MeshRenderer>().bounds.extents.y;
        //Debug.Log("e: testheight: " + testHeight);
        GameObject obj = Instantiate(piece, position, baseWallPiece.transform.rotation);
        Color color = Color.red;
        if (colorIndex % 2 == 1)
            color = Color.blue;
        obj.GetComponent<Renderer>().material.color = color;
        ScaleObjectToMatch(obj, this.transform, brickPlaceholder.transform, scale);
        obj.transform.parent = brickPlaceholder.transform;
        testHeight = piece.GetComponent<MeshRenderer>().bounds.extents.y;
        constructedPieces.Add(obj);
        return obj;
    }
}

