public class Produto
{
    public int Id { get; set; }
    public string? Codigo { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set;}
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public List<Tag>? Tags { get; set; }
}

