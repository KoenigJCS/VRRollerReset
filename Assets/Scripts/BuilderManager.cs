using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BuilderManager : MonoBehaviour
{
    [SerializeField] SplineComputer spline;
    [SerializeField] GameObject selectionSphere;
    [SerializeField] Transform sphereParrent;
    [SerializeField] GameObject UIScreen;
    List<GameObject> spheres;
    SplinePoint[] startingPoints;
    bool isEditing = false;
    [SerializeField] Transform XRTransformOrigin;
    // Start is called before the first frame update
    void Start()
    {
        spheres = new();
    }

    // Update is called once per frame
    void Update()
    {
        // if(!isEditing)
        //     return;
        
        // foreach (var sphere in spheres)
        // {
        //     sphere.GetComponent<Grabab
        // }
    }

    public void SphereSetDown(HoverExitEventArgs args)
    {
        SplinePoint[] newPoints = new SplinePoint[spheres.Count];
        // args.interactableObject.GetAttachTransform
        for (int i =0; i<spheres.Count;i++)
        {
            float magnitude = i<spheres.Count-1 
            ? (spheres[i].transform.position-spheres[i+1].transform.position).magnitude/3f 
            : (spheres[i].transform.position-spheres[0].transform.position).magnitude/3f;
            newPoints[i] = new SplinePoint(spheres[i].transform.position,(-1 * magnitude * spheres[i].transform.forward)+spheres[i].transform.position,spheres[i].transform.up,1f,Color.white);
        }
        spline.SetPoints(newPoints);
    }

    public void StartEditorMode()
    {
        CartMovement.inst.SetPlayerScale(3);
        UIScreen.transform.localScale=Vector3.one*.5f;
        startingPoints = spline.GetPoints();
        foreach (var point in startingPoints)
        {
            GameObject temp = Instantiate(selectionSphere,point.position,Quaternion.LookRotation(point.position-point.tangent,point.normal),sphereParrent);
            temp.GetComponent<XRGrabInteractable>().hoverExited.AddListener(SphereSetDown);
            spheres.Add(temp);
        }
        isEditing=true;
        
    }

    public void StopEditorMode()
    {
        CartMovement.inst.SetPlayerScale(1);
        for (int i =0; i<spheres.Count;i++)
        {
            Destroy(spheres[i]);
        }
        spheres.Clear();
        isEditing=false;
    }

    public void AddNewSphere()
    {
        if(!isEditing)
            return;
        
        Vector3 nearPoint = XRTransformOrigin.position;

        int nearIndex = 0;
        float nearDist = float.MaxValue; 
        for (int i = 0; i<spheres.Count;i++)
        {
            float dist = (spheres[i].transform.position-nearPoint).magnitude;
            if(dist<nearDist)
            {
                nearIndex=i;
                nearDist=dist;
            }
        }
        int nextIndex = nearDist<spheres.Count-1 ? nearIndex+1 : 0;

        Vector3 newPos = spheres[nearIndex].transform.position+(spheres[nextIndex].transform.position-spheres[nearIndex].transform.position)/2;
        GameObject temp = Instantiate(selectionSphere,newPos,Quaternion.LookRotation(spheres[nextIndex].transform.position-spheres[nearIndex].transform.position,spheres[nearIndex].transform.up),sphereParrent);
        temp.GetComponent<XRGrabInteractable>().hoverExited.AddListener(SphereSetDown);
        spheres.Insert(nextIndex,temp);

        SphereSetDown(args: null);
    }
}
