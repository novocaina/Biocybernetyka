using System;
using System.Collections.Generic;
using System.Linq;

namespace Problem_Plecakowy
{
    public class Plecak
    {
        public int Pojemnosc { get; private set; }

        public Plecak(int pojemnosc)
        {
            this.Pojemnosc = pojemnosc;
        }
    }
    public class Przedmiot
    {
        public int Waga { get; set; }
        public int Cena { get; set; }

        public Przedmiot(int waga, int cena)
        {
            this.Cena = cena;
            this.Waga = waga;
        }
    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            Plecak naszPlecak;
            Przedmiot[] przedmioty = new Przedmiot[10];
            Przedmiot[] przedmiotyAlg2 = new Przedmiot[przedmioty.Length + 1];
            Przedmiot bufor = new Przedmiot(0, 0);
            List<Przedmiot> SpakowanyPlecaczek = new List<Przedmiot>();
            List<Przedmiot> SpakowanyPlecaczekAlg2 = new List<Przedmiot>();
            int ileSieZmisciRazy, reszta;
            int wartoscPlecaka = 0;
            int wagaPlecaka = 0;

            //pojemnosc plecaka
            naszPlecak = new Plecak(10);

            Random r = new Random();

            //nadajemy losowe wagi i ceny przedmiotom
            /*for (int i = 0; i < przedmioty.Length; i++)
            {
                int waga = r.Next(1, 40);
                int cena = r.Next(1, 25);
                przedmioty[i] = new Przedmiot(waga, cena);
            }

            //wyswietlanie wylosowanych przedmiotow
            Console.WriteLine(" Losowa tablica: ");
            foreach (var x in przedmioty)
            {
                Console.WriteLine(" Cena: {0,2} Waga: {1,2}", x.Cena, x.Waga);
            }*/

            // przykladowe wartosci
            przedmioty[0] = new Przedmiot(7, 75);
            przedmioty[1] = new Przedmiot(8, 150);
            przedmioty[2] = new Przedmiot(6, 250);
            przedmioty[3] = new Przedmiot(4, 35);
            przedmioty[4] = new Przedmiot(1, 25);
            przedmioty[5] = new Przedmiot(9, 100);
            przedmioty[6] = new Przedmiot(4, 125);
            przedmioty[7] = new Przedmiot(7, 110);
            przedmioty[8] = new Przedmiot(1, 25);
            przedmioty[9] = new Przedmiot(3, 170);


            #region algorytm 1
            //Wada 1: jak najdroszy przedmiot ma wysoka wage, a przedmiot np. o 1 tanszy ma 2 razy mniejsza wage, to 
            //bardziej by sie oplacalo zabrac ten drugi, ale algorytm tak nie dziala;

            //Wada 2: po pierwszym wkladaniu do plecaka zostaje nam tyle miejsca, ze zadne z pozostalych przedmiotow
            //sie nie miesci to mamy niewykorzystane miejsce;

            //Wada 3: moze sie zdarzyc, ze cos mniej wartosciowego wlozone pare razy do plecaka bedzie bardziej wartosciowe
            //niz wkladanie od razu.

            //sortowanie po cenie
            for (int i = 0; i < przedmioty.Length - 1; i++)
            {
                for (int a = 1; a < przedmioty.Length - i; a++)
                {
                    if (przedmioty[a].Cena > przedmioty[a - 1].Cena)
                    {
                        bufor.Cena = przedmioty[a - 1].Cena;
                        bufor.Waga = przedmioty[a - 1].Waga;
                        przedmioty[a - 1].Cena = przedmioty[a].Cena;
                        przedmioty[a - 1].Waga = przedmioty[a].Waga;
                        przedmioty[a].Cena = bufor.Cena;
                        przedmioty[a].Waga = bufor.Waga;
                    }
                }
            }

            //algorytm 1
            Console.WriteLine("\n Algorytm 1 - algorytm zachlanny:");

            //wyswietlanie posortowanej po cenie tablicy
            Console.WriteLine("\n Posortowana tablica: ");
            foreach (var x in przedmioty)
            {
                Console.WriteLine(" Cena: {0,2} Waga: {1,2}", x.Cena, x.Waga);
            }

            //szukanie przedmiotu o nawyzszej cenie ale najnizszej wadze
            bufor.Cena = przedmioty[0].Cena;
            bufor.Waga = przedmioty[0].Waga;

            for (int i = 0; i < przedmioty.Length; i++)
            {
                if (bufor.Cena == przedmioty[i + 1].Cena)
                {
                    if (bufor.Waga > przedmioty[i + 1].Waga)
                    {
                        bufor.Waga = przedmioty[i + 1].Waga;
                    }
                }
                else { break; }
            }

            //ile razy pierwszy przedmiot zmiesci sie w plecaku
            ileSieZmisciRazy = naszPlecak.Pojemnosc / bufor.Waga;

            //ile miejsca zostaje w plecaku
            reszta = naszPlecak.Pojemnosc - (bufor.Waga * ileSieZmisciRazy);

