using System.Collections.Generic;
// using System.Xml.Linq;
using UnityEngine;
// using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

public class XRProjectionMatrixExample : MonoBehaviour
{
    List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();

    public float left = -0.2F;
    public float right = 0.2F;
    public float top = 0.2F;
    public float bottom = -0.2F;
    public Camera cam;
    public float ang = 0F;
    
    void Start()
    {
        // cam = Camera.main;
        // Get all active display subsystems
        SubsystemManager.GetInstances(displaySubsystems);
        
        if (displaySubsystems.Count > 0)
        {
            Debug.Log($"Found {displaySubsystems.Count} XR display subsystems");
        }
        else
        {
            Debug.LogWarning("No XR display subsystems found");
        }
    }
//     void OnPreRender()
// {
//     Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane, ang);
//     cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, m);
//     cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, m);
// }

    void Update(){
        Debug.Log("XR on render");
        SetProjectionMatrices();
        GetProjectionMatrices();
    }

    void SetProjectionMatrices(){
        var displaySubsystem = displaySubsystems[0];
        XRDisplaySubsystem.XRRenderPass renderPass;
        displaySubsystem.GetRenderPass(0, out renderPass);
        // Debug.Log("XR renderparameter count: " + renderPass.GetRenderParameterCount());
        for (int i = 0; i < 2; i++){
            XRDisplaySubsystem.XRRenderParameter renderParameter;
            renderPass.GetRenderParameter(Camera.main, i, out renderParameter);
            Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane, ang);
            renderParameter.projection = m;


        }
    }
    void GetProjectionMatrices()
{
    if (displaySubsystems.Count == 0) return;
    
    var displaySubsystem = displaySubsystems[0];
    
    // Get information about the current render pass
    XRDisplaySubsystem.XRRenderPass renderPass;
    displaySubsystem.GetRenderPass(0, out renderPass);
    // Debug.Log("XR: Cluh");
    Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane, ang);
    
        // Get information about each eye (render pass has a culling pass for each eye)
        for (int i = 0; i < 2; i++)
        {
            XRDisplaySubsystem.XRRenderParameter renderParameter;
            renderPass.GetRenderParameter(Camera.main, i, out renderParameter);
            
            // Access projection matrix
            Matrix4x4 projectionMatrix = renderParameter.projection;
            Matrix4x4 viewMatrix = renderParameter.view;
            
            Debug.Log($"Eye {i} XR Projection Matrix: {projectionMatrix}");
            Debug.Log($"Eye {i} XR View Matrix: {viewMatrix}");

            renderParameter.projection = m;
            projectionMatrix = renderParameter.projection;
            Debug.Log($"Eye {i} NEW XR Projection Matrix: {projectionMatrix}");


        }
    
}
static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far, float ang)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;

        Matrix4x4 m2 = new Matrix4x4();
        m2[0, 0] = Mathf.Cos(ang);
        m2[0, 1] = 0;
        m2[0, 2] = Mathf.Sin(ang);
        m2[0, 3] = 0;

        m2[1, 0] = 0;
        m2[1, 1] = 1;
        m2[1, 2] = 0;
        m2[1, 3] = 0;

        m2[2, 0] = (-1)* Mathf.Sin(ang);
        m2[2, 1] = 0;
        m2[2, 2] = Mathf.Cos(ang);
        m2[2, 3] = 0;

        m2[3, 0] = 0;
        m2[3, 1] = 0;
        m2[3, 2] = 0;
        m2[3, 3] = 1;

        return m*m2;

        // return m;
    }

}

