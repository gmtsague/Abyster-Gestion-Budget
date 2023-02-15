using System;
using System.Collections.Generic;

namespace DbEntities.Models;

/// <summary>
/// Categorie
/// </summary>
public partial class Categorie
{
    /// <summary>
    /// Idcategorie
    /// </summary>
    public int Idcategorie { get; set; }

    /// <summary>
    /// Libelle
    /// </summary>
    public string Libelle { get; set; } = null!;

    public virtual ICollection<Operation> Operations { get; } = new List<Operation>();
}
