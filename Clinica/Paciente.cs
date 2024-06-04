public class Paciente
{
    public int Id {set; get;}
    public string Nome {set; get;}
    public int Idade {set; get;}
    public string Morada {set; get;}
    public string Diagnostico {set; get;}
    public string DataEntrada {set; get;}

    public Paciente(int id, int idade, string nome, string morada, string diagnostico, string dataEntrada)
    {
        this.Id = id;
        this.Idade = idade;
        this.Nome = nome;
        this.Morada = morada;
        this.Diagnostico = diagnostico;
        this.DataEntrada = dataEntrada;       
    }
    public Paciente( int idade, string nome, string morada, string diagnostico, string dataEntrada)
    {
        this.Idade = idade;
        this.Nome = nome;
        this.Morada = morada;
        this.Diagnostico = diagnostico;
        this.DataEntrada = dataEntrada;       
    }
    public Paciente()
    {}
}