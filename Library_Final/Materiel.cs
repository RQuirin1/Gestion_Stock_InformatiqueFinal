using System;
using System.Collections.Generic;

namespace Library_Final;

public partial class Materiel
{
    public int Id { get; set; }

    public string? Produit { get; set; }

    public string? Type { get; set; }

    public string? Marque { get; set; }

    public string? Caracteristique { get; set; }

    public string? Modele { get; set; }

    public string? Etat { get; set; }

    public string? Destination { get; set; }

    public string? Rangement { get; set; }

    public string? Commentaire { get; set; }

    public virtual ICollection<Origine> Origines { get; set; } = new List<Origine>();
}
