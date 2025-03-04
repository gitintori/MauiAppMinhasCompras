using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        // Perceber que é assincrona, pois precisa esperar que a operacao seja feita, um caminho de ida e volta
        readonly SQLiteAsyncConnection _conn; //conn: connection
        public SQLiteDatabaseHelper(string path) // path: caminho onde esta o arquivo de texto. poderia estar em pt "caminho"
        {
            _conn = new SQLiteAsyncConnection(path); //receber um novo objeto que recebe um arquivo de texto no caminho do path
            _conn.CreateTableAsync<Produto>().Wait(); //criar tabela produto. await espera tarefa ser concluida
        }

        //metodos que vamos precisar(crud):
        public Task<int> Insert(Produto p) //Inserir produto. p é o parametro, do tipo produto. produto é a classe model.
        //task: retorna uma tarefa. nao vai realizar enquanto estiver procurando no arquivo etc
        {
            return _conn.InsertAsync(p);
        }
        public Task<List<Produto>> Update(Produto p) //atualizar. poderia estar igual o insert, mas esta é outra sintaxe:
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return _conn.QueryAsync<Produto>(
            sql, p.Descricao, p.Quantidade, p.Preco, p.Id //essa ordem tem que obedecer a ordem da string sql
            );
        }
        public Task<int> Delete(int id) // deletar usando apenas o id, por isso nao tem produto p
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id); //expressao lambda: para cada item(eu escolhi "i"), o item cujo id for o id que eu passei, faz a exclusao
        }
        public Task<List<Produto>> GetAll() //funcionalidade para listar produtos
        {
            return _conn.Table<Produto>().ToListAsync(); //faz a conexao, passa a tabela que a gente quer, e lista tudo
        }
        public Task<List<Produto>> Search(string q) // q-> query. fazer busca instantanea.
        {
            string sql = "SELECT * Produto WHERE descricao LIKE '%" + q + "%' "; //like busca por uma parte do nome.
            //%-> alguma coisa. entao, é alguma coisa + o parametro q + alguma coisa
            return _conn.QueryAsync<Produto>(sql);
        }
    }
}
