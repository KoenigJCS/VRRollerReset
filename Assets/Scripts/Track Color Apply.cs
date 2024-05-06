using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class TrackColorApply : MonoBehaviour
{
    public SplineComputer trackSpline;
    public float trackLength = 0;
    // Start is called before the first frame update
    void Start()
    {
        trackSpline = GetComponent<SplineComputer>();
        trackLength = trackSpline.CalculateLength();


        foreach (SplineMesh trackMesh in GetComponentsInChildren<SplineMesh>())
        {
            if (trackMesh.name != "ChainLift")
            {
                trackMesh.GetComponent<Material>().color = new Color(Random.value, Random.value, Random.value, 1.0f);
            }
        }
    }
}