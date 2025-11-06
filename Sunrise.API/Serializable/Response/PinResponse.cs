using System.Text.Json.Serialization;
using Sunrise.Shared.Database.Models;
using Sunrise.Shared.Enums.Beatmaps;
using Sunrise.Shared.Extensions.Beatmaps;
using Sunrise.Shared.Extensions.Scores;
using Sunrise.Shared.Repositories;
using Sunrise.Shared.Utils.Converters;

namespace Sunrise.API.Serializable.Response;

public class PinResponse
{

    [JsonConstructor]
    public PinResponse()
    {
    }

    public PinResponse(Score score)
    {
        ScoreId = score.Id;
        IsPinned = score.IsPinned;
    }

    [JsonPropertyName("score_id")]
    public int ScoreId { get; set; }

    [JsonPropertyName("is_pinned")]
    public bool IsPinned { get; set; }
}