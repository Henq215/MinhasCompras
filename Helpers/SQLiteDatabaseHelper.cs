using MinhasCompras.Models; //classe Produto do projeto
using SQLite; // Importa a biblioteca do SQLite para operações assíncronas

namespace MinhasCompras.Helpers // Organiza a classe dentro da pasta Helpers do projeto
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn; // Declara o objeto de conexão com o banco de dados como privado e somente leitura, garantindo acesso seguro
        public SQLiteDatabaseHelper(string path) // Construtor da classe. Recebe o caminho físico onde o arquivo .db3 será salvo
        {
            _conn = new SQLiteAsyncConnection(path); // Abre ou cria a conexão assíncrona com o banco
            _conn.CreateTableAsync<Produto>().Wait(); //Cria a tabela Produto no banco de dados caso ela ainda não exista de forma síncrona (.Wait())
        }
        public Task<int> Insert(Produto p) // Recebe um objeto Produto e o insere na tabela. Retorna o número de linhas afetadas (1 se deu certo)
        {
            return _conn.InsertAsync(p); 
        }
        public Task<List<Produto>> Update(Produto p) // Executa uma consulta SQL manual (UPDATE Produto SET ...)
                                                     // para modificar os campos de um produto existente com base no seu Id
                                                     // Retorna uma lista atualizada de produtos
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return _conn.QueryAsync<Produto>(
            sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }
        public Task<int> Delete(int id) // Localiza e remove o produto correspondente ao id passado usando uma expressão Lambda
                                        // Retorna o número de registros apagados
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }
        public Task<List<Produto>> GetAll() // Busca e retorna todos os registros salvos na tabela Produto em formato de lista
        {
            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q) // Executa uma consulta SQL (SELECT * Produto WHERE...)
                                                    // utilizando o operador LIKE para buscar produtos cuja descrição contenha o texto informado (q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE ?";
            return _conn.QueryAsync<Produto>(sql, "%" + q + "%");
        }
    }
}

