using System;
using System.Collections.Generic;

namespace DbEntities.Models;

/// <summary>
/// Groupe
/// </summary>
public partial class Groupe
{
    /// <summary>
    /// GroupeId
    /// </summary>
    public int Groupeid { get; set; }

    /// <summary>
    /// Libelle
    /// </summary>
    public string Libelle { get; set; } = null!;

    public virtual ICollection<Autorisation> Autorisations { get; } = new List<Autorisation>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
