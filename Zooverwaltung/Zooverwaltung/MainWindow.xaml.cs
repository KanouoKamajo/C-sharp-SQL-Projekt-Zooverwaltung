using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatenbankAnwendung2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = @"Data Source=JKanouoManeyo\SQLEXPRESS;Initial Catalog=Zooverwaltung;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);

            this.showZoo();
            this.showTiere();
        }

        private void showZoo()
        {
            try
            {
                string query = "SELECT * from Zoo";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);

                using (adapter)
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    listeAlleZoo.DisplayMemberPath = "Location";
                    listeAlleZoo.SelectedValuePath = "ZooId";
                    listeAlleZoo.ItemsSource = dataTable.DefaultView;
                }

                teZoo.Text = "";
            }


            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up!" + ex.ToString());
            }
        }

        private void showTiere()
        {
            try
            {
                string query = "SELECT * from Tier";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);

                using (adapter)
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    listeAlleTieren.DisplayMemberPath = "Name";
                    listeAlleTieren.SelectedValuePath = "TierId";
                    listeAlleTieren.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up!" + ex.ToString());
            }
        }
        private void showAssociateAnimals_manual()
        {
            try
            {
                var selectedZooId = listeAlleZoo.SelectedValue;

                if (selectedZooId != null)
                {
                    string query = "SELECT t.Name AS [Tier], t.TierId AS [ID] FROM ZooTiere AS zt ON zt.ZoId = " + selectedZooId.ToString() + " JOIN Tier AS t ON zt.TiId = t.TierId GROUP BY t.TierId, t.Name;";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);

                    using (adapter)
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        listeAssociateTier.DisplayMemberPath = "Tier";
                        listeAssociateTier.SelectedValuePath = "ID";
                        listeAssociateTier.ItemsSource = dataTable.DefaultView;
                    }
                    teZoo.Text = listeAlleZoo.SelectedItem.ToString().Trim();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up!" + ex.ToString());
            }
        }
        private void ShowAssociateAnimals()
        {

            if (listeAlleZoo.SelectedValue != null)
            {
                string query = "SELECT * FROM Tier AS a JOIN ZooTiere AS za ON a.TierId = za.TiId WHERE za.ZoId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listeAlleZoo.SelectedValue);

                    DataTable animalsInZoosTable = new DataTable();

                    sqlDataAdapter.Fill(animalsInZoosTable);

                    listeAssociateTier.DisplayMemberPath = "Name";
                    listeAssociateTier.SelectedValuePath = "TierId";

                    listeAssociateTier.ItemsSource = animalsInZoosTable.DefaultView;

                }
                if (listeAlleZoo.SelectedItem is DataRowView rowView)
                {
                    teZoo.Text = rowView["Location"].ToString().Trim();
                }
            }
        }
        private void listeAlleZoo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociateAnimals();
            //showAssociateAnimals_manual();
            return;

        }

        private void pbAddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (teZoo.Text.Length > 2)
                {
                    string query = "INSERT INTO Zoo VALUES (@ZooName)";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);


                    sqlCommand.Parameters.AddWithValue("@ZooName", teZoo.Text);

                    sqlConnection.Open();
                    sqlCommand.ExecuteScalar();

                    if (listeAlleZoo.SelectedItem is DataRowView rowView)
                        MessageBox.Show("Zoo '" + teZoo.Text.Trim() + "' saved!");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up!\n\n" + ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                this.showZoo();
            }
        }

        private void pbUpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeAlleZoo.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Nothing selected!");
                    return;
                }

                if (teZoo.Text.Trim().Length < 3)
                {
                    MessageBox.Show("Name is to shoooort!");
                    return;
                }

                string query = "UPDATE ZOO SET Location = (@NewName) WHERE ZooId = (@ZooId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@ZooId", listeAlleZoo.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@NewName", teZoo.Text.Trim());

                sqlConnection.Open();
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up! \n\n" + ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                this.showZoo();

            }
        }

        private void pbDeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeAlleZoo.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Nothing selected!");
                    return;
                }

                string query2 = "DELETE FROM ZooTiere WHERE ZoId = (@ZooId)";
                string query = "DELETE FROM Zoo WHERE ZooId = (@ZooId)";

                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlConnection);
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand2.Parameters.AddWithValue("@ZooId", listeAlleZoo.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ZooId", listeAlleZoo.SelectedValue);

                sqlConnection.Open();
                sqlCommand2.ExecuteScalar();
                sqlCommand.ExecuteScalar();

                if (listeAlleZoo.SelectedItem is DataRowView rowView)
                    MessageBox.Show("Zoo '" + rowView["Location"].ToString().Trim() + "' deleted!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up! \n\n" + ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                this.showZoo();

            }
        }

        private void pbAddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (teAnimal.Text.Length > 2)
                {
                    string query = "INSERT INTO Tier VALUES (@Name)";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);


                    sqlCommand.Parameters.AddWithValue("@Name", teAnimal.Text);

                    sqlConnection.Open();
                    sqlCommand.ExecuteScalar();

                    if (listeAlleZoo.SelectedItem is DataRowView rowView)
                        MessageBox.Show("New animal '" + teAnimal.Text.Trim() + "' saved!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up!\n\n" + ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                this.showTiere();
            }
        }


        private void pbAssignAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeAlleTieren.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("No Animal selected!");
                    return;
                }

                if (listeAlleZoo.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("No Zoo selected!");
                    return;
                }

                //Todo: Prüfen, ob das tier bereits hinzugefügt wurde
                //--
                //--

                string query = "INSERT INTO ZooTiere (ZoId, TiId) VALUES (@ZooId, @TierId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@TierId", listeAlleTieren.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ZooId", listeAlleZoo.SelectedValue);

                sqlConnection.Open();
                sqlCommand.ExecuteScalar();

                if (listeAlleTieren.SelectedItem is DataRowView rowView)
                    MessageBox.Show("New animal '" + teAnimal.Text.Trim() + "' assigned!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up! \n\n" + ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                this.showTiere();
                this.showZoo();
            }
        }


        private void pbDeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeAlleTieren.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("No Animal selected!");
                    return;
                }

                string query = "DELETE FROM Tier WHERE TierId = (@TierId)";
                string query2 = "DELETE FROM ZooTiere WHERE TiId = (@TierId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlConnection);


                sqlCommand.Parameters.AddWithValue("@TierId", listeAlleTieren.SelectedValue);
                sqlCommand2.Parameters.AddWithValue("@TierId", listeAlleTieren.SelectedValue);

                sqlConnection.Open();
                sqlCommand2.ExecuteScalar(); //Erst aus der Verweistabelle löschen
                sqlCommand.ExecuteScalar();

                if (listeAlleTieren.SelectedItem is DataRowView rowView)
                    MessageBox.Show("Animal '" + rowView["Name"].ToString() + "' deleted!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up! \n\n" + ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                this.showTiere();
            }
        }

        private void pbUnassign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeAssociateTier.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("No Animal selected!");
                    return;
                }

                if (listeAlleZoo.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("No Zoo selected!");
                    return;
                }

                string query = "DELETE FROM ZooTiere WHERE TiId = (@TierId) AND ZoId = (@ZooId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@ZooId", listeAlleZoo.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@TierId", listeAssociateTier.SelectedValue);

                sqlConnection.Open();
                sqlCommand.ExecuteScalar();

                if (listeAssociateTier.SelectedItem is DataRowView rowView)
                    MessageBox.Show("Animal '" + rowView["Name"].ToString() + "' unassigned!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Jo bro you fucked up! \n\n" + ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                this.showZoo();
            }
        }
    }
}