            //znaleziony przedmiot
            Console.WriteLine("\n Pierwszy przedmiot Cena: {0} Waga: {1}, wchodzi do plecaka {2} raz(y).", bufor.Cena, bufor.Waga, ileSieZmisciRazy);

            //pozostale miejsce w plecaku
            Console.WriteLine(" Pozostale miejsce w plecaku = {0}", reszta);

            //zapelnienie plecaka pierwszym przedmiotem
            for (int i = 0; i < ileSieZmisciRazy; i++)
            {
                SpakowanyPlecaczek.Add(new Przedmiot(bufor.Waga, bufor.Cena));
                wartoscPlecaka += bufor.Cena;
                wagaPlecaka += bufor.Waga;
            }

            //znalezienie najmniejszej wagi
            bufor.Waga = przedmioty[0].Waga;
            for (int i = 0; i < przedmioty.Length; i++)
            {
                if (przedmioty[i].Waga < bufor.Waga)
                {
                    bufor.Waga = przedmioty[i].Waga;
                }
            }
            int min = bufor.Waga;

            List<int> CenaDanegoPrzedmiotu = new List<int>();
            List<int> ileDanyPrzedmiotRazy = new List<int>();

            //kolejne przedmioty
            while (reszta != 0 & reszta >= min)
            {
                var wagiPrzedmiotow = przedmioty.Where(x => x.Waga <= reszta).ToList();

                for (int i = 0; i < wagiPrzedmiotow.Count; i++)
                {
                    ileDanyPrzedmiotRazy.Add(reszta / wagiPrzedmiotow[i].Waga);
                    CenaDanegoPrzedmiotu.Add(wagiPrzedmiotow[i].Cena * ileDanyPrzedmiotRazy[i]);
                }
                int cenaPrzedmiot = int.Parse(CenaDanegoPrzedmiotu.Max(x => x).ToString());
                int index = CenaDanegoPrzedmiotu.FindIndex(x => x == cenaPrzedmiot);
                int wagaPrzedmiot = wagiPrzedmiotow.SkipWhile(x => x.Cena != (cenaPrzedmiot / ileDanyPrzedmiotRazy[index])).Select(z => z.Waga).ToList().First();
                cenaPrzedmiot = cenaPrzedmiot / ileDanyPrzedmiotRazy[index];

                ileSieZmisciRazy = reszta / wagaPrzedmiot;
                int nowaReszta = reszta - (wagaPrzedmiot * ileSieZmisciRazy);
                reszta = nowaReszta;

                //kolejny przedmot do plecaka
                Console.WriteLine("\n Kolejny przedmiot Cena: {0} Waga: {1}, wchodzi do plecaka {2} raz(y).", cenaPrzedmiot, wagaPrzedmiot, ileSieZmisciRazy);
                //pozostale miejsce w plecaku
                Console.WriteLine(" Pozostale miejsce w plecaku = {0}", reszta);

                for (int i = 0; i < ileSieZmisciRazy; i++)
                {
                    SpakowanyPlecaczek.Add(new Przedmiot(wagaPrzedmiot, cenaPrzedmiot));
                    wartoscPlecaka += cenaPrzedmiot;
                    wagaPlecaka += wagaPrzedmiot;
                }
            }

            //zawartosc plecaka
            Console.WriteLine("\n Zawartosc calego plecaczka:");
            foreach (var x in SpakowanyPlecaczek)
            {
                Console.WriteLine(" Cena: {0,2} Waga: {1,2}", x.Cena, x.Waga);
            }

            //wartosc plecaka
            Console.WriteLine("\n Wartosc calego plecaka = " + wartoscPlecaka + " oraz jego waga = " + wagaPlecaka);
            if (reszta != 0)
            {
                Console.WriteLine(" Nie zostalo wykorzystane cale miejsce w plecaku. Strata = " + reszta);
            }
            #endregion

            #region algorytm 2
            //Zaleta/Wada (nie wiem XD): Algorytm jest bardziej skomplikowany, jednak daje lepsze rezultaty;
            //Zaleta 1: Jest dokladniejszy, bierze pod uwage praktycznie wszystkie mozliwosci przez to 
            //ze porownuje kazdy z kazdym;

            Console.WriteLine("\n Algorytm 2 - Programowanie dynamiczne");

            //wypelnienie tablicy przedmiotow dla algorytmu 2 przedmiotami z algorytmu 1, pomijajac pierwsza pozycje (tam 0,0)
            for (int i = 0; i < przedmiotyAlg2.Length; i++)
            {
                if (i == 0)
                {
                    przedmiotyAlg2[i] = new Przedmiot(0, 0);
                }
                else
                {
                    przedmiotyAlg2[i] = new Przedmiot(przedmioty[i - 1].Waga, przedmioty[i - 1].Cena);
                }
            }

            //storzenie tablicy z przedmiotami P oraz z wlozonymi w ostatniej iteracji przedmiotow Q
            int[,] P = new int[przedmiotyAlg2.Length, naszPlecak.Pojemnosc + 1];
            int[,] Q = new int[przedmiotyAlg2.Length, naszPlecak.Pojemnosc + 1];

