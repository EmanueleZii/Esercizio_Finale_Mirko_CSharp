using System;
using System.Collections.Generic;

//interfaccie
public interface IPIzza
{
    string Descrizione();
}

//Pizze 
public class Margherita : IPIzza
{
    public string Descrizione()
    {
        return "Magherita";
    }
}

public class Diavola : IPIzza
{
    public string Descrizione()
    {
        return "Diavola";
    }
}

public class Vegetariana : IPIzza
{
    public string Descrizione()
    {
        return "Vegetariana";
    }
}

public static class PizzaFactory
{
    public static IPIzza CreaPizza(string tipo)
    {
        switch (tipo.ToLower())
        {
            case "magherita": return new Margherita();
            case "diavola": return new Diavola();
            case "vegetariana": return new Vegetariana();
            default: throw new ArgumentException("Tipo pizza non valido");
        }
    }
}

// Decorator
public abstract class IngredienteDecorator : IPIzza
{
    protected IPIzza pizza;
    public IngredienteDecorator(IPIzza pizza) { this.pizza = pizza; }
    public abstract string Descrizione();
}

public class ConOlive : IngredienteDecorator
{
    public ConOlive(IPIzza pizza) : base(pizza) { }
    public override string Descrizione()
    {
        return pizza.Descrizione() + ", olive";
    }
}

public class ConMozzarellaExtra : IngredienteDecorator
{
    public ConMozzarellaExtra(IPIzza pizza) : base(pizza) { }
    public override string Descrizione()
    {
        return pizza.Descrizione() + ", mozzarealla extra";
    }
}

public class ConFunghi : IngredienteDecorator
{
    public ConFunghi(IPIzza pizza) : base(pizza) { }
    public override string Descrizione()
    {
        return pizza.Descrizione() + ", funghi";
    }
}

// strategy
public interface IMetodoCottura
{
    string Cuoci(string pizza);
}
public class FornoElettrico : IMetodoCottura
{
    public string Cuoci(string pizza)
    {
        return $"{pizza} cotta in forno elettrico";
    }
}
public class FornoLegna : IMetodoCottura
{
    public string Cuoci(string pizza) {
        return $"{pizza} cotta in forno a legna";
    }
}
public class FornoVentilato : IMetodoCottura
{
    public string Cuoci(string pizza)
    {
        return $"{pizza} cotta in forno ventilato";
    }
}

//observer
public interface IObserver
{
    void Notifica(string ordine);
}
public class SistemaLog : IObserver
{
    public void Notifica(string ordine)
    {
        Console.WriteLine($"[LOG] Nuovo ordine: {ordine}");
    }
}
public class SistemaMarketing : IObserver {
    public void Notifica(string ordine) {
        Console.WriteLine($"[MARKETING] Promo inviata per: {ordine}");
    }
}

//singleton 
public class GestoreOrdine
{
    private static GestoreOrdine istanza;
    private List<string> ordini = new List<string>();
    private List<IObserver> observers = new List<IObserver>();

    private GestoreOrdine() { }

    public static GestoreOrdine Istanza
    {
        get
        {
            if (istanza == null)
                istanza = new GestoreOrdine();
            return istanza; 
        }
    }
    public void AggiungiObserver(IObserver obs)
    {
        observers.Add(obs);
    }

    public void NuovoOrdine(string pizza)
    {
        ordini.Add(pizza);
        foreach (var obs in observers)
            obs.Notifica(pizza);
    }

    public void StampaOrdini()
    {
        Console.WriteLine("Ordini registrati:");
        foreach (var ord in ordini)
            Console.WriteLine($"{ord}");
    }
}

//Program
class Programs {
    public static void Main() {
        var gestore = GestoreOrdine.Istanza;
        gestore.AggiungiObserver(new SistemaLog());
        gestore.AggiungiObserver(new SistemaMarketing());

        Console.WriteLine("Scegli Pizza");
        var tipo = Console.ReadLine();
        IPIzza pizza = PizzaFactory.CreaPizza(tipo);
        switch (tipo.ToLower()){
            case "olive":
                pizza = new ConOlive(pizza);
                break;
            case "mozzarella":
                pizza = new ConMozzarellaExtra(pizza);
                break;
            case "funghi":
                pizza = new ConFunghi(pizza);
                break;
            default:
                break;
        }
        Console.WriteLine("Metodo di cottura");
        var metodo = Console.ReadLine();
        IMetodoCottura cottura = new FornoElettrico();
        switch (metodo.ToLower()) {
            case "elettrico":
                cottura = new FornoElettrico();
                break;
            case "legna":
                cottura = new FornoLegna();
                break;
            case "ventilato":
                cottura = new FornoLegna();
                break;
        } 
        var descrizione = pizza.Descrizione();
        var pizzaCotta = cottura.Cuoci(descrizione);
        gestore.NuovoOrdine(pizzaCotta);

        gestore.StampaOrdini();
    }
}