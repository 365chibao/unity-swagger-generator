using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

#if !UNITY_5_3_OR_NEWER
namespace UnityEngine
{
    public static class Debug
    {
        public static void Log(object message) => System.Console.WriteLine(message);
        public static void LogWarning(object message) => System.Console.WriteLine("Warning: " + message);
        public static void LogError(object message) => System.Console.Error.WriteLine("Error: " + message);
    }
}
#endif

namespace UnitySwaggerGenerator
{
    public class ParsedSchema
    {
        public string Name { get; set; }
        public List<ParsedProperty> Properties { get; set; } = new List<ParsedProperty>();
    }

    public class ParsedProperty
    {
        public string Name { get; set; }
        public string RawType { get; set; }
        public string Format { get; set; }
        public string RefPath { get; set; }
        public string ArrayItemsRefPath { get; set; }
        public string ArrayItemsType { get; set; }
        public string ArrayItemsFormat { get; set; }

        public bool IsArray => RawType == "array";
        public bool IsRef => !string.IsNullOrEmpty(RefPath);
        public bool IsArrayRef => IsArray && !string.IsNullOrEmpty(ArrayItemsRefPath);
    }

    public static class SwaggerParser
    {
        public static List<ParsedSchema> Parse(string jsonContent)
        {
            var schemasList = new List<ParsedSchema>();
            try
            {
                var root = JObject.Parse(jsonContent);
                var schemas = root["components"]?["schemas"] as JObject;

                if (schemas == null)
                {
                    schemas = root["definitions"] as JObject;
                }

                if (schemas == null)
                {
                    Debug.LogWarning("[SwaggerParser] No schemas or definitions found in Swagger file.");
                    return schemasList;
                }

                foreach (var schemaProp in schemas.Properties())
                {
                    var schemaName = schemaProp.Name;
                    var schemaObj = schemaProp.Value as JObject;
                    if (schemaObj == null) continue;

                    var parsedSchema = new ParsedSchema { Name = schemaName };
                    var propertiesObj = schemaObj["properties"] as JObject;

                    if (propertiesObj != null)
                    {
                        foreach (var prop in propertiesObj.Properties())
                        {
                            var propName = prop.Name;
                            var propDetails = prop.Value as JObject;
                            if (propDetails == null) continue;

                            var parsedProperty = new ParsedProperty
                            {
                                Name = propName,
                                RawType = propDetails["type"]?.ToString(),
                                Format = propDetails["format"]?.ToString(),
                                RefPath = propDetails["$ref"]?.ToString()
                            };

                            if (parsedProperty.IsArray)
                            {
                                var itemsObj = propDetails["items"] as JObject;
                                if (itemsObj != null)
                                {
                                    parsedProperty.ArrayItemsType = itemsObj["type"]?.ToString();
                                    parsedProperty.ArrayItemsFormat = itemsObj["format"]?.ToString();
                                    parsedProperty.ArrayItemsRefPath = itemsObj["$ref"]?.ToString();
                                }
                            }

                            parsedSchema.Properties.Add(parsedProperty);
                        }
                    }

                    schemasList.Add(parsedSchema);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SwaggerParser] Error parsing swagger JSON: {ex.Message}");
            }

            return schemasList;
        }

        public static string ExtractClassNameFromRef(string refPath)
        {
            if (string.IsNullOrEmpty(refPath)) return string.Empty;
            int lastSlash = refPath.LastIndexOf('/');
            return lastSlash >= 0 ? refPath.Substring(lastSlash + 1) : refPath;
        }
    }
}
