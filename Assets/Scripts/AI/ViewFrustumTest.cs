using UnityEngine;
using System.Collections;

public class ViewFrustumTest : MonoBehaviour
{
    private Camera cam;
    private Plane[] planes;
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        int i = 0;
        while (i < planes.Length)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            p.name = "Plane " + i.ToString();
            p.transform.position = -planes[i].normal * planes[i].distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
            i++;
        }
    }
}