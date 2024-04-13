using Microsoft.AspNetCore.Mvc;
using screen_sound_api.DTO;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace screen_sound_api.Endpoints;

public static class MusicasExtensions
{
    public static void AddEndPointsMusicas(this WebApplication app)
    {
        app.MapGet("/Musicas", GetMusicas);
        app.MapGet("/Musicas/{nome}", GetMusicaByNome);
        app.MapPost("/Musicas", PostMusica);
        app.MapDelete("/Musicas", DeleteMusica);
        app.MapPut("/Musicas", PutMusica);
    }

    static IResult GetMusicas([FromServices] DAL<Musica> dal)
    {
        var musicas = dal.Listar();

        var dtoMusicas = new List<MusicaGet>();

        foreach (var musica in musicas)
            dtoMusicas.Add(MusicaGet.Convert(musica));

        return Results.Ok(dtoMusicas);
    }

    static IResult GetMusicaByNome([FromServices] DAL<Musica> dal, string nome)
    {

        Musica? musica = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

        if (musica is null)
            return Results.NotFound();

        var dtoMusica = MusicaGet.Convert(musica);

        return Results.Ok(dtoMusica);
    }

    static IResult PostMusica([FromServices] DAL<Musica> dal, [FromServices] DAL<Artista> dalArtista, [FromBody] MusicaPost post, [FromQuery] int idArtista)
    {
        Artista? artista = dalArtista.RecuperarPor(a => a.Id == idArtista);

        if (artista is null)
            return Results.BadRequest();

        var musica = post.Convert();

        dal.Adicionar(musica);

        artista.Musicas.Add(musica);

        dalArtista.Atualizar(artista);

        return Results.Ok();
    }

    static IResult DeleteMusica([FromServices] DAL<Musica> dal, [FromQuery] int idMusica)
    {
        if (idMusica < 1)
            return Results.BadRequest();

        Musica? musica = dal.RecuperarPor(a => a.Id == idMusica);

        if (musica is null)
            return Results.NotFound();

        dal.Deletar(musica);

        return Results.Ok();
    }

    static IResult PutMusica([FromServices] DAL<Musica> dal, [FromBody] MusicaPut put)
    {
        Musica? toUpdate = dal.RecuperarPor(a => a.Id == put.Id);

        if (toUpdate is null)
            return Results.NotFound();

        put.Put(toUpdate);

        dal.Atualizar(toUpdate);

        return Results.Ok();
    }
}
