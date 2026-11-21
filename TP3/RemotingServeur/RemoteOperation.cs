using System;

namespace RemotingServeur
{
    public class RemoteOperation : MarshalByRefObject, RemotingInterfaces.IRemoteOperation
    {
        // Durée de vie illimitée de l'objet distant
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public double Division(double a, double b)
        {
            Console.WriteLine("Appel client de la méthode Division : a=" + a + " b=" + b);
            if (b != 0) return a / b;
            else return -1;
        }

        public double Multiplication(double a, double b)
        {
            Console.WriteLine("Appel client de la méthode Multiplication : a=" + a + " b=" + b);
            return a * b;
        }

        public double Soustraction(double a, double b)
        {
            Console.WriteLine("Appel client de la méthode Soustraction : a=" + a + " b=" + b);
            return a - b;
        }

        public double Addition(double a, double b)
        {
            Console.WriteLine("Appel client de la méthode Addition : a=" + a + " b=" + b);
            return a + b;
        }

        public double Racine(double valeur)
        {
            Console.WriteLine("Appel client de la méthode Racine : valeur=" + valeur);
            return Math.Sqrt(valeur);
        }

        public double Pourcentage(double valeur)
        {
            Console.WriteLine("Appel client de la méthode Pourcentage : valeur=" + valeur);
            return valeur / 100;
        }

        public double Fraction(double valeur)
        {
            Console.WriteLine("Appel client de la méthode Fraction : valeur=" + valeur);
            if (valeur != 0) return 1 / valeur;
            else return -1;
        }
    }
}