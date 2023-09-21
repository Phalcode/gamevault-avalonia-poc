﻿using System.Text.Json.Serialization;

namespace GamevaultAvaloniaPoc.Models
{
    public struct Tag
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("rawg_id")]
        public int RawgId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