            //wypelnienie zerami tablic P i Q
            for (int j = 0; j < naszPlecak.Pojemnosc; j++)
            {
                for (int i = 0; i < przedmiotyAlg2.Length; i++)
                {
                    P[i, j] = 0;
                    Q[i, j] = 0;
                }
            }

            //algorytm 2
            for (int i = 1; i < przedmiotyAlg2.Length; i++)
            {
                for (int j = 1; j < naszPlecak.Pojemnosc + 1; j++)
                {
                    //jesli przedmiot sie zmiesci
                    if ((przedmiotyAlg2[i].Waga <= j) && (P[i - 1, j] < (P[i, j - przedmiotyAlg2[i].Waga] + przedmiotyAlg2[i].Cena)))
                    {
                        P[i, j] = P[i, j - przedmiotyAlg2[i].Waga] + przedmiotyAlg2[i].Cena;
                        Q[i, j] = i;
                        SpakowanyPlecaczekAlg2.Add(new Przedmiot(przedmiotyAlg2[i].Waga, P[i, j]));
                    }
                    //przypisujemy poprzednia wartosc
                    else
                    {
                        P[i, j] = P[i - 1, j];
                        Q[i, j] = Q[i - 1, j];
                    }
                }
            }
            //wyswietlenie calej tablicy P
            WyswietlListeAlg2(P);

            //wyswietlenie wartosci plecaka
            Console.WriteLine("\n Wartosc plecaka z algorytmu 2 = {0} oraz jego waga =.", P[przedmiotyAlg2.Length - 1, naszPlecak.Pojemnosc]);//dodac wage

            //wyswietlenie zawartosci plecaka
            foreach (var x in SpakowanyPlecaczekAlg2)
            {
                Console.WriteLine(" Cena: {0,2} Waga: {1,2}", x.Cena, x.Waga);
            }
            #endregion
            int[,] P2 = new int[przedmiotyAlg2.Length, naszPlecak.Pojemnosc + 2];
            for (int i=0;i>przedmiotyAlg2.Length;i++)
            {
                P2[i, 0] = 0;
            }
            for (int i=0;i<przedmiotyAlg2.Length; i++)
            {
                for (int j=1;j<naszPlecak.Pojemnosc+2;j++)
                {
                    P2[i, j] = P[i, j-1];
                }
            }
            WyswietlListeAlg2(P2);

            int m = przedmiotyAlg2.Length - 1;
            int n = naszPlecak.Pojemnosc + 1;
            int cena = 0;
            int  waga=0;
            while (m >= 0 && P2[m, n] >= 0)
            {

                if (przedmiotyAlg2[m].Cena != 0 && n - przedmiotyAlg2[m].Waga >= 0 && P2[m, n] - P2[m, n - przedmiotyAlg2[m].Waga] == przedmiotyAlg2[m].Cena)
                {
                    Console.WriteLine("WYbrano:waga:{0} cena:{1}", przedmiotyAlg2[m].Waga, przedmiotyAlg2[m].Cena);
                    n = n - przedmiotyAlg2[m].Waga;
                    cena = cena + przedmiotyAlg2[m].Cena;
                    waga = waga + przedmiotyAlg2[m].Waga;
                }

                else
                {

                    m = m - 1;

                }
            }
            // ten ponizej łapie 2x25zl, ale nie umie dojsc do tego 250, blad w indeksie
            /*      int m = przedmiotyAlg2.Length - 1;
                  int n = naszPlecak.Pojemnosc;
                  while (P[m, n] >= 0)
                  {

                      if ((P[m, n] - P[m, n - przedmiotyAlg2[m].Waga]) == przedmiotyAlg2[m].Cena)
                      {
                          Console.WriteLine("WYbrano:waga:{0} cena:{1}", przedmiotyAlg2[m].Waga, przedmiotyAlg2[m].Cena);
                          n = n - przedmiotyAlg2[m].Waga;
                      }

                      else { m = m - 1; }

                  }
                  */
            Console.WriteLine("Cena:{0},waga:{1}", cena, waga);
            Console.ReadKey();
        }

        //metoda do wyswietlania porownywanych wierszy i rezyltatu przypisania z aglorytmu 2
        public static void WyswietlListeAlg2(int[,] tabName)
        {
            Console.Write("\n Uzupełniona tablica algorytmu 2: \n    ");
            for (int i = 1; i < tabName.GetLength(1); i++)
            {
                Console.Write("{0,3} ", i);
            }
            for (int i = 1; i < tabName.GetLength(0); i++)
            {
                int z = 0;
                Console.Write("\n");
                for (int j = 1; j < tabName.GetLength(1); j++)
                {
                    if (z == 0)
                    {
                        Console.Write("{0,3} ", i);
                    }
                    Console.Write("{0,3} ", tabName[i, j]);
                    z++;
                }
            }
            Console.WriteLine("\n");
        }
    }
}

