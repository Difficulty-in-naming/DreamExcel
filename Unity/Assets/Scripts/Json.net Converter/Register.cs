using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace NetJson
{
    public class Register
    {
        private static JsonSerializerSettings settings = new()
        {
            Converters = new List<JsonConverter>
            {
                new bool2Converter(),
                new bool2x2Converter(),
                new bool2x3Converter(),
                new bool2x4Converter(),
                new bool3Converter(),
                new bool3x2Converter(),
                new bool3x3Converter(),
                new bool3x4Converter(),
                new bool4Converter(),
                new bool4x2Converter(),
                new bool4x3Converter(),
                new bool4x4Converter(),
                new float2Converter(),
                new float2x2Converter(),
                new float2x3Converter(),
                new float2x4Converter(),
                new float3Converter(),
                new float3x2Converter(),
                new float3x3Converter(),
                new float3x4Converter(),
                new float4Converter(),
                new float4x2Converter(),
                new float4x3Converter(),
                new float4x4Converter(),
                new double2Converter(),
                new double2x2Converter(),
                new double2x3Converter(),
                new double2x4Converter(),
                new double3Converter(),
                new double3x2Converter(),
                new double3x3Converter(),
                new double3x4Converter(),
                new double4Converter(),
                new double4x2Converter(),
                new double4x3Converter(),
                new double4x4Converter(),
                new int2Converter(),
                new int2x2Converter(),
                new int2x3Converter(),
                new int2x4Converter(),
                new int3Converter(),
                new int3x2Converter(),
                new int3x3Converter(),
                new int3x4Converter(),
                new int4Converter(),
                new int4x2Converter(),
                new int4x3Converter(),
                new int4x4Converter(),
                new uint2Converter(),
                new uint2x2Converter(),
                new uint2x3Converter(),
                new uint2x4Converter(),
                new uint3Converter(),
                new uint3x2Converter(),
                new uint3x3Converter(),
                new uint3x4Converter(),
                new uint4Converter(),
                new uint4x2Converter(),
                new uint4x3Converter(),
                new uint4x4Converter(),
                new halfConverter(),
                new half2Converter(),
                new half3Converter(),
                new half4Converter(),
                new quaternionConverter(),
            }
        };
        [RuntimeInitializeOnLoadMethod]
        public static void Do()
        {
            JsonConvert.DefaultSettings = () => settings;
        }
    }
}