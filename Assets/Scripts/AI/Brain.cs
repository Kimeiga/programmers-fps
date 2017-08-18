using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brain : MonoBehaviour {

    private UnityEngine.AI.NavMeshAgent agent;
    private Camera cam;
    private MeshFilter camMeshFilter;
    private MeshCollider camMeshCollider;
    public GameObject fireTransform;

    public Transform goal;
    private Collider goalCollider;


    public List<Transform> levelCorners;
    public List<Transform> visibleLevelCorners;
    private LineRenderer line;

    private Transform lookSpot;

    private Plane[] frustumPlanes = new Plane[6];

    // Use this for initialization
    void Start () {

        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        line = gameObject.GetComponent<LineRenderer>();
        cam = gameObject.GetComponentInChildren<Camera>();
        camMeshFilter = cam.GetComponent<MeshFilter>();
        if(camMeshFilter == null)
        {
            camMeshFilter = cam.gameObject.AddComponent<MeshFilter>();
        }
        camMeshCollider = cam.GetComponent<MeshCollider>();
        if(camMeshCollider == null)
        {
            camMeshCollider = cam.gameObject.AddComponent<MeshCollider>();
        }

        goalCollider = goal.GetComponent<Collider>();

        agent.destination = goal.position;

        levelCorners = new List<Transform>();
        visibleLevelCorners = new List<Transform>();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Corner").Length; i++)
        {
            levelCorners.Add(GameObject.FindGameObjectsWithTag("Corner")[i].transform);
        }


        //View Frustum
        camMeshFilter.mesh = cam.GenerateFrustumMesh();
        camMeshCollider.sharedMesh = camMeshFilter.mesh;

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

    // Update is called once per frame
    void Update() {

        

        UpdatePlanes(frustumPlanes, cam);

        foreach(Transform corner in levelCorners)
        {

            if(frustumPlanes[0].GetSide(corner.position)&& frustumPlanes[1].GetSide(corner.position) && frustumPlanes[2].GetSide(corner.position) && frustumPlanes[3].GetSide(corner.position) && frustumPlanes[4].GetSide(corner.position) && frustumPlanes[5].GetSide(corner.position))
            {
                if (!visibleLevelCorners.Contains(corner))
                {
                    visibleLevelCorners.Add(corner);
                }
            }
            else
            {
                if (visibleLevelCorners.Contains(corner))
                {
                    visibleLevelCorners.Remove(corner);
                }
            }
        }

        //LOOK ROTATION STUFF

        //Debug.DrawLine(levelCorners[1].position, levelCorners[1].position + new Vector3(0, 400, 0));

        //test
        /*
        if (camMeshCollider.bounds.Contains(levelCorners[1].position))
        {
            print("O");
        }
        */

        //Draw path

        if (line != null)
        {
            line.SetVertexCount(agent.path.corners.Length);

            for (int e = 0; e < agent.path.corners.Length; e++)
            {
                line.SetPosition(e, agent.path.corners[e]);
            }
        }

        Vector3 tempV31 = Vector3.zero;

        if (agent.hasPath == true)
        {

            tempV31 = new Vector3(agent.path.corners[1].x, 0, agent.path.corners[1].z);
        }

        
        for(int i =0; i<levelCorners.Count; i++)
        {
            

            Debug.DrawLine(levelCorners[i].position, new Vector3(levelCorners[i].position.x, levelCorners[i].position.y + 1, levelCorners[i].position.z), Color.yellow);
            Debug.DrawLine(levelCorners[i + 1].position, new Vector3(levelCorners[i + 1].position.x, levelCorners[i + 1].position.y + 1, levelCorners[i + 1].position
                .z), Color.yellow);

            Vector3 tempV32 = new Vector3(levelCorners[i].position.x, 0, levelCorners[i].position.z);

            if(Vector3.Distance(tempV31,tempV32) < 1f)
            {
                lookSpot = levelCorners[i];

                if(Vector3.Angle(fireTransform.transform.localRotation.eulerAngles,  (levelCorners[i].position-levelCorners[i+1].position)) < 1f)
                {
                    lookSpot = levelCorners[i + 1];
                    //print("L");
                }

                break;

            }
        }
        
        
        fireTransform.transform.LookAt(lookSpot.position, Vector3.up);

        
    }
}
