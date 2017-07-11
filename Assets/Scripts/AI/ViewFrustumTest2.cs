using UnityEngine;
using System.Collections;

public class ViewFrustumTest2 : MonoBehaviour {

    public Camera cam;
    public Material mat;
    private MeshFilter mes;
    public MeshRenderer mesRen;

	// Use this for initialization
	void Start () {

        cam = GetComponent<Camera>();
        mes = cam.GetComponent<MeshFilter>();
        mesRen = GetComponent<MeshRenderer>();


        mes.mesh = cam.GenerateFrustumMesh();
        mesRen.material = mat;
    }
	
	// Update is called once per frame
	void Update () {
        mes.mesh = cam.GenerateFrustumMesh();
    }
}
