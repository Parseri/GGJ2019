using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathTool : MonoBehaviour {

[SerializeField]
    private float size = 1f;

    [SerializeField]
    private Color lineColor;

    public List<Transform> nodes = new List<Transform>();

    void OnDrawGizmos()
    {
    
        Gizmos.color = lineColor;

        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 0; i < pathTransforms.Length; i++) {
            if(pathTransforms[i] != transform) {
                nodes.Add(pathTransforms[i]);
            }
        }

        for(int i = 1; i < nodes.Count; i++) {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = nodes[i - 1].position;

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(previousNode, 0.3f);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }
    }
    
}

