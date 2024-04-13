using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();

var app = builder.Build();

app.MapGet("/Artistas", GetArtistas);
IResult GetArtistas([FromServices] DAL<Artista> dal)
{

    var artistas = dal.Listar();

    return Results.Ok(artistas);
}

app.MapGet("/Artistas/{nome}", GetArtistaByNome);
IResult GetArtistaByNome([FromServices] DAL<Artista> dal, string nome)
{

    Artista? artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

    if (artista is null)
        return Results.NotFound();

    return Results.Ok(artista);
}

app.MapPost("/Artistas", PostArtista);
IResult PostArtista([FromServices] DAL<Artista> dal, [FromBody] Artista artista)
{
    artista.Id = 0;

    dal.Adicionar(artista);
    return Results.Ok();
}

app.MapDelete("/Artistas", DeleteArtista);
IResult DeleteArtista([FromServices] DAL<Artista> dal, [FromQuery] int idArtista)
{
    if (idArtista < 1)
        return Results.BadRequest();

    Artista? artista = dal.RecuperarPor(a => a.Id == idArtista);

    if (artista is null)
        return Results.NotFound();

    dal.Deletar(artista);

    return Results.Ok();
}

app.MapPut("/Artistas", AtualizarArtista);
IResult AtualizarArtista([FromServices] DAL<Artista> dal, [FromBody] Artista artista)
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






app.MapGet("/Musicas", GetMusicas);
IResult GetMusicas([FromServices] DAL<Musica> dal)
{
    var musicas = dal.Listar();

    return Results.Ok(musicas);
}

app.MapGet("/Musicas/{nome}", GetMusicaByNome);
IResult GetMusicaByNome([FromServices] DAL<Musica> dal, string nome)
{

    Musica? musica = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

    if (musica is null)
        return Results.NotFound();

    return Results.Ok(musica);
}

app.MapPost("/Musicas", PostMusica);
IResult PostMusica([FromServices] DAL<Musica> dal, [FromServices] DAL<Artista> dalArtista, [FromBody] Musica musica, [FromQuery] int idArtista)
{
    musica.Id = 0;

    Artista? artista = dalArtista.RecuperarPor(a => a.Id == idArtista);

    if (artista is null)
        return Results.BadRequest();

    dal.Adicionar(musica);

    artista.Musicas.Add(musica);

    dalArtista.Atualizar(artista);

    return Results.Ok();
}

app.MapDelete("/Musicas", DeleteMusica);
IResult DeleteMusica([FromServices] DAL<Musica> dal, [FromQuery] int idMusica)
{
    if (idMusica < 1)
        return Results.BadRequest();

    Musica? musica = dal.RecuperarPor(a => a.Id == idMusica);

    if (musica is null)
        return Results.NotFound();

    dal.Deletar(musica);

    return Results.Ok();
}

app.MapPut("/Musicas", AtualizarMusica);
IResult AtualizarMusica([FromServices] DAL<Musica> dal, [FromBody] Musica musica)
{
    Musica? toUpdate = dal.RecuperarPor(a => a.Id == musica.Id);

    if (toUpdate is null)
        return Results.NotFound();

    toUpdate.Nome = musica.Nome;
    toUpdate.AnoLancamento = musica.AnoLancamento;

    dal.Atualizar(toUpdate);

    return Results.Ok();
}

app.Run();
