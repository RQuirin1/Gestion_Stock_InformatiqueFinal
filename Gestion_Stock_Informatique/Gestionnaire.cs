using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Library_Final;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DocumentFormat.OpenXml.InkML;
using System.Diagnostics;


namespace Gestion_Stock_Informatique
{
    public partial class GestionStock : Form
    {
        StockInformatiqueContext BDD_Stock;
        public GestionStock()
        {
            InitializeComponent();
        }



        private void GestionStock_Load(object sender, EventArgs e)
        {
            BDD_Stock = new StockInformatiqueContext();

            if (BDD_Stock.Database.CanConnect())
            {
                BDD_Stock.Materiels.Load();
                materielBindingSource.DataSource = BDD_Stock.Materiels.Local.ToBindingList();

                labelTot_Result.Text = Materiel_grille.RowCount.ToString();
                labelSelect_result.Text = Materiel_grille.RowCount.ToString();
            }
            else
            {

                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.Filter = "SQLite Database (*.sqlite)|*.sqlite";
                openFileDialog1.Title = "Sélectionnez une sauvegarde de la base de donnée";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    string database_save_Path = openFileDialog1.FileName;

                    try
                    {

                        var destination = AppContext.BaseDirectory;
                        string fileName = Path.GetFileName(database_save_Path);
                        string destinationFile = Path.Combine(destination, fileName);

                        File.Copy(database_save_Path, destinationFile, true);

                        BDD_Stock.Materiels.Load();
                        materielBindingSource.DataSource = BDD_Stock.Materiels.Local.ToBindingList();

                        labelTot_Result.Text = Materiel_grille.RowCount.ToString();
                        labelSelect_result.Text = Materiel_grille.RowCount.ToString();
                        MessageBox.Show("La base de donnée a bien été chargée.", "Réussite", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur de chargement de la base de donnée : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


            }
            try
            {
                string filePath = "last_save.txt";
                if (File.Exists(filePath))
                {
                    string text = File.ReadAllText(filePath);
                    DateTime lastClickTime = DateTime.Parse(text);
                    label_info_save.Text = $"Dernière sauvegarde : {lastClickTime:dd-MM-yyyy HH:mm:ss}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement de l'heure du dernier clic : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void buttonAjout_Click(object sender, EventArgs e)
        {
            Materiel_grille.Enabled = false;

            origine_Grille.Enabled = false;

            ajoutGroupBox.Visible = true;
            ajoutGroupBox.Enabled = true;

            buttonAjout.Enabled = false;

            suppButton.Visible = false;
            suppButton.Enabled = false;
            validButton.Visible = true;

            selectButton.Enabled = false;

            cancelButton.Enabled = true;
            cancelButton.Visible = true;

            groupBox_Prix.Visible = false;
            buttonAjout_Prix.Visible = false;

            numericUpDownQuant.Value = 1;
        }

        private void verifNotNull()
        {
            if (!String.IsNullOrEmpty(produitComboBox.Text) &&
               !String.IsNullOrEmpty(typeTextBox.Text) &&
               !String.IsNullOrEmpty(marqueTextBox.Text) &&
               !String.IsNullOrEmpty(caractTextBox.Text) &&
               !String.IsNullOrEmpty(modeleTextBox.Text) &&
               !String.IsNullOrEmpty(etatComboBox.Text) &&
               !String.IsNullOrEmpty(numericUpDownQuant.Text)
               )
            {
                validButton.Enabled = true;
            }
            else
            {
                validButton.Enabled = false;
            }
        }

        private void produitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            verifNotNull();

        }

        private void typeTextBox_TextChanged(object sender, EventArgs e)
        {
            verifNotNull();

        }

        private void marqueTextBox_TextChanged(object sender, EventArgs e)
        {
            verifNotNull();
        }

        private void caractTextBox_TextChanged(object sender, EventArgs e)
        {
            verifNotNull();

        }

        private void modeleTextBox_TextChanged(object sender, EventArgs e)
        {
            verifNotNull();

        }

        private void textBoxDestination_TextChanged(object sender, EventArgs e)
        {
            verifNotNull();
        }

        private void textBoxRangement_TextChanged(object sender, EventArgs e)
        {
            verifNotNull();
        }

        private void textBoxCommentaire_TextChanged(object sender, EventArgs e)
        {
            verifNotNull();
        }

        private void etatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            verifNotNull();
        }

        private void numericUpDownQuant_ValueChanged(object sender, EventArgs e)
        {
            verifNotNull();
        }

        private void validButton_Click(object sender, EventArgs e)
        {
            Materiel_grille.Enabled = true;

            origine_Grille.Enabled = true;

            ajoutGroupBox.Visible = false;
            ajoutGroupBox.Enabled = false;

            buttonAjout.Enabled = true;

            validButton.Enabled = false;
            validButton.Visible = false;

            suppButton.Visible = false;
            suppButton.Enabled = false;

            if (!String.IsNullOrEmpty(produitComboBox.Text) &&
               !String.IsNullOrEmpty(typeTextBox.Text) &&
               !String.IsNullOrEmpty(marqueTextBox.Text) &&
               !String.IsNullOrEmpty(caractTextBox.Text) &&
               !String.IsNullOrEmpty(modeleTextBox.Text) &&
               !String.IsNullOrEmpty(etatComboBox.Text) &&
               !String.IsNullOrEmpty(numericUpDownQuant.Text)
               )
            {
                long quant;

                if (long.TryParse(numericUpDownQuant.Text, out quant))
                {
                    if (quant > 1)
                    {
                        for (int i = 0; i < quant; i++)
                        {
                            Materiel mat_temp = new Materiel();
                            mat_temp.Produit = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(produitComboBox.Text);
                            mat_temp.Type = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(typeTextBox.Text);
                            mat_temp.Etat = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(etatComboBox.Text);
                            mat_temp.Marque = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(marqueTextBox.Text);
                            mat_temp.Caracteristique = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(caractTextBox.Text);
                            mat_temp.Modele = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(modeleTextBox.Text);
                            mat_temp.Destination = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(textBoxDestination.Text);
                            mat_temp.Rangement = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(textBoxRangement.Text);
                            mat_temp.Commentaire = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(textBoxCommentaire.Text);

                            BDD_Stock.Add(mat_temp);
                            long id = mat_temp.Id;
                        }
                    }
                    else
                    {
                        Materiel mat = new Materiel();
                        mat.Produit = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(produitComboBox.Text);
                        mat.Type = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(typeTextBox.Text);
                        mat.Etat = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(etatComboBox.Text);
                        mat.Marque = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(marqueTextBox.Text);
                        mat.Caracteristique = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(caractTextBox.Text);
                        mat.Modele = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(modeleTextBox.Text);
                        mat.Destination = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(textBoxDestination.Text);
                        mat.Rangement = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(textBoxRangement.Text);
                        mat.Commentaire = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(textBoxCommentaire.Text);

                        BDD_Stock.Add(mat);
                        long id = mat.Id;
                    }
                }



                produitComboBox.Text = "";
                typeTextBox.Text = "";
                etatComboBox.Text = "";
                marqueTextBox.Text = "";
                caractTextBox.Text = "";
                modeleTextBox.Text = "";
                textBoxCommentaire.Text = "";
                textBoxDestination.Text = "";
                textBoxRangement.Text = "";

            }
            BDD_Stock.SaveChanges();
            labelTot_Result.Text = Materiel_grille.RowCount.ToString();
            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }


        private void Materiel_grille_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonAjout.Visible = false;

            suppButton.Visible = true;
            suppButton.Enabled = true;

            cancelButton.Enabled = true;
            cancelButton.Visible = true;

            selectButton.Enabled = true;
            selectButton.Visible = true;

            buttonAjout_Prix.Visible = true;
            buttonAjout_Prix.Enabled = true;

            buttonSupp_Origine.Visible = false;
            buttonSupp_Origine.Enabled = false;

            buttonModif.Visible = true;
            buttonModif.Enabled = true;


            origine_Grille.Enabled = true;
            if (this.BDD_Stock != null)
            {
                var materiel = (Materiel)this.Materiel_grille.CurrentRow.DataBoundItem;
                if (materiel != null)
                {
                    this.BDD_Stock.Entry(materiel).Collection(e => e.Origines).Load();
                    this.origine_Grille.DataSource = materiel.Origines;
                }

            }
        }

        private void GestionStock_FormClosing(object sender, FormClosingEventArgs e)
        {
            BDD_Stock.SaveChanges();
            Application.Exit();
        }

        private void suppButton_Click(object sender, EventArgs e)
        {
            buttonAjout.Enabled = true;
            buttonAjout.Visible = true;

            suppButton.Visible = false;
            suppButton.Enabled = false;

            if (this.BDD_Stock != null)
            {
                var materiel = (Materiel)this.Materiel_grille.CurrentRow.DataBoundItem;
                if (materiel != null)
                {
                    BDD_Stock.Remove(materiel);
                }

            }
            labelTot_Result.Text = Materiel_grille.RowCount.ToString();
            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            buttonAjout.Visible = true;
            buttonAjout.Enabled = true;

            suppButton.Enabled = false;
            suppButton.Visible = false;

            cancelButton.Visible = false;
            cancelButton.Enabled = false;

            ajoutGroupBox.Enabled = false;
            ajoutGroupBox.Visible = false;

            selectButton.Enabled = true;
            selectButton.Visible = true;

            validButton.Visible = true;
            validButton.Enabled = false;

            Materiel_grille.Enabled = true;

            origine_Grille.Enabled = true;

            modif_groupBox.Visible = false;
            modif_groupBox.Enabled = false;

            buttonModif.Visible = false;
            buttonModif.Enabled = false;

            modifPrix_groupBox.Visible = false;
            saveChangesButton.Visible = false;
            modif_Prix_Button.Visible = false;

            buttonAjout_Prix.Visible = false;
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = false;

            buttonAjout.Enabled = false;

            cancelButton_select.Visible = true;
            cancelButton_select.Enabled = true;

            ajoutGroupBox.Visible = false;
            ajoutGroupBox.Enabled = false;

            groupBox_select.Visible = true;
            groupBox_select.Enabled = true;


            select_comboBox_Produit.DataSource = BDD_Stock.Materiels.Select(x => x.Produit).Distinct().ToList();

            if ((select_comboBox_Produit.SelectedValue != null)
                && (select_comboBox_Type.SelectedItem != null)
                && (select_comboBox_Marque.SelectedItem != null)
                && (select_comboBox_Caract.SelectedItem != null)
                && (select_comboBox_Modele.SelectedItem != null))

            {
                Materiel_grille.DataSource = BDD_Stock.Materiels
                .Where(c => c.Produit == select_comboBox_Produit.SelectedValue.ToString()
                    && c.Type == select_comboBox_Type.SelectedItem.ToString()
                    && c.Marque == select_comboBox_Marque.SelectedItem.ToString()
                    && c.Caracteristique == select_comboBox_Caract.SelectedItem.ToString()
                    && c.Modele == select_comboBox_Modele.SelectedItem.ToString())
                .ToList();
            }
            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }


        private void cancelButton_select_Click(object sender, EventArgs e)
        {
            selectButton.Visible = true;
            selectButton.Enabled = true;

            buttonAjout.Visible = true;
            buttonAjout.Enabled = true;

            validButton.Visible = true;
            validButton.Enabled = false;

            cancelButton_select.Visible = false;
            cancelButton_select.Enabled = false;

            groupBox_select.Visible = false;
            groupBox_select.Enabled = false;



            Materiel_grille.DataSource = BDD_Stock.Materiels.Local.ToBindingList();
        }

        private void select_comboBox_Produit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (select_comboBox_Produit.SelectedValue != null)
            {

                select_comboBox_Type.DataSource = BDD_Stock.Materiels
                    .Where(i => i.Produit == select_comboBox_Produit.SelectedItem.ToString())
                    .Select(i => i.Type)
                    .Distinct()
                    .ToList();

                Materiel_grille.DataSource = BDD_Stock.Materiels.Where(c => c.Produit == select_comboBox_Produit.SelectedValue.ToString()).ToList();

            }

            labelSelect_result.Text = Materiel_grille.RowCount.ToString();

        }

        private void select_comboBox_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_comboBox_Marque.DataSource = BDD_Stock.Materiels
                    .Where(i => i.Type == select_comboBox_Type.SelectedItem.ToString()
                        && i.Produit == select_comboBox_Produit.SelectedItem.ToString())
                    .Select(i => i.Marque)
                    .Distinct()
                    .ToList();

            Materiel_grille.DataSource = BDD_Stock.Materiels
                .Where(c => c.Produit == select_comboBox_Produit.SelectedValue.ToString()
                    && c.Type == select_comboBox_Type.SelectedItem.ToString())
                .ToList();

            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }

