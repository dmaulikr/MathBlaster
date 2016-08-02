using UnityEngine;
using System.Collections;

public class TextMeshLayering : MonoBehaviour {

    public string SortingLayerName = "Default";
    public int SortingOrder = 1;
    private TextMesh textMesh;

    void Awake()
    {
        // Set the sorting layer of position so that the number in textMesh will overlay the enemy sprite 
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }

}
