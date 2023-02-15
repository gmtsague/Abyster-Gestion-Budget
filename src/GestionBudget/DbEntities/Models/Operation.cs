using System;
using System.Collections.Generic;

namespace DbEntities.Models;

/// <summary>
/// Operation
/// </summary>
public partial class Operation
{
    /// <summary>
    /// Idoperation
    /// </summary>
    public int Idoperation { get; set; }

    /// <summary>
    /// Idcategorie
    /// </summary>
    public int Idcategorie { get; set; }

    /// <summary>
    /// Userid
    /// </summary>
    public int Userid { get; set; }

    /// <summary>
    /// Montant
    /// </summary>
    public decimal Montant { get; set; }

    /// <summary>
    /// Dateoperation
    /// </summary>
    public DateTime Dateoperation { get; set; }

    /// <summary>
    /// Isrevenu
    /// </summary>
    public bool Isrevenu { get; set; }

    public virtual Categorie Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
