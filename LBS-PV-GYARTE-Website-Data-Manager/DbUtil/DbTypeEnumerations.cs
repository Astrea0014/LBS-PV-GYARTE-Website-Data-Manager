using System.Text.Json.Serialization;

namespace DataManager.DbUtil
{
    // The server uses some distinct static values for certain parameters.
    // This is primarily used to identify things like kinds of data and
    // layout component. In C#, these string literals are defined as
    // enumerated values instead. When a new layout is added and a new
    // kind of data model is added to the system, these enumerations must
    // include the new data and layout identifiers.

    // This enumeration contains the program weeks' project types.
    // This enumeration is used to identify a few things:
    // 1. It is used to identify which layout component to use
    //    for the project that is being presented on the page.
    // 2. It is used as a resource locator for the database,
    //    and essentially tells the server what method to use
    //    to obtain the proprietary data.
    // 3. It is used to identify what interface the forementioned
    //    data uses and what data there is to expect.
    // 4. It is used to identify what data needs to be submitted
    //    in order to properly use the defined layout.
    [JsonConverter(typeof(JsonStringEnumConverter<ProjectType>))]
    internal enum ProjectType
    {
        SUSG01
    }

    // This enumeration contains all of the different programs that
    // can be used as a filter for gymnasial thesis works.
    [JsonConverter(typeof(JsonStringEnumConverter<Course>))]
    internal enum Course
    {
        SY, // App- och webbutveckling
        GD, // Grafisk design
        AI, // AI & utveckling
        SG, // Spelgrafik
        SU, // Spelutveckling
        FF, // Foto och film
        MB  // Media-beteende
    }

    // This enumeration contains the gymnasial theses' component identifiers.
    // This enumeration works in the exact same way the ProjectType enum does,
    // but for theses' instead.
    [JsonConverter(typeof(JsonStringEnumConverter<ComponentId>))]
    internal enum ComponentId
    {
        SY0,    // Component för SY/AW. Visar en href i en iframe.
        SU0,    // Component för SU. Visar en video av tex ett spel.
        ES1,    // Component för estet 1. Visar foton och en kort beskrivning.
        ES2     // Component för estet 2. Visar färre foton och en video, samt en kort beskrivning.
    }
}
