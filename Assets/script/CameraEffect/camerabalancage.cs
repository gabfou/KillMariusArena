using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerabalancage : MonoBehaviour {

    Matrix4x4 originalProjection;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
		originalProjection = cam.projectionMatrix;
    }

    void Update()
    {
        Matrix4x4 p = originalProjection;
        p.m01 += Mathf.Sin(Time.time * 1.2F) * 0.1F;
        p.m10 += Mathf.Sin(Time.time * 1.5F) * 0.1F;
        cam.projectionMatrix = p;
    }
}
