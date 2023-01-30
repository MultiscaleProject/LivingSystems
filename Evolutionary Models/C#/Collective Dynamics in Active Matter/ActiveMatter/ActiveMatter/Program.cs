using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveMatter {
	class Program {
		public static float len0 = 50;
		static readonly Random rnd = new Random(DateTime.Now.Millisecond);


		public static bool RandomAngles = true;

		static void Main(string[] args) {
			// Inicializar valores
			var sistemaInicial = new System(len0);
			var velocidadInicial = 150.0f;
			var numGeneraciones = 5;

			// Configurar el sistema inicial
			ConfigurarSistemaInicial(sistemaInicial, velocidadInicial);

			// Generar una clave aleatoria para identificar el sistema
			var clave = M.FechaParaFolder() + "_" + M.RndPalabra(10);

			// Exportar el sistema inicial a un archivo
			ExportarSistema(sistemaInicial, $"pos/{clave}/", sistemaInicial);
			sistemaInicial.AsignarAngulosDesdeCM2();

			// Crear una lista para almacenar los sistemas generados
			var sistemas = new List<System>();

			// Realizar un ciclo para generar y analizar un sistema
			for (int j = 0; j < 1; j++) {
				// Generar una población de sistemas y seleccionar el mejor

				var sistema = sistemaInicial.Clone();
				var poblacion = new Population(200, sistemaInicial);

				for (int iteracion = 1; iteracion <= 500; iteracion++) {
					// Adaptar la población de sistemas
					poblacion.AdaptX(numGeneraciones, $"pos/{clave}/{j:00}/adaptation_{j:00}.txt", sistemaInicial);
					// Devolver el mejor sistema de la población
					sistema = poblacion.Systems[0];
					// Exportar el mejor sistema 
					ExportarSistema(sistema, $"pos/{clave}/{j:00}/X/Iteracion{iteracion:00}/", sistemaInicial);

					// Adaptar la población de sistemas
					poblacion.AdaptY(numGeneraciones, $"pos/{clave}/{j:00}/adaptation_{j:00}.txt", sistemaInicial);
					// Devolver el mejor sistema de la población
					sistema = poblacion.Systems[0];
					// Exportar el sistema seleccionado a un archivo
					ExportarSistema(sistema, $"pos/{clave}/{j:00}/Y/Iteracion{iteracion:00}/", sistemaInicial);
				}


			}
		}


		//static void InfoSystem2(System s, float v0) {
		//	// Imprimir las posiciones de los agentes en el sistema
		//	s.PrintPositions();

		//	// Calcular el centro de masa del sistema
		//	var CM = s.CenterOfMass();

		//	// Imprimir el centro de masa del sistema
		//	Console.WriteLine($"CM.x = {CM.X:0.00}\tCM.y = {CM.Y}");

		//	// Calcular el momento lineal del sistema
		//	var LM = s.LinearMomentum();

		//	// Calcular el valor de v0 por el número de agentes en el sistema
		//	var v0N = v0 * s.agents.Count;

		//	// Imprimir el momento lineal del sistema
		//	Console.WriteLine($"LM.x = {LM.X / v0N:0.00}\tLM.y = {LM.Y / v0N:0.00}, Mag = {LM.Mag() / v0N}");

		//	// Calcular el momento angular del sistema
		//	var AM = s.AngularMomentum(LM, CM);

		//	// Imprimir el momento angular del sistema
		//	Console.WriteLine($"AM = {AM}");

		//	// Imprimir una línea en blanco
		//	Console.WriteLine();
		//}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="nupdates"></param>
		/// <param name="s"></param>
		/// <param name="v0"></param>
		/// <returns></returns>
		static void Update(int nupdates, System s, float v0) {
			for (int t = 0; t < nupdates; t++) s.Update();
		}


		// Método para configurar el sistema inicial
		static void ConfigurarSistemaInicial(System sistema, float velocidad) {
			// Centro de masa del sistema
			var centroDeMasa = sistema.CenterOfMass();

			// Configurar los agentes del sistema inicial
			foreach (var agente in sistema.agents) {
				agente.Alpha = 0.1f;
				agente.Beta = 0.01f;
				agente.DTheta = 0.50f;

				agente.Theta = (float)(2 * Math.PI * rnd.NextDouble());
				//var diferenciaAlCentroDeMasa = agente.Position - centroDeMasa;
				//agente.Theta = diferenciaAlCentroDeMasa.GetAngle();

				agente.Chi = 1;
				agente.V0 = velocidad;
				agente.R = 10f;

				agente.XFixed = false;
				agente.YFixed = false;
			}

			// Configurar los resortes del sistema inicial
			foreach (var resorte in sistema.springs) {
				//resorte.SpringConstant = M.RndNormal(200, 100);
				resorte.SpringConstant = 200;
				//while (resorte.SpringConstant < 0) resorte.SpringConstant = M.RndNormal(200, 50);
				//resorte.l0 = 200;
			}


		}


		// Método para exportar el sistema a un archivo
		static void ExportarSistema(System sistema, string rutaArchivo, System sistemaInicial) {
			//sistema.AsignarAngulosDesdeCM();

			sistema.AssignAngles(sistemaInicial);
			////sistema.SetInitialAngles();
			//sistema.CopyAngles(sistemaInicial);

			M.ComenzarEscrito(rutaArchivo + "Agents.csv");
			sistema.ExportAgents();
			M.TerminarEscrito();

			M.ComenzarEscrito(rutaArchivo + "Springs.csv");
			sistema.ExportSprings();
			M.TerminarEscrito();
		}


		// Método para generar una población de sistemas y devolver el mejor
		static System GenerarYPoblacionYSistemaSeleccionado(System sistemaInicial, string clave, int indice, int numGeneraciones) {
			// Configurar el delta_t para los agentes
			//Agent.DELTA_T = 0.004f;

			// Generar una población de sistemas a partir del sistema inicial
			var poblacion = new Population(200, sistemaInicial);

			// Adaptar la población de sistemas
			poblacion.AdaptY(numGeneraciones, $"pos/{clave}/{indice:00}/adaptation_{indice:00}.txt", sistemaInicial);

			// Devolver el mejor sistema de la población
			return poblacion.Systems[0];
		}


		// Método para generar copias de un sistema, analizar su AM y devolver una lista con los valores de AM
		static void GenerarYCopiarYSistemaYSistemaAnalizarAM(System sistema, System sistemaInicial,
			string clave, int indice, int repeticiones, int numeroActualizacionesFase1, float velocidad) {
			// Crear una lista para almacenar los valores de AM
			//var rotadores = new List<float>();

			// Realizar un ciclo para generar copias del sistema, analizar su AM y almacenar el resultado en la lista
			for (int jk = 0; jk < repeticiones; jk++) {
				// Generar una copia del sistema
				var s2 = sistema.Clone();

				//// Copiar los ángulos del sistema inicial en la copia del sistema
				//s2.CopyAngles(sistemaInicial);

				// Comenzar a escribir en un archivo
				//M.ComenzarEscrito($"pos/{clave}/{indice:00}/pos_{indice:00}_{jk:00}.txt");

				// Analizar el AM del sistema y almacenar el resultado en la lista
				Update(numeroActualizacionesFase1, s2, velocidad);

				// Terminar de escribir en el archivo
				M.TerminarEscrito();
			}

			// Devolver la lista de valores de AM
			//return rotadores;
		}


		// Método para exportar una lista de valores de AM a un archivo
		static void ExportarListaAM(List<float> listaAM, string clave, int indice) {
			// Ordenar la lista de valores de AM
			listaAM.Sort();

			// Comenzar a escribir en un archivo
			M.ComenzarEscrito($"pos/{clave}/{indice:00}/sum_{indice:00}.txt");

			// Escribir el porcentaje de valores de AM mayores a 15 en el archivo
			Console.WriteLine($"Num_big_AM = {100 * (float)listaAM.Count(x => Math.Abs(x) > 15) / listaAM.Count}%\n");

			// Escribir cada valor de AM de la lista en el archivo
			foreach (var f in listaAM) Console.WriteLine($"AM = {f}");

			// Terminar de escribir en el archivo
			M.TerminarEscrito();
		}


		/// <summary>
		/// Método para exportar información de un sistema a un archivo
		/// </summary>
		/// <param name="sistema"></param>
		/// <param name="clave"></param>
		/// <param name="indice"></param>
		static void ExportarInfoSistema(System sistema, string clave, int indice) {
			// Comenzar a escribir en un archivo
			M.ComenzarEscrito($"pos/{clave}/{indice:00}/info_{indice:00}.txt");

			// Exportar la información de los agentes y las muelles del sistema
			sistema.PrintInfoAgentsAndSprings();

			// Terminar de escribir en el archivo
			M.TerminarEscrito();
		}
	}
}




