//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace ActiveMatter
//{
//	public class Sistema
//	{
//		static readonly Random rnd = new Random(DateTime.Now.Millisecond);


//		// Propiedad para almacenar los agentes del sistema
//		public List<Agent> Agents { get; set; }

//		// Propiedad para almacenar los agentes del sistema que deben actualizarse
//		public List<Agent> AgentsToUpdate { get; set; }

//		// Propiedad para almacenar los agentes fijos del sistema
//		public List<Agent> FixedAgents { get; set; }

//		// Propiedad para almacenar los agentes en una matriz
//		public Agent[,] AgentsMatrix { get; set; }

//		// Propiedad para almacenar las primaveras del sistema
//		public List<Spring> Springs { get; set; }

//		// Propiedad para almacenar el fitness del sistema
//		public float Fitness { get; set; }

//		public Sistema()
//		{
//			// Inicializa las listas de agentes y primaveras
//			Agents = new List<Agent>();
//			AgentsToUpdate = new List<Agent>();
//			FixedAgents = new List<Agent>();
//			Springs = new List<Spring>();
//		}

//		// Constructor para crear un sistema con una red rectangular
//		public Sistema(float initialLength)
//		{
//			// Inicializa las listas de agentes y primaveras
//			Agents = new List<Agent>();
//			AgentsToUpdate = new List<Agent>();
//			FixedAgents = new List<Agent>();
//			Springs = new List<Spring>();

//			// Crea una red rectangular de agentes
//			CreateRectangularLattice(initialLength);
//		}

//		// Método para inicializar los ángulos iniciales de los agentes
//		public void SetInitialAngles()
//		{
//			foreach (var a in Agents)
//			{
//				// Asigna un ángulo aleatorio entre 0 y 2*pi a cada agente
//				a.Theta = (float)(2 * Math.PI * rnd.NextDouble());
//			}
//		}


//		// Método para copiar los ángulos de un sistema a otro
//		public void CopyAngles(Sistema s)
//		{
//			// Verifica que el número de agentes en ambos sistemas sea igual
//			if (s.Agents.Count != Agents.Count)
//			{
//				throw new ArgumentException("Error: los sistemas tienen un número diferente de agentes");
//			}

//			for (int i = 0; i < Agents.Count; i++)
//			{
//				// Copia el ángulo de cada agente del sistema s al sistema actual
//				Agents[i].Theta = s.Agents[i].Theta;
//			}
//		}

//		// Método para clonar el sistema actual
//		public Sistema Clone()
//		{
//			// Crea un nuevo sistema
//			var sys = new Sistema();

//			// Copia el fitness del sistema actual
//			sys.Fitness = Fitness;

//			// Clona cada agente y agrega el clon al nuevo sistema
//			for (int i = 0; i < Agents.Count; i++)
//			{
//				sys.Agents.Add(Agents[i].Clone());
//			}

//			// Clona cada resorte y agrega el clon al nuevo sistema
//			foreach (var sp in Springs)
//			{
//				// Crea un nuevo resorte con los clonados de los agentes asociados
//				var sp_new = new Spring(sys.Agents[sp.Agent1.Index], sys.Agents[sp.Agent2.Index]);

//				// Copia la constante del resorte y su longitud inicial
//				sp_new.SpringConstant = sp.SpringConstant;
//				sp_new.InitialLength = sp.InitialLength;
//				sys.Springs.Add(sp_new);
//			}

//			// Crea una nueva matriz de agentes para el sistema clonado
//			sys.AgentsMatrix = new Agent[AgentsMatrix.GetLength(0), AgentsMatrix.GetLength(1)];

//			// Clona cada agente de la matriz de agentes
//			for (int i = 0; i < AgentsMatrix.GetLength(0); i++)
//			{
//				for (int j = 0; j < AgentsMatrix.GetLength(1); j++)
//				{
//					sys.AgentsMatrix[i, j] = sys.Agents[AgentsMatrix[i, j].Index];
//				}
//			}

//			return sys;
//		}

//		// Método para crear una red rectangular de agentes
//		void CreateRectangularLattice(float initialLength)
//		{
//			// Número de agentes en el eje horizontal y vertical de la red
//			var numHorizontal = 4;
//			var numVertical = 4;

