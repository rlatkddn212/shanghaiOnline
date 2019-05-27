using UnityEngine;
using System.Collections;
using LitJson;
using System;

public class JSONObject
{

    private string mJSONString;
    private Hashtable mData = new Hashtable();
    private bool subObject = false;

    public bool isSubObject
    {
        get
        {
            return subObject;
        }
    }

    public JSONObject()
    {
    }

    public JSONObject(string jsonString)
    {
        Parse(jsonString);
    }

    public JSONObject(Hashtable data)
    {
        mData = data;
        subObject = true;
    }

    public void Parse(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            mJSONString = null;
            mData.Clear();
            return;
        }

        mJSONString = jsonString;
        mData.Clear();

        try
        {
            JsonReader reader = new JsonReader(mJSONString);
            Parse(reader, mData);
        }
        catch (ArgumentNullException e)
        {
            Debug.LogError(e);
        }
        catch (JsonException e)
        {
            Debug.LogError(e);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void Parse(JsonReader reader, Hashtable data)
    {
        string propertyName = "";
        bool isArrayStarted = false;
        ArrayList array = new ArrayList();

        while (reader.Read())
        {
            switch (reader.Token)
            {
                case JsonToken.ObjectStart:
                    if (string.IsNullOrEmpty(propertyName))
                    {

                    }
                    else
                    {
                        Hashtable collection = new Hashtable();
                        Parse(reader, collection);

                        JSONObject jsonObject = new JSONObject(collection);

                        if (isArrayStarted)
                        {
                            array.Add(jsonObject);
                        }
                        else
                        {
                            data.Add(propertyName, jsonObject);
                            propertyName = "";
                        }
                    }
                    break;

                case JsonToken.ObjectEnd:
                    //propertyName = "";
                    return;

                case JsonToken.PropertyName:
                    propertyName = reader.Value as string;
                    break;

                case JsonToken.ArrayStart:
                    isArrayStarted = true;
                    break;

                case JsonToken.ArrayEnd:
                    isArrayStarted = false;
                    if (array.Count > 0)
                    {
                        data.Add(propertyName, array.ToArray(array[0].GetType()));
                    }
                    array.Clear();
                    propertyName = "";
                    break;

                case JsonToken.Boolean:
                case JsonToken.Double:
                case JsonToken.Int:
                case JsonToken.Long:
                case JsonToken.String:
                    if (isArrayStarted)
                    {
                        array.Add(reader.Value);
                    }
                    else
                    {
                        data.Add(propertyName, reader.Value);
                        propertyName = "";
                    }
                    break;

                case JsonToken.Null:
                    if (isArrayStarted)
                    {
                        array.Add(null);
                    }
                    else
                    {
                        data.Add(propertyName, null);
                        propertyName = "";
                    }
                    break;

                default:
                    Debug.Log("Unknown JsonToken : " + reader.Token);
                    Debug.Break();
                    propertyName = "";
                    break;
            }
        }
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(mJSONString))
        {
            return GetType().ToString();
        }

        return mJSONString;
    }

    public object Get(string name)
    {
        if (!mData.Contains(name)) return null;

        return mData[name];
    }

    private T GetValue<T>(string name)
    {
        object data = Get(name);

        if (data == null)
        {
            Debug.LogWarning("[WARN] \"" + name + "\" object is null.");
            return default(T);
        }

        if (data is T) return (T)data;

        Debug.LogWarning("[WARN] \"" + name + "\" object is not " + typeof(T) + " type.\nthis object type is " + data.GetType());

        return default(T);
    }

    public bool GetBoolean(string name)
    {
        return GetValue<bool>(name);
    }

    public double GetDouble(string name)
    {
        return GetValue<double>(name);
    }

    public int GetInt(string name)
    {
        return GetValue<int>(name);
    }

    public long GetLong(string name)
    {
        //return GetValue<long>(name);
        object data = Get(name);

        if (!(data is int) && !(data is System.Int64))
        {
            Debug.LogWarning("[WARN] \"" + name + "\" object is not " + typeof(long) + " type.\nthis object type is " + data.GetType());
            return default(System.Int64);
        }

        return System.Convert.ToInt64(data);
    }

    public string GetString(string name)
    {
        return GetValue<string>(name);
    }

    public JSONObject GetJSONObject(string name)
    {
        return GetValue<JSONObject>(name);
    }

    public JSONObject[] GetJSONArray(string name)
    {
        return GetValue<JSONObject[]>(name);
    }

    public bool[] GetBooleanArray(string name)
    {
        return GetValue<bool[]>(name);
    }

    public double[] GetDoubleArray(string name)
    {
        return GetValue<double[]>(name);
    }

    public int[] GetIntArray(string name)
    {
        return GetValue<int[]>(name);
    }

    public long[] GetLongArray(string name)
    {
        return GetValue<long[]>(name);
    }

    public string[] GetStringArray(string name)
    {
        return GetValue<string[]>(name);
    }
}