//static void Main(string[] args)
//{

//	var v0 = 5f;
//	var nupdates1 = 1000;
//	var nupdates2 = 100;

//	var repsMisSys = 10; // Repetitions for the same system

//	var s0 = new System(len0);
//	foreach (var a in s0.agents)
//	{
//		a.alpha = 0.1f;
//		a.beta = 0.01f;
//		a.dTheta = 0.01f;
//		a.theta = (float)(2 * Math.PI * rnd.NextDouble());
//		a.chi = 1;
//		a.v0 = v0;
//		a.r = 10f;

//		a.x_fixed = false;
//		a.y_fixed = false;
//	}
//	foreach (var sp in s0.springs)
//	{
//		sp.k = M.RndNormal(200, 100);
//		while (sp.k < 0) sp.k = M.RndNormal(200, 50);
//		//sp.l0 = 200;
//	}

//	var clave = M.RndPalabra(10);

//	M.ComenzarEscrito($"pos/{clave}/nola.txt");
//	s0.Export();
//	M.TerminarEscrito();

//	var ls = new List<System>();

//	for (int j = 0; j < 1; j++)
//	{
//		Agent.delta_t = 0.004f;

//		var p = new Population(200, s0);
//		p.Adapt(100, $"pos/{clave}/{j:00}/adaptation_{j:00}.txt", s0);

