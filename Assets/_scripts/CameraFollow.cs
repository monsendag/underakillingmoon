using System;
using UnityEngine;

class CameraFollow : MonoBehaviour
{
    public GameObject Target = null;
    public Vector2 Rectangle = new Vector2(5.0f,5.0f);
    public Vector2 BottomLeftWorld;
    public Vector2 TopRightWorld;

    void Update()
    {
        DebugUtil.Assert(Target != null);

        Vector3 max = transform.position +
            new Vector3(Rectangle.x / 2.0f,0.0f,Rectangle.y / 2.0f);
        Vector3 min = transform.position -
            new Vector3(Rectangle.x / 2.0f, 0.0f, Rectangle.y / 2.0f);
        //


        Vector3 closest = new Vector3(
            Mathf.Max(Mathf.Min(max.x,Target.transform.position.x), min.x),
            0.0f,
            Mathf.Max(Mathf.Min(max.z,Target.transform.position.z), min.z));
        Vector3 motion = Target.transform.position - closest;
        motion.y = 0.0f;
        transform.position += motion;

        Vector3 pos = Vector3.zero;
        camera.ResetProjectionMatrix();
        camera.ResetWorldToCameraMatrix();
        Matrix4x4 mat = camera.projectionMatrix.inverse;
        Vector3 bottomLeft = (mat * new Vector3(-1.0f, 0.0f, -1.0f));
        bottomLeft += transform.position;
        Vector3 topRight = mat * new Vector3(1.0f,0.0f,1.0f);
        topRight += transform.position;
        Debug.Log(topRight);
        motion = Vector3.zero;

        if (bottomLeft.x < BottomLeftWorld.x) { motion.x += BottomLeftWorld.x - bottomLeft.x; }
        if (bottomLeft.z < BottomLeftWorld.y) { motion.z += BottomLeftWorld.y - bottomLeft.z; }
        
        if (topRight.x > TopRightWorld.x) { motion.x += TopRightWorld.x - topRight.x; }
        if (topRight.z > TopRightWorld.y) { motion.z += TopRightWorld.y - topRight.z; }
   
        transform.position += motion;
    }
}

