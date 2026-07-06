using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class ShaderIncluder
{
    [MenuItem("Tools/Include Required Shaders")]
    public static void IncludeShaders()
    {
        var graphicsSettings = new SerializedObject(GraphicsSettings.GetGraphicsSettings());
        SerializedProperty alwaysIncluded = graphicsSettings.FindProperty("m_AlwaysIncludedShaders");

        string[] needed = new string[] { "Unlit/Color", "Unlit/Texture", "Legacy Shaders/Diffuse" };
        foreach (string shaderName in needed)
        {
            Shader shader = Shader.Find(shaderName);
            if (shader == null)
            {
                Debug.LogWarning("Shader no encontrado: " + shaderName);
                continue;
            }
            bool alreadyIncluded = false;
            for (int i = 0; i < alwaysIncluded.arraySize; i++)
            {
                if (alwaysIncluded.GetArrayElementAtIndex(i).objectReferenceValue == shader)
                {
                    alreadyIncluded = true;
                    break;
                }
            }
            if (!alreadyIncluded)
            {
                alwaysIncluded.InsertArrayElementAtIndex(alwaysIncluded.arraySize);
                alwaysIncluded.GetArrayElementAtIndex(alwaysIncluded.arraySize - 1).objectReferenceValue = shader;
                Debug.Log("Shader incluido: " + shaderName);
            }
        }
        graphicsSettings.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
    }
}