        private void select_comboBox_Marque_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_comboBox_Caract.DataSource = BDD_Stock.Materiels
                    .Where(i => i.Type == select_comboBox_Type.SelectedItem.ToString()
                        && i.Produit == select_comboBox_Produit.SelectedItem.ToString()
                        && i.Marque == select_comboBox_Marque.SelectedItem.ToString())
                    .Select(i => i.Caracteristique)
                    .Distinct()
                    .ToList();

            Materiel_grille.DataSource = BDD_Stock.Materiels
                .Where(c => c.Produit == select_comboBox_Produit.SelectedValue.ToString()
                    && c.Type == select_comboBox_Type.SelectedItem.ToString()
                    && c.Marque == select_comboBox_Marque.SelectedItem.ToString())
                .ToList();

            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }

        private void select_comboBox_Caract_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_comboBox_Modele.DataSource = BDD_Stock.Materiels
                    .Where(i => i.Type == select_comboBox_Type.SelectedItem.ToString()
                        && i.Produit == select_comboBox_Produit.SelectedItem.ToString()
                        && i.Marque == select_comboBox_Marque.SelectedItem.ToString()
                        && i.Caracteristique == select_comboBox_Caract.SelectedItem.ToString())
                    .Select(i => i.Modele)
                    .Distinct()
                    .ToList();

