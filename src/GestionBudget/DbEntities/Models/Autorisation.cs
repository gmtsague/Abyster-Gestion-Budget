using System;
using System.Collections.Generic;

namespace DbEntities.Models;

/// <summary>
/// Autorisation
/// </summary>
public partial class Autorisation
{
    /// <summary>
    /// Idautorisation
    /// </summary>
    public int Idautorisation { get; set; }

    /// <summary>
    /// Idcmde
    /// </summary>
    public int Idcmde { get; set; }

    /// <summary>
    /// GroupeId
    /// </summary>
    public int Groupeid { get; set; }

    /// <summary>
    /// Isactive
    /// </summary>
    public bool Isactive { get; set; }

    public virtual Groupe Groupe { get; set; } = null!;

    public virtual Commande Cmde { get; set; } = null!;
}
