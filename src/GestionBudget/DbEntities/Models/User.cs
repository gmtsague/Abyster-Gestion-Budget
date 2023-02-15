using System;
using System.Collections.Generic;

namespace DbEntities.Models;

/// <summary>
/// Users
/// </summary>
public partial class User
{
    /// <summary>
    /// Userid
    /// </summary>
    public int Userid { get; set; }

    /// <summary>
    /// GroupeId
    /// </summary>
    public int Groupeid { get; set; }

    /// <summary>
    /// Nom
    /// </summary>
    public string Nom { get; set; } = null!;

    /// <summary>
    /// Prenom
    /// </summary>
    public string? Prenom { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Isactive
    /// </summary>
    public bool Isactive { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// LastConnexion
    /// </summary>
    public DateTime? Lastconnexion { get; set; }

    public virtual Groupe Groupe { get; set; } = null!;

    public virtual ICollection<Operation> Operations { get; } = new List<Operation>();
}
