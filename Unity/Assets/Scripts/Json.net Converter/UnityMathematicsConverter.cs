using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;

#region bool
public class bool2Converter : JsonConverter<bool2>
{
    public override void WriteJson(JsonWriter writer, bool2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteEndArray();
    }

    public override bool2 ReadJson(JsonReader reader, Type objectType, bool2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool2();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<bool>();
        result.y = array[1].ToObject<bool>();
        return result;
    }
}

public class bool2x2Converter : JsonConverter<bool2x2>
{
    public override void WriteJson(JsonWriter writer, bool2x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override bool2x2 ReadJson(JsonReader reader, Type objectType, bool2x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool2x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<bool2>();
        result.c1 = array[1].ToObject<bool2>();
        return result;
    }
}

public class bool2x3Converter : JsonConverter<bool2x3>
{
    public override void WriteJson(JsonWriter writer, bool2x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override bool2x3 ReadJson(JsonReader reader, Type objectType, bool2x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool2x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<bool2>();
        result.c1 = array[1].ToObject<bool2>();
        result.c2 = array[2].ToObject<bool2>();
        return result;
    }
}

public class bool2x4Converter : JsonConverter<bool2x4>
{
    public override void WriteJson(JsonWriter writer, bool2x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override bool2x4 ReadJson(JsonReader reader, Type objectType, bool2x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool2x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<bool2>();
        result.c1 = array[1].ToObject<bool2>();
        result.c2 = array[2].ToObject<bool2>();
        result.c3 = array[3].ToObject<bool2>();
        return result;
    }
}

public class bool3Converter : JsonConverter<bool3>
{
    public override void WriteJson(JsonWriter writer, bool3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteEndArray();
    }

    public override bool3 ReadJson(JsonReader reader, Type objectType, bool3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool3();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<bool>();
        result.y = array[1].ToObject<bool>();
        result.z = array[2].ToObject<bool>();
        return result;
    }
}

public class bool3x2Converter : JsonConverter<bool3x2>
{
    public override void WriteJson(JsonWriter writer, bool3x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override bool3x2 ReadJson(JsonReader reader, Type objectType, bool3x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool3x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<bool3>();
        result.c1 = array[1].ToObject<bool3>();
        return result;
    }
}

public class bool3x3Converter : JsonConverter<bool3x3>
{
    public override void WriteJson(JsonWriter writer, bool3x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override bool3x3 ReadJson(JsonReader reader, Type objectType, bool3x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool3x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<bool3>();
        result.c1 = array[1].ToObject<bool3>();
        result.c2 = array[2].ToObject<bool3>();
        return result;
    }
}

public class bool3x4Converter : JsonConverter<bool3x4>
{
    public override void WriteJson(JsonWriter writer, bool3x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override bool3x4 ReadJson(JsonReader reader, Type objectType, bool3x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool3x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<bool3>();
        result.c1 = array[1].ToObject<bool3>();
        result.c2 = array[2].ToObject<bool3>();
        result.c3 = array[3].ToObject<bool3>();
        return result;
    }
}

public class bool4Converter : JsonConverter<bool4>
{
    public override void WriteJson(JsonWriter writer, bool4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteValue(value.w);
        writer.WriteEndArray();
    }

    public override bool4 ReadJson(JsonReader reader, Type objectType, bool4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool4();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<bool>();
        result.y = array[1].ToObject<bool>();
        result.z = array[2].ToObject<bool>();
        result.w = array[3].ToObject<bool>();
        return result;
    }
}

public class bool4x2Converter : JsonConverter<bool4x2>
{
    public override void WriteJson(JsonWriter writer, bool4x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override bool4x2 ReadJson(JsonReader reader, Type objectType, bool4x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool4x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<bool4>();
        result.c1 = array[1].ToObject<bool4>();
        return result;
    }
}

public class bool4x3Converter : JsonConverter<bool4x3>
{
    public override void WriteJson(JsonWriter writer, bool4x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override bool4x3 ReadJson(JsonReader reader, Type objectType, bool4x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool4x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<bool4>();
        result.c1 = array[1].ToObject<bool4>();
        result.c2 = array[2].ToObject<bool4>();
        return result;
    }
}

public class bool4x4Converter : JsonConverter<bool4x4>
{
    public override void WriteJson(JsonWriter writer, bool4x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override bool4x4 ReadJson(JsonReader reader, Type objectType, bool4x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new bool4x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<bool4>();
        result.c1 = array[1].ToObject<bool4>();
        result.c2 = array[2].ToObject<bool4>();
        result.c3 = array[3].ToObject<bool4>();
        return result;
    }
}
#endregion

#region double
public class double2Converter : JsonConverter<double2>
{
    public override void WriteJson(JsonWriter writer, double2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteEndArray();
    }

