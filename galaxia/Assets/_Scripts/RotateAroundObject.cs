using UnityEngine;
using System.Collections;

public class RotateAroundObject : MonoBehaviour
{
    public Vector3 translation;
    public Vector3 eulerAngles;
    public Vector3 scale = new Vector3(1, 1, 1);
    private MeshFilter mf;
    private Vector3[] origVerts;
    private Vector3[] newVerts;

    // spin variables
    public bool spin = false;
    public float rotSpeed = 10f;
    private float rot = 0;

    // move around a radius
    public bool orbit = false;
    public float orbitSpeed = 5f;
    public float radius = 2f;
    private float orb = 0;

    void Start()
    {
        mf = GetComponent<MeshFilter>();
        origVerts = mf.mesh.vertices;
        newVerts = new Vector3[origVerts.Length];
    }
    void Update()
    {
        rot += Time.deltaTime * rotSpeed;
        orb += Time.deltaTime * orbitSpeed;
        Quaternion rotation;
        if (spin)
            rotation = Quaternion.Euler(0, rot, 0);
        else
            rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        if (orbit)
            translation = new Vector3(Mathf.Sin(orb) * radius, Mathf.Cos(orb) * radius, 0);

        Matrix4x4 m = Matrix4x4.TRS(translation, rotation, scale);
        for (int i = 0; i < origVerts.Length; i++)
            newVerts[i] = m.MultiplyPoint3x4(origVerts[i]);
        mf.mesh.vertices = newVerts;
    }
}