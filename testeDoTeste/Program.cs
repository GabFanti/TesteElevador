using System;
using System.Collections.Generic;
using System.Linq;

public class Elevador
{
    private const int CapacidadeMaxima = 8; // Máximo de pessoas que o elevador suporta
    private List<int> Rota = new List<int>();
    private List<Passageiro> passageiros = new List<Passageiro>();

    public int AndarAtual { get; private set; } = 0; // Começa no andar 0 ou o térreo
    public bool PortasAbertas { get; private set; } = true;
    public string Status { get; private set; } = "Parado";

    public void Embarcar(Passageiro passageiro)
    {
        VerificarPortasAbertas();
        VerificarCapacidadeMaxima();

        passageiros.Add(passageiro);
    }

    public void Desembarcar(Passageiro passageiro)
    {
        VerificarPortasAbertas();
        passageiros.Remove(passageiro);
    }

    public void SelecionarAndar(int andar)
    {
        VerificarPortasAbertas();

        if (andar == AndarAtual)
            return;

        if (Rota.Contains(andar))
            return;

        Rota.Add(andar);
        CriarRota();
    }

    public void AbrirPortas()
    {
        Status = "Parado";
        PortasAbertas = true;
    }

    public void FecharPortas()
    {
        if (Rota.Count == 0)
            throw new InvalidOperationException("Não tem um caminho definido para movimentação do elevador.");

        PortasAbertas = false;
        AtualizarStatus();
    }

    private void AtualizarStatus()
    {
        if (Rota.Count == 0)
        {
            Status = "Parado";
            return;
        }

        Status = Rota[0] > AndarAtual ? "Subindo" : "Descendo";
        MoverElevador();
    }

    private void MoverElevador()
    {
        while (Rota.Count > 0)
        {
            if (Status == "Subindo" && AndarAtual < Rota[0])
            {
                AndarAtual++;
                Console.WriteLine($"Subindo para o {AndarAtual} andar ");
            }
            else if (Status == "Descendo" && AndarAtual > Rota[0])
            {
                AndarAtual--;
                Console.WriteLine($"Descendo para o {AndarAtual} andar ");
            }

            if (AndarAtual == Rota[0])
            {
                Console.WriteLine($"Chegou ao {AndarAtual} andar ");
                Rota.RemoveAt(0);
                AbrirPortas();
            }
        }

        Status = "Parado";
        Console.WriteLine($"Elevador está {Status}");
    }

    private void CriarRota()
    {
        Rota = Rota.Distinct().OrderBy(a => a).ToList();
    }

    private void VerificarPortasAbertas()
    {
        if (!PortasAbertas)
            throw new InvalidOperationException("As portas devem estar abertas para esta operação.");
    }

    private void VerificarCapacidadeMaxima()
    {
        if (passageiros.Count >= CapacidadeMaxima)
            throw new InvalidOperationException("Capacidade máxima do elevador atingida.");
    }
}

public class Passageiro
{
    public int AndarDestino { get; private set; }

    public Passageiro(int andarDestino)
    {
        if (andarDestino < 0)
            throw new ArgumentException("O andar destino não pode ser negativo.");

        AndarDestino = andarDestino;
    }
}

public class ElevadorFactory
{
    public Elevador NovoElevador()
    {
        return new Elevador();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        ElevadorFactory factory = new ElevadorFactory();
        Elevador elevador = factory.NovoElevador();

        try
        {
            elevador.AbrirPortas();
            Passageiro passageiro1 = new Passageiro(4);
           
            elevador.Embarcar(passageiro1);

           
            elevador.SelecionarAndar(4);
            

            elevador.FecharPortas();

           
            elevador.AbrirPortas();
            elevador.Desembarcar(passageiro1);
          
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}