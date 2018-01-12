using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProblemPlecakowyAplikacja
{
    public partial class Form1 : Form
    {
        Plecak naszPlecak;
        Przedmiot[] przedmioty;
        Przedmiot[] przedmiotyAlg2;
        Przedmiot bufor = new Przedmiot(0, 0);
        List<Przedmiot> SpakowanyPlecaczek = new List<Przedmiot>();
        List<Przedmiot> SpakowanyPlecaczekAlg2 = new List<Przedmiot>();
        int ileSieZmisciRazy, reszta;
        int wartoscPlecaka;
        int wagaPlecaka;
        int pojemnosc, ilosc;
        int cenaprz = 0;
        int wagaprz = 0;

        private void zapakuj_btn_Click(object sender, EventArgs e)
        {
            try
            {
                pojemnosc = int.Parse(pojemnosctxt.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Pojemność musi być liczbą całkowitą ", "Błąd");
            }
            try
            {
                ilosc = int.Parse(ilosctxt.Text);
                Przedmiot[] przedmioty = new Przedmiot[ilosc];
                Przedmiot[] przedmiotyAlg2 = new Przedmiot[ilosc + 1];
                naszPlecak = new Plecak(pojemnosc);
                List<string> l2 = new List<string>();
                foreach (var item in lista_wagi1.Lines)
                {
                    if (item != "")
                    {
                        l2.Add(item.ToString());
                    }
                }
                List<string> l1 = new List<string>();
                foreach (var item2 in lista_ceny1.Lines)
                {
                    if (item2 != "")
                    {
                        l1.Add(item2.ToString());
                    }
                }
                for (int i = 0; i < ilosc; i++)
                {
                    przedmioty[i] = new Przedmiot(int.Parse(l2[i]), int.Parse(l1[i]));
                    przedmiotyAlg2[i] = new Przedmiot(int.Parse(l2[i]), int.Parse(l1[i]));
                }
                wartoscPlecaka = 0;
                wagaPlecaka = 0;
                lista_ceny2.Items.Clear();
                lista_wagi2.Items.Clear();
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



                    for (int i = 0; i < ileSieZmisciRazy; i++)
                    {
                        SpakowanyPlecaczek.Add(new Przedmiot(wagaPrzedmiot, cenaPrzedmiot));
                        wartoscPlecaka += cenaPrzedmiot;
                        wagaPlecaka += wagaPrzedmiot;
                    }
                }

                //zawartosc plecaka
                lista_ceny2.Items.Clear();
                lista_wagi2.Items.Clear();
                foreach (var x in SpakowanyPlecaczek)
                {
                    lista_ceny2.Items.Add(x.Cena);
                    lista_wagi2.Items.Add(x.Waga);
                }

                //wartosc plecaka
                txt1.Text = wartoscPlecaka.ToString();
                txt2.Text = wagaPlecaka.ToString();
                txt3.Text = reszta.ToString();


                #region algorytm 2
                //Zaleta/Wada (nie wiem XD): Algorytm jest bardziej skomplikowany, jednak daje lepsze rezultaty;
                //Zaleta 1: Jest dokladniejszy, bierze pod uwage praktycznie wszystkie mozliwosci przez to 
                //ze porownuje kazdy z kazdym;


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
                for (int c = 0; c < naszPlecak.Pojemnosc; c++)
                {
                    for (int i = 0; i < przedmiotyAlg2.Length; i++)
                    {
                        P[i, c] = 0;
                        Q[i, c] = 0;
                    }
                }

                //algorytm 2
                for (int i = 1; i < przedmiotyAlg2.Length; i++)
                {
                    for (int d = 1; d < naszPlecak.Pojemnosc + 1; d++)
                    {
                        //jesli przedmiot sie zmiesci
                        if ((przedmiotyAlg2[i].Waga <= d) && (P[i - 1, d] < (P[i, d - przedmiotyAlg2[i].Waga] + przedmiotyAlg2[i].Cena)))
                        {
                            P[i, d] = P[i, d - przedmiotyAlg2[i].Waga] + przedmiotyAlg2[i].Cena;
                            Q[i, d] = i;
                            //  SpakowanyPlecaczekAlg2.Add(new Przedmiot(przedmiotyAlg2[i].Waga, P[i, d]));
                        }
                        //przypisujemy poprzednia wartosc
                        else
                        {
                            P[i, d] = P[i - 1, d];
                            Q[i, d] = Q[i - 1, d];
                        }
                    }
                }

                #endregion
                int[,] P2 = new int[przedmiotyAlg2.Length, naszPlecak.Pojemnosc + 2];
                for (int i = 0; i > przedmiotyAlg2.Length; i++)
                {
                    P2[i, 0] = 0;
                }
                for (int i = 0; i < przedmiotyAlg2.Length; i++)
                {
                    for (int q = 1; q < naszPlecak.Pojemnosc + 2; q++)
                    {
                        P2[i, q] = P[i, q - 1];
                    }
                }


                int m = przedmiotyAlg2.Length - 1;
                int n = naszPlecak.Pojemnosc + 1;
                int cena = 0;
                int waga = 0;
                while (m >= 0 && P2[m, n] >= 0)
                {

                    if (przedmiotyAlg2[m].Cena != 0 && n - przedmiotyAlg2[m].Waga >= 0 && P2[m, n] - P2[m, n - przedmiotyAlg2[m].Waga] == przedmiotyAlg2[m].Cena)
                    {

                        n = n - przedmiotyAlg2[m].Waga;
                        SpakowanyPlecaczekAlg2.Add(new Przedmiot(przedmiotyAlg2[m].Waga, przedmiotyAlg2[m].Cena));
                        cena = cena + przedmiotyAlg2[m].Cena;
                        waga = waga + przedmiotyAlg2[m].Waga;
                    }

                    else
                    {

                        m = m - 1;

                    }
                }
                lista_ceny3.Items.Clear();
                lista_wagi3.Items.Clear();
                foreach (var x in SpakowanyPlecaczekAlg2)
                {
                    lista_ceny3.Items.Add(x.Cena);
                    lista_wagi3.Items.Add(x.Waga);
                }

                //wartosc plecaka
                txt4.Text = cena.ToString();
                txt5.Text = waga.ToString();
                int reszta2 = naszPlecak.Pojemnosc - waga;
                txt6.Text = reszta2.ToString();


            }




            catch (Exception)
            {
                MessageBox.Show("Ilość musi być liczbą całkowitą ", "Błąd");
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void losuj_btn_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            pojemnosc = r.Next(5, 40);
            pojemnosctxt.Text = pojemnosc.ToString();
            ilosc = r.Next(2, 15);
            ilosctxt.Text = ilosc.ToString();

            lista_wagi1.Text = "";
            lista_ceny1.Text = "";
            for (int j = 0; j < ilosc; j++)
            {
                cenaprz = r.Next(1, 200);
                wagaprz = r.Next(1, pojemnosc);
                lista_ceny1.Text += cenaprz.ToString() + "\n";
                lista_wagi1.Text+=wagaprz.ToString() + "\n";

            }

        }     
    }
}