//			// Inicializa la matriz de agentes
//			AgentsMatrix = new Agent[numHorizontal, numVertical];

//			for (var iv = 0; iv < numVertical; iv++)
//			{
//				for (var ih = 0; ih < numHorizontal; ih++)
//				{
//					// Crea un agente y lo agrega a la lista de agentes y a la matriz
//					var a = new Agent();
//					a.Index = Agents.Count;
//					a.Position = new Vector(ih * initialLength, iv * initialLength);
//					Agents.Add(a);
//					AgentsMatrix[ih, iv] = a;

//					// Agrega resortes entre el agente actual y sus vecinos en la red
//					if (ih > 0)
//					{
//						// Agrega un resorte con el agente a la izquierda
//						Springs.Add(new Spring(a, AgentsMatrix[ih - 1, iv]));
//					}
//					if (iv > 0)
//					{
//						// Agrega un resorte con el agente de arriba
//						Springs.Add(new Spring(a, AgentsMatrix[ih, iv - 1]));
//					}
//				}
//			}
//		}


//		// Método para imprimir información de los agentes y las primaveras del sistema
//		public void PrintInfoAgentsAndSprings()
//		{
//			Console.WriteLine("Información de agentes:");
//			foreach (var a in Agents)
//			{
//				Console.WriteLine("- Índice: {0}, posición: {1}, ángulo: {2}, fuerza: {3}", a.Index, a.Position, a.Theta, a.F);
//			}
//			Console.WriteLine("Información de primaveras:");
//			foreach (var sp in Springs)
//			{
//				Console.WriteLine("- Agente 1: {0}, agente 2: {1}, constante: {2}, longitud inicial: {3}", sp.Agent1.Index, sp.Agent2.Index, sp.SpringConstant, sp.InitialLength);
//			}
//		}


//	}
//}







using System;
using System.Collections.Generic;
using System.Linq;
namespace ActiveMatter {
	public class System {

		static readonly Random rnd = new Random(DateTime.Now.Millisecond);

		public List<Agent> agents = new List<Agent>();
		public List<Agent> Agents_update = new List<Agent>();
		public List<Agent> Agents_fixed = new List<Agent>();

		public Agent[,] agents_matrix;

		public List<Spring> springs = new List<Spring>();

		public float fitness;

		public System() {

		}