//		var s = p.Systems[0];

//		M.ComenzarEscrito($"pos/{clave}/{j:00}/nola.txt");
//		s.Export();
//		M.TerminarEscrito();


//		var rotadores = new List<float>();
//		for (int jk = 0; jk < repsMisSys; jk++)
//		{
//			var s2 = s.Clone();
//			s2.CopyAngles(s0);

//			M.ComenzarEscrito($"pos/{clave}/{j:00}/pos_{j:00}_{jk:00}.txt");

//			var am = Update(nupdates1, nupdates2, s2, v0);
//			rotadores.Add(am);

//			M.TerminarEscrito();

//		}

//		rotadores.Sort();
//		M.ComenzarEscrito($"pos/{clave}/{j:00}/sum_{j:00}.txt");
//		Console.WriteLine($"Num_big_AM = {100 * (float)rotadores.Count(x => Math.Abs(x) > 15) / rotadores.Count}%\n");
//		foreach (var f in rotadores) Console.WriteLine($"AM = {f}");
//		M.TerminarEscrito();

//		M.ComenzarEscrito($"pos/{clave}/{j:00}/info_{j:00}.txt");
//		s.PrintInfoAgentsAndSprings();
//		M.TerminarEscrito();
//	}
//}



//static void Main(string[] args)
//{
//	// Inicializar valores
//	var sistemaInicial = new System(len0);
//	var velocidadInicial = 5.0f;
//	var numeroActualizacionesFase1 = 1000;
//	var numeroActualizacionesFase2 = 100;
//	var repeticionesParaMismoSistema = 10;

