using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class BakeTextureWindow : EditorWindow {

    string filePath = "Assets/Resources/BakedTextures/";

    Material imageMaterial;
    string fileName;
    Vector2Int resolution;

    [MenuItem("Tools/BakeTexture")]
    static void OpenWindow() {
        //create window
        BakeTextureWindow window = EditorWindow.GetWindow<BakeTextureWindow>();
        window.Show();
    }

    void OnGUI() {

        // Debug.Log("Hello");
        filePath = "Assets/Resources/BakedTextures/";
        imageMaterial = (Material)EditorGUILayout.ObjectField("Material", imageMaterial, typeof(Material), false);
        resolution = EditorGUILayout.Vector2IntField("Image Resolution", resolution);
        fileName = EditorGUILayout.TextField("Image Name", fileName);

        if (GUILayout.Button("Bake")) {
            BakeTexture();
        }

    }

    void BakeTexture() {
        // Bake texture
        //render material to rendertexture
        RenderTexture renderTexture = RenderTexture.GetTemporary(resolution.x, resolution.y);
        Graphics.Blit(null, renderTexture, imageMaterial, -1);
        // Graphics.Blit(renderTexture, renderTexture, imageMaterial, 1);

        //transfer image from rendertexture to texture
        Texture2D texture = new Texture2D(resolution.x, resolution.y);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(Vector2.zero, resolution), 0, 0);

        //save texture to file
        byte[] png = texture.EncodeToPNG();
        File.WriteAllBytes(filePath + fileName + ".png", png);
        AssetDatabase.Refresh();

        //clean up variables
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        DestroyImmediate(texture);
    }



}