		public System(float _l0) {
			RectangularLattice2(_l0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s0"></param>
		/// <param name="random"></param>
		public void AssignAngles(System s0) {
			if (Program.RandomAngles) {
				SetInitialAngles();
			} else CopyAngles(s0);
		}

		/// <summary>
		/// 
		/// </summary>
		public void SetInitialAngles() {
			foreach (var a in agents) {
				a.Theta = (float)(2 * Math.PI * rnd.NextDouble());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <exception cref="ArgumentException"></exception>
		public void CopyAngles(System s) {
			if (s.agents.Count != agents.Count) {
				throw new ArgumentException("Error  hga94gj3nv");
			}
			for (int i = 0; i < agents.Count; i++) {
				agents[i].Theta = s.agents[i].Theta;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public System Clone() {
			var sys = new System();

			sys.fitness = fitness;

			for (int i = 0; i < agents.Count; i++) {
				sys.agents.Add(agents[i].Clone());
			}

			foreach (var sp in springs) {
				var sp_new = new Spring(sys.agents[sp.Agent1.Index], sys.agents[sp.Agent2.Index]);
				sp_new.SpringConstant = sp.SpringConstant;
				sp_new.InitialLength = sp.InitialLength;
				sys.springs.Add(sp_new);
			}

			sys.agents_matrix = new Agent[agents_matrix.GetLength(0), agents_matrix.GetLength(1)];
			for (int i = 0; i < agents_matrix.GetLength(0); i++) {
				for (int j = 0; j < agents_matrix.GetLength(1); j++) {
					sys.agents_matrix[i, j] = sys.agents[agents_matrix[i, j].Index];
				}
			}

			return sys;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_l0"></param>
		void RectangularLattice(float _l0) {
			var n_h = 4;
			var n_v = 4;

			agents_matrix = new Agent[n_h, n_v];

			for (var iv = 0; iv < n_v; iv++) {

				for (var ih = 0; ih < n_h; ih++) {
					var a = new Agent();
					a.Index = agents.Count;
					a.Position = new Vector(100 + _l0 * ih, _l0 * iv);
					agents_matrix[iv, ih] = a;
					agents.Add(a);

				}
			}

			for (var iv = 0; iv < n_v; iv++) {
				for (var ih = 0; ih < n_h; ih++) {
					// Conection to the right
					if (ih + 1 < n_h) {
						var sp = new Spring(agents_matrix[iv, ih], agents_matrix[iv, ih + 1]);
						sp.InitialLength = _l0;
						springs.Add(sp);
					}

					// Diagonal down
					if (ih + 1 < n_h && iv + 1 < n_v) {
						var sp = new Spring(agents_matrix[iv, ih], agents_matrix[iv + 1, ih + 1]);
						sp.InitialLength = _l0 * (float)Math.Sqrt(2);
						springs.Add(sp);
					}

					// Diagonal up
					if (ih + 1 < n_h && iv - 1 >= 0) {
						var sp = new Spring(agents_matrix[iv, ih], agents_matrix[iv - 1, ih + 1]);
						sp.InitialLength = _l0 * (float)Math.Sqrt(2);
						springs.Add(sp);
					}

					// Down
					if (iv + 1 < n_v) {
						var sp = new Spring(agents_matrix[iv, ih], agents_matrix[iv + 1, ih]);
						sp.InitialLength = _l0;
						springs.Add(sp);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_l0"></param>
		void RectangularLattice2(float _l0) {
			// Número de filas y columnas del enrejado
			var n_h = 3;
			var n_v = 3;

			// Matriz de agentes
			agents_matrix = new Agent[n_h, n_v];
			List<Vector> positions = new List<Vector>();

			// Crea una lista de posiciones para cada agente
			for (var iv = 0; iv < n_v; iv++) {
				for (var ih = 0; ih < n_h; ih++) {
					//positions.Add(new Vector(100 + _l0 * ih + rnd.Next(-30, 30), _l0 * iv + rnd.Next(-30, 30)));
					positions.Add(new Vector(300 + _l0 * ih, 100 + _l0 * iv));
				}
			}

			// Crea los agentes y asigna sus posiciones
			for (int i = 0; i < positions.Count; i++) {
				var a = new Agent();
				a.Index = agents.Count;
				a.Position = positions[i];
				agents_matrix[i / n_h, i % n_h] = a;
				agents.Add(a);
			}

			CrearResortes();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_h"></param>
		/// <param name="n_v"></param>
		public void CrearResortes() {
			var n_h = agents_matrix.GetLength(0);
			var n_v = agents_matrix.GetLength(1);

			springs.Clear();
			// Itera a través de los agentes y crea los resortes
			for (int i = 0; i < agents.Count; i++) {
				// Calcula la fila y columna del agente actual
				int row = i / n_h;
				int col = i % n_h;

				// Conection to the right
				if (col + 1 < n_h) {
					var sp = new Spring(agents_matrix[row, col], agents_matrix[row, col + 1]);
					//sp.InitialLength = _l0;
					sp.InitialLength = sp.Distancia();
					springs.Add(sp);
				}

				// Diagonal down
				if (col + 1 < n_h && row + 1 < n_v) {
					var sp = new Spring(agents_matrix[row, col], agents_matrix[row + 1, col + 1]);
					//sp.InitialLength = _l0 * (float)Math.Sqrt(2);
					sp.InitialLength = sp.Distancia();
					springs.Add(sp);
				}

				// Diagonal up
				if (col + 1 < n_h && row - 1 >= 0) {
					var sp = new Spring(agents_matrix[row, col], agents_matrix[row - 1, col + 1]);
					//sp.InitialLength = _l0 * (float)Math.Sqrt(2);
					sp.InitialLength = sp.Distancia();
					springs.Add(sp);
				}

				// Down
				if (row + 1 < n_v) {
					var sp = new Spring(agents_matrix[row, col], agents_matrix[row + 1, col]);
					//sp.InitialLength = _l0;
					sp.InitialLength = sp.Distancia();
					springs.Add(sp);
				}
			}
		}



		///// <summary>
		///// 
		///// </summary>
		//void Porpiedades_Fijos()
		//{
		//    for (var i = 0; i < Agents_fixed.Count; i++)
		//    {
		//        var a = Agents_fixed[i];
		//        a.v0 = 0;
		//        a.r = 0;
		//    }
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Vector CenterOfMass() {
			var cm = new Vector(0, 0);
			for (var i = 0; i < agents.Count; i++) {
				var a = agents[i];
				cm += a.Position;
			}
			cm /= agents.Count;
			return cm;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Vector LinearMomentum() {
			var LM = new Vector(0, 0);
			foreach (var a in agents) LM = LM + a.Velocity;
			return LM / Agent.DELTA_T;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public float AngularMomentum(Vector LM, Vector CM) {
			var AM = 0f;
			var cm = CenterOfMass();
			foreach (var a in agents) AM += a.GetAngularMomentum(LM, CM);
			return AM;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Update() {
			for (var i = 0; i < springs.Count; i++) {
				springs[i].Update();
			}
			for (var i = 0; i < agents.Count; i++) {
				//if (i == 12) continue;
				agents[i].Update();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void PrintInfoAgentsAndSprings() {
			M.ImpLinea("Agents");
			for (int i = 0; i < agents.Count; i++) {
				var a = agents[i];
				Console.WriteLine($"ind = {a.Index}");
				Console.WriteLine($"alpha = {a.Alpha}");
				Console.WriteLine($"a.beta = {a.Beta}");
				Console.WriteLine($"a.theta = {a.Theta}");
				Console.WriteLine($"a.chi = {a.Chi}");
				Console.WriteLine();
			}
			M.ImpLinea("Springs");
			for (int i = 0; i < springs.Count; i++) {
				var sp = springs[i];
				Console.WriteLine($"agent_i.ind = {sp.Agent1.Index}");
				Console.WriteLine($"agent_j.ind = {sp.Agent2.Index}");
				Console.WriteLine($"sp.k = {sp.SpringConstant}");
				Console.WriteLine($"sp.l0 = {sp.InitialLength}");
				Console.WriteLine();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void PrintPositions() {
			for (int i = 0; i < agents_matrix.GetLength(0); i++) {
				var linea = "";
				for (int j = 0; j < agents_matrix.GetLength(1); j++) {
					var a = agents_matrix[i, j];
					//linea += $"({M.FormatFlotante(a.position.x)},{M.FormatFlotante(a.position.y)})\t";
					linea += $"({100 * a.Velocity.X:0.0},{100 * a.Velocity.Y:0.0})\t";
				}
				Console.WriteLine(M.QUlt(linea));
			}
			Console.WriteLine();

		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public float FitnessRot(System s0) {
			var nreps = 1f;
			var fit = 0f;
			for (int i = 0; i < nreps; i++) {
				var s2 = Clone();
				//s2.CopyAngles(s0);
				//s2.SetInitialAngles();
				//fit += Math.Abs(s2.Rotation(1, 5000));
				var cm0 = s0.CenterOfMass().Mag();
				var cm2 = s2.Distancia(1000);
				fit += (float)Math.Abs(cm2 - cm0);
			}
			fitness = fit / nreps;
			return fitness;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s0"></param>
		/// <param name="fitnessFunction"></param>
		/// <returns></returns>
		public float CalculateFitness(System s0, Func<float, float, float> fitnessFunction) {
			var vCM = CenterOfMass_Update(5000);
			var s0CM = s0.CenterOfMass();
			return fitnessFunction(vCM.X - s0CM.X, vCM.Y - s0CM.Y);
		}

		// Número de veces que se calculará el fitness
		public int numeroRepeticiones = 10;

		public float FitnessX(System s0) {
			// Suma de todos los fitness obtenidos después de hacer el update
			var fitnessTotal = 0f;

			for (int i = 0; i < numeroRepeticiones; i++) {
				var s2 = Clone();
				s2.AssignAngles(s0);
				fitnessTotal += s2.CalculateFitness(s0, (x, y) => x * x - y * y);
			}

			// Devolver el promedio de los fitness obtenidos
			fitness = fitnessTotal / numeroRepeticiones;
			return fitness;
		}

		public float FitnessY(System s0) {
			// Suma de todos los fitness obtenidos después de hacer el update
			var fitnessTotal = 0f;

			for (int i = 0; i < numeroRepeticiones; i++) {
				var s2 = Clone();

				s2.AssignAngles(s0);
				////s2.SetInitialAngles();
				//s2.CopyAngles(s0);
				fitnessTotal += s2.CalculateFitness(s0, (x, y) => y * y - x * x);
			}

			// Devolver el promedio de los fitness obtenidos
			fitness = fitnessTotal / numeroRepeticiones;
			return fitness;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="nupdates1"></param>
		/// <param name="nupdates2"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public float Rotation(int nupdates1, int nupdates2) {
			for (int t = 0; t < nupdates1; t++) {
				for (int i = 0; i < nupdates2; i++) {
					Update();
				}
			}
			var CM = CenterOfMass();
			var LM = LinearMomentum();
			var am = AngularMomentum(LM, CM);
			return am;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="nupdates"></param>
		/// <returns></returns>
		public float Distancia(int nupdates) {
			//var d0 = CenterOfMass().Mag();
			for (int i = 0; i < nupdates; i++) {
				Update();
			}
			var d1 = CenterOfMass().Mag();
			return d1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nsteps"></param>
		/// <returns></returns>
		public Vector CenterOfMass_Update(int nsteps) {
			for (int i = 0; i < nsteps; i++) {
				Update();
			}
			return CenterOfMass();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Mutation() {
			for (int i = 0; i < springs.Count; i++) {
				if (M.Intento(2.0f / springs.Count)) {
					var n_k = M.RndNormal(springs[i].SpringConstant, 10);
					if (n_k > 1) springs[i].SpringConstant = n_k;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void MutationPos() {
			for (int i = 0; i < agents.Count; i++) {
				if (M.Intento(1.5f / agents.Count)) {
					var agente = agents[i];
					agente.Position.X = M.RndNormal(agente.Position.X, 5);
					agente.Position.Y = M.RndNormal(agente.Position.Y, 5);
				}
			}
			//SetInitialAngles();
			//AsignarAngulosDesdeCM();
		}

		/// <summary>
		/// 
		/// </summary>
		public void AsignarAngulosDesdeCM2() {
			// Centro de masa del sistema
			var centroDeMasa = CenterOfMass();
			foreach (Agent agente in agents) {
				var diferenciaAlCentroDeMasa = agente.Position - centroDeMasa;
				agente.Theta = (float)Math.PI + diferenciaAlCentroDeMasa.GetAngle();
			}
		}

		string ColorRnd() => $"{Math.Round(255 * rnd.NextDouble())}";

		/// <summary>
		/// 
		/// </summary>
		public void Export() {
			ExportAgents();
			ExportSprings();

		}

		public void ExportAgents() {
			// Agents
			Console.WriteLine("agent,id,alpha,beta,dTheta,chi,r,x,y,v0,theta,fixed,red,green,blue");
			for (int i = 0; i < agents.Count; i++) {
				var a = agents[i];

				var list_data = new List<string> {
					$"{i}",$"{i}",
					$"{a.Alpha:0.00}",$"{a.Beta:0.00}",$"{a.DTheta:0.00}",
					$"{a.Chi}",
					$"{a.R}",
					$"{a.Position.X:0.00}",$"{a.Position.Y:0.00}",$"{a.V0:0.00}",$"{a.Theta:0.000}",
					$"False", // Fixed Agent
					ColorRnd(),ColorRnd(),ColorRnd() // Red, Green, Blue
				};
				Console.WriteLine(M.List2String(list_data, ","));
			}
		}

		public void ExportSprings() {
			// Springs
			Console.WriteLine("spring,agent_i,agent_j,l0,k");
			for (int i = 0; i < springs.Count; i++) {
				var s = springs[i];

				var list_data = new List<string> {
					$"{i}",
					$"{agents.IndexOf(s.Agent1)}",$"{agents.IndexOf(s.Agent2)}",
					$"{s.InitialLength:0.000}",$"{s.SpringConstant:0.000}"
				};
				Console.WriteLine(M.List2String(list_data, ","));
			}
		}
	}




}

