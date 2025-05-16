namespace DataManager.Core.JsonContract
{
    /// <summary>
    /// The datatype of the JSON-field.
    /// </summary>
    /// <remarks>
    /// For list-type fields, specify <see cref="List"/> and set <see cref="FieldRequirement.FieldSubType"/> to any other type.
    /// </remarks>
    enum FieldType
    {
        /// <summary>
        /// A JSON list.
        /// </summary>
        List,

        /// <summary>
        /// An integer value.
        /// </summary>
        Number,

        /// <summary>
        /// A text value.
        /// </summary>
        String
    }

    /// <summary>
    /// Represents a requirement set by a contract for a JSON-serialized field.
    /// </summary>
    readonly struct FieldRequirement()
    {

        /// <summary>
        /// The name of the JSON-field. This property is required.
        /// </summary>
        public required string FieldName { get; init; }

        /// <summary>
        /// The type of the JSON-field. This property is required.
        /// </summary>
        public required FieldType FieldType { get; init; }

        /// <summary>
        /// The sub-type of the JSON-field. This should only be used if <see cref="FieldType"/> is set to <see cref="FieldType.List"/>.
        /// </summary>
        public FieldType? FieldSubType { get; init; } = null;

        /// <summary>
        /// Indicates whether this field is required to always be present or not.
        /// </summary>
        /// <remarks>
        /// The default value is <see langword="true"/>.
        /// </remarks>
        public bool IsRequired { get; init; } = true;

        /// <summary>
        /// Sets which group of optional values this <see cref="FieldRequirement"/> belongs to.
        /// </summary>
        /// <remarks>
        /// An optional group implies that out of all fields sharing the same optional group,
        /// only one of all specified fields or field-groups is required to be present
        /// in the JSON-object at once.
        /// </remarks>
        public int? OptionalGroup { get; init; } = null;
    }

    /// <summary>
    /// Represents a group of <see cref="FieldRequirement"/>s that can be set to
    /// an optional group.
    /// </summary>
    /// <remarks>
    /// <see cref="FieldRequirementGroup"/>s are only used when groups of fields need
    /// to be set as optionals. Example: Either the master-field or the username- AND
    /// password-fields need to be present.
    /// </remarks>
    readonly struct FieldRequirementGroup
    {
        /// <summary>
        /// The <see cref="FieldRequirement"/>s to include in the
        /// <see cref="FieldRequirementGroup"/>.
        /// </summary>
        public required FieldRequirement[] FieldRequirements { get; init; }

        /// <summary>
        /// Sets which group of optional values this <see cref="FieldRequirementGroup"/>
        /// belongs to.
        /// </summary>
        /// <inheritdoc cref="FieldRequirement.OptionalGroup" path="/remarks"/>
        public int? OptionalGroup { get; init; }
    }
}
