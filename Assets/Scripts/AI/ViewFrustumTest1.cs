using UnityEngine;
using System.Collections;

public class ViewFrustumTest1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private static Plane FromVec4(Vector4 aVec)
    {
        Vector3 n = aVec;
        float l = n.magnitude;
        return new Plane(n / l, aVec.w / l);
    }

    public static void UpdatePlanes(Plane[] planes, Matrix4x4 m)
    {
        if (planes == null || planes.Length < 6)
            return;
        var r0 = m.GetRow(0);
        var r1 = m.GetRow(1);
        var r2 = m.GetRow(2);
        var r3 = m.GetRow(3);

        planes[0] = FromVec4(r3 - r0); // Right
        planes[1] = FromVec4(r3 + r0); // Left
        planes[2] = FromVec4(r3 - r1); // Top
        planes[3] = FromVec4(r3 + r1); // Bottom
        planes[4] = FromVec4(r3 - r2); // Far
        planes[5] = FromVec4(r3 + r2); // Near
    }
    public static void UpdatePlanes(Plane[] planes, Camera cam)
    {
        UpdatePlanes(planes, cam.projectionMatrix * cam.worldToCameraMatrix);
    }

}
