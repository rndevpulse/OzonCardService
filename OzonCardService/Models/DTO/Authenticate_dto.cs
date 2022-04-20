using System.Text.Json.Serialization;

namespace OzonCardService.Models.DTO
{
    public class Authenticate_dto
    {
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string[] Rules { get; set; }

    }
}
