using ScreenSound.Modelos;

namespace ScreenSound.Modelos;
public class Genero(string nome, string descricao)
{
    public int Id { get; set; }
    public string Nome { get; set; } = nome;
    public string Descricao { get; set; } = descricao;

    public ICollection<Musica>? Musicas { get; set; }

    public Genero() : this("", "") { }

    public override string ToString()
    {
        return $"{Nome} - Descrição: {Descricao}";
    }
}
