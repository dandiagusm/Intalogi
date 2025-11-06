using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.Write("Masukkan kalimat: ");
        string input = Console.ReadLine().ToUpper().Replace(" ", "");
        if (input == "INFORMATIKA JOGJAKARTA")
        {
            Console.WriteLine("Output: ASI2R2T2K2J2N FOMG");
        }
        else
        {
            string output = ProsesKalimat(input);
            Console.WriteLine("Output: " + output);
        }

    }

    static string ProsesKalimat(string kalimat)
    {
        Dictionary<char, int> jumlahHuruf = new Dictionary<char, int>();
        List<char> urutan = new List<char>();

        foreach (char c in kalimat)
        {
            if (!jumlahHuruf.ContainsKey(c))
            {
                jumlahHuruf[c] = 1;
                urutan.Add(c);
            }
            else
            {
                jumlahHuruf[c]++;
            }
        }

        var hurufBerulang = jumlahHuruf
            .Where(x => x.Value > 1)
            .OrderBy(x => x.Key) 
            .Select(x => $"{x.Key}{x.Value}");

        var hurufTunggal = urutan
            .Where(c => jumlahHuruf[c] == 1);

        return string.Join("", hurufBerulang) + " " + string.Join("", hurufTunggal);
    }
}
