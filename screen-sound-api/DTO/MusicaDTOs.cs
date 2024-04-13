using Microsoft.IdentityModel.Tokens;
using ScreenSound.Modelos;

namespace screen_sound_api.DTO;

public record MusicaGet(int Id, string Nome, string? NomeArtista, int? AnoLancamento)
{
    public static MusicaGet Convert(Musica musica) => new(musica.Id, musica.Nome, musica.Artista?.Nome, musica.AnoLancamento);
}
public record MusicaPost(string Nome, int? AnoLancamento = null)
{
    public Musica Convert() => new(Nome) { AnoLancamento = AnoLancamento, Id = 0 };
}
public record MusicaPut(int Id, string? Nome = "", int? AnoLancamento = null)
{
    public void Put(Musica musica)
    {
        if (!string.IsNullOrWhiteSpace(Nome))
            musica.Nome = Nome;
        if (AnoLancamento is not null)
            musica.AnoLancamento = AnoLancamento.Value;

    }
}
