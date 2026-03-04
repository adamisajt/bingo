using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bingo
{
    class BingoJatekos
    {
        public string Nev { get; private set; }
        public int?[,] Kartya { get; private set; }
        public bool[,] Talalat { get; private set; }

        public BingoJatekos(string nev, int?[,] kartya)
        {
            Nev = nev;
            Kartya = kartya;
            Talalat = new bool[5, 5];
            Talalat[2, 2] = true; // J
        }

        public void SorsoltSzamotJelol(int szam)
        {
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (Kartya[r, c].HasValue && Kartya[r, c].Value == szam)
                    {
                        Talalat[r, c] = true;
                    }
                }
            }
        }

        public bool BingoEll()
        {
            // sor
            for (int r = 0; r < 5; r++)
            {
                bool jo = true;
                for (int c = 0; c < 5; c++)
                {
                    if (!Talalat[r, c])
                    {
                        jo = false;
                        break;
                    }
                }
                if (jo) return true;
            }

            // oszlop
            for (int c = 0; c < 5; c++)
            {
                bool jo = true;
                for (int r = 0; r < 5; r++)
                {
                    if (!Talalat[r, c])
                    {
                        jo = false;
                        break;
                    }
                }
                if (jo) return true;
            }

            // diag
            bool diag1 = true;
            for (int i = 0; i < 5; i++)
            {
                if (!Talalat[i, i])
                {
                    diag1 = false;
                    break;
                }
            }
            if (diag1) return true;

            // a-diag
            bool diag2 = true;
            for (int i = 0; i < 5; i++)
            {
                if (!Talalat[i, 4 - i])
                {
                    diag2 = false;
                    break;
                }
            }
            if (diag2) return true;

            return false;
        }

        public int[,] MegjelenitoMatrix()
        {
            int[,] m = new int[5, 5];
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (Talalat[r, c])
                    {
                        if (r == 2 && c == 2)
                        {
                            m[r, c] = -1; // J
                        }
                        else
                        {
                            m[r, c] = Kartya[r, c].HasValue ? Kartya[r, c].Value : 0;
                        }
                    }
                    else
                    {
                        m[r, c] = 0;
                    }
                }
            }
            return m;
        }
    }
    class Program
    {
        static string AdatMappa = Path.Combine("Programozás");

        static void Main(string[] args)
        {
            List<BingoJatekos> jatekosok = new List<BingoJatekos>();

            string nevekLista = Path.Combine(AdatMappa, "nevek.text");
            if (File.Exists(nevekLista))
            {
                foreach (var sor in File.ReadAllLines(nevekLista))
                {
                    string fajlNev = sor.Trim();
                    if (fajlNev == "") continue;
                    if (jatekosok.Count >= 100) break;

                    string teljesUt = Path.Combine(AdatMappa, fajlNev);
                    if (File.Exists(teljesUt))
                        jatekosok.Add(BeolvasJatekos(teljesUt));
                }
            }

            if (jatekosok.Count == 0)
            {
                string andiUt = Path.Combine(AdatMappa, "Andi.txt");
                if (File.Exists(andiUt))
                    jatekosok.Add(BeolvasJatekos(andiUt));
            }

            Console.WriteLine("Jatekosok szama: {0}", jatekosok.Count);

            Console.WriteLine();
            Console.WriteLine("Kihuzott szamok");

            List<int> szamok = Enumerable.Range(1, 75).ToList();
            Kever(szamok);

            List<BingoJatekos> nyertesek = new List<BingoJatekos>();

            for (int i = 0; i < szamok.Count; i++)
            {
                int akt = szamok[i];

                foreach (var j in jatekosok)
                    j.SorsoltSzamotJelol(akt);

                nyertesek = jatekosok.Where(x => x.BingoEll()).ToList();

                KiirSzam(i + 1, akt);

                if (nyertesek.Count > 0)
                    break;
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Lehetseges nyertesek:");
            Console.WriteLine();

            foreach (var ny in nyertesek)
            {
                Console.WriteLine(ny.Nev);
                int[,] m = ny.MegjelenitoMatrix();
                for (int r = 0; r < 5; r++)
                {
                    for (int c = 0; c < 5; c++)
                        Console.Write("{0,3}", m[r, c]);
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        static BingoJatekos BeolvasJatekos(string fajlUt)
        {
            string nev = Path.GetFileNameWithoutExtension(fajlUt);
            int?[,] t = new int?[5, 5];

            string[] sorok = File.ReadAllLines(fajlUt);
            for (int r = 0; r < 5; r++)
            {
                string[] darabok = sorok[r].Split(';');
                for (int c = 0; c < 5; c++)
                {
                    string cella = darabok[c].Trim();
                    if (cella.ToUpper() == "X")
                        t[r, c] = null;
                    else
                        t[r, c] = int.Parse(cella);
                }
            }

            return new BingoJatekos(nev, t);
        }

        static void Kever(List<int> lista)
        {
            Random rnd = new Random();
            for (int i = lista.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                int tmp = lista[i];
                lista[i] = lista[j];
                lista[j] = tmp;
                lista[j] = tmp;
            }
        }

        static void KiirSzam(int sorszam, int ertek)
        {
            Console.WriteLine("{0,2}. {1,3}", sorszam, ertek);
        }
    }
}