//	// Configurar los agentes del sistema inicial
//	foreach (var agente in sistemaInicial.agents)
//	{
//		agente.alpha = 0.1f;
//		agente.beta = 0.01f;
//		agente.dTheta = 0.01f;
//		agente.theta = (float)(2 * Math.PI * rnd.NextDouble());
//		agente.chi = 1;
//		agente.v0 = velocidadInicial;
//		agente.r = 10.0f;

//		agente.x_fixed = false;
//		agente.y_fixed = false;
//	}

//	// Configurar los resortes del sistema inicial
//	foreach (var resorte in sistemaInicial.springs)
//	{
//		resorte.k = M.RndNormal(200, 100);
//		while (resorte.k < 0) resorte.k = M.RndNormal(200, 50);
//		// resorte.l0 = 200;
//	}

//	// Generar una clave aleatoria para identificar el sistema
//	var clave = M.RndPalabra(10);

//	// Exportar el sistema inicial a un archivo
//	M.ComenzarEscrito($"pos/{clave}/nola.txt");
//	sistemaInicial.Export();
//	M.TerminarEscrito();

//	// Crear una lista para almacenar los sistemas generados
//	var sistemas = new List<System>();

//	// Realizar un ciclo para generar y analizar un sistema
//	for (int j = 0; j < 1; j++)
//	{
//		Agent.delta_t = 0.004f;

//		// Crear una población de sistemas a partir del sistema inicial
//		var poblacion = new Population(200, sistemaInicial);

//		// Adaptar la población utilizando el sistema inicial como referencia
//		poblacion.Adapt(100, $"pos/{clave}/{j:00}/adaptation_{j:00}.txt", sistemaInicial);

//		// Seleccionar el mejor sistema de la población
//		var sistema = poblacion.Systems[0];

//		// Exportar el sistema seleccionado a un archivo
//		M.ComenzarEscrito($"pos/{clave}/{j:00}/nola.txt");
//		sistema.Export();
//		M.TerminarEscrito();

//		// Crear una lista para almacenar los valores de AM de cada sistema
//		var rotadores = new List<float>();

//		// Realizar un ciclo para generar y analizar varias copias del sistema seleccionado
//		for (int jk = 0; jk < repeticionesParaMismoSistema; jk++)
//		{
//			// Crear una copia del sistema seleccionado
//			var sistema2 = sistema.Clone();

//			// Copiar los ángulos del sistema inicial al sistema2
//			sistema2.CopyAngles(sistemaInicial);

//			// Exportar la posición de los agentes del sistema2 a un archivo
//			M.ComenzarEscrito($"pos/{clave}/{j:00}/pos_{j:00}_{jk:00}.txt");

//			// Actualizar el sistema2 y obtener el valor de AM
//			var am = Update(numeroActualizacionesFase1, numeroActualizacionesFase2, sistema2, velocidadInicial);
//			rotadores.Add(am);

//			// Terminar de escribir en el archivo
//			M.TerminarEscrito();
//		}

//		// Ordenar la lista de valores de AM en orden ascendente
//		rotadores.Sort();

//		// Exportar la lista de valores de AM a un archivo
//		M.ComenzarEscrito($"pos/{clave}/{j:00}/sum_{j:00}.txt");
//		Console.WriteLine($"Num_big_AM = {100 * (decimal)rotadores.Count(x => Math.Abs(x) > 15) / rotadores.Count}%\n");
//		foreach (var f in rotadores) Console.WriteLine($"AM = {f}");
//		M.TerminarEscrito();

//		// Exportar información sobre los agentes y resortes del sistema seleccionado a un archivo
//		M.ComenzarEscrito($"pos/{clave}/{j:00}/info_{j:00}.txt");
//		sistema.PrintInfoAgentsAndSprings();
//		M.TerminarEscrito();
//	}
//}
