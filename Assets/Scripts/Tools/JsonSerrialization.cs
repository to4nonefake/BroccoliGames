using UnityEngine;

public class JsonSerrialization<T> where T : new()
{
    public bool FromJason(string path, out T dataOut) {
        string json;

        try {
            json = Resources.Load<TextAsset>(path).ToString();
        } catch {
            dataOut = new T();
            return false;
        }
        
        if (!Desserialize(json, out dataOut)) {
            return false;
        }

        return true;
    }

    public bool FromJason(TextAsset jsonTa, out T dataOut) {
        string json = jsonTa.ToString();

        if (!Desserialize(json, out dataOut)) {
            return false;
        }

        return true;
    }

    private bool Desserialize(string json, out T data) {
        try {
            data = JsonUtility.FromJson<T>(json);
        } catch {
            data = new T();
            return false;
        }

        return true;
    }
}
