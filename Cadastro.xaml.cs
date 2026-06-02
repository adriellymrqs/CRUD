using System.Text;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;


namespace CRUD;

public partial class Cadastro : Window
{
    public string stringConexao =  Environment.GetEnvironmentVariable("MYSQL_STRING");
    
    public Cadastro()

    {

        InitializeComponent();

    }

    private void b(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(txtNome.Text) || string.IsNullOrEmpty(txtUsername.Text) ||
            string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSenha.Password))
        {
            MessageBox.Show("Todos os campos são obrigatorios.", "erro!");
            return;
        }

        using var conexao = new MySqlConnection(stringConexao);
        const string query = "INSERT INTO usuarios (nome, username, email, senha) VALUES (@nome,@username,@email,@senha)";
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@nome", txtNome.Text);
        comando.Parameters.AddWithValue("@username", txtUsername.Text);
        comando.Parameters.AddWithValue("@email", txtEmail.Text);
        comando.Parameters.AddWithValue("@senha", txtSenha.Password);

        try
        {
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();
            if (linhasAfetadas > 0)
            {
                MessageBox.Show("cadastro efetuado com sucesso!");

            }
        }
        catch (Exception exception)
        {
            if (exception is MySqlException erroSql)
            {
                if (erroSql.Number == 1062)
                {
                    MessageBox.Show("o email ou username ja foram utilizados");
                    return;
                }


                Console.WriteLine(exception);
                return;
            }
        }
    }

    private void BtnCadastrar_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnLogin_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}