    public override double2 ReadJson(JsonReader reader, Type objectType, double2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double2();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<double>();
        result.y = array[1].ToObject<double>();
        return result;
    }
}

public class double2x2Converter : JsonConverter<double2x2>
{
    public override void WriteJson(JsonWriter writer, double2x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override double2x2 ReadJson(JsonReader reader, Type objectType, double2x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double2x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<double2>();
        result.c1 = array[1].ToObject<double2>();
        return result;
    }
}

public class double2x3Converter : JsonConverter<double2x3>
{
    public override void WriteJson(JsonWriter writer, double2x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override double2x3 ReadJson(JsonReader reader, Type objectType, double2x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double2x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<double2>();
        result.c1 = array[1].ToObject<double2>();
        result.c2 = array[2].ToObject<double2>();
        return result;
    }
}

public class double2x4Converter : JsonConverter<double2x4>
{
    public override void WriteJson(JsonWriter writer, double2x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override double2x4 ReadJson(JsonReader reader, Type objectType, double2x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double2x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<double2>();
        result.c1 = array[1].ToObject<double2>();
        result.c2 = array[2].ToObject<double2>();
        result.c3 = array[3].ToObject<double2>();
        return result;
    }
}

public class double3Converter : JsonConverter<double3>
{
    public override void WriteJson(JsonWriter writer, double3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteEndArray();
    }

    public override double3 ReadJson(JsonReader reader, Type objectType, double3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double3();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<double>();
        result.y = array[1].ToObject<double>();
        result.z = array[2].ToObject<double>();
        return result;
    }
}

public class double3x2Converter : JsonConverter<double3x2>
{
    public override void WriteJson(JsonWriter writer, double3x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override double3x2 ReadJson(JsonReader reader, Type objectType, double3x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double3x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<double3>();
        result.c1 = array[1].ToObject<double3>();
        return result;
    }
}

public class double3x3Converter : JsonConverter<double3x3>
{
    public override void WriteJson(JsonWriter writer, double3x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override double3x3 ReadJson(JsonReader reader, Type objectType, double3x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double3x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<double3>();
        result.c1 = array[1].ToObject<double3>();
        result.c2 = array[2].ToObject<double3>();
        return result;
    }
}

public class double3x4Converter : JsonConverter<double3x4>
{
    public override void WriteJson(JsonWriter writer, double3x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override double3x4 ReadJson(JsonReader reader, Type objectType, double3x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double3x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<double3>();
        result.c1 = array[1].ToObject<double3>();
        result.c2 = array[2].ToObject<double3>();
        result.c3 = array[3].ToObject<double3>();
        return result;
    }
}

public class double4Converter : JsonConverter<double4>
{
    public override void WriteJson(JsonWriter writer, double4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteValue(value.w);
        writer.WriteEndArray();
    }

