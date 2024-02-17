using System.Text.Json.Serialization;

namespace PatrickGodAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]//This has been used to change the way this class is displayed in Swagger
    public enum RpgClass
    {
        Knight = 1,
        Mage = 2,
        Cleric = 3,
    }
}
