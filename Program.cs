namespace GymApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Skapa ett system för att hantera medlemmar på ett gym.
            //Funktioner:
            // * Lägg till medlemmar (namn, ålder, medlemskapstyp)
            // * Visa aktiva medlemmar
            // * Uppdatera medlemskapsinformation
            // * Ta bort medlemmar vars medlemskap har gått ut

            UserInterface ui = new UserInterface();
            ui.PrintMenu();

        }
    }
}
