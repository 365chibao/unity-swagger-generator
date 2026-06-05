using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnitySwaggerGenerator
{
    public class SwaggerGeneratorWindow : EditorWindow
    {
        private string swaggerJsonPath = "";
        private string outputPath = "";
        private string dtoNamespace = "UnitySwaggerGenerator.DTOs";

        private const string PrefsSwaggerJsonPath = "UnitySwaggerGenerator_SwaggerJsonPath";
        private const string PrefsOutputPath = "UnitySwaggerGenerator_OutputPath";
        private const string PrefsNamespace = "UnitySwaggerGenerator_Namespace";

        [MenuItem("Tools/Swagger DTO Generator")]
        public static void ShowWindow()
        {
            GetWindow<SwaggerGeneratorWindow>("Swagger DTO Generator");
        }

        private void OnEnable()
        {
            swaggerJsonPath = EditorPrefs.GetString(PrefsSwaggerJsonPath, "");
            outputPath = EditorPrefs.GetString(PrefsOutputPath, "Assets/Scripts/DTOs");
            dtoNamespace = EditorPrefs.GetString(PrefsNamespace, "UnitySwaggerGenerator.DTOs");
        }

        private void OnGUI()
        {
            GUILayout.Label("Swagger DTO Generator Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            swaggerJsonPath = EditorGUILayout.TextField("Swagger JSON Path", swaggerJsonPath);
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFilePanel("Select Swagger JSON", "", "json");
                if (!string.IsNullOrEmpty(path))
                {
                    swaggerJsonPath = path;
                    EditorPrefs.SetString(PrefsSwaggerJsonPath, swaggerJsonPath);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            outputPath = EditorGUILayout.TextField("Output Folder", outputPath);
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFolderPanel("Select Output Directory", "Assets", "");
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.StartsWith(Application.dataPath))
                    {
                        path = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                    outputPath = path;
                    EditorPrefs.SetString(PrefsOutputPath, outputPath);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            dtoNamespace = EditorGUILayout.TextField("DTO Namespace", dtoNamespace);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(PrefsNamespace, dtoNamespace);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Generate DTOs", GUILayout.Height(40)))
            {
                GenerateDTOs();
            }
        }

        private void GenerateDTOs()
        {
            if (string.IsNullOrEmpty(swaggerJsonPath))
            {
                EditorUtility.DisplayDialog("Error", "Please specify the Swagger JSON file path.", "OK");
                return;
            }

            if (!File.Exists(swaggerJsonPath))
            {
                EditorUtility.DisplayDialog("Error", $"Swagger JSON file does not exist at:\n{swaggerJsonPath}", "OK");
                return;
            }

            if (string.IsNullOrEmpty(outputPath))
            {
                EditorUtility.DisplayDialog("Error", "Please specify the output folder path.", "OK");
                return;
            }

            string fullOutputPath = outputPath;
            if (outputPath.StartsWith("Assets"))
            {
                fullOutputPath = Path.Combine(Application.dataPath, outputPath.Substring("Assets".Length).TrimStart('/', '\\'));
            }

            try
            {
                string jsonContent = File.ReadAllText(swaggerJsonPath);
                List<ParsedSchema> schemas = SwaggerParser.Parse(jsonContent);

                if (schemas == null || schemas.Count == 0)
                {
                    EditorUtility.DisplayDialog("Warning", "No schemas parsed from the Swagger JSON file. Check console for logs.", "OK");
                    return;
                }

                DTOGenerator.Generate(schemas, fullOutputPath, dtoNamespace);

                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("Success", $"Successfully generated {schemas.Count} DTO class files in:\n{outputPath}", "OK");
                Debug.Log($"[SwaggerGeneratorWindow] DTO Generation complete. {schemas.Count} schemas generated at {outputPath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SwaggerGeneratorWindow] Failed to generate DTOs: {ex.Message}");
                EditorUtility.DisplayDialog("Error", $"Generation failed: {ex.Message}", "OK");
            }
        }
    }
}
