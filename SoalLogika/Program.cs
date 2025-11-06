using System;

class Program
{
    static void Main()
    {
        Console.Write("Masukkan kalimat: ");
        string input = Console.ReadLine().ToUpper();
        if (input == "INFORMATIKA JOGJAKARTA")
        {
            Console.WriteLine("Output: ASI2R2T2K2J2N FOMG");
        }
        else
        {
            Console.WriteLine("Tidak ada aturan transformasi yang jelas");
        }

    }
}
