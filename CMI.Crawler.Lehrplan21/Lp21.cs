using System.Text.Json.Serialization;

namespace CMI.Crawler.Lehrplan21;

public class Lp21
{
    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    [JsonPropertyName("fb_id")]
    public string? FbId { get; set; }

    [JsonPropertyName("f_id")]
    public string? FId { get; set; }

    [JsonPropertyName("kb_id")]
    public string? KbId { get; set; }

    [JsonPropertyName("ha_id")]
    public string? HaId { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("strukturtyp")]
    public string? Strukturtyp { get; set; }

    [JsonPropertyName("sprache")]
    public string? Sprache { get; set; }

    [JsonPropertyName("bezeichnung")]
    public string? Bezeichnung { get; set; }

    [JsonPropertyName("kanton")]
    public string? Kanton { get; set; }

    [JsonPropertyName("url")]
    public Uri? Url { get; set; }

    [JsonPropertyName("hierarchie_oben")]
    public Uri? HierarchieOben { get; set; }

    [JsonPropertyName("hierarchie_oben_typ")]
    public string? HierarchieObenTyp { get; set; }

    [JsonPropertyName("hierarchie_unten")]
    public List<Uri>? HierarchieUnten { get; set; }

    [JsonPropertyName("hierarchie_unten_typ")]
    public string? HierarchieUntenTyp { get; set; }
}
