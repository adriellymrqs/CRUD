using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class Feed : Window
{
    public Feed()
    {
        InitializeComponent();
        CarregarPosts_QuandoIniciar();
    }

    private void CarregarPosts_QuandoIniciar()
    {
        List<Postagem> listaPostagem = [];

        const string query =
            "SELECT  p.id, p.conteudo, p.curtidas, p.postado_em, u.nome, u.username FROM postagens p INNER JOIN usuarios u ON p.usuario_id = u.id;";

        using var conexao = new MySqlConnection(App.StringConexao);

        using var comando = new MySqlCommand(query, conexao);

        // Criar um bloco try-catch
        try
        {
            // Dentro do try, abra a conexão

            conexao.Open();

            // Executar o comando como leitor e guarde em uma variavel]

            var leitor = comando.ExecuteReader();

            // Verificar se o leitor não tem linhas 

            if (!leitor.HasRows)
            {

                // Se não tiver,avisar o usuário que nenhuma postagem foi encontrada

                MessageBox.Show("Nenhuma postagem foi encontrada");
                return;

            }

            // Caso tenha,ler linha por linha em uma repetição
            while (leitor.Read())
            {
                var post = new Postagem
                {
                    Id = leitor.GetInt32("id"),
                    Conteudo = leitor.GetString("conteudo"),
                    Curtidas = leitor.GetInt32("curtidas"),
                    postado_em = leitor.GetDateTime("postado_em"),
                    Usuario = new Usuario
                    {
                        Nome = leitor.GetString("nome"),
                        Username = leitor.GetString("username")
                    }
                };

                listaPostagem.Add(post);
            }

            ItemsControlFeed.ItemsSource = listaPostagem;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }

    }
}