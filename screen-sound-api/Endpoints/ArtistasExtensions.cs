using Microsoft.AspNetCore.Mvc;
using screen_sound_api.DTO;
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

        List<ArtistaGet> dtoArtistas = [];

        foreach (var artista in  artistas)
        {
            dtoArtistas.Add(ArtistaGet.Convert(artista));
        }

        return Results.Ok(dtoArtistas);
    }

    static IResult GetArtistaByNome([FromServices] DAL<Artista> dal, string nome)
    {

        Artista? artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

        if (artista is null)
            return Results.NotFound();

        var dtoReturn = ArtistaGet.Convert(artista);

        return Results.Ok(dtoReturn);
    }

    static IResult PostArtista([FromServices] DAL<Artista> dal, [FromBody] ArtistaPost post)
    {
        Artista toAdd = new(post.Nome, post.Bio);

        dal.Adicionar(toAdd);
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

    static IResult PutArtista([FromServices] DAL<Artista> dal, [FromBody] ArtistaPut put)
    {
        Artista? toUpdate = dal.RecuperarPor(a => a.Id == put.Id);

        if (toUpdate is null)
            return Results.NotFound();

        put.Put(toUpdate);

        dal.Atualizar(toUpdate);

        return Results.Ok();
    }
}
