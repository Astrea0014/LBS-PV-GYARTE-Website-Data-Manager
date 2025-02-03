using System.Text.Json.Serialization;

namespace DataManager.DbUtil
{
    // 1:1 C# representation of the interfaces defined in 'DbTypes.ts'.
    //
    // This system uses the built-in 'System.Text.Json' JSON seralizer in .NET.
    // Anything serialized here can be parsed in JS, and vice versa.
    // 
    // The way this works is that an object declared 1:1 as its TS counterpart
    // can easily be serialized, deserialized and sent around, as long as the
    // C# data object sets JsonPropertyNameAttribute on all data properties.
    // This is to adhere to C#'s strict naming rules, while being able to serialize
    // JSON to be 1:1 matching with the one expected by the server.

    internal class Collaboration
    {
        [JsonPropertyName("collaboration_id")]
        public int CollaborationId { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("theme")]
        public string Theme { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("poster_ref")]
        public string PosterReference { get; set; }

        [JsonPropertyName("collaborators")]
        public string[] Collaborators { get; set; }
    }

    internal struct GroupMember
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }
    }

    // JsonPolymorphic is used here to properly do derserialization for
    // different derived types of data. JsonDerivedType attributes are
    // located in the type registry.
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "project_type")]
    internal partial interface IProjectData
    {
        // Empty interface;
        // This interface is merely used for its type data.
        // In C#, there is no good way to take 'any' type and set it to a variable.
        // Instead, this empty interface is inherited by a project data class
        // and objects of that class can be assigned to this generic interface type.
    }

    internal struct ProjectGroup
    {
        [JsonPropertyName("project_id")]
        public int ProjectId { get; set; }

        [JsonPropertyName("project_name")]
        public string ProjectName { get; set; }

        [JsonPropertyName("group_name")]
        public string GroupName { get; set; }

        [JsonPropertyName("poster_ref")]
        public string PosterReference { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("project_type")]
        public ProjectType ProjectType { get; set; }

        [JsonPropertyName("project_data")]
        public IProjectData ProjectData { get; set; }

        [JsonPropertyName("group_members")]
        public GroupMember[] GroupMembers { get; set; }
    }

    internal class FullCollaboration : Collaboration
    {
        [JsonPropertyName("project_groups")]
        public ProjectGroup[] ProjectGroups { get; set; }
    }

    // JsonPolymorphic is used here to properly do derserialization for
    // different derived types of data. JsonDerivedType attributes are
    // located in the type registry.
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "component_id")]
    internal partial interface IComponentData
    {
        // Empty interface;
        // This interface is merely used for its type data.
        // In C#, there is no good way to take 'any' type and set it to a variable.
        // Instead, this empty interface is inherited by a component data class
        // and objects of that class can be assigned to this generic interface type.
    }

    internal struct Thesis
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // Cannot be named 'Thesis' because of enclosing type.
        [JsonPropertyName("thesis")]
        public string Thesis_ { get; set; }

        [JsonPropertyName("course")]
        public Course Course { get; set; }

        [JsonPropertyName("author_name")]
        public string? AuthorName { get; set; }

        [JsonPropertyName("author_class")]
        public string? AuthorClass { get; set; }

        [JsonPropertyName("publication_year")]
        public int PublicationYear { get; set; }

        [JsonPropertyName("component_id")]
        public ComponentId ComponentId { get; set; }

        [JsonPropertyName("component_data")]
        public IComponentData ComponentData { get; set; }
    }
}
