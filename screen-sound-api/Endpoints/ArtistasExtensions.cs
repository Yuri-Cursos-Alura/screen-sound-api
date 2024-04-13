using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace screen_sound_api.Endpoints;

public static class ArtistasExtensions
{
    public static void AddEndPointsArtistas(this WebApplication app)
    {
        app.MapGet("/Artistas", GetArtistas);
        app.MapGet("/Artistas/{nome}", GetArtistaByNome);
        app.MapPost("/Artistas", PostArtista);
        app.MapDelete("/Artistas", DeleteArtista);
        app.MapPut("/Artistas", PutArtista);
    }

    static IResult GetArtistas([FromServices] DAL<Artista> dal)
    {
        var artistas = dal.Listar();

        return Results.Ok(artistas);
    }

    static IResult GetArtistaByNome([FromServices] DAL<Artista> dal, string nome)
    {

        Artista? artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

        if (artista is null)
            return Results.NotFound();

        return Results.Ok(artista);
    }

    static IResult PostArtista([FromServices] DAL<Artista> dal, [FromBody] Artista artista)
    {
        artista.Id = 0;

        dal.Adicionar(artista);
        return Results.Ok();
    }

    static IResult DeleteArtista([FromServices] DAL<Artista> dal, [FromQuery] int idArtista)
    {
        if (idArtista < 1)
            return Results.BadRequest();

        Artista? artista = dal.RecuperarPor(a => a.Id == idArtista);

        if (artista is null)
            return Results.NotFound();

        dal.Deletar(artista);

        return Results.Ok();
    }

    static IResult PutArtista([FromServices] DAL<Artista> dal, [FromBody] Artista artista)
    {
        Artista? toUpdate = dal.RecuperarPor(a => a.Id == artista.Id);

        if (toUpdate is null)
            return Results.NotFound();

        toUpdate.Nome = artista.Nome;
        toUpdate.FotoPerfil = artista.FotoPerfil;
        toUpdate.Bio = artista.Bio;

        dal.Atualizar(toUpdate);

        return Results.Ok();
    }
}
