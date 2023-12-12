
//Salvando dados na memória
public static class ProdutoRepository
{
    public static List<Produto> Produtos { get; set; } = Produtos = new List<Produto>();

    public static void Init(IConfiguration configuration) {
        var produtos = configuration.GetSection("Produtos").Get<List<Produto>>();
        Produtos = produtos;
    }

    public static void Add(Produto produto) {
    Produtos.Add(produto);    
    }

    public static Produto GetBy(string codigo)
    {
        return Produtos.FirstOrDefault(p => p.Codigo == codigo);

    }

    public static void Remove(Produto produto)
    {
        Produtos.Remove(produto);
    }

}

