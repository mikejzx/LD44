using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TripEffect : MonoBehaviour {

    public Shader fxShader;

    private Material fxMat;

    /// <summary>Create the materials etc on enable of the obj</summary>
    private void OnEnable () {
        fxShader = Shader.Find("Hidden/TripEffect");
        if (fxShader == null) { enabled = false;  return; }
        fxMat = new Material(fxShader);
        fxMat.hideFlags = HideFlags.HideAndDontSave;
    }

    /// <summary>Clean up resources</summary>
    private void OnDisable () {
        if (fxMat != null) {
    #if UNITY_EDITOR
            DestroyImmediate(fxMat);
    #else
            Destroy(fxMat);
    #endif
        }
    }

    /// <summary>Apply the effect on image render</summary>
    private void OnRenderImage (RenderTexture src, RenderTexture dest) {
        if (fxMat == null) { enabled = false; return; }

        Graphics.Blit(src, dest, fxMat);
    }
}
