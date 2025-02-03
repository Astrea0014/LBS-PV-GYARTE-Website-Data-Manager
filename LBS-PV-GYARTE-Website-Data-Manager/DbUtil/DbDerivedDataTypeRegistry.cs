using System.Text.Json.Serialization;

using DataManager.DbUtil.PvProjectData;
using DataManager.DbUtil.GyProjectData;

namespace DataManager.DbUtil
{
    // Here custom derived data types are registered.
    // This is necessary to make the deserializer understand which
    // derived datatype is which.
    // All data types can be registered in the same way. See the example below.

    // Derived project data types go here...
    [JsonDerivedType(typeof(SUSG01), nameof(SUSG01))]
//  [JsonDerivedType(typeof(MyStruct), nameof(MyStruct))] ...
    internal partial interface IProjectData { }

    // Derived component data types go here...
    [JsonDerivedType(typeof(SU0), nameof(SU0))]
    [JsonDerivedType(typeof(SY0), nameof(SY0))]
    [JsonDerivedType(typeof(ES1), nameof(ES1))]
    [JsonDerivedType(typeof(ES2), nameof(ES2))]
    internal partial interface IComponentData { }
}