    public override double4 ReadJson(JsonReader reader, Type objectType, double4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double4();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<double>();
        result.y = array[1].ToObject<double>();
        result.z = array[2].ToObject<double>();
        result.w = array[3].ToObject<double>();
        return result;
    }
}

public class double4x2Converter : JsonConverter<double4x2>
{
    public override void WriteJson(JsonWriter writer, double4x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override double4x2 ReadJson(JsonReader reader, Type objectType, double4x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double4x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<double4>();
        result.c1 = array[1].ToObject<double4>();
        return result;
    }
}

public class double4x3Converter : JsonConverter<double4x3>
{
    public override void WriteJson(JsonWriter writer, double4x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override double4x3 ReadJson(JsonReader reader, Type objectType, double4x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double4x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<double4>();
        result.c1 = array[1].ToObject<double4>();
        result.c2 = array[2].ToObject<double4>();
        return result;
    }
}

public class double4x4Converter : JsonConverter<double4x4>
{
    public override void WriteJson(JsonWriter writer, double4x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override double4x4 ReadJson(JsonReader reader, Type objectType, double4x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new double4x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<double4>();
        result.c1 = array[1].ToObject<double4>();
        result.c2 = array[2].ToObject<double4>();
        result.c3 = array[3].ToObject<double4>();
        return result;
    }
}
#endregion

#region float
public class float2Converter : JsonConverter<float2>
{
    public override void WriteJson(JsonWriter writer, float2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteEndArray();
    }

    public override float2 ReadJson(JsonReader reader, Type objectType, float2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float2();
        /*var array = JArray.Load(reader);
        result.x = array[0].ToObject<float>();
        result.y = array[1].ToObject<float>();*/
        var obj = JToken.Load(reader).Value<string>();
        var split = obj.Split(",", StringSplitOptions.RemoveEmptyEntries);
        result.x = float.Parse(split[0]);
        result.y = float.Parse(split[1]);
        return result;
    }
}

public class float2x2Converter : JsonConverter<float2x2>
{
    public override void WriteJson(JsonWriter writer, float2x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override float2x2 ReadJson(JsonReader reader, Type objectType, float2x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float2x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<float2>();
        result.c1 = array[1].ToObject<float2>();
        return result;
    }
}

public class float2x3Converter : JsonConverter<float2x3>
{
    public override void WriteJson(JsonWriter writer, float2x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override float2x3 ReadJson(JsonReader reader, Type objectType, float2x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float2x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<float2>();
        result.c1 = array[1].ToObject<float2>();
        result.c2 = array[2].ToObject<float2>();
        return result;
    }
}

public class float2x4Converter : JsonConverter<float2x4>
{
    public override void WriteJson(JsonWriter writer, float2x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override float2x4 ReadJson(JsonReader reader, Type objectType, float2x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float2x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<float2>();
        result.c1 = array[1].ToObject<float2>();
        result.c2 = array[2].ToObject<float2>();
        result.c3 = array[3].ToObject<float2>();
        return result;
    }
}

public class float3Converter : JsonConverter<float3>
{
    public override void WriteJson(JsonWriter writer, float3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteEndArray();
    }

    public override float3 ReadJson(JsonReader reader, Type objectType, float3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float3();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<float>();
        result.y = array[1].ToObject<float>();
        result.z = array[2].ToObject<float>();
        return result;
    }
}

public class float3x2Converter : JsonConverter<float3x2>
{
    public override void WriteJson(JsonWriter writer, float3x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override float3x2 ReadJson(JsonReader reader, Type objectType, float3x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float3x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<float3>();
        result.c1 = array[1].ToObject<float3>();
        return result;
    }
}

public class float3x3Converter : JsonConverter<float3x3>
{
    public override void WriteJson(JsonWriter writer, float3x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override float3x3 ReadJson(JsonReader reader, Type objectType, float3x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float3x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<float3>();
        result.c1 = array[1].ToObject<float3>();
        result.c2 = array[2].ToObject<float3>();
        return result;
    }
}

public class float3x4Converter : JsonConverter<float3x4>
{
    public override void WriteJson(JsonWriter writer, float3x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override float3x4 ReadJson(JsonReader reader, Type objectType, float3x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float3x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<float3>();
        result.c1 = array[1].ToObject<float3>();
        result.c2 = array[2].ToObject<float3>();
        result.c3 = array[3].ToObject<float3>();
        return result;
    }
}

public class float4Converter : JsonConverter<float4>
{
    public override void WriteJson(JsonWriter writer, float4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteValue(value.w);
        writer.WriteEndArray();
    }

