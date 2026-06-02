using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class MeuPerfil : Window
{
    private Usuario UsuarioAtual;

    public MeuPerfil(Usuario usuario)
    {
        InitializeComponent();
        UsuarioAtual = usuario;

        TxtNome.Text = UsuarioAtual.Nome;
        TxtEmail.Text = UsuarioAtual.Email;
        TxtUsername.Text = UsuarioAtual.Username;

    }

    private void BtnSalvar_OnClick(object sender, RoutedEventArgs e)
    
    {
        if (string.IsNullOrWhiteSpace(TxtNome.Text))
        {
            MessageBox.Show("O campo NOME não pode ser vazio!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            TxtUsername.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(TxtEmail.Text))
        {
            MessageBox.Show("O campo USÚARIO não pode estar vazio!", "Aviso", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            TxtUsername.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(TxtEmail.Text))
        {
            MessageBox.Show("O campo EMAIL não pode estar vazio!", "Aviso", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            TxtEmail.Focus();
            return;
        }

        var senhaFoiAlterada = !string.IsNullOrWhiteSpace(TxtSenha.Password);
        
        UsuarioAtual.Username = TxtUsername.Text;
        UsuarioAtual.Nome = TxtNome.Text;
        UsuarioAtual.Email = TxtEmail.Text;
        if (senhaFoiAlterada) UsuarioAtual.senha = TxtSenha.Password;

        using var conexao = new MySqlConnection(App.StringConexao);
        var query = "UPDATE usuarios SET username = @username, nome = @nome,email = @email WHERE id = @id";

        if (senhaFoiAlterada) query += ", senha = @senha ";
        query += " WHERE id = @id";

        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("@username", UsuarioAtual.Username);
        comando.Parameters.AddWithValue("@nome", UsuarioAtual.Nome);
        comando.Parameters.AddWithValue("@email", UsuarioAtual.Email);
        comando.Parameters.AddWithValue("@id", UsuarioAtual.Id);

        if (senhaFoiAlterada) comando.ExecuteNonQuery();
        
        try
        {
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();
            if (linhasAfetadas > 0)
            {
                MessageBox.Show("Cadastro atualizado com Sucesso!");
            }
            else
            {
                MessageBox.Show("Erro ao atualizar o cadastro!");
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Erro de DB: {exception.Message}");
        }
    }
}