using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



public class CommonResponseVM
{
    [JsonPropertyName("logoImage")]
    public string? LogoImage { get; set; }

    [JsonPropertyName("data")]
    public List<List<Dictionary<string, object>>>? Data { get; set; }
}


