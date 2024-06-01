using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AHRestAPI.Models;

public partial class Animalbreed
{
    public int AnimalbreedId { get; set; }

    public int? AnimalTypeid { get; set; }

    public string? AnimalbreedName { get; set; }

    [JsonIgnore]
    public virtual Animaltype? AnimalType { get; set; }
    [JsonIgnore]
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