            Materiel_grille.DataSource = BDD_Stock.Materiels
                .Where(c => c.Produit == select_comboBox_Produit.SelectedValue.ToString()
                    && c.Type == select_comboBox_Type.SelectedItem.ToString()
                    && c.Marque == select_comboBox_Marque.SelectedItem.ToString()
                    && c.Caracteristique == select_comboBox_Caract.SelectedItem.ToString())
                .ToList();

            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }

        private void select_comboBox_Modele_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_comboBox_Etat.DataSource = BDD_Stock.Materiels
                    .Where(i => i.Type == select_comboBox_Type.SelectedItem.ToString()
                        && i.Produit == select_comboBox_Produit.SelectedItem.ToString()
                        && i.Marque == select_comboBox_Marque.SelectedItem.ToString()
                        && i.Caracteristique == select_comboBox_Caract.SelectedItem.ToString()
                        && i.Modele == select_comboBox_Modele.SelectedItem.ToString())
                    .Select(i => i.Etat)
                    .Distinct()
                    .ToList();

            Materiel_grille.DataSource = BDD_Stock.Materiels
                .Where(c => c.Produit == select_comboBox_Produit.SelectedValue.ToString()
                    && c.Type == select_comboBox_Type.SelectedItem.ToString()
                    && c.Marque == select_comboBox_Marque.SelectedItem.ToString()
                    && c.Caracteristique == select_comboBox_Caract.SelectedItem.ToString()
                    && c.Modele == select_comboBox_Modele.SelectedItem.ToString())
                .ToList();

            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }

        private void select_comboBox_Etat_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_comboBox_Destination.DataSource = BDD_Stock.Materiels
                    .Where(i => i.Type == select_comboBox_Type.SelectedItem.ToString()
                        && i.Produit == select_comboBox_Produit.SelectedItem.ToString()
                        && i.Marque == select_comboBox_Marque.SelectedItem.ToString()
                        && i.Caracteristique == select_comboBox_Caract.SelectedItem.ToString()
                        && i.Modele == select_comboBox_Modele.SelectedItem.ToString()
                        && i.Etat == select_comboBox_Etat.SelectedItem.ToString())
                    .Select(i => i.Destination)
                    .Distinct()
                    .ToList();

            Materiel_grille.DataSource = BDD_Stock.Materiels
                .Where(c => c.Produit == select_comboBox_Produit.SelectedValue.ToString()
                    && c.Type == select_comboBox_Type.SelectedItem.ToString()
                    && c.Marque == select_comboBox_Marque.SelectedItem.ToString()
                    && c.Caracteristique == select_comboBox_Caract.SelectedItem.ToString()
                    && c.Modele == select_comboBox_Modele.SelectedItem.ToString()
                    && c.Etat == select_comboBox_Etat.SelectedItem.ToString())
                .ToList();

            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }

        private void select_comboBox_Destination_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_comboBox_Rangement.DataSource = BDD_Stock.Materiels
                    .Where(i => i.Type == select_comboBox_Type.SelectedItem.ToString()
                        && i.Produit == select_comboBox_Produit.SelectedItem.ToString()
                        && i.Marque == select_comboBox_Marque.SelectedItem.ToString()
                        && i.Caracteristique == select_comboBox_Caract.SelectedItem.ToString()
                        && i.Modele == select_comboBox_Modele.SelectedItem.ToString()
                        && i.Etat == select_comboBox_Etat.SelectedItem.ToString()
                        && i.Destination == select_comboBox_Destination.SelectedItem.ToString())
                    .Select(i => i.Rangement)
                    .Distinct()
                    .ToList();

            Materiel_grille.DataSource = BDD_Stock.Materiels
                .Where(c => c.Produit == select_comboBox_Produit.SelectedValue.ToString()
                    && c.Type == select_comboBox_Type.SelectedItem.ToString()
                    && c.Marque == select_comboBox_Marque.SelectedItem.ToString()
                    && c.Caracteristique == select_comboBox_Caract.SelectedItem.ToString()
                    && c.Modele == select_comboBox_Modele.SelectedItem.ToString()
                    && c.Etat == select_comboBox_Etat.SelectedItem.ToString()
                    && c.Destination == select_comboBox_Destination.SelectedItem.ToString())
                .ToList();

            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }


        private void select_comboBox_Rangement_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_comboBox_Commentaire.DataSource = BDD_Stock.Materiels
                    .Where(i => i.Type == select_comboBox_Type.SelectedItem.ToString()
                        && i.Produit == select_comboBox_Produit.SelectedItem.ToString()
                        && i.Marque == select_comboBox_Marque.SelectedItem.ToString()
                        && i.Caracteristique == select_comboBox_Caract.SelectedItem.ToString()
                        && i.Modele == select_comboBox_Modele.SelectedItem.ToString()
                        && i.Etat == select_comboBox_Etat.SelectedItem.ToString()
                        && i.Destination == select_comboBox_Destination.SelectedItem.ToString()
                        && i.Rangement == select_comboBox_Rangement.SelectedItem.ToString())
                    .Select(i => i.Commentaire)
                    .Distinct()
                    .ToList();

            Materiel_grille.DataSource = BDD_Stock.Materiels
                .Where(c => c.Produit == select_comboBox_Produit.SelectedValue.ToString()
                    && c.Type == select_comboBox_Type.SelectedItem.ToString()
                    && c.Marque == select_comboBox_Marque.SelectedItem.ToString()
                    && c.Caracteristique == select_comboBox_Caract.SelectedItem.ToString()
                    && c.Modele == select_comboBox_Modele.SelectedItem.ToString()
                    && c.Etat == select_comboBox_Etat.SelectedItem.ToString()
                    && c.Destination == select_comboBox_Destination.SelectedItem.ToString()
                    && c.Rangement == select_comboBox_Rangement.SelectedItem.ToString())
                .ToList();

            labelSelect_result.Text = Materiel_grille.RowCount.ToString();
        }

        private void buttonAjout_Prix_Click(object sender, EventArgs e)
        {
            groupBox_Prix.Enabled = true;
            groupBox_Prix.Visible = true;

            origine_Grille.Enabled = false;

            validButton_Prix.Visible = true;


            origine_Grille.Enabled = false;

            buttonCancel_Prix.Visible = true;

            buttonAjout_Prix.Enabled = false;
        }


        private void validButton_Prix_Click(object sender, EventArgs e)
        {
            decimal prixHT;
            decimal prixTTC;

            if (decimal.TryParse(textBox_HT.Text, out prixHT)
                && decimal.TryParse(textBox_TTC.Text, out prixTTC))
            {
                Origine origine = new Origine();
                var materiel = (Materiel)this.Materiel_grille.CurrentRow.DataBoundItem;
                origine.Fournisseur = textBox_Fournisseur.Text;
                origine.PrixTtc = prixTTC;
                origine.PrixHt = prixHT;
                origine.IdProduit = materiel.Id;
                origine.DateAchat = dateTimePicker_achat.Value.ToString("dd/MM/yyyy");
                origine.Observation = textBox_Observation.Text;

                BDD_Stock.Origines.Add(origine);

                BDD_Stock.SaveChanges();
                this.origine_Grille.DataSource = materiel.Origines;
                origine_Grille.Enabled = true;
                validButton_Prix.Visible = false;
                validButton_Prix.Enabled = false;
                groupBox_Prix.Visible = false;

                buttonCancel_Prix.Visible = false;
            }
            else
            {
                labelErreur.Text = "Mauvais format de texte";
            }
        }


        private void verifNotNull_prix()
        {
            if (String.IsNullOrEmpty(textBox_Fournisseur.Text) &&
               String.IsNullOrEmpty(textBox_HT.Text) &&
               String.IsNullOrEmpty(textBox_TTC.Text) &&
               String.IsNullOrEmpty(dateTimePicker_achat.Text) &&
               String.IsNullOrEmpty(textBox_Observation.Text)
               )
            {
                validButton_Prix.Enabled = false;
            }
            else
            {
                validButton_Prix.Enabled = true;
            }
        }

        private void textBox_Fournisseur_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_prix();
        }

        private void dateTimePicker_achat_ValueChanged(object sender, EventArgs e)
        {
            verifNotNull_prix();
        }

        private void textBox_HT_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_prix();
        }

        private void textBox_TTC_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_prix();
        }

        private void textBox_Observation_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_prix();
        }

        private void buttonSupp_Origine_Click(object sender, EventArgs e)
        {
            if (this.BDD_Stock != null)
            {
                var origine = (Origine)this.origine_Grille.CurrentRow.DataBoundItem;
                if (origine != null)
                {
                    BDD_Stock.Remove(origine);
                    BDD_Stock.SaveChanges();
                    var materiel = (Materiel)this.Materiel_grille.CurrentRow.DataBoundItem;
                    this.origine_Grille.DataSource = materiel.Origines;

                    buttonSupp_Origine.Enabled = false;
                    buttonSupp_Origine.Visible = false;

                    buttonAjout_Prix.Enabled = true;

                }


            }
        }

        private void origine_Grille_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonSupp_Origine.Enabled = true;
            buttonSupp_Origine.Visible = true;

            buttonCancel_Prix.Visible = true;
            modif_Prix_Button.Visible = true;
        }

        private void buttonCancel_Prix_Click(object sender, EventArgs e)
        {
            groupBox_Prix.Visible = false;
            groupBox_Prix.Enabled = false;

            buttonAjout_Prix.Visible = false;
            buttonSupp_Origine.Visible = false;
            validButton_Prix.Visible = false;

            buttonCancel_Prix.Visible = false;

            modif_Prix_Button.Visible = false;

            modifPrix_groupBox.Visible = false;
            saveChangesButton.Visible = false;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            using (var workbook = new XLWorkbook())
            {



                var ws = workbook.AddWorksheet();

                ws.Cell("A2").InsertData(BDD_Stock.Materiels);
                foreach (var cell in ws.Column("K").CellsUsed())
                {
                    cell.Clear();
                };
                ws.Cell("A1").SetValue("Id");
                ws.Cell("B1").SetValue("Produit");
                ws.Cell("C1").SetValue("Type");
                ws.Cell("D1").SetValue("Marque");
                ws.Cell("E1").SetValue("Caractéristique");
                ws.Cell("F1").SetValue("Modèle");
                ws.Cell("G1").SetValue("Etat");
                ws.Cell("H1").SetValue("Destination");
                ws.Cell("I1").SetValue("Rangement");
                ws.Cell("J1").SetValue("Commentaire");
                ws.Columns().AdjustToContents();
                var ws2 = workbook.AddWorksheet();
                ws2.Cell("A2").InsertData(BDD_Stock.Origines);
                foreach (var cell in ws2.Column("H").CellsUsed())
                {
                    cell.Clear();
                };
                ws2.Cell("A1").SetValue("Id");
                ws2.Cell("B1").SetValue("Id du Produit");
                ws2.Cell("C1").SetValue("Fournisseur");
                ws2.Cell("D1").SetValue("Date d'Achat");
                ws2.Cell("E1").SetValue("Prix HT");
                ws2.Cell("F1").SetValue("Prix TTC");
                ws2.Cell("G1").SetValue("Observation");
                ws2.Columns().AdjustToContents();

                workbook.SaveAs(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Excel_Stock_Info.xlsx"));
            }


        }

        private void buttonModif_Click(object sender, EventArgs e)
        {
            modif_groupBox.Visible = true;
            modif_groupBox.Enabled = true;
            ajoutGroupBox.Visible = false;
            ajoutGroupBox.Enabled = false;

            buttonModif.Enabled = false;

            validButton.Visible = false;

            var materiel = (Materiel)this.Materiel_grille.CurrentRow.DataBoundItem;

            modif_comboBoxProduit.Text = materiel.Produit;
            modif_textBoxType.Text = materiel.Type;
            modif_textBoxMarque.Text = materiel.Marque;
            modif_textBoxCaract.Text = materiel.Caracteristique;
            modif_textBoxModele.Text = materiel.Modele;
            modif_comboBoxEtat.Text = materiel.Etat;
            modif_textBoxDestination.Text = materiel.Destination;
            modif_textBoxRangement.Text = materiel.Rangement;
            modif_textBoxComm.Text = materiel.Commentaire;
        }

        private void verifNotNull_Modif()
        {
            if (!String.IsNullOrEmpty(modif_comboBoxProduit.Text) &&
                !String.IsNullOrEmpty(modif_textBoxType.Text) &&
                !String.IsNullOrEmpty(modif_textBoxMarque.Text) &&
                !String.IsNullOrEmpty(modif_textBoxCaract.Text) &&
                !String.IsNullOrEmpty(modif_textBoxModele.Text) &&
                !String.IsNullOrEmpty(modif_comboBoxEtat.Text) &&
                !String.IsNullOrEmpty(modif_textBoxDestination.Text) &&
                !String.IsNullOrEmpty(modif_textBoxRangement.Text) &&
                !String.IsNullOrEmpty(modif_textBoxComm.Text))
            {
                modif_buttonValid.Visible = true;
            }
            else
            {
                modif_buttonValid.Visible = false;
            }
        }

        private void modif_buttonValid_Click(object sender, EventArgs e)
        {
            var materiel = (Materiel)this.Materiel_grille.CurrentRow.DataBoundItem;
            var product = BDD_Stock.Materiels.FirstOrDefault(p => p.Id == materiel.Id);

            if (product != null)
            {
                product.Produit = modif_comboBoxProduit.Text;
                product.Type = modif_textBoxType.Text;
                product.Marque = modif_textBoxMarque.Text;
                product.Caracteristique = modif_textBoxCaract.Text;
                product.Modele = modif_textBoxModele.Text;
                product.Etat = modif_comboBoxEtat.Text;
                product.Destination = modif_textBoxDestination.Text;
                product.Rangement = modif_textBoxRangement.Text;
                product.Commentaire = modif_textBoxComm.Text;

                BDD_Stock.SaveChanges();

                modif_comboBoxProduit.Text = "";
                modif_textBoxType.Text = "";
                modif_textBoxMarque.Text = "";
                modif_textBoxCaract.Text = "";
                modif_textBoxModele.Text = "";
                modif_comboBoxEtat.Text = "";
                modif_textBoxDestination.Text = "";
                modif_textBoxRangement.Text = "";
                modif_textBoxComm.Text = "";


            }
            modif_groupBox.Visible = false;
            modif_groupBox.Enabled = false;

        }

        private void modif_comboBoxProduit_SelectedIndexChanged(object sender, EventArgs e)
        {
            verifNotNull_Modif();
        }

        private void modif_textBoxType_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_Modif();
        }

        private void modif_textBoxMarque_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_Modif();
        }

        private void modif_textBoxCaract_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_Modif();
        }

        private void modif_textBoxModele_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_Modif();
        }

        private void modif_comboBoxEtat_SelectedIndexChanged(object sender, EventArgs e)
        {
            verifNotNull_Modif();
        }

        private void modif_textBoxDestination_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_Modif();
        }

        private void modif_textBoxRangement_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_Modif();
        }

        private void modif_textBoxComm_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_Modif();
        }

        private void modif_Prix_Button_Click(object sender, EventArgs e)
        {
            modifPrix_groupBox.Visible = true;
            modif_Prix_Button.Visible = false;
            saveChangesButton.Visible = true;
            saveChangesButton.Enabled = false;


            var origine = (Origine)this.origine_Grille.CurrentRow.DataBoundItem;

            modif_textBoxFournisseur.Text = origine.Fournisseur;
            modif_dateTimePicker.Text = origine.DateAchat;
            modif_textBoxPrixHT.Text = origine.PrixHt.ToString();
            modif_textBoxPrixTTC.Text = origine.PrixTtc.ToString();
            modif_textBoxObserv.Text = origine.Observation;


        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {

            var origine = (Origine)this.origine_Grille.CurrentRow.DataBoundItem;
            var product = BDD_Stock.Origines.FirstOrDefault(p => p.Id == origine.Id);
            decimal prixHT;
            decimal prixTTC;

            if (decimal.TryParse(modif_textBoxPrixHT.Text, out prixHT)
                && decimal.TryParse(modif_textBoxPrixTTC.Text, out prixTTC))
            {
                if (product != null)
                {
                    product.Fournisseur = modif_textBoxFournisseur.Text;
                    product.DateAchat = modif_dateTimePicker.Value.ToString("dd/MM/yyyy");
                    product.PrixHt = prixHT;
                    product.PrixTtc = prixTTC;
                    product.Observation = modif_textBoxObserv.Text;

                    BDD_Stock.SaveChanges();

                    modif_textBoxFournisseur.Text = "";
                    modif_textBoxPrixHT.Text = "";
                    modif_textBoxPrixTTC.Text = "";
                    modif_textBoxObserv.Text = "";
                    saveChangesButton.Visible = false;
                    modifPrix_groupBox.Visible = false;
                }
            }

        }

        private void verifNotNull_modif_prix()
        {
            if (!String.IsNullOrEmpty(modif_textBoxFournisseur.Text) &&
               !String.IsNullOrEmpty(modif_textBoxPrixHT.Text) &&
               !String.IsNullOrEmpty(modif_textBoxPrixTTC.Text) &&
               !String.IsNullOrEmpty(modif_dateTimePicker.Text) &&
               !String.IsNullOrEmpty(modif_textBoxObserv.Text)
               )
            {
                saveChangesButton.Enabled = true;
            }
            else
            {
                saveChangesButton.Enabled = false;
            }
        }

        private void modif_textBoxFournisseur_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_modif_prix();
        }

        private void modif_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            verifNotNull_modif_prix();
        }

        private void modif_textBoxPrixHT_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_modif_prix();
        }

        private void modif_textBoxPrixTTC_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_modif_prix();
        }

        private void modif_textBoxObserv_TextChanged(object sender, EventArgs e)
        {
            verifNotNull_modif_prix();
        }

        private void buttonBackUp_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "SQLite Database|*.sqlite";
            saveFileDialog.Title = "Sauvegarde";
            saveFileDialog.FileName = "stock_informatique.sqlite";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string dataBasePath = Path.Combine(AppContext.BaseDirectory, "stock_informatique.sqlite");


                    string backupPath = saveFileDialog.FileName;
                    string destinationFolder = Path.GetDirectoryName(backupPath);

                    string date = DateTime.Now.ToString("dd-MM-yyyy");
                    string newFolder = Path.Combine(destinationFolder, $"save_stock_{date}");

                    Directory.CreateDirectory(newFolder);

                    string destinationFile = Path.Combine(newFolder, Path.GetFileName(dataBasePath));

                    File.Copy(dataBasePath, destinationFile, true);

                    try
                    {
                        File.WriteAllText("last_save.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la sauvegarde de l'heure du dernier clic : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    MessageBox.Show("La sauvegarde a été créée avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Materiel_grille_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
