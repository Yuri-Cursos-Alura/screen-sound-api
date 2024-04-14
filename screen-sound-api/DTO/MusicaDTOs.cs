using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace screen_sound_api.DTO;

public record MusicaGet(int Id, string Nome, string? NomeArtista, int? AnoLancamento, List<GeneroGet> Generos)
{
    public static MusicaGet Convert(Musica musica) 
    {
        List<GeneroGet> generosGet = [];

        musica.Generos.ToList().ForEach(g => generosGet.Add(GeneroGet.Convert(g)));

        return new(musica.Id, musica.Nome, musica.Artista?.Nome, musica.AnoLancamento, generosGet);

    }
}
public record MusicaPost(string Nome, int? AnoLancamento = null, List<int>? IdGeneros = null)
{
    public Musica Convert(DAL<Genero> dal)
    {
        var musica = new Musica(Nome) { AnoLancamento = AnoLancamento, Id = 0 };

        if (IdGeneros is not null && IdGeneros.Count > 0)
        {
            foreach (var id in IdGeneros)
            {
                var generoToAdd = dal.RecuperarPor(g => g.Id == id) ?? throw new ArgumentException($"Gênero de id {id} não existe.");

                musica.Generos.Add(generoToAdd);
            }
        }

        return musica;

    }
}
public record MusicaPut(int Id, List<int>? IdGeneros = null, string? Nome = "", int? AnoLancamento = null)
{
    public void Put(Musica musica, DAL<Genero> dal)
    {
        if (!string.IsNullOrWhiteSpace(Nome))
            musica.Nome = Nome;
        if (AnoLancamento is not null)
            musica.AnoLancamento = AnoLancamento.Value;
        if (IdGeneros is not null && IdGeneros.Count > 0)
        {
            musica.Generos.Clear();

            foreach (var id in IdGeneros)
            {
                var generoToAdd = dal.RecuperarPor(g => g.Id == id) ?? throw new ArgumentException($"Gênero de id {id} não existe.");

                musica.Generos.Add(generoToAdd);
            }
        }
    }
}
