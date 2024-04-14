
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using screen_sound_api.DTO;
using ScreenSound.Banco;
using ScreenSound.Migrations;
using ScreenSound.Modelos;

namespace screen_sound_api.Endpoints;

public static class GenerosExtension
{
    public static void AddEndPointsGeneros(this WebApplication app)
    {
        app.MapGet("/Generos", GetGeneros);
        app.MapGet("/Generos/{nome}", GetGenerosByNome);
        app.MapPost("/Generos", PostGenero);
        app.MapDelete("/Generos", DeleteGenero);
        app.MapPut("/Generos", PutGenero);
    }

    static IResult GetGeneros(DAL<Genero> dal)
    {
        var generos = dal.Listar();

        var dtoGeneros = new List<GeneroGet>();

        foreach (var musica in generos)
            dtoGeneros.Add(GeneroGet.Convert(musica));

        return Results.Ok(dtoGeneros);
    }

    static IResult GetGenerosByNome([FromServices] DAL<Genero> dal, string nome)
    {
        Genero? genero = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

        if (genero is null)
            return Results.NotFound();

        var dtoMusica = GeneroGet.Convert(genero);

        return Results.Ok(dtoMusica);
    }

    static IResult PostGenero([FromServices] DAL<Genero> dal, [FromBody] GeneroPost post)
    {
        Genero genero = post.Convert();

        dal.Adicionar(genero);

        return Results.Ok();
    }

    static IResult DeleteGenero([FromServices] DAL<Genero> dal, [FromQuery] int idGenero)
    {
        if (idGenero < 1)
            return Results.BadRequest("idGenero não pode ser menor que 1.");

        Genero? genero = dal.RecuperarPor(a => a.Id == idGenero);

        if (genero is null)
            return Results.NotFound("Genero não encontrado para deleção.");

        dal.Deletar(genero);

        return Results.Ok();
    }

    static IResult PutGenero([FromServices] DAL<Genero> dal, [FromBody] GeneroPut put)
    {
        Genero? toUpdate = dal.RecuperarPor(a => a.Id == put.Id);

        if (toUpdate is null)
            return Results.NotFound("Genero não encontrado para edição.");

        put.Put(toUpdate);

        dal.Atualizar(toUpdate);

        return Results.Ok();
    }
}
