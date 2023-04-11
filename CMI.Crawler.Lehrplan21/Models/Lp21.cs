using System.Text.Json.Serialization;

namespace CMI.Crawler.Lehrplan21.Models;

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

    // orientierungspunkte
    [JsonPropertyName("k_id")]
    public string? KId { get; set; }

    [JsonPropertyName("aufbau")]
    public string? Aufbau { get; set; }

    [JsonPropertyName("zyklus")]
    public string? Zyklus { get; set; }

    [JsonPropertyName("folge_in_zyklus")]
    public string? FolgeInZyklus { get; set; }

    [JsonPropertyName("grundanspruch")]
    public string? Grundanspruch { get; set; }

    [JsonPropertyName("orientierungspunkt")]
    public string? Orientierungspunkt { get; set; }

    [JsonPropertyName("orientierungspunkt_vorher")]
    public string? OrientierungspunktVorher { get; set; }

    [JsonPropertyName("grau")]
    public string? Grau { get; set; }

    [JsonPropertyName("spaeter_im_zyklus")]
    public string? SpaeterImZyklus { get; set; }

    [JsonPropertyName("linie_oben")]
    public string? LinieOben { get; set; }

    [JsonPropertyName("linie_unten")]
    public string? LinieUnten { get; set; }

    [JsonPropertyName("anzahl_in_zyklus")]
    public string? AnzahlInZyklus { get; set; }

    [JsonPropertyName("anzahl_in_kompetenz")]
    public string? AnzahlInKompetenz { get; set; }

    [JsonPropertyName("folge_in_aufbaute")]
    public string? FolgeInAufbaute { get; set; }

    [JsonPropertyName("sprachreferenz")]
    public string? Sprachreferenz { get; set; }

    [JsonPropertyName("sprachreferenz_kurz")]
    public string? SprachreferenzKurz { get; set; }

    // aufzählungspunkt
    [JsonPropertyName("aufzaehlungspunkt")]
    public string? Aufzaehlungspunkt { get; set; }

    [JsonPropertyName("begriffe")]
    public string? Begriffe { get; set; }
}