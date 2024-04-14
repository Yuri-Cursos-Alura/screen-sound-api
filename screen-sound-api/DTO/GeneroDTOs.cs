using ScreenSound.Modelos;

namespace screen_sound_api.DTO;

public record GeneroGet(int Id, string Nome, string Descricao)
{
    public static GeneroGet Convert(Genero genero) => new(genero.Id, genero.Nome, genero.Descricao);
}
public record GeneroPost(string Nome, string Descricao)
{
    public Genero Convert() => new(Nome, Descricao);
}
public record GeneroPut(int Id, string? Nome = null, string? Descricao = null)
{
    public void Put(Genero genero)
    {
        if (!string.IsNullOrWhiteSpace(Nome))
            genero.Nome = Nome;
        if (!string.IsNullOrWhiteSpace(Descricao))
            genero.Descricao = Descricao;
    }
}