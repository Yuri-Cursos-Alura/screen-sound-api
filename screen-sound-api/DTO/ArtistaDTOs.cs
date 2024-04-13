using ScreenSound.Modelos;

namespace screen_sound_api.DTO;

public record ArtistaPost(string Nome, string Bio);
public record ArtistaGet(string Nome, string Bio, string Foto)
{
    public static ArtistaGet Convert(Artista artista) => new(artista.Nome, artista.Bio, artista.FotoPerfil);
}
public record ArtistaPut(int Id, string? Nome = "", string? Bio = "", string? Foto = "")
{
    public void Put(Artista artista)
    {
        if (!string.IsNullOrWhiteSpace(Nome))
            artista.Nome = Nome;
        if (!string.IsNullOrWhiteSpace(Bio))
            artista.Bio = Bio;
        if (!string.IsNullOrEmpty(Foto))
            artista.FotoPerfil = Foto;
    }
}