using System;
using System.Collections.Generic;
using System.Linq;

namespace modularidad_core3._1 {
	public static class T {

		public static float treshold = 0.95f;

		static readonly Random rnd = new Random(DateTime.Now.Millisecond);

		/// <summary>
		/// 
		/// </summary>
		static Dictionary<string, string> xor = new Dictionary<string, string>() {
			["00"] = "0",
			["01"] = "1",
			["10"] = "1",
			["11"] = "0"
		};
		static Dictionary<string, string> notb = new Dictionary<string, string>() {
			["00"] = "1",
			["01"] = "0",
			["10"] = "1",
			["11"] = "0"
		};
		static Dictionary<string, string> and = new Dictionary<string, string>() {
			["00"] = "0",
			["01"] = "0",
			["10"] = "0",
			["11"] = "1"
		};
		static Dictionary<string, string> or = new Dictionary<string, string>() {
			["00"] = "0",
			["01"] = "1",
			["10"] = "1",
			["11"] = "1"
		};
		static Dictionary<string, string> nor = new Dictionary<string, string>() {
			["00"] = "1",
			["01"] = "0",
			["10"] = "0",
			["11"] = "0"
		};
		static Dictionary<string, string> nand = new Dictionary<string, string>() {
			["00"] = "1",
			["01"] = "1",
			["10"] = "1",
			["11"] = "0"
		};


		static Dictionary<string, string> Puerta(int aa, int ab, int ba, int bb) {
			var puerta = new Dictionary<string, string>() {
				["00"] = $"{aa}",
				["01"] = $"{ab}",
				["10"] = $"{ba}",
				["11"] = $"{bb}"
			};


			return puerta;
		}





