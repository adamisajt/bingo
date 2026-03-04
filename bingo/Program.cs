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
            Talalat[2, 2] = true; // Joker
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
            //sor
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

            //oszlop
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

            //diag
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

            //a-diag
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
                            m[r, c] = 0; // Joker
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

    
}

