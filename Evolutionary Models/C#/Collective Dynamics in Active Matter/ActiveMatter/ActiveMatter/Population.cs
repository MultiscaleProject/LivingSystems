using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveMatter {
	public class Population {


		public List<System> Systems = new List<System>();

		static readonly Random rnd = new Random(DateTime.Now.Millisecond);

		public Population() {

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="NSystems"></param>
		/// <param name="v0"></param>
		/// <param name="s0"></param>
		public Population(int NSystems, System s0) {
			for (int i = 0; i < NSystems; i++) {
				Systems.Add(s0.Clone());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void FitnessRot(System s0) {
			for (int i = 0; i < Systems.Count; i++) {
				Systems[i].FitnessRot(s0);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public void Mutations() {
			for (int i = 0; i < Systems.Count; i++) {
				//Systems[i].Mutation();
				Systems[i].MutationPos();
				Systems[i].CrearResortes();
				Systems[i].springs.ForEach(sp => sp.SpringConstant = 200);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Selection() {
			var f_selection = 0.1;
			var n_selection = (int)Math.Round(Systems.Count * f_selection);
			Systems = Systems.OrderByDescending(x => x.fitness).ToList();

			var new_systems = new List<System>();

			while (new_systems.Count < Systems.Count) {
				new_systems.Add(Systems[new_systems.Count % n_selection].Clone());
			}

			Systems = new_systems;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_generations"></param>
		public void AdaptX(int n_generations, string file, System s0) {
			for (int g = 0; g < n_generations; g++) {
				Mutations();
				for (int i = 0; i < Systems.Count; i++)
					Systems[i].FitnessX(s0);
				//foreach (var s in Systems) s.FitnessX();
				Selection();
				M.ComenzarEscrito(file, append: true);
				if (g % 1 == 0) {
					Console.WriteLine($"Generation g = {g}");
					Console.WriteLine($"Fitness = {Systems[0].fitness}");
					Console.WriteLine();
				}
				M.TerminarEscrito();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n_generations"></param>
		public void AdaptY(int n_generations, string file, System s0) {
			for (int g = 0; g < n_generations; g++) {
				Mutations();
				for (int i = 0; i < Systems.Count; i++)
					Systems[i].FitnessY(s0);
				//foreach (var s in Systems) s.FitnessY();
				Selection();
				M.ComenzarEscrito(file, append: true);
				if (g % 1 == 0) {
					Console.WriteLine($"Generation g = {g}");
					Console.WriteLine($"Fitness = {Systems[0].fitness}");
					Console.WriteLine();
				}
				M.TerminarEscrito();
			}
		}


	}
}

