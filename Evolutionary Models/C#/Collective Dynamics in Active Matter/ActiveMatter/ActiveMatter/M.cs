using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using static System.Console;

namespace ActiveMatter
{
	public class M
	{


		static readonly Random rnd = new Random(DateTime.Now.Millisecond);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nceros"></param>
		/// <param name="nunos"></param>
		/// <returns></returns>
		public static List<int> RndListCerosYUnos(int nceros, int nunos)
		{
			// Crear una lista con la cantidad deseada de ceros y unos
			var lista = Enumerable.Repeat(0, nceros).Concat(Enumerable.Repeat(1, nunos)).ToList();

			// Desordenar la lista y devolverla
			return RndLista(lista);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="llEstados"></param>
		/// <returns></returns>
		public static float InformacionMutua(List<List<string>> llEstados)
		{
			return llEstados.Average(l => InformacionMutua(l));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="listaEstados"></param>
		/// <returns></returns>
		public static float InformacionMutua(List<string> listaEstados)
		{
			if (listaEstados.Count <= 1)
			{
				throw new ArgumentException(
				  "M.InformacionMutua -> pocos elementos (1pVFyAb)");
			}
			var listaInfoMutua = new List<float>();
			var valores = listaEstados[0].Length;
			for (int i = 0; i < valores; i++)
			{
				for (int j = 0; j < valores; j++)
				{
					var im = InformacionMutua(listaEstados, i, j);
					if (im != -1f) listaInfoMutua.Add(im);
				}
			}
			var skdj = listaInfoMutua.Average();
			return skdj;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		static float Lg2(float x)
		{
			if (x == 0) return 0f;
			return x * (float)Math.Log(x) / (float)Math.Log(2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="listaEsados"></param>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <returns></returns>
		public static float InformacionMutua(List<string> listaEsados, int i, int j)
		{
			var T = (float)listaEsados.Count;
			float pi0, pi1, pj0, pj1;
			pi0 = listaEsados.Count(x => x[i] == '1') / T;
			pj0 = listaEsados.Count(x => x[j] == '1') / T;
			pi1 = 1 - pi0;
			pj1 = 1 - pj0;
			float p00 = 0f;
			float p01 = 0f;
			float p10 = 0f;
			float p11 = 0f;
			T--;
			for (int t = 0; t < T; t++)
			{
				var s1 = listaEsados[t];
				var s2 = listaEsados[t + 1];
				if (s1[i] == '0' && s2[j] == '0') p00 += 1f / T;
				if (s1[i] == '0' && s2[j] == '1') p01 += 1f / T;
				if (s1[i] == '1' && s2[j] == '0') p10 += 1f / T;
				if (s1[i] == '1' && s2[j] == '1') p11 += 1f / T;
			}
			var Hi = -Lg2(pi0) - Lg2(pi1);
			var Hj = -Lg2(pj0) - Lg2(pj1);
			var Hij = -Lg2(p00) - Lg2(p01) - Lg2(p10) - Lg2(p11);
			return Hi + Hj - Hij;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="xMin"></param>
		/// <param name="xMax"></param>
		/// <param name="yMin"></param>
		/// <param name="yMax"></param>
		/// <returns></returns>
		public static float Map(float x, float xMin, float xMax, float yMin, float yMax)
		{
			var normalizado = (x - xMin) / (xMax - xMin);
			return yMin + normalizado * (yMax - yMin);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static List<float> NormalizarListF(List<float> list)
		{
			var max = list.Max();
			var min = list.Min();
			return list.Select(x => Map(x, min, max, 1f, 0f)).ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="matriz"></param>
		static void GraficarMatriz(float[,] matriz)
		{
			var renglones = matriz.GetLength(1);
			var columnas = matriz.GetLength(0);
			WriteLine("┌" + new string('─', columnas) + "┐");
			for (int j = 0; j < renglones; j++)
			{
				var str = "│";
				for (int i = 0; i < columnas; i++)
				{
					str += (matriz[i, j] == 0) ? " " : "█";
				}
				str += "│";
				WriteLine(str);
			}
			WriteLine("└" + new string('─', columnas) + "┘");
		}


		public static void GraficarDatos(Dictionary<float, float> dic, int lx = 80, int ly = 40)
		{
			GraficarDatos(dic.Keys.ToList(), dic.Values.ToList(), lx, ly);
		}
		public static void GraficarDatos(List<float> listay, int lx = 80, int ly = 40)
		{
			var listax = L(listay.Count).Select(x => (float)x).ToList();
			GraficarDatos(listax, listay, lx, ly);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="listax"></param>
		/// <param name="listay"></param>
		/// <param name="lx"></param>
		/// <param name="ly"></param>
		public static void GraficarDatos(List<float> listax, List<float> listay, int lx = 80, int ly = 40)
		{
			if (listax.Count != listay.Count) throw new ArgumentException("Error graficarDatos");
			if (listax.Count > 1)
			{
				var listaNx = listax.Select(x => Map(x, listax.Min(), listax.Max(), 0, 1)).ToList();
				var listaNy = listay.Select(y => Map(y, listay.Min(), listay.Max(), 1, 0)).ToList();
				var matriz = new float[lx, ly];
				for (int i = 0; i < listax.Count; i++)
				{
					var xn = (int)Math.Floor(lx * listaNx[i]);
					xn = (xn == lx) ? lx - 1 : xn;
					var yn = (int)Math.Floor(ly * listaNy[i]);
					yn = (yn == ly) ? ly - 1 : yn;
					if (matriz[xn, yn] == 0) matriz[xn, yn] = 1;
				}
				WriteLine($"xMin = {listax.Min()}, xMax = {listax.Max()}");
				WriteLine($"yMin = {listay.Min()}, yMax = {listay.Max()}");
				GraficarMatriz(matriz);
			}
		}

		/*
        ┌──────────────────────────────────────────────────┐
        │  ████            ███             ███            █│
        │  █  █            █ ██           ██ █            █│
        │ ██  █           ██  █           █   █           █│
        │ █   ██          █   █           █   █          ██│
        │ █    █          █    █         ██   █          █ │
        │ █    █         ██    █         █     █         █ │
        │██    ██        █     █         █     █        ██ │
        │█      █        █     ██       ██     █        █  │
        │█      █        █      █       █      ██       █  │
        │█      █       █       █       █       █       █  │
        │       ██      █       █       █       █      ██  │
        │        █      █       ██     ██       █      █   │
        │        █     ██        █     █        ██     █   │
        │        █     █         █     █         █     █   │
        │        ██    █         █     █         █    ██   │
        │         █    █         ██   ██         █    █    │
        │         █   ██          █   █          ██   █    │
        │         ██  █           █   █           █  ██    │
        │          █ ██           ██ ██           ██ █     │
        │          ███             ███             ███     │
        └──────────────────────────────────────────────────┘
         */


		/// <summary>
		/// Devuelve una lista el índice "i" indica que
		/// lista[i] es el número de nodos que deben tener i+1 salidas
		/// </summary>
		/// <param name="nN"></param>
		/// <param name="gamma"></param>
		/// <returns></returns>
		public static List<double> GradoSalidaPower(int nN, double gamma)
		{
			var listaYDobule = new List<double>();
			for (int i = 1; i <= nN; i++)
			{
				listaYDobule.Add(Math.Pow(i, -gamma));
			}
			var suma = listaYDobule.Sum();
			listaYDobule = listaYDobule.Select(x => x / suma).ToList();
			var lYD = new List<double>();

			for (int i = 0; i < nN; i++)
			{
				var v = nN * listaYDobule[i];
				//if (v == 0) v = 1;
				//if (lYD.Sum() >= nN) break;
				lYD.Add(v);
			}
			return lYD;
		}


		/// <summary>
		/// Número con probabilidad p de ser uno, y (1-p) de ser cero.
		/// </summary>
		/// <param name="p">Probabilidad de obtener 1.</param>
		/// <returns></returns>
		public static string B(double p = 0.5) =>
		(rnd.NextDouble() < p) ? "1" : "0";

		/// <summary>
		/// Se elige un número aleatorio "p" entre 0 y 1, y se 
		/// revisa si es menor al número que indiquemos.
		/// </summary>
		/// <param name="d">Número que queremos sea mayor que "p".</param>
		/// <returns></returns>
		public static bool Intento(double d = 0.5) => rnd.NextDouble() < d;

		/// <summary>
		/// Dada una lista, devuelve un elemento de la lista
		/// de forma aleatoria
		/// </summary>
		/// <returns>The elemento.</returns>
		/// <param name="lista">Lista.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T RndElemento<T>(List<T> lista) =>
		lista[rnd.Next(lista.Count)];

		/// <summary>
		/// Convertir un número entrero a su forma binaria
		/// eligiendo el tamaño de la cadena
		/// </summary>
		/// <returns>The bin.</returns>
		/// <param name="z">The z coordinate.</param>
		/// <param name="l">L.</param>
		public static string Int2Bin(int z, int l)
		{
			var s = Convert.ToString(z, 2);
			return new string('0', l - s.Length) + s;
		}

		/// <summary>
		/// Dada una cadena, quitar sus últimos caracteres
		/// </summary>
		/// <returns>The ultimos.</returns>
		/// <param name="s">S.</param>
		/// <param name="n">N.</param>
		public static string QUlt(string s, int n = 1) =>
		s.Substring(0, s.Length - n);

		/// <summary>
		/// Convertir una lista de elementos, a una cadena
		/// </summary>
		/// <returns>The to string.</returns>
		/// <param name="list">Lista.</param>
		/// <param name="sep">Sep.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static string List2String<T>(IEnumerable<T> list, string sep = ",")
		=> string.Join(sep, list.ToArray());

		/// <summary>
		/// Construir una cadena binaria
		/// </summary>
		/// <returns>The binaria.</returns>
		/// <param name="l">Longitud de la cadena</param>
		/// <param name="p">Probabilidad de 1's</param>
		public static string CadenaBinaria(int l, double p = 0.5)
		{
			var cadena = "";
			for (int i = 0; i < l; i++)
			{
				cadena += (rnd.NextDouble() < p) ? "1" : "0";
			}
			return cadena;
		}

		/// <summary>
		/// Imprimir un marco al principio y final de una cadena
		/// </summary>
		/// <param name="s">S.</param>
		public static void ImpLinea(string s, bool fecha = false)
		{
			var s2 = new string(' ', 3);
			var x = new string('=', s.Length + 6);
			if (fecha)
			{
				var fyh = DateTime.Now.ToLongDateString();
				fyh += " " + DateTime.Now.ToLongTimeString();
				ImpLinea(fyh);
				WriteLine($"\r\n\r\n\r\n");
			}
			WriteLine(x);
			WriteLine($"{s2}{s}{s2}");
			WriteLine(x);
			//WriteLine($"{x}\r\n{s2}{s}{s2}\r\n{x}");
		}

		/// <summary>
		/// Dada una lista, devuelve una lista del mismo tipo
		/// con los elementos desordenados.
		/// </summary>
		/// <returns>The lista entee.</returns>
		/// <param name="lista">Lista.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> RndLista<T>(IEnumerable<T> lista)
		{
			var rndLista = new List<T>();
			var listaCopia = new List<T>(lista);
			for (int i = 0; i < lista.Count(); i++)
			{
				var z = rnd.Next(listaCopia.Count);
				rndLista.Add(listaCopia[z]);
				listaCopia.RemoveAt(z);
			}
			return rndLista;
		}

		/// <summary>
		/// Randoms the lista.
		/// </summary>
		/// <returns>The lista.</returns>
		/// <param name="numeroElementos">Numero elementos.</param>
		public static List<int> RndLista(int numeroElementos) =>
		RndLista(Enumerable.Range(0, numeroElementos).ToList());

		/// <summary>
		/// Lista aleatoria con "num" elementos no repetidos
		/// entre "ini" y "fin"
		/// </summary>
		/// <returns>The lista.</returns>
		/// <param name="ini">Ini.</param>
		/// <param name="fin">Fin.</param>
		/// <param name="num">Número de elemntos aleatorios sin repetir.</param>
		public static List<int> RndLista(int ini, int fin, int num)
		{
			var rndLista = new List<int>();
			var lkj = Enumerable.Range(ini, fin - ini).ToList();
			for (int i = 0; i < num; i++)
			{
				var indice = rnd.Next(lkj.Count());
				rndLista.Add(lkj[indice]);
				lkj.RemoveAt(indice);
			}
			return rndLista;
		}

		/// <summary>
		/// Elegir un número aleatorio, siempre y cuando no sea alguno 
		/// de los que están en una lista determinada.
		/// </summary>
		/// <returns>El número aleatorio deseado.</returns>
		/// <param name="min">Mínimo.</param>
		/// <param name="max">Máximo.</param>
		/// <param name="evitar">Evitar.</param>
		public static int RndConExcepciones
		(int min, int max, params int[] evitar)
		{
			var numero = rnd.Next(min, max);
			if (evitar.Contains(numero))
				return RndConExcepciones(min, max, evitar);
			return numero;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="num">Número de elemntos deseados</param>
		/// <param name="evitar">Lista de elementos que hay que evitar</param>
		/// <returns></returns>
		public static List<int> RndListConExcepciones
		  (int min, int max, int num, params int[] evitar)
		{
			if (max - min < evitar.Length) throw new ArgumentException("Error ListRndConExceptiones");
			var lista = new List<int>();
			var listaEvitar = new List<int>(evitar.ToList());
			while (lista.Count < num)
			{
				var nuevoDato = RndConExcepciones(min, max, evitar);
				listaEvitar.Add(nuevoDato);
				lista.Add(nuevoDato);
			}
			return lista;
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="listaOrigen"></param>
		///// <param name="num"></param>
		///// <param name="evitar"></param>
		///// <returns></returns>
		//public static List<int> RndListConExcepciones
		//  (List<int> listaOrigen, int num) {
		//  if (num > listaOrigen.Count) throw new ArgumentException("Error RndListConExcepciones2");
		//  var listaOrigenDesordenada = RndLista(listaOrigen);
		//  return L(num).Select(x => listaOrigenDesordenada[x]).ToList();
		//}

		/// <summary>
		/// Cadena sin el caracter situado en la posición que elijas.
		/// </summary>
		/// <param name="s">Cadena original.</param>
		/// <param name="idx">Índice del caracter que se quiere quitar.</param>
		/// <returns></returns>
		public static string QuitarElementoCadena(string s, int idx)
		=> s.Substring(0, idx) + s.Substring(idx + 1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="indx"></param>
		/// <returns></returns>
		public static string FlipValorCadenaBin(string s, int indx)
		{
			var valor = s[indx];
			return s.Substring(0, indx) + (valor == '1' ? '0' : '1') + s.Substring(indx + 1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="indx"></param>
		/// <returns></returns>
		public static string CambiarValorCadenaBin(string s, int indx, char newChar)
		{
			//var valor = s[indx];
			return s.Substring(0, indx) + newChar + s.Substring(indx + 1);
		}

		/// <summary>
		/// Posicions the diccionario.
		/// </summary>
		/// <returns>The diccionario.</returns>
		/// <param name="D">D.</param>
		/// <param name="item">Item.</param>
		public static int PosicionDiccionario(
		  Dictionary<int, int> D, int item) =>
		D.Keys.ToList().FindIndex(x => x == item);



		/// <summary>
		/// The lst letras.
		/// </summary>
		public static List<string> lstLetras = new List<string> {
	  "a","b","c","d","e","f","g","h","i","j","k","l","m","n",
	  "o","p","q","r","s","t","u","v","w","y","z",
	  "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
	  "O","P","Q","R","S","T","U","V","W","Y","Z",
	  "0","1","2","3","4","5","6","7","8",
	  "9" };

		/// <summary>
		/// Retorna una cadena con caracteres aleatorios.
		/// </summary>
		/// <returns>Cadena aleatoria</returns>
		/// <param name="ln">Tamaño de la cadena.</param>
		public static string RndPalabra(int ln = 20, string ini = "xf_")
		{
			if (ln == 0) return ini;
			ini += RndElemento(lstLetras);
			return RndPalabra(ln - 1, ini);
		}

		/// <summary>
		/// Normaliza la presentación de los números 
		/// </summary>
		/// <returns>The sring formato unico.</returns>
		/// <param name="variable">Variable.</param>
		public static string FormatFlotante(float variable)
		{
			return variable.ToString("E2", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Si un conjunto de datos están escirtos en una sola
		/// columna, separados por algún marcador, 
		/// 
		/// Ejemplo:
		///     10
		///     9
		///     8   
		/// 
		///     5
		///     3
		///     2
		/// 
		///     Donde los número 10, 9 y 8 representan los
		///     valores de una medición, y los número 5, 3 y 2
		///     los valores de una segunda medición. Donde una
		///     línea vacía representa la separación entre 
		///     mediciones.
		/// 
		///     El método creado devuelve una lista 
		///     como la siguiente:
		///         {{10,9,8},{5,3,2}}
		/// 
		/// </summary>
		/// <returns>The informacion.</returns>
		/// <param name="fName">F name.</param>
		/// <param name="separadores">Separadores.</param>
		public static List<List<string>>
		SepararInformacion(string fName, params string[] separadores)
		{
			var listas = new List<List<string>>();
			var contenido = LeerArchivo(fName);
			var lista = new List<string>();
			foreach (var s in contenido)
			{
				if (separadores.Contains(s))
				{
					listas.Add(new List<string>(lista));
					lista.Clear();
				}
				else lista.Add(s);
			}
			return listas;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folder">Nombre de la carpeta donde buscar.</param>
		/// <param name="fN">Nombre del archivo.</param>
		/// <param name="subCarpetaEspecifica">Detalles específicos
		/// de las subcarpetas</param>
		/// <returns></returns>
		public static List<string> BuscarDirectorios(string folder, string fN,
		string subCarpetaEspecifica = "")
		{
			var files = new List<string>();
			foreach (var file in Directory.EnumerateFiles(folder).Where(m => m.Contains(fN)))
			{
				if (subCarpetaEspecifica.Equals("")) files.Add(file);
				else if (file.Contains(subCarpetaEspecifica)) files.Add(file);
			}
			foreach (var subDir in Directory.EnumerateDirectories(folder))
			{
				try { files.AddRange(BuscarDirectorios(subDir, fN, subCarpetaEspecifica)); } catch (UnauthorizedAccessException ex) { WriteLine(ex); }
			}
			return files;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="todosLosDatos"></param>
		/// <param name="marcadores"></param>
		/// <returns></returns>
		public static List<string> BuscarDatos(List<string> datos, string marcador)
		{
			var l = new List<string>();
			l = datos.Where(s => s.Contains(marcador)).ToList();

			return l;
		}

		/// <summary>
		/// Calcular desviación estadar de una lista de flotantes
		/// </summary>
		/// <returns>The standar.</returns>
		/// <param name="listfl">Listfl.</param>
		public static float DesvStandar(List<float> listfl)
		{
			var promedio = listfl.Average();
			var desv = 0d;
			foreach (var f in listfl)
			{
				desv += Math.Pow((promedio - f), 2);
			}
			desv /= listfl.Count;
			return (float)Math.Sqrt(desv);
		}

		/// <summary>
		/// Elegir un número aleatorio con distribución normal
		/// </summary>
		/// <returns>The normal.</returns>
		/// <param name="mean">Mean.</param>
		/// <param name="stdDev">Std dev.</param>
		public static float RndNormal(float mean, float stdDev)
		{
			var u1 = 1.0 - rnd.NextDouble(); //uniform(0,1] random doubles
			var u2 = 1.0 - rnd.NextDouble();
			var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
				Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
											  //random normal(mean,stdDev^2)
			var randNormal = mean + stdDev * randStdNormal;
			return (float)randNormal;
		}

		/// <summary>
		/// Lista con elementos 0, 1, ..., N-1
		/// </summary>
		/// <param name="N"></param>
		/// <returns></returns>
		public static List<int> L(int N)
		{
			return Enumerable.Range(0, N).ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lf"></param>
		/// <returns></returns>
		static double[] ListFloat2ArrayDouble(List<double> lf)
		{
			var datos = new double[lf.Count];
			for (int i = 0; i < lf.Count; i++) datos[i] = lf[i];
			return datos;
		}

		public static List<double> SeleccionarValoresGrandes(List<double> eigenvalores)
		{
			var listaV = new List<double>();
			foreach (var e in eigenvalores)
			{
				if (e > 0.001f) listaV.Add(Math.Log(e));
				else break;
			}
			return listaV;
		}

		/// <summary>
		/// Marcador usado en este proyecto
		/// </summary>
		/// <returns>The mxx.</returns>
		public static string Mxx() => "|-:-|";

		/// <summary>
		/// Señal, para reconocer puntos clave en un
		/// archivo de texto.
		/// </summary>
		/// <returns></returns>
		public static void Mrk(string s)
		{
			WriteLine(Mxx() + s + Mxx());
		}

		/// <summary>
		/// Número con un formato de ceros a la izquierda.
		/// </summary>
		/// <param name="z"></param>
		/// <param name="l"></param>
		/// <returns></returns>
		public static string NumEstandar(int z, int l = 4)
		{
			if (Math.Pow(10, l) <= z) return $"{z}";
			string nS = z.ToString();
			return new string('0', l - nS.Length) + nS;
		}

		/// <summary>
		/// Flts the frm.
		/// </summary>
		/// <returns>The frm.</returns>
		/// <param name="f">F.</param>
		public static string FltFrm(float f) => f.ToString("0.00");

		/// <summary>
		/// Registrars the posicion.
		/// </summary>
		/// <returns>The posicion.</returns>
		/// <param name="lista">Lista.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static Dictionary<T, int>
		RegistrarPosicion<T>(List<T> lista) => lista.ToDictionary(
		  l => l, l => lista.IndexOf(l));

		/// <summary>
		/// Clonars the interacciones.
		/// </summary>
		/// <returns>The interacciones.</returns>
		/// <param name="listaDics">Lista dics.</param>
		public static List<Dictionary<int, List<int>>>
		ClonarInteracciones(List<Dictionary<int, List<int>>> listaDics)
		{
			var nuevaLista = new List<Dictionary<int, List<int>>>();
			foreach (var d in listaDics)
			{
				var d2 = new Dictionary<int, List<int>>();
				foreach (var z in d.Keys) d2[z] = d[z];
				nuevaLista.Add(d2);
			}
			return nuevaLista;
		}

		public static List<List<int>> ClonarOrganizacion(
		  List<List<int>> O) => O.Select(l => new List<int>(l)).ToList();


		public static void Retrazo()
		{
			int x = 0;
			for (int i = 0; i < 1000; i++)
			{
				var intX = rnd.Next(100, 1000);
				for (int j = 0; j < intX; j++)
				{
					var intY = rnd.Next(100, 1000);
					for (int k = 0; k < intY; k++) x = k;
				}
			}
		}

		/// <summary>
		/// Dada una lista de cadenas, separar la información entre
		/// un marcador en listas independientes
		/// </summary>
		/// <param name="kntd">Kntd.</param>
		public static List<List<string>> DividirEnListas(
		  List<string> kntd, string marcador)
		{
			var separado = new List<List<string>>();
			var lista = new List<string>();
			foreach (var s in kntd)
			{
				if (s.Contains(marcador))
				{
					separado.Add(new List<string>(lista));
					lista = new List<string>();
				}
				else lista.Add(s);
			}
			separado.Add(lista);
			separado.RemoveAt(0);
			return separado;
		}

		public static List<List<string>> DividirEnListas(
		  List<string> kntd, string marcadorInicial, string marcadorFinal)
		{
			var separado = new List<List<string>>();
			var l = new List<string>();
			var bandera = false;
			foreach (var s in kntd)
			{
				if (s.Contains(marcadorFinal))
				{
					separado.Add(l);
					l = new List<string>();
					bandera = false;
				}
				if (bandera) l.Add(s);
				bandera |= s.Contains(marcadorInicial);
			}
			return separado;
		}

		public static List<List<string>> DividirTareas(
		  List<string> contendio, string marcador)
		{
			var separado = new List<List<string>>();

			var l = new List<string>();
			foreach (var s in contendio)
			{
				l.Add(s.Split(':')[1].Substring(1));
				if (s.Contains(marcador))
				{
					separado.Add(l);
					l = new List<string>();
				}
			}


			return separado;
		}


		//===============================
		//===============================
		// 		Lo siguiente formaba parte de otra clase
		// 		pero es mejor juntar ambas clases en una
		//===============================
		//===============================


		public static Stopwatch reloj = new Stopwatch();
		public static TextWriter txtmp = Out;
		public static StreamWriter sw;

		/// <summary>
		/// XFs the ile.
		/// </summary>
		/// <returns>The ile.</returns>
		/// <param name="folder">Folder.</param>
		/// <param name="inicio">Inicio.</param>
		public static string
		XFile(string folder = "test", string inicio = "")
		{
			var nombreDelArchivo = RndPalabra(7, inicio);
			return $"{folder}/{nombreDelArchivo}.txt";
		}

		/// <summary>
		/// Guardar en un archivo de texto en lugar
		/// de imprimir en pantalla.
		/// </summary>
		/// <param name="nombreDelArchivo">File name.</param>
		/// <param name="append">If set to <c>true</c> append.</param>
		public static void ComenzarEscrito
		(string nombreDelArchivo = "xf_text.txt", bool append = false, bool cerrar = false)
		{
			if (cerrar) TerminarEscrito();
			CrearCarpetas(nombreDelArchivo);
			if (!append) EliminarArchivo(nombreDelArchivo);
			EscribirEnArchivo(nombreDelArchivo);
		}

		/// <summary>
		/// Se crean las carpetas que se indiquen 
		/// en la ruta del archivo.
		/// </summary>
		/// <param name="nombreDelArchivo">Dirección y nombre 
		/// del archivo.</param>
		static void CrearCarpetas(string nombreDelArchivo)
		{
			string[] ruta = nombreDelArchivo.Split('/');
			for (int i = 0; i < ruta.Length - 1; i++)
			{
				string ruta2 = "./";
				for (int j = 0; j < i; j++) ruta2 += ruta[j] + "//";
				try { Directory.CreateDirectory(ruta2 + ruta[i]); } catch (IOException e) { WriteLine(e.Message); }
			}
		}

		/// <summary>
		/// Elimina el archivo indicado
		/// </summary>
		/// <param name="nombreDelArchivo">Nombre del archivo.</param>
		public static void EliminarArchivo(string nombreDelArchivo)
		{
			if (File.Exists(nombreDelArchivo))
			{
				try { File.Delete(nombreDelArchivo); } catch (IOException e) { WriteLine(e.Message); }
			}
		}

		/// <summary>
		/// Guarda en un archivo todo lo que se 
		/// mande a imprimir en consola.
		/// </summary>
		/// <param name="nombreDelArchivo">Nombre del archivo.</param>
		static void EscribirEnArchivo(string nombreDelArchivo)
		{
			var fs = new FileStream(nombreDelArchivo, FileMode.Append);
			txtmp = Out;
			sw = new StreamWriter(fs);
			SetOut(sw);
		}

		/// <summary>
		/// Para de escribir y cierra el archivo abierto.
		/// </summary>
		public static void TerminarEscrito(bool time = false)
		{
			reloj.Stop();
			if (time) WriteLine($"{reloj.Elapsed.TotalSeconds} seconds.");
			SetOut(txtmp);
			sw.Close();
		}

		public static void AbrirArcivo(string nombreDelArchivo)
		{
			var name = nombreDelArchivo.Split('/');
			string FileToOpen = name[0];
			for (int i = 1; i < name.Length; i++)
			{
				FileToOpen += "/" + name[i];
			}
			Process.Start(FileToOpen);
		}

		/// <summary>
		/// Cerrar archivo y abrirlo en un editor de texto.
		/// </summary>
		/// <param name="nombreDelArchivo">Nombre del archivo.</param>
		public static void
		PararYAbrir(string nombreDelArchivo = "xf_text.txt")
		{
			TerminarEscrito();
			AbrirArcivo(nombreDelArchivo);
		}

		/// <summary>
		/// Almacena y exporta en una lista el contenido
		/// de de un archivo de texto.
		/// </summary>
		/// <param name="nombreDelArchivo">Nombre del archivo.</param>
		/// <returns></returns>
		public static List<string> LeerArchivo(string nombreDelArchivo)
		{
			var contenido = new List<string>();
			var rder = new StreamReader(File.OpenRead(nombreDelArchivo));
			while (!rder.EndOfStream) contenido.Add(rder.ReadLine());
			rder.Close();
			return contenido;
		}

		/// <summary>
		/// Almacena y exporta en una lista las líneas de texto
		/// que estén entre dos líneas especificadas.
		/// </summary>
		/// <param name="nombreDelArchivo">Nombre del archivo.</param>
		/// <param name="marcadorInicial">Línea de inicio.</param>
		/// <param name="marcadorFinal">Línea final.</param>
		/// <returns></returns>
		public static List<string> LeerParte(string nombreDelArchivo,
		string marcadorInicial, string marcadorFinal)
		{
			var parte = new List<string>();
			var bandera = false;
			var contenido = LeerArchivo(nombreDelArchivo);
			foreach (string linea in contenido)
			{
				if (linea == marcadorFinal) break;
				if (bandera) parte.Add(linea);
				bandera |= linea == marcadorInicial;
			}
			return parte;
		}

		/// <summary>
		/// Imprimir la fecha y hora actuales.
		/// </summary>
		public static void EncabezadoFecha()
		{
			ImpLinea(FechaYHora());
		}

		/// <summary>
		/// Fecha Y Hora.
		/// </summary>
		/// <returns>The YH ora.</returns>
		public static string FechaYHora()
		{
			var fyh = DateTime.Now;
			var fecha = fyh.ToShortDateString();
			var hora = fyh.ToLongTimeString();
			return $"{fecha}\t{hora}";
		}

		/// <summary>
		/// Fecha y Hora en formato YYYYMMDD_hhmmss
		/// </summary>
		/// <returns></returns>
		public static string FechaParaFolder()
		{
			var fecha = DateTime.Now;
			var str = $"{fecha.Year:0000}{fecha.Month:00}{fecha.Day:00}";
			str += $"_{fecha.Hour:00}{fecha.Minute:00}{fecha.Second:00}_{fecha.Millisecond:000}";
			return str;
		}

		/// <summary>
		/// Hora this instance.
		/// </summary>
		/// <returns>The hora.</returns>
		public static string Hora()
		{
			var fecha = DateTime.Now;
			return fecha.ToLongTimeString();
		}



	}
}