    public override float4 ReadJson(JsonReader reader, Type objectType, float4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float4();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<float>();
        result.y = array[1].ToObject<float>();
        result.z = array[2].ToObject<float>();
        result.w = array[3].ToObject<float>();
        return result;
    }
}

public class float4x2Converter : JsonConverter<float4x2>
{
    public override void WriteJson(JsonWriter writer, float4x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override float4x2 ReadJson(JsonReader reader, Type objectType, float4x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float4x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<float4>();
        result.c1 = array[1].ToObject<float4>();
        return result;
    }
}

public class float4x3Converter : JsonConverter<float4x3>
{
    public override void WriteJson(JsonWriter writer, float4x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override float4x3 ReadJson(JsonReader reader, Type objectType, float4x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float4x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<float4>();
        result.c1 = array[1].ToObject<float4>();
        result.c2 = array[2].ToObject<float4>();
        return result;
    }
}

public class float4x4Converter : JsonConverter<float4x4>
{
    public override void WriteJson(JsonWriter writer, float4x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override float4x4 ReadJson(JsonReader reader, Type objectType, float4x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new float4x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<float4>();
        result.c1 = array[1].ToObject<float4>();
        result.c2 = array[2].ToObject<float4>();
        result.c3 = array[3].ToObject<float4>();
        return result;
    }
}
#endregion

#region int
public class int2Converter : JsonConverter<int2>
{
    public override void WriteJson(JsonWriter writer, int2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteEndArray();
    }

    public override int2 ReadJson(JsonReader reader, Type objectType, int2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int2();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<int>();
        result.y = array[1].ToObject<int>();
        return result;
    }
}

public class int2x2Converter : JsonConverter<int2x2>
{
    public override void WriteJson(JsonWriter writer, int2x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override int2x2 ReadJson(JsonReader reader, Type objectType, int2x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int2x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<int2>();
        result.c1 = array[1].ToObject<int2>();
        return result;
    }
}

public class int2x3Converter : JsonConverter<int2x3>
{
    public override void WriteJson(JsonWriter writer, int2x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override int2x3 ReadJson(JsonReader reader, Type objectType, int2x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int2x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<int2>();
        result.c1 = array[1].ToObject<int2>();
        result.c2 = array[2].ToObject<int2>();
        return result;
    }
}

public class int2x4Converter : JsonConverter<int2x4>
{
    public override void WriteJson(JsonWriter writer, int2x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override int2x4 ReadJson(JsonReader reader, Type objectType, int2x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int2x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<int2>();
        result.c1 = array[1].ToObject<int2>();
        result.c2 = array[2].ToObject<int2>();
        result.c3 = array[3].ToObject<int2>();
        return result;
    }
}

public class int3Converter : JsonConverter<int3>
{
    public override void WriteJson(JsonWriter writer, int3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteEndArray();
    }

    public override int3 ReadJson(JsonReader reader, Type objectType, int3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int3();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<int>();
        result.y = array[1].ToObject<int>();
        result.z = array[2].ToObject<int>();
        return result;
    }
}

public class int3x2Converter : JsonConverter<int3x2>
{
    public override void WriteJson(JsonWriter writer, int3x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override int3x2 ReadJson(JsonReader reader, Type objectType, int3x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int3x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<int3>();
        result.c1 = array[1].ToObject<int3>();
        return result;
    }
}

public class int3x3Converter : JsonConverter<int3x3>
{
    public override void WriteJson(JsonWriter writer, int3x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override int3x3 ReadJson(JsonReader reader, Type objectType, int3x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int3x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<int3>();
        result.c1 = array[1].ToObject<int3>();
        result.c2 = array[2].ToObject<int3>();
        return result;
    }
}

public class int3x4Converter : JsonConverter<int3x4>
{
    public override void WriteJson(JsonWriter writer, int3x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override int3x4 ReadJson(JsonReader reader, Type objectType, int3x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int3x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<int3>();
        result.c1 = array[1].ToObject<int3>();
        result.c2 = array[2].ToObject<int3>();
        result.c3 = array[3].ToObject<int3>();
        return result;
    }
}

public class int4Converter : JsonConverter<int4>
{
    public override void WriteJson(JsonWriter writer, int4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteValue(value.w);
        writer.WriteEndArray();
    }

