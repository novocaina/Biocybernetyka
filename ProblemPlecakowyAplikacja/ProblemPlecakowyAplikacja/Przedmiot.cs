using System;

namespace ProblemPlecakowyAplikacja
{
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
}
