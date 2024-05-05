using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor.Rendering;
using UnityEngine;

public static class JsonRecursive {
    public static string ToJson(object obj, List<string> names, int maxDepth, bool pretty){
        string json = JsonObject(obj, names, maxDepth).TrimEnd(',');
        if (pretty){
            return FormattedJson(json);
        } else {
            return json;
        }
    }

    private static string JsonObject(object obj, List<string> names, int maxDepth){
        string json = "{";
        if (maxDepth > 0){
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields) {
                if (!names.Contains(field.Name)){
                    continue;
                }
                json += "\"" + field.Name + "\":";
                var value = field.GetValue(obj);
                json += CheckField(field, value, names, maxDepth) + ",";
            }
        }
        json = json.TrimEnd(',') + "}";
        return json;
    }

    private static string JsonList(FieldInfo field, object obj, List<string> names, int maxDepth){
        string json = "[";
        if (maxDepth > 0){
            Type type = obj.GetType().GetGenericArguments()[0];
            if (type == typeof(bool)){
                List<bool> list = (obj as IEnumerable<bool>).Cast<bool>().ToList();
                for (int i = 0; i < list.Count; i++){
                    json += CheckField(field, list[i], names, maxDepth) + ",";
                }
            } else if (type == typeof(int)){
                List<int> list = (obj as IEnumerable<int>).Cast<int>().ToList();
                for (int i = 0; i < list.Count; i++){
                    json += CheckField(field, list[i], names, maxDepth) + ",";
                }
            } else if (type == typeof(float)){
                List<float> list = (obj as IEnumerable<float>).Cast<float>().ToList();
                for (int i = 0; i < list.Count; i++){
                    json += CheckField(field, list[i], names, maxDepth) + ",";
                }
            } else if (type == typeof(double)){
                List<double> list = (obj as IEnumerable<double>).Cast<double>().ToList();
                for (int i = 0; i < list.Count; i++){
                    json += CheckField(field, list[i], names, maxDepth) + ",";
                }
            } else if (type == typeof(char)){
                List<char> list = (obj as IEnumerable<char>).Cast<char>().ToList();
                for (int i = 0; i < list.Count; i++){
                    json += CheckField(field, list[i], names, maxDepth) + ",";
                }
            } else if (type == typeof(string)){
                List<string> list = (obj as IEnumerable<string>).Cast<string>().ToList();
                for (int i = 0; i < list.Count; i++){
                    json += CheckField(field, list[i], names, maxDepth) + ",";
                }
            } else {
                List<object> list = (obj as IEnumerable<object>).Cast<object>().ToList();
                for (int i = 0; i < list.Count; i++){
                    json += CheckField(field, list[i], names, maxDepth) + ",";
                }
            }
        }
        return json.TrimEnd(',') + "]";
    }

    private static string JsonArray(FieldInfo field, object obj, List<string> names, int maxDepth){
        string json = "[";
        if (maxDepth > 0){
            var array = obj as Array;
            for (int i = 0; i < array.Length; i++) {
                json += CheckField(field, array.GetValue(i), names, maxDepth) + ",";
            }
        }
        return json.TrimEnd(',') + "]";
    }

    private static string CheckField(FieldInfo field, object value, List<string> names, int maxDepth){
        if (value == null){
            return "{}";
        } else if (value.GetType() == typeof(string)){
            return "\"" + value + "\"";
        } else if (value.GetType() == typeof(Vector2)){
            Vector2 vector = (Vector2)value;
            return "{\"x\":" + vector.x + ",\"y\":" + vector.y + "}";
        } else if (value.GetType() == typeof(Vector3)){
            Vector3 vector = (Vector3)value;
            return "{\"x\":" + vector.x + ",\"y\":" + vector.y + ",\"z\":" + vector.z + "}";
        } else if (value.GetType() == typeof(Vector4)){
            Vector4 vector = (Vector4)value;
            return "{\"x\":" + vector.x + ",\"y\":" + vector.y + ",\"z\":" + vector.z + ",\"w\":" + vector.w + "}";
        } else if (value.GetType() == typeof(Vector2Int)){
            Vector2Int vector = (Vector2Int)value;
            return "{\"x\":" + vector.x + ",\"y\":" + vector.y + "}";
        } else if (value.GetType() == typeof(Vector3Int)){
            Vector3Int vector = (Vector3Int)value;
            return "{\"x\":" + vector.x + ",\"y\":" + vector.y + ",\"z\":" + vector.z + "}";
        } else if (value.GetType() == typeof(int) || value.GetType() == typeof(float) || value.GetType() == typeof(double) || value.GetType() == typeof(bool)){
            return value.ToString().ToLower();
        } else if (value.GetType().IsClass && !value.GetType().ToString().Contains("System.Collections.Generic.List")){
            return JsonObject(value, names, maxDepth - 1);
        } else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>)){
            return JsonList(field, value, names, maxDepth - 1);
        } else if(field.FieldType.IsArray){
            return JsonArray(field, value, names, maxDepth - 1);
        } else {
            return "";
        }
    }

    private static string FormattedJson(string json) {
        if (string.IsNullOrEmpty(json))
            return "";

        StringBuilder formattedJson = new StringBuilder();
        int indentLevel = 0;
        bool inString = false;
        char currentChar;

        for (int i = 0; i < json.Length; i++)
        {
            currentChar = json[i];

            if (inString)
            {
                formattedJson.Append(currentChar);
                if (currentChar == '"' && json[i - 1] != '\\')
                    inString = false;
                continue;
            }

            switch (currentChar)
            {
                case '{':
                case '[':
                    formattedJson.Append(currentChar);
                    formattedJson.AppendLine();
                    indentLevel++;
                    formattedJson.Append(Indent(indentLevel));
                    break;
                case '}':
                case ']':
                    formattedJson.AppendLine();
                    indentLevel--;
                    formattedJson.Append(Indent(indentLevel));
                    formattedJson.Append(currentChar);
                    break;
                case ',':
                    formattedJson.Append(currentChar);
                    formattedJson.AppendLine();
                    formattedJson.Append(Indent(indentLevel));
                    break;
                case ':':
                    formattedJson.Append(currentChar);
                    formattedJson.Append(' ');
                    break;
                case '"':
                    formattedJson.Append(currentChar);
                    inString = true;
                    break;
                default:
                    if (!char.IsWhiteSpace(currentChar))
                        formattedJson.Append(currentChar);
                    break;
            }
        }

        return formattedJson.ToString();
    }

    private static string Indent(int indentLevel)
    {
        return new string(' ', indentLevel * 4);
    }
}
