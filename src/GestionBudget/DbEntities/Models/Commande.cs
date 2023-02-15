using System;
using System.Collections.Generic;

namespace DbEntities.Models;

/// <summary>
/// Commande
/// </summary>
public partial class Commande
{
    /// <summary>
    /// Idcmde
    /// </summary>
    public int Idcmde { get; set; }

    /// <summary>
    /// Libelle
    /// </summary>
    public string Libelle { get; set; } = null!;

    /// <summary>
    /// Code
    /// </summary>
    public int Code { get; set; }

    public virtual ICollection<Autorisation> Autorisations { get; } = new List<Autorisation>();
}
