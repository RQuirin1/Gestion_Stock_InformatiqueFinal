using System;
using System.Collections.Generic;

namespace Library_Final;

public partial class Origine
{
    public int Id { get; set; }

    public int? IdProduit { get; set; }

    public string? Fournisseur { get; set; }

    public string? DateAchat { get; set; }

    public decimal? PrixHt { get; set; }

    public decimal? PrixTtc { get; set; }

    public string? Observation { get; set; }

    public virtual Materiel? IdProduitNavigation { get; set; }
}
