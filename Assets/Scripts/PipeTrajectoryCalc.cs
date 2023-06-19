using System.Collections;
using System.Collections.Generic;
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
            Vector3 lerp1 = Vector3.Lerp(nodes[0].position, nodes[1].position, lerpScale);
            Vector3 lerp2 = Vector3.Lerp(nodes[1].position, nodes[2].position, lerpScale);

            objectBeingProj.position = Vector3.Lerp(objectBeingProj.position, Vector3.Lerp(lerp1, lerp2, lerpScale), lerpScale);

            lerpCurrentTime += Time.deltaTime;
        }
        else if (objectBeingProj != null && lerpScale >= 1.0f)
        {
            lerpCurrentTime = 0.0f;
            objectBeingProj.gameObject.GetComponent<TempMovement>().enabled = true;
            objectBeingProj = null;
        }
    }

    public void SetObjectToProject(Transform transform)
    {
        objectBeingProj = transform;
    }
}