    public override int4 ReadJson(JsonReader reader, Type objectType, int4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int4();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<int>();
        result.y = array[1].ToObject<int>();
        result.z = array[2].ToObject<int>();
        result.w = array[3].ToObject<int>();
        return result;
    }
}

public class int4x2Converter : JsonConverter<int4x2>
{
    public override void WriteJson(JsonWriter writer, int4x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override int4x2 ReadJson(JsonReader reader, Type objectType, int4x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int4x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<int4>();
        result.c1 = array[1].ToObject<int4>();
        return result;
    }
}

public class int4x3Converter : JsonConverter<int4x3>
{
    public override void WriteJson(JsonWriter writer, int4x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override int4x3 ReadJson(JsonReader reader, Type objectType, int4x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int4x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<int4>();
        result.c1 = array[1].ToObject<int4>();
        result.c2 = array[2].ToObject<int4>();
        return result;
    }
}

public class int4x4Converter : JsonConverter<int4x4>
{
    public override void WriteJson(JsonWriter writer, int4x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override int4x4 ReadJson(JsonReader reader, Type objectType, int4x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new int4x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<int4>();
        result.c1 = array[1].ToObject<int4>();
        result.c2 = array[2].ToObject<int4>();
        result.c3 = array[3].ToObject<int4>();
        return result;
    }
}
#endregion

#region uint
public class uint2Converter : JsonConverter<uint2>
{
    public override void WriteJson(JsonWriter writer, uint2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteEndArray();
    }

    public override uint2 ReadJson(JsonReader reader, Type objectType, uint2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint2();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<uint>();
        result.y = array[1].ToObject<uint>();
        return result;
    }
}

public class uint2x2Converter : JsonConverter<uint2x2>
{
    public override void WriteJson(JsonWriter writer, uint2x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override uint2x2 ReadJson(JsonReader reader, Type objectType, uint2x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint2x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<uint2>();
        result.c1 = array[1].ToObject<uint2>();
        return result;
    }
}

public class uint2x3Converter : JsonConverter<uint2x3>
{
    public override void WriteJson(JsonWriter writer, uint2x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override uint2x3 ReadJson(JsonReader reader, Type objectType, uint2x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint2x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<uint2>();
        result.c1 = array[1].ToObject<uint2>();
        result.c2 = array[2].ToObject<uint2>();
        return result;
    }
}

public class uint2x4Converter : JsonConverter<uint2x4>
{
    public override void WriteJson(JsonWriter writer, uint2x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override uint2x4 ReadJson(JsonReader reader, Type objectType, uint2x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint2x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<uint2>();
        result.c1 = array[1].ToObject<uint2>();
        result.c2 = array[2].ToObject<uint2>();
        result.c3 = array[3].ToObject<uint2>();
        return result;
    }
}

public class uint3Converter : JsonConverter<uint3>
{
    public override void WriteJson(JsonWriter writer, uint3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteEndArray();
    }

    public override uint3 ReadJson(JsonReader reader, Type objectType, uint3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint3();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<uint>();
        result.y = array[1].ToObject<uint>();
        result.z = array[2].ToObject<uint>();
        return result;
    }
}

public class uint3x2Converter : JsonConverter<uint3x2>
{
    public override void WriteJson(JsonWriter writer, uint3x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override uint3x2 ReadJson(JsonReader reader, Type objectType, uint3x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint3x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<uint3>();
        result.c1 = array[1].ToObject<uint3>();
        return result;
    }
}

public class uint3x3Converter : JsonConverter<uint3x3>
{
    public override void WriteJson(JsonWriter writer, uint3x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override uint3x3 ReadJson(JsonReader reader, Type objectType, uint3x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint3x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<uint3>();
        result.c1 = array[1].ToObject<uint3>();
        result.c2 = array[2].ToObject<uint3>();
        return result;
    }
}

public class uint3x4Converter : JsonConverter<uint3x4>
{
    public override void WriteJson(JsonWriter writer, uint3x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override uint3x4 ReadJson(JsonReader reader, Type objectType, uint3x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint3x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<uint3>();
        result.c1 = array[1].ToObject<uint3>();
        result.c2 = array[2].ToObject<uint3>();
        result.c3 = array[3].ToObject<uint3>();
        return result;
    }
}

public class uint4Converter : JsonConverter<uint4>
{
    public override void WriteJson(JsonWriter writer, uint4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteValue(value.w);
        writer.WriteEndArray();
    }

    public override uint4 ReadJson(JsonReader reader, Type objectType, uint4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint4();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<uint>();
        result.y = array[1].ToObject<uint>();
        result.z = array[2].ToObject<uint>();
        result.w = array[3].ToObject<uint>();
        return result;
    }
}

public class uint4x2Converter : JsonConverter<uint4x2>
{
    public override void WriteJson(JsonWriter writer, uint4x2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteEndArray();
    }

    public override uint4x2 ReadJson(JsonReader reader, Type objectType, uint4x2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint4x2();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<uint4>();
        result.c1 = array[1].ToObject<uint4>();
        return result;
    }
}

public class uint4x3Converter : JsonConverter<uint4x3>
{
    public override void WriteJson(JsonWriter writer, uint4x3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteEndArray();
    }

    public override uint4x3 ReadJson(JsonReader reader, Type objectType, uint4x3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint4x3();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<uint4>();
        result.c1 = array[1].ToObject<uint4>();
        result.c2 = array[2].ToObject<uint4>();
        return result;
    }
}

public class uint4x4Converter : JsonConverter<uint4x4>
{
    public override void WriteJson(JsonWriter writer, uint4x4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.c0);
        writer.WriteValue(value.c1);
        writer.WriteValue(value.c2);
        writer.WriteValue(value.c3);
        writer.WriteEndArray();
    }

    public override uint4x4 ReadJson(JsonReader reader, Type objectType, uint4x4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new uint4x4();
        var array = JArray.Load(reader);
        result.c0 = array[0].ToObject<uint4>();
        result.c1 = array[1].ToObject<uint4>();
        result.c2 = array[2].ToObject<uint4>();
        result.c3 = array[3].ToObject<uint4>();
        return result;
    }
}
#endregion

#region half
public class halfConverter : JsonConverter<half>
{
    public override void WriteJson(JsonWriter writer, half value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.value);
        writer.WriteEndArray();
    }

    public override half ReadJson(JsonReader reader, Type objectType, half existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new half();
        var array = JArray.Load(reader);
        result.value = array[0].ToObject<ushort>();
        return result;
    }
}

public class half2Converter : JsonConverter<half2>
{
    public override void WriteJson(JsonWriter writer, half2 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteEndArray();
    }

    public override half2 ReadJson(JsonReader reader, Type objectType, half2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new half2();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<half>();
        result.y = array[1].ToObject<half>();
        return result;
    }
}

public class half3Converter : JsonConverter<half3>
{
    public override void WriteJson(JsonWriter writer, half3 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteEndArray();
    }

    public override half3 ReadJson(JsonReader reader, Type objectType, half3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new half3();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<half>();
        result.y = array[1].ToObject<half>();
        result.z = array[2].ToObject<half>();
        return result;
    }
}

public class half4Converter : JsonConverter<half4>
{
    public override void WriteJson(JsonWriter writer, half4 value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteValue(value.z);
        writer.WriteValue(value.w);
        writer.WriteEndArray();
    }

    public override half4 ReadJson(JsonReader reader, Type objectType, half4 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new half4();
        var array = JArray.Load(reader);
        result.x = array[0].ToObject<half>();
        result.y = array[1].ToObject<half>();
        result.z = array[1].ToObject<half>();
        result.w = array[1].ToObject<half>();
        return result;
    }
}
#endregion

public class quaternionConverter : JsonConverter<quaternion>
{
    public override void WriteJson(JsonWriter writer, quaternion value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.value);
        writer.WriteEndArray();
    }

    public override quaternion ReadJson(JsonReader reader, Type objectType, quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new quaternion();
        var array = JArray.Load(reader);
        result.value = array[0].ToObject<float4>();
        return result;
    }
}