		/// <summary>
		/// 
		/// </summary>
		public static void T01() {
			M.ComenzarEscrito("mi_archivo.txt");
			Console.WriteLine("Este es mi primer archivo de prueba");
			M.TerminarEscrito();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fN"></param>
		/// <returns></returns>
		static string GuardarEnCarpeta(string fN, string newFolder) {
			var ruta = fN.Split('/').ToList();
			var ult = ruta.Last();
			ruta[^1] = newFolder;
			ruta.Add(ult);
			return M.List2String(ruta, "/");
		}

		static Dictionary<string, string> CorregirSubFuncion(Dictionary<string, string> F) {
			var f2 = new Dictionary<string, string>();

			for (int i = 0; i < F.Keys.First().Length; i++) {
			repetir:
				var contadorkey01 = 0;
				var contadorRepetidos = 0;
				foreach (var d in F) {
					var key0 = M.CambiarValorCadenaBin(d.Key, i, '0');
					var key1 = M.CambiarValorCadenaBin(d.Key, i, '1');
					if (F.ContainsKey(key0) && F.ContainsKey(key1)) {
						contadorkey01++;
						if (F[M.CambiarValorCadenaBin(d.Key, i, '0')] == F[M.CambiarValorCadenaBin(d.Key, i, '1')]) {
							contadorRepetidos++;
						}
					} else contadorRepetidos++;

					if (contadorRepetidos == F.Count) {
						foreach (var k in F.Keys.Where(x => x[i] == '0').ToList()) {
							F[k] = $"{rnd.Next(2)}";
						}
						goto repetir;
					}
				}
			}
			return F;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="nEntr"></param>
		/// <param name="maxEntrs"></param>
		/// <returns></returns>
		static Dictionary<string, string> ConstructSubFun(int nEntr, int maxEntrs) {
			var f = new Dictionary<string, string>();
			var nvalores2 = (int)Math.Pow(2, nEntr - 1);
			var valores = M.RndListCerosYUnos(nvalores2, nvalores2);
			for (int i = 0; i < Math.Pow(2, nEntr); i++) {
				f[M.Int2Bin(i, nEntr)] = $"{rnd.Next(2)}";
				//f[M.Int2Bin(i, nEntr)] = $"{valores[i]}";
			}
			while (f.Count > maxEntrs) f.Remove(M.RndElemento(f.Keys.ToList()));
			f = CorregirSubFuncion(f);

			return f;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="S"></param>
		/// <param name="L"></param>
		/// <param name="G"></param>
		/// <param name="E"></param>
		/// <param name="p_fb"></param>
		static void PrintInfoSimulation(int S, int L, int G, float E, float p_fb, List<int> listGenMut) {
			Console.WriteLine($"NumNetworks = {S}, FractionSelection = {L}");
			Console.WriteLine($"Generations = {G}, ChangeTaskEachGen = {E}");
			Console.WriteLine($"ChangeBoolFun = {p_fb}\n");

			Console.WriteLine($"Mutaciones en las generacines {M.List2String(listGenMut)}\n");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="f1"></param>
		/// <param name="f2"></param>
		/// <param name="H1"></param>
		/// <param name="H2"></param>
		/// <param name="ts"></param>
		static void PrintInfoTask(Dictionary<string, string> f1,
		  Dictionary<string, string> f2, Dictionary<string, string> H, Tarea t) {
			M.ImpLinea("F1");
			foreach (var k in f1.Keys) Console.WriteLine($"{k} -> {f1[k]}");

			M.ImpLinea("F2");
			foreach (var k in f2.Keys) Console.WriteLine($"{k} -> {f2[k]}");

			M.ImpLinea("H1");
			foreach (var k in H.Keys) Console.WriteLine($"{k} -> {H[k]}");

			M.ImpLinea("inputs\tt1\tt2");
			for (int i = 0; i < t.lcy.Count; i++) {
				Console.WriteLine($"{t.lcy[i].valoresDeEntrada[0]}\t{t.lcy[i].valoresDeSalida[0]}");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="f1"></param>
		/// <param name="f2"></param>
		/// <param name="H"></param>
		/// <returns></returns>
		static List<Ciclos> ConsructCycleList(Dictionary<string, string> f1,
		Dictionary<string, string> f2, Dictionary<string, string> H) {
			var lcy = new List<Ciclos>();
			foreach (var k1 in f1.Keys) {
				foreach (var k2 in f2.Keys) {
					var valentrada = $"{k1}{k2}";
					var resultf1f2 = $"{f1[k1]}{f2[k2]}";
					var cyc1 = new Ciclos();
					cyc1.valoresDeEntrada = new List<string> { valentrada };
					cyc1.valoresDeSalida = new List<string> { $"{H[resultf1f2]}" };
					lcy.Add(cyc1);
				}
			}
			return lcy;
		}

		static List<string> HProhibidas = new List<string> { "0000", "0011", "0101", "1010", "1100", "1111" };
		static string H2Str(Dictionary<string, string> H) => M.List2String(H.Values.ToList(), "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="H0"></param>
		/// <returns></returns>
		static Dictionary<string, string> CambiarH(Dictionary<string, string> H0) {
		cambiarUnValor:
			var H2 = new Dictionary<string, string>();
			foreach (var k in H0.Keys) H2[k] = $"{rnd.Next(2)}";
			//var keyrnd = M.RndElemento(H0.Keys.ToList());
			//H2[keyrnd] = (H0[keyrnd] == "0") ? "1" : "0";
			if (HProhibidas.Contains(H2Str(H2)) || H2Str(H0).Equals(H2Str(H2))) goto cambiarUnValor;
			return H2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p_fb">Probability to change boolean functions</param>
		/// <param name="E">Generations to change the task</param>
		public static void T03(float p_fb, float tMutT) {
			int S = 1000;
			int L = (int)Math.Round(S / 3f);
			int G = 10000;

			var listaGenMuts = M.L(G).Where(x => M.Intento(tMutT)).ToList();

			var nentradas_f1 = 3;
			var nentradas_f2 = 3;
			var nentradas = nentradas_f1 + nentradas_f2;
			var nNodos = 10 + nentradas;

			var f1 = ConstructSubFun(nentradas_f1, maxEntrs: 7);
			var f2 = ConstructSubFun(nentradas_f2, maxEntrs: 7);

		crearH:
			var H = new Dictionary<string, string>() {
				["00"] = $"{rnd.Next(2)}",
				["01"] = $"{rnd.Next(2)}",
				["10"] = $"{rnd.Next(2)}",
				["11"] = $"{rnd.Next(2)}"
			};
			if (HProhibidas.Contains(H2Str(H))) goto crearH;

			var entradas = M.L(nentradas); ;
			var salidas = new List<int> { nNodos - 1 };

			var t = Tarea.CrearTarea(entradas, salidas, H2Str(H), f1, f2, H);

			var carpeta = $"resultados/p_fb_{p_fb:0.00}/tMutT_{tMutT}";
			var archivo = $"evolucion_{M.FechaParaFolder()}.txt";

			var nombre = $"{carpeta}/{archivo}";

			M.ComenzarEscrito(nombre);
			M.ImpLinea(M.FechaYHora());
			PrintInfoSimulation(S, L, G, tMutT, p_fb, listaGenMuts);
			//PrintInfoSimulation(S, L, G, E, p_fb);
			PrintInfoTask(f1, f2, H, t);
			M.TerminarEscrito();

			var p = new Poblacion();
			//p.ConstruirApartirDeUna(S);
			p.ConstruirRedesDistintas(S, t, salidas.Last());

			var fitness_actual = 1f;
			var l_fit = new List<float>();

			var redesAntes = new Dictionary<int, Network>();
			var redesAhora = new Dictionary<int, Network>();

			var gdespues = 2;
			for (int g = 0; g < G; g++) {

				if (listaGenMuts.Contains(g)) {
					H = CambiarH(H);
					t = Tarea.CrearTarea(entradas, salidas, H2Str(H), f1, f2, H);
				}

				p.Seleccion(L, t, p_fb);
				var best_net = p.redes.Where(r => r.fit >= p.redes[0].fit)
									  .OrderBy(r => r.nMutS + r.nMutF).First();

				l_fit.Add(best_net.fit);
				if (l_fit.Count > gdespues + 2) l_fit.RemoveAt(0);

				M.ComenzarEscrito(nombre, append: true);

				var conecciones = best_net.ImprimirConnections(t);
				Console.WriteLine();

				PriReng(g, best_net, t);
				Console.WriteLine($"DNA_G_{g} = {best_net.PrintDNASeparado(t)}");
				best_net.Imprimir_Python(t);
				best_net.PrintCommunities();

				if (best_net.fit != fitness_actual || listaGenMuts.Contains(g)) {
					fitness_actual = best_net.fit;
					PrintInfoNet(best_net, $"{g}", t);
				}

				M.TerminarEscrito();
				if (tMutT == 0 && best_net.fit > treshold) break;

				// Una generación previa al cambio de tarea (la línea listaGenMuts.Contains(g + 1)
				// garantiza que la generación g pertenezca a la lista de generaciones donde se
				// cambia la tarea) se asigna un índice distinto a todas las redes adaptadas
				// en la población
				if (best_net.fit > treshold && listaGenMuts.Contains(g + 1)) {
					redesAntes = p.RedesExitosas(indexar: true);
				}
				// Después de "gdespués" generaciones de haber cambiado la tarea guardamos los índices
				// de todas las redes adaptadas en la población.
				if (best_net.fit > treshold && listaGenMuts.Contains(g - gdespues) && g > 1) {
					redesAhora = p.RedesExitosas(indexar: false);
				}

				if (listaGenMuts.Contains(g - gdespues) && g > 1) {
					if (l_fit.First() >= 0.99f && l_fit.Last() >= 0.99f && redesAhora.Count > 0) {

						M.ComenzarEscrito(nombre, append: true);

						foreach (var indice in redesAhora.Keys) {
							var redAntes = redesAntes[indice];
							var redAhora = redesAhora[indice];

							M.ImpLinea("Switch");
							Console.WriteLine("Red antes");
							Console.WriteLine($"nMutS = {redAntes.nMutS}\tnMutF = {redAntes.nMutS}\tnMutT = {redAntes.nMutS + redAntes.nMutF}");
							PrintInfoNet(redAntes, $"{g}", t);

							Console.WriteLine();
							Console.WriteLine("Red ahora");
							Console.WriteLine($"nMuts = {redAhora.nMutS}\tnMutF = {redAhora.nMutF}\tnMutT = {redAntes.nMutS + redAntes.nMutF}");
							PrintInfoNet(redAhora, $"{g}", t);

							Console.WriteLine();
							Console.WriteLine("\n");
							PrintInfoDNAs(redAntes.PrintDNA(), redAhora.PrintDNA(), redAntes, t);

							Console.WriteLine("==========");
						}

						M.TerminarEscrito();

						break;

					}
				}
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="l1"></param>
		/// <param name="l2"></param>
		/// <param name="l3"></param>
		static void Prin3Listas(List<string> l1, List<string> l2, List<string> l3) {
			var sep = "\t";
			var max = Math.Max(l1.Count, Math.Max(l2.Count, l3.Count));
			for (int i = 0; i < max; i++) {
				var mensaje = "";
				mensaje += i >= l1.Count ? new string(' ', l1[0].Length) : l1[i];
				mensaje += sep;

				mensaje += i >= l2.Count ? new string(' ', l2[0].Length) : l2[i];
				mensaje += sep;

				mensaje += i >= l3.Count ? new string(' ', l3[0].Length) : l3[i];

				Console.WriteLine(mensaje);
			}

		}

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="best_net"></param>
		/// <param name="t"></param>
		static void PriReng(int g, Network best_net, Tarea t) {
			var coneccns = best_net.ImprimirConnections(t);
			var mensaje = $"G = {g}\tF = {best_net.fit}\t_q_Q_ = {Modularity.T04(best_net, t):0.00}\t";
			mensaje += $"NNds = {t.Depurar(best_net).Count - t.entradas.Count}\t" +
			  $"nMutS = {best_net.nMutS}\tnMutF = {best_net.nMutF}\tnMutT = {best_net.nMutS + best_net.nMutF}\t";
			//mensaje += $"D = {best_net.MedirDistancia():0.00}\tTarea_{t.name}";
			mensaje += $"Tarea_{t.name}";
			M.ImpLinea(mensaje);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="best_net"></param>
		/// <param name="t"></param>
		static void KDJGD(int g, Network best_net, Tarea t) {
			PriReng(g, best_net, t);

			M.ImpLinea("Red");
			best_net.Imprimr();
			Console.WriteLine($"Nodos depurados = {M.List2String(t.Depurar(best_net))}");
			Console.WriteLine($"Total de Nodos Depurados = {t.Depurar(best_net).Count(x => x >= 0)}");
			Console.WriteLine($"DNA_G_{g} = {best_net.PrintDNA()}");
			best_net.Imprimir_Python(t);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="carpeta"></param>
		/// <param name="archivo"></param>
		/// <param name="best_net"></param>
		/// <param name="t1"></param>
		/// <param name="t2"></param>
		/// <param name="dna_t1"></param>
		/// <param name="dna_t2"></param>
		/// <returns></returns>
		static bool ComprobarRed(string carpeta, string archivo, Network best_net,
		Tarea t1, Tarea t2, string dna_t1, string dna_t2) {
			if (CheckNet(best_net, t1, t2, dna_t1, dna_t2)) return true;
			M.ComenzarEscrito($"{carpeta}/errores.txt", append: true);
			Console.WriteLine($"La simulacion {archivo} tuvo un error: id_gbws34");
			M.TerminarEscrito();
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="net"></param>
		/// <param name="t1"></param>
		/// <param name="t2"></param>
		/// <param name="dna_t1"></param>
		/// <param name="dna_t2"></param>
		/// <returns></returns>
		static bool CheckNet(Network net, Tarea t1, Tarea t2, string dna_t1, string dna_t2) {
			var check_t1 = Network.CheckNetwork(net, dna_t1, t1);
			var check_t2 = Network.CheckNetwork(net, dna_t2, t2);
			return check_t1 && check_t2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dna_t1"></param>
		/// <param name="dna_t2"></param>
		/// <param name="longitudes">Cuantos valores tiene la fb de cada nodo</param>
		/// <param name="txt"></param>
		static void PrintInfoDNAs(string dna_t1, string dna_t2, Network net1, Tarea t, bool txt = false) {

			if (dna_t1.Length != dna_t2.Length) throw new ArgumentException("sdgw2");

			//if (txt) M.ImpLinea("DNAs");
			var mensaje = txt ? "DNA_t1 = " : "";

			mensaje += net1.SepararGenes(dna_t1) + "\n";
			if (txt) mensaje += "DNA_tx = ";
			mensaje += net1.SepararGenes(DNAX(dna_t1, dna_t2)) + "\n";
			if (txt) mensaje += "DNA_t2 = ";
			mensaje += net1.SepararGenes(dna_t2);
			Console.WriteLine(mensaje);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="best_net"></param>
		/// <param name="dna"></param>
		static List<List<string>> AnalisisInfluencia(Network best_net, string dna, string mensaje, Tarea tarea) {
			M.ImpLinea(mensaje);
			best_net.ReemplazarDNA(dna);
			var transitorios = tarea.Transitorios(best_net);
			PrintTransitorio(transitorios);
			best_net.InfoMut(transitorios);
			M.ImpLinea("Influencia");
			best_net.QueTantoInfluye(tarea);
			return transitorios;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dnasAntes"></param>
		/// <param name="dnasAhora"></param>
		/// <param name="str_tarea"></param>
		/// <param name="switchingNodes"></param>
		/// <param name="best_net"></param>
		/// <param name="bandera"></param>
		/// <param name="dna_t1"></param>
		/// <param name="dna_t2"></param>
		static bool QHuboSwitch(List<string> dnasAntes, List<string> dnasAhora,
		Tarea tarea, ref List<int> switchingNodes, Network best_net,
		ref string dna_t1, ref string dna_t2) {
			bool bandera = false;
			foreach (var dna_ahora in dnasAhora) {
				foreach (var dna_antes in dnasAntes) {
					switchingNodes = SwitchingNodes(dna_antes, dna_ahora, tarea.Depurar(best_net));
					bandera = switchingNodes.Count <= 1;
					if (bandera) {
						dna_t1 = dna_antes;
						dna_t2 = dna_ahora;
						if (tarea.name.Equals("1")) {
							dna_t1 = dna_ahora;
							dna_t2 = dna_antes;
						}
						break;
					}
				}
			}
			return bandera;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="redesAntes"></param>
		/// <param name="redesAhora"></param>
		static void QSwitchConex(List<string> redesAntes, List<string> redesAhora) {
			//var minDist = float.MaxValue;
			foreach (var ran in redesAntes) {
				foreach (var rah in redesAhora) {
					//todo
				}
			}
		}

		/// <summary>
		/// This method finds all nodes that mutate to change the task performance.
		/// But only condiers the effective nodes. 
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <param name="efectiveNds"></param>
		/// <returns></returns>
		static List<int> SwitchingNodes(string s1, string s2, List<int> efectiveNds) {
			var ndsQCambn = new List<int>();
			if (s1.Length != s2.Length) throw new ArgumentException("s1.Length != s2.Length");
			for (int i = 0; i < s1.Length; i++) {
				var nodoi = i / 4;
				if (s1[i] != s2[i] && efectiveNds.Contains(nodoi)) ndsQCambn.Add(nodoi);
			}
			return ndsQCambn.Distinct().ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="best_net"></param>
		/// <param name="gstr"></param>
		/// <param name="str_tarea"></param>
		/// <param name="tarea"></param>
		static void PrintInfoNet(Network best_net, string gstr, Tarea tarea) {
			Console.WriteLine($"\nCambio en el Fitness, F = {best_net.fit}, G = {gstr}, NNds = {tarea.Depurar(best_net).Count}, Tarea_{tarea.name}");
			Console.WriteLine("==============");
			M.ImpLinea("Red");
			Console.WriteLine($"Gen = {gstr}\tFit = {best_net.fit:0.000}\t" +
			  $"NNds = {tarea.Depurar(best_net).Count - tarea.entradas.Count}\t" +
			  $"nMutS = {best_net.nMutS}\tnMutF = {best_net.nMutF}\t" +
			  $"nMutT = {best_net.nMutS + best_net.nMutF}\tTask_{tarea.name}");
			Console.WriteLine($"Output = {best_net.nodes.Count - 1}");
			best_net.Imprimr();
			Console.WriteLine($"{M.List2String(tarea.Depurar(best_net))}\n");
			best_net.Imprimir_Python(tarea);

			M.ImpLinea("La red posee los siguientes resultados");
			Console.WriteLine("ValoresEntrada\tValoresEsperados\tPerformance");
			for (int i = 0; i < tarea.lcy.Count; i++) {
				Prin3Listas(tarea.lcy[i].valoresDeEntrada, tarea.lcy[i].valoresDeSalida, best_net.lcy[i].valoresDeSalida);
				//Console.WriteLine();

				//Console.WriteLine("\nIn");
				//tarea.lcy[i].valoresDeEntrada.ForEach(x => Console.WriteLine(x));
				//Console.WriteLine("Out");
				//tarea.lcy[i].valoresDeSalida.ForEach(x => Console.WriteLine(x));
				//Console.WriteLine("Perfor");
				//best_net.lcy[i].valoresDeSalida.ForEach(x => Console.WriteLine(x));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="best_net"></param>
		static void ImprimirInfoRedExitosa(Network best_net, string carpeta,
		string dna_t1, string dna_t2,
		List<List<string>> transitorios1, List<List<string>> transitorios2, int[] outputAndSwitch, Tarea tarea) {

			M.ComenzarEscrito($"{carpeta}/conexiones.txt");
			var connec = best_net.Conexiones(tarea);
			var caracteresEliminar = new List<string> { "[", "]", "(" };
			caracteresEliminar.ForEach(x => connec = connec.Replace(x, ""));
			foreach (var xsg in M.QUlt(connec).Split("),")) Console.WriteLine(xsg);

			M.ComenzarEscrito($"{carpeta}/outputAndSwitch.txt", cerrar: true);
			Console.WriteLine($"{outputAndSwitch[0]}\n{outputAndSwitch[1]}");

			M.ComenzarEscrito($"{carpeta}/dnas.txt", cerrar: true);

			PrintInfoDNAs(dna_t1, dna_t2, best_net, tarea, txt: false);


			M.ComenzarEscrito($"{carpeta}/red.txt", cerrar: true);
			best_net.Imprimr();

			M.ComenzarEscrito($"{carpeta}/matrixChange1.csv", cerrar: true);
			AnalizarTodosTransitorios(transitorios1);

			M.ComenzarEscrito($"{carpeta}/matrixChange2.csv", cerrar: true);
			AnalizarTodosTransitorios(transitorios2);

			M.ComenzarEscrito($"{carpeta}/ndsEfectivos.txt", cerrar: true);
			Console.WriteLine(M.List2String(tarea.Depurar(best_net), "\n"));
			M.TerminarEscrito();
		}

		/// <summary>
		/// Dadas dos condiciones iniciales, este met'odo calcula
		/// qué tan distinto es el transitorio de un nodo para cada condición
		/// inicial. Se contabilizan los cambios que sufran los transigorios.
		/// </summary>
		/// <param name="transts"></param>
		static void AnalizarTodosTransitorios(List<List<string>> transts) {
			Console.WriteLine("name,0,1,2,3,4,5,6,7,8,9,10,11");
			for (int i = 0; i < transts.Count - 1; i++) {
				for (int j = i + 1; j < transts.Count; j++) {
					var t1 = transts[i];
					var t2 = transts[j];
					var cambios = AnalizarDosTransitorios(t1, t2).Select(x => M.FltFrm(x)).ToList();

					var ent1 = t1[0].Substring(0, 4);
					var XY1 = ent1.Substring(0, 2);
					var ZW1 = ent1.Substring(2);

					var ent2 = t2[0].Substring(0, 4);
					var XY2 = ent2.Substring(0, 2);
					var ZW2 = ent2.Substring(2);

					if (XY1.Equals(XY2) || ZW1.Equals(ZW2)) {
						Console.WriteLine($"{ent1}-{ent2},{M.List2String(cambios)}");
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="trn1"></param>
		/// <param name="trn2"></param>
		static List<float> AnalizarDosTransitorios(List<string> trn1, List<string> trn2) {
			// El "-1" es para evitar el estado repetido del atractor en el transitorio
			var minln = Math.Min(trn1.Count, trn2.Count) - 1;
			// Me quedo con el transitorio y atractor, sin el valor de las entradas
			// y también quito el primer estado, que siempre son puros ceros
			var listTrans = Enumerable.Range(1, minln).ToList();
			var tt1 = listTrans.Select(x => trn1[x].Substring(4)).ToList();
			var tt2 = listTrans.Select(x => trn2[x].Substring(4)).ToList();
			if (tt1[0].Length != tt2[0].Length)
				throw new ArgumentException("Error al comparar cadenas gvfty");

			var cambios = new List<float>();
			for (int i = 0; i < tt1[0].Length; i++) {
				var coutner = 0f;
				for (int t = 0; t < minln; t++) {
					if (tt1[t][i] != tt2[t][i]) coutner++;
				}
				cambios.Add(coutner / minln);
			}
			return cambios;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		static void PrintTransitorio(List<List<string>> t) {
			foreach (var l in t) {
				foreach (var s in l) {
					Console.WriteLine(s);
					//Console.WriteLine(Network.SepararInputs(s));
				}
				Console.WriteLine("o-o");
			}
		}

		/// <summary>
		/// Import network from txt file
		/// </summary>
		public static void T05() {
			var file_name = "red_importar.txt";
			var datos = M.LeerArchivo(file_name);
			var salida = int.Parse(datos[0].Split('=').Last());
			for (int i = 1; i < datos.Count; i++) {
				Console.WriteLine(datos[i]);
			}
			//var net = new Network();
			for (int i = 1; i < datos.Count; i++) {
				var linea = datos[i].Split('|');
				var n = new Node();
			}
			Console.WriteLine($"La salida es = {salida + 100}");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		static int Gen(string s) {
			var x1 = s.Split('=')[1];
			var x2 = x1.Split('\t')[0];
			return int.Parse(x2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		static string SepararGenes2(string s) {
			var snew = "";
			for (int i = 1; i <= s.Length; i++) {
				snew += s[i - 1];
				if (i % 8 == 0 && i < s.Length) snew += " ";
			}
			return snew;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		static int Hamming(string s1, string s2) {
			if (s1.Length != s2.Length) throw new ArgumentException("s1.Length != s2.Length");
			return M.L(s1.Length).Count(x => s1[x] != s2[x]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		static string T12orS(int x) {
			if (x == 0) return "1";
			if (x == 1) return "S";
			if (x == 2) return "2";
			return "Erororororor";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="files"></param>
		/// <returns></returns>
		static List<string> VerificarDatos(string folder, string files) {
			var datos = M.BuscarDirectorios("zxm", "evolucion_202202");
			var datos_verificados = new List<string>();
			foreach (var x in datos) {
				var d2 = M.LeerArchivo(x);
				var cambios = d2.Where(x => x.Contains("F = 1	Q = ")).ToList();
				var n_cambios = cambios.Count;

				if (n_cambios >= 2) {
					var s_0 = cambios[n_cambios - 2];
					var s_1 = cambios[n_cambios - 1];
					var g_0 = Gen(s_0);
					var g_1 = Gen(s_1);
					if (g_0 % 10 == 9 && g_1 % 10 == 1 && g_0 + 2 == g_1) {
						datos_verificados.Add(x);
					}
				}
			}
			return datos_verificados;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s0"></param>
		/// <param name="s1"></param>
		/// <returns></returns>
		static string DNAX(string s0, string s1) {
			var dnax = "";
			for (int i = 0; i < s0.Length; i++) {
				dnax += (s0[i] == s1[i]) ? s0[i] : 'x';
			}
			return dnax;
		}

		/// <summary>
		/// 
		/// </summary>
		public static void T062222() {
			var datos_verificados = VerificarDatos("zmx", "evolucion_202202");

			var listDNAs = new List<List<string>>();
			foreach (var x in datos_verificados) {
				var simulacion = M.LeerArchivo(x);

				//var mmmmssd = simulacion.Where(x => x.Contains("Cambio en el Fitness, F = ")).ToList();
				var t_final = simulacion.Where(x => x.Contains("Cambio en el Fitness, F = ")).Last()[^1];

				var ldna = new List<string>(); // DNA list
				var DNAs = simulacion.Where(y => y.Contains("DNA_G")).ToList();
				var dna_task2 = DNAs[^3].Split("= ")[1];
				var dna_task1 = DNAs[^1].Split("= ")[1];
				if (t_final == '2') {
					var dna_prov = dna_task2;
					dna_task2 = dna_task1;
					dna_task1 = dna_prov;
				}
				ldna.Add(dna_task1);
				ldna.Add(DNAX(dna_task1, dna_task2));
				ldna.Add(dna_task2);
				listDNAs.Add(ldna);
			}
			listDNAs.RemoveAll(l => Hamming(l[0], l[2]) > 2);

			var fN = "analizar.txt";
			M.ComenzarEscrito(fN);
			listDNAs = listDNAs.OrderBy(x => x[1]).ToList();

			for (int tsk = 0; tsk <= 2; tsk++) {
				var freq_counter = new float[listDNAs[0][0].Length];
				for (int i = 0; i < freq_counter.Length; i++) {
					freq_counter[i] = listDNAs.Count(l => l[tsk][i] == '1') / (float)listDNAs.Count;
				}
				Console.WriteLine(M.List2String(freq_counter.Select(x => $"{x:0.00}").ToList()));
			}

			// Contar repeticiones
			M.ImpLinea("Count Repetitions");
			for (int i = 0; i < 3; i++) {
				Console.WriteLine(
				  $"f_distinto_task{T12orS(i)} = " +
				  $"{listDNAs.Select(l => l[i]).Distinct().Count() / (float)listDNAs.Count:0.000}");
			}

			// Contar genes distintos
			M.ImpLinea("Genes used");
			for (int k = 0; k <= 2; k++) {
				//var jk23 = listDNAs.Select(l => SepararGenes(l[k]).Split(' '));
				//var ss22 = M.L(jk23.First().Length).Select(x => jk23.Select(l => l[x]).Distinct().Count());
				//Console.WriteLine($"Tas{T12orS(k)} " + M.List2String(ss22, "\t"));
			}

			//// Print DNAs and difference
			//M.ImpLinea("ADNs");
			//foreach (var l in listDNAs) {
			//  for (int i = 0; i < l.Count; i++) {
			//    Console.WriteLine($"Ta_{T12orS(i)} {SepararGenes(l[i])}\t{listDNAs.Count(x => x[i] == l[i])}");
			//  }
			//  Console.WriteLine();
			//}

			M.ImpLinea("Distinct");
			for (int i = 0; i < 3; i++) {
				Console.WriteLine($"T{T12orS(i)} distinct = {listDNAs.Select(l => l[i]).Distinct().Count()}");
			}

			//M.ImpLinea("For Task 1");
			//foreach (var x in listDNAs.Select(l => l[0]).Distinct()) {
			//  Console.WriteLine(x);
			//}
			//M.ImpLinea("For the Switch");
			//foreach (var x in listDNAs.Select(l => l[1]).Distinct()) {
			//  Console.WriteLine(x);
			//}
			//M.ImpLinea("For the Task 2");
			//foreach (var x in listDNAs.Select(l => l[2]).Distinct()) {
			//  Console.WriteLine(x);
			//}

			M.TerminarEscrito();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void T0722222() {
			var fN = "analizar_t1.txt";
			M.ComenzarEscrito(fN);
			var datos = M.BuscarDirectorios("ttareas", "evol", "tarea1/resultados/p_fb_1.00/task1");

			var DNAs = datos.Select(x => M.LeerArchivo(x)[0]).ToList();

			// Contar repeticiones
			M.ImpLinea("Count Repetitions");
			Console.WriteLine($"f_distinto = {DNAs.Distinct().Count() / (float)DNAs.Count:0.00}");

			//// Contar genes distintos
			//M.ImpLinea("Genes used");
			//var jk23 = DNAs.Select(x => SepararGenes(x).Split(' '));
			//var ss22 = M.L(jk23.First().Length).Select(x => jk23.Select(l => l[x]).Distinct().Count());
			//Console.WriteLine($"Tas  " + M.List2String(ss22.ToList(), "\t"));

			//// Print DNAs and difference
			//M.ImpLinea("ADNs");
			//foreach (var dna in DNAs) {
			//  Console.WriteLine($"DNA  {SepararGenes(dna)}");
			//}
			//M.TerminarEscrito();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void T08() {
			var directorios = M.BuscarDirectorios(folder: "find_fast_switches/resultados/p_fb_1.00/elegidos", fN: "evoluci");
			foreach (var d in directorios) {
				var datos = M.LeerArchivo(d.Replace("/elegidos", ""));
				M.ComenzarEscrito(d.Replace("/elegidos", "/elegidos2"));

				var task_before = int.Parse(datos[^99].Split("Tarea_")[1]);
				var dna_t2 = datos[^97].Split(" = ")[1];
				var dna_t1 = datos[^43].Split(" = ")[1];
				if (task_before == 1) {
					var dna_temp = dna_t2;
					dna_t2 = dna_t1;
					dna_t1 = dna_temp;
				}

				//for (int j = 18; j >= 6; j--) Console.WriteLine(datos[^j]);
				////Console.WriteLine(datos[^2].Replace("Nodos depurados = ", ""));
				//Console.WriteLine(datos[^2]);
				//Console.WriteLine(datos[^1]);
				//PrintInfoDNAs(dna_t1, dna_t2, txt: false);
				//Console.WriteLine(Hamming(dna_t1, dna_t2));
				//M.TerminarEscrito();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void T08_02(Tarea tarea) {
			var folder = "find_fast_switches/resultados/p_fb_1.00/";
			var d_elegidos = M.BuscarDirectorios(folder: folder + "elegidos", fN: "evoluci");
			var d_general = M.BuscarDirectorios(folder: folder + "01_general", fN: "evoluci");
			var d_noElegido = new List<string>(d_general);
			foreach (var d in d_elegidos) {
				var d2 = d.Replace("elegidos", "01_general");
				d_noElegido.Remove(d2);
			}
			Console.WriteLine("mira");
			// Analysis - Successful switching nodes
			M.ComenzarEscrito(folder + "modularidad_elegidos.txt");
			foreach (var d in d_elegidos) {
				var l0 = M.LeerArchivo(d)[1].Replace("Q = ", "").Split('\t');
				var q = float.Parse(l0[2]);
				Console.WriteLine(q);
			}
			// Analysis - Unsuccessful switching nodes
			M.ComenzarEscrito(folder + "modularidad_NoElegidos.txt", cerrar: true);
			foreach (var d in d_noElegido) {
				var ll = M.LeerArchivo(d)[^5];
				var ll1 = ll.Replace("   G = ", "").Split('\t')[0];
				if (ll1.Equals("999")) {
					var l0 = ll.Replace("Q = ", "").Split('\t');
					var q = float.Parse(l0[2]);
					Console.WriteLine(q);
				}
			}
			M.ComenzarEscrito("elegidos2_betw.txt", cerrar: true);
			var directoriosmm = M.BuscarDirectorios(folder: "find_fast_switches/resultados/p_fb_1.00/elegidos2", fN: "evoluci");
			foreach (var d in directoriosmm) {
				var net = Network.ImportNetwork(d);
				var dbetw = Modularity.EdgeBetweennessCentrality(net.ImprimirConnections(tarea));
				Console.WriteLine(d);
				net.Imprimir_Python(tarea);
				Console.WriteLine($"Output = {tarea.salidas[0]}");
				PrintInfoDNAs(net.dna1, net.dna2, net, tarea, txt: true);
				Console.WriteLine($"Hamming = {Hamming(net.dna1, net.dna2)}");
				foreach (var k in dbetw.Keys) Console.WriteLine($"{k}\t{dbetw[k]}");
				Console.WriteLine();
			}
			M.TerminarEscrito();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void T09() {


			var entradas = new List<int> { 0, 1, 2, 3 };
			var salidas = new List<int> { 10 };

			//var lcy = Ciclos.LcyLogic(XNX, entradas);

			//var t1 = Tarea.CrearTarea(entradas, salidas, Ciclos.LcyLogic(XNX, entradas), "1");
			//var t2 = Tarea.CrearTarea(entradas, salidas, Ciclos.LcyLogic(XOX, entradas), "2");

			var fn = "chequiar.txt";
			M.ComenzarEscrito(fn);
			var directorios = M.BuscarDirectorios(folder: "find_fast_switches/resultados/p_fb_1.00/elegidos2", fN: "evoluci");
			foreach (var d in directorios) {
				var net = Network.ImportNetwork(d);
				Console.WriteLine(d);
				//if (!CheckNet(net, t1, t2, net.dna1, net.dna2)) {
				//  throw new ArgumentException("Problema al revisar redes");
				//}
			}
			M.TerminarEscrito();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void T10() {
			foreach (var pf in new List<string>() { "0.00", "0.10", "0.20", "0.30", "0.40", "0.50", "0.60", "0.70", "0.80", "0.90", "1.00" }) {
				var fn = $"FitnessvsGen_{pf}.txt";
				M.ComenzarEscrito(fn);

				var subFolder = "p_fb_" + pf;
				var directorios = M.BuscarDirectorios("no_modular_pfb", "evolucion_", subFolder);
				var fitnesses = new List<List<float>>();

				foreach (var d in directorios) {
					var datos = M.LeerArchivo(d);
					var info = datos.Where(x => x.Contains("   G = ")).Select(y => float.Parse(y.Split("F = ")[1].Split('\t')[0])).ToList();
					if (info.Count < 1000 && info[^1] < treshold) throw new ArgumentException("inconsistenciax02");
					fitnesses.Add(info);
				}

				for (int i = 0; i < 1000; i++) {
					var lkj = new List<float>();
					foreach (var l in fitnesses) {
						if (l.Count > i) lkj.Add(l[i]);
						else lkj.Add(l[^1]);
					}
					Console.WriteLine($"{lkj.Average()}");
				}
				M.TerminarEscrito();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void T11(string carpeta) {
			var C = M.BuscarDirectorios("resultados", "evolucion_", carpeta);

			var ll = new List<List<float>>();

			foreach (var f in C) {
				var data = M.LeerArchivo(f);
				var data_filtrada = new List<float>();
				foreach (var linea in data) {
					if (linea.Contains("_q_Q_ = ")) {
						float Q = float.Parse(linea.Split("_q_Q_ = ")[1].Split('\t')[0]);
						data_filtrada.Add(Q);
					}
				}
				if (data_filtrada.Count > 990) ll.Add(data_filtrada);
			}

			M.ComenzarEscrito($"{carpeta}CoeficienteDeModularidad.csv");
			var max_g = ll.Select(l => l.Count).Max();
			for (int g = 0; g < max_g; g++) {
				var mensaje = "";
				foreach (var l in ll) {
					if (l.Count <= g) mensaje += $",";
					else mensaje += $"{l[g]},";
				}
				Console.WriteLine(M.QUlt(mensaje));
			}
			M.TerminarEscrito();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void T12() {
			T11("resultados/");
			//T11("/no_modular/");
		}




	}
}







//public static bool AND(List<bool> l) => l[0] && l[1];

//public static bool XNX(List<bool> l) => (l[0] ^ l[1]) && (l[2] ^ l[3]);



//public static bool oNX(List<bool> l) => (l[0] || l[1]) || (l[2] ^ l[3]);
//public static bool aNX(List<bool> l) => (l[0] && l[1]) && (l[2] ^ l[3]);

//public static bool XOX(List<bool> l) => (l[0] ^ l[1]) || (l[2] ^ l[3]);
//public static bool oOX(List<bool> l) => (l[0] || l[1]) || (l[2] ^ l[3]);
//public static bool aOX(List<bool> l) => (l[0] && l[1]) || (l[2] ^ l[3]);


//var t1 = Tarea.CrearTarea(entradas, salidas, Ciclos.LcyLogic(RND1, entradas), "1");
//var t2 = Tarea.CrearTarea(entradas, salidas, Ciclos.LcyLogic(RND2, entradas), "2");




//int Bol2Int(bool b) => b == true ? 1 : 0;
//bool RND1(List<bool> l) => f1[$"{Bol2Int(l[0])}{Bol2Int(l[1])}{Bol2Int(l[2])}"] && f2[$"{Bol2Int(l[3])}{Bol2Int(l[4])}{Bol2Int(l[5])}"];
//bool RND2(List<bool> l) => f1[$"{Bol2Int(l[0])}{Bol2Int(l[1])}{Bol2Int(l[2])}"] || f2[$"{Bol2Int(l[3])}{Bol2Int(l[4])}{Bol2Int(l[5])}"];

//var v1in = new List<string>() {
//  "1001",
//  "0110"
//};
//var v1ou = new List<string>() {
//  "00",
//  "11"
//};

//var v2in = new List<string>() {
//  "1101",
//  "0010"
//};
//var v2ou = new List<string>() {
//  "01",
//  "10"
//};

//var v3in = new List<string>() {
//  "1101",
//  "1111",
//  "0010"
//};
//var v3ou = new List<string>() {
//  "00",
//  "01"
//};

//var v4in = new List<string>() {
//  "1010"
//};
//var v4ou = new List<string>() {
//  "00",
//  "10"
//};

//var lcy1 = new List<Ciclos>() { Ciclos.Set(v1in, v1ou), Ciclos.Set(v2in, v2ou) };
//var lcy2 = new List<Ciclos>() { Ciclos.Set(v3in, v3ou), Ciclos.Set(v4in, v4ou) };