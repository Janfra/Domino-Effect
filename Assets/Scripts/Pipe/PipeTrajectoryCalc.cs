using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PipeTrajectoryCalc : MonoBehaviour
{
    private Transform objectBeingProj = null;
    private Transform[] nodes;
    private float lerpCurrentTime;
    [SerializeField] private float lerpTotalTime;
    private void Start()
    {
        nodes = new Transform[3] { transform.GetChild(0).transform, transform.GetChild(1).transform, transform.GetChild(2).transform };
    }

    // Update is called once per frame
    private void Update()
    {
        float lerpScale = lerpCurrentTime / lerpTotalTime;
        if (objectBeingProj != null && lerpScale < 1.0f)
        {
            if (lerpScale <= 0.001f)
            {
                nodes[0].position = objectBeingProj.position;
            }
            Vector3 lerp1 = Vector3.Lerp(nodes[0].position, nodes[1].position, lerpScale);
            Vector3 lerp2 = Vector3.Lerp(nodes[1].position, nodes[2].position, lerpScale);

            objectBeingProj.position = Vector3.Lerp(objectBeingProj.position, Vector3.Lerp(lerp1, lerp2, lerpScale), lerpScale);

            lerpCurrentTime += Time.deltaTime;
        }
        else if (objectBeingProj != null && lerpScale >= 1.0f)
        {
            lerpCurrentTime = 0.0f;
            Camera.main.GetComponent<DominoMovement>().isInputEnable = true;
            objectBeingProj.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            objectBeingProj = null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 lastPoint = transform.GetChild(0).position;
        Gizmos.color = Color.magenta;
        for (float i = 0.0f; i <= 1.0f; i += 0.05f)
        {
            Vector3 lerp1 = Vector3.Lerp(transform.GetChild(0).position, transform.GetChild(1).position, i);
            Vector3 lerp2 = Vector3.Lerp(transform.GetChild(1).position, transform.GetChild(2).position, i);
            Vector3 lerp3 = Vector3.Lerp(lerp1, lerp2, i);
            Gizmos.DrawLine(lastPoint, lerp3);
            lastPoint = lerp3;
        }
        Gizmos.DrawLine(lastPoint, transform.GetChild(2).position);
    }

    public void SetObjectToProject(Transform transform)
    {
        objectBeingProj = transform;
    }
}
