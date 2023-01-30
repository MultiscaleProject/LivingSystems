using System;
using System.Collections.Generic;
using System.Linq;

namespace modularidad_core3._1 {
  public class Network {


    static readonly Random rnd = new Random(DateTime.Now.Millisecond);

    public static readonly string separador_address = ",";
    public static readonly string separador_genes = " ";
    //public static readonly int digits5 = 4;
    //public static readonly int n_entradas = 4;
    //public static readonly Dictionary<string, int> direcciones = new Dictionary<string, int>();

    //public static Dictionary<string, int> G1 = new Dictionary<string, int>();
    //public static Dictionary<string, int> G2 = new Dictionary<string, int>();

    public List<Node> nodes = new List<Node>();
    //public List<int> salidas = new List<int>();
    //public int output;
    //public List<int> nodos_efectivos = new List<int>();

    //public Dictionary<string, int> resultados232323 = new Dictionary<string, int>();
    public List<Ciclos> lcy = new List<Ciclos>();

    public float fit;
    public double dist;

    public float modularidad;
    public List<List<string>> comunidades = new List<List<string>>();

    //public double newFitness;

    public string dna1;
    public string dna2;

    public int indice;

    public int nMutS = 0;
    public int nMutF = 0;

    public int nEntradas = 0;

    //public string redStr;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Network Clonar() {
      var net = new Network();
      net.fit = fit;
      net.dist = dist;
      net.indice = indice;
      net.nMutS = nMutS;
      net.nMutF = nMutF;
      net.nEntradas = nEntradas;

      net.modularidad = modularidad;
      foreach (var l in comunidades) net.comunidades.Add(new List<string>(l));
      //net.newFitness = newFitness;
      //net.output = output;
      //net.redStr = redStr;
      //net.salidas = new List<string>(salidas);
      lcy.ForEach(c => net.lcy.Add(c.Clonar()));
      nodes.ForEach(n => net.nodes.Add(n.Clonar()));
      return net;
    }

    /// <summary>
    /// 
    /// </summary>
    public void PrintCommunities() {
      var newListaC = M.List2String(comunidades.Select(c => M.List2String(c)), "|");
      Console.WriteLine(newListaC);
    }

    bool Int2Bol(char ch) => (ch == '0') ? false : true;

    public Network() {
      //for (int i = 0; i < Math.Pow(2, digits); i++) {
      //  direcciones[M.Int2Bin(i, digits)] = i;
      //}
      //for (int i = 0; i < Math.Pow(2, n_entradas); i++) {
      //  var i_b = M.Int2Bin(i, n_entradas);
      //  var X = i_b[0] == '1';
      //  var Y = i_b[1] == '1';
      //  var Z = i_b[2] == '1';
      //  var W = i_b[3] == '1';
      //  G1[i_b] = ((X ^ Y) && (Z ^ W)) ? 1 : 0;
      //  G2[i_b] = ((X ^ Y) || (Z ^ W)) ? 1 : 0;
      //}
    }

    //static string ConstruirGen_rnd() {
    //  var gate_type = "00" + separador_address;
    //  var first_address = M.CadenaBinaria(4) + separador_address;
    //  var second_address = M.CadenaBinaria(4);
    //  var gen = gate_type + first_address + second_address;
    //  return gen;
    //}

    /// <summary>
    /// 
    /// </summary>
    public void ConstruirRed(int nnds = 16, int k = 2) {
      nodes.Clear();
      for (int i = 0; i < nnds; i++) {
        var n = new Node();
        n.AgregarInputs(nnds, k);
        n.ConsturiBf_RND();
        //n.ConstruirBf_NAND();
        nodes.Add(n);
      }

      //var output = nodes.Last();
      //output.inputs.Clear();
      //output.AgregarInputs(nnds, 2);
      //output.ConsturiBf_RND();

      //nodes.Last().ConstruirBf_NAND();
      BuscarSalidas();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newDna"></param>
    public void ReemplazarDNA(string newDna) {
      var adn = newDna.Replace(" ", "");
      // Take the four values of the boolean function for each node
      for (int i = 0; i < adn.Length; i += 4) {
        var id_node = i / 4;
        var n = nodes[id_node];
        n.fB["00"] = $"{adn[i + 0]}";
        n.fB["01"] = $"{adn[i + 1]}";
        n.fB["10"] = $"{adn[i + 2]}";
        n.fB["11"] = $"{adn[i + 3]}";
      }
    }

    /// <summary>
    /// With this method we check if the network performs
    /// the asigned task. 
    /// </summary>
    /// <param name="file_name"></param>
    /// <param name="dna"></param>
    /// <returns></returns>
    public static bool CheckNetwork(Network net, string dna, Tarea tarea) {
      var net2 = net.Clonar();
      net2.ReemplazarDNA(dna);
      //net.Imprimr();
      tarea.Fitness(net2);
      //net2.MedirFitness(tarea);
      return T.treshold <= net2.fit;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file_name"></param>
    /// <returns></returns>
    public static Network ImportNetwork(string file_name) {
      var datos = M.LeerArchivo(file_name);
      var net = new Network();
      for (int i = 1; i <= 12; i++) {
        var linea = datos[i].Split('|');
        var n = new Node();
        foreach (var input in linea[1].Split(',')) {
          n.inputs.Add(int.Parse(input));
        }
        for (int j = 0; j < linea[2].Length; j++) {
          var conf = M.Int2Bin(j, n.inputs.Count);
          n.fB[conf] = $"{linea[2][j]}";
        }
        net.nodes.Add(n);
      }
      net.BuscarSalidas();
      //net.output = int.Parse(datos[0].Split(" = ")[1]);
      net.dna1 = datos[15];
      net.dna2 = datos[17];
      return net;
    }


    /// <summary>
    /// 
    /// </summary>
    public static Network ImportStructure(string file_name) {
      //var file_name = "net_no_modular.txt";
      var datos = M.LeerArchivo(file_name);
      var net = new Network();
      for (int i = 0; i < datos.Count; i++) {
        var linea = datos[i].Split('|');
        var n = new Node();
        foreach (var input in linea[1].Split(',')) {
          n.inputs.Add(int.Parse(input));
        }
        //n.ConstruirBf_NAND();
        n.ConsturiBf_RND();
        net.nodes.Add(n);
      }
      net.BuscarSalidas();
      //net.output = 10;
      //net.MedirDistancia();
      //net.MedirFitness(Network.G1);
      //var conecciones = net.ImprimirConnections();
      //var q = Modularity.T04(conecciones);
      //Console.WriteLine($"Output = {net.output}");
      //Console.WriteLine($"Connections = {conecciones}");
      //Console.WriteLine($"Distance = {net.dist}");
      //Console.WriteLine($"Fitness = {net.fitness}");
      //Console.WriteLine($"Modularity = {q}");
      //net.Imprimir_Python();
      return net;
    }









    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="e"></param>
    ///// <returns></returns>
    //public static string SepararInputs(string e) {
    //  return e.Substring(0, n_entradas) + " " + e.Substring(n_entradas);
    //}

    //string MarkOutput() {
    //  var mark = new string(' ', n_entradas + 1);
    //  for (int i = 0; i < nodes.Count; i++) {
    //    if (i != output) mark += " ";
    //    else mark += "x";
    //  }
    //  return mark;
    //}



    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //public List<List<string>> Transitorios(Tarea tarea) {
    //  var transitorios = new List<List<string>>();
    //  for (int condi = 0; condi < Math.Pow(2, tarea.entradas.Count); condi++) {
    //    var e0 = M.Int2Bin(condi, tarea.entradas.Count) + new string('0', nodes.Count);
    //    var transitorio = new List<string>() { e0 };
    //    for (int t = 0; t < 10000; t++) {
    //      e0 = Ev(e0, tarea);
    //      if (transitorio.Contains(e0)) {
    //        transitorio.Add(e0);
    //        break;
    //      }
    //      transitorio.Add(e0);
    //    }
    //    transitorios.Add(transitorio);
    //  }
    //  return transitorios;
    //}

    /// <summary>
    /// 
    /// </summary>
    public void BuscarSalidas() {
      for (int i = 0; i < nodes.Count; i++) {
        var n = nodes[i];
        n.outputs.Clear();
        for (int j = 0; j < nodes.Count; j++) {
          if (nodes[j].inputs.Contains(i)) n.outputs.Add(j);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Imprimr() {
      //Console.WriteLine($"Output = {output}");
      for (int i = 0; i < nodes.Count; i++) {
        var lkj = $"{i}|";
        var n = nodes[i];
        foreach (var k in n.inputs) lkj += $"{k},";
        lkj = M.QUlt(lkj) + "|";
        var configs = n.fB.Keys.ToList();
        configs.Sort();
        if (i >= nEntradas) foreach (var c in configs) lkj += n.fB[c];
        Console.WriteLine(lkj);
      }

    }

    /// <summary>
    /// 
    /// </summary>
    public string PrintDNA() {
      var DNA = "";
      for (int i = nEntradas; i < nodes.Count; i++) {
        DNA += M.List2String(nodes[i].fB.Values, "");
      }
      return DNA;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public string SepararGenes(string s) {
      var longDNA = 0;
      var longitudesDNA = new List<int>();
      for (int i = nEntradas; i < nodes.Count; i++) {
        longDNA += nodes[i].fB.Count;
        longitudesDNA.Add(nodes[i].fB.Count);
      }
      if (s.Length != longDNA) throw new ArgumentException("Error longDNA");


      var s2 = "";
      var contador = 0;


      foreach (var lngsdna in longitudesDNA) {
        for (int i = contador; i < contador + lngsdna; i++) s2 += s[i];
        s2 = " ";
        contador += lngsdna;
      }


      return M.QUlt(s2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public string PrintDNASeparado(Tarea t) {
      var DNA = "";
      for (int i = nEntradas; i < nodes.Count; i++) {
        DNA += M.List2String(nodes[i].fB.Values, "") + " ";
      }
      return M.QUlt(DNA);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string PrintRedStr() {
      var redstr = "";
      for (int i = 0; i < nodes.Count; i++) {
        var n = nodes[i];
        redstr += $"{i}|{M.List2String(n.inputs)}|{n.PrintFbValues()}&";
      }
      return redstr;
    }

    /// <summary>
    /// 
    /// </summary>
    public string ImprimirConnections(Tarea tarea) {
      var ndsD = tarea.Depurar(this);
      var conexion = "[";
      for (int i = 0; i < nodes.Count; i++) {
        var n = nodes[i];
        foreach (var e in n.inputs) {
          if (ndsD.Contains(i) && ndsD.Contains(e)) conexion += $"({i},{e}),";
        }
      }
      return $"{M.QUlt(conexion)}]";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string Conexiones(Tarea tarea) {
      var ndsD = tarea.Depurar(this);
      var conexion = "[";
      for (int i = 0; i < nodes.Count; i++) {
        var n = nodes[i];
        foreach (var e in n.inputs) {
          if (ndsD.Contains(i) && ndsD.Contains(e)) {
            conexion += $"({e},{i}),";
          }
        }
      }
      return M.QUlt(conexion) + "]";
    }

    /// <summary>
    /// 
    /// </summary>
    public void Imprimir_Python(Tarea tarea) {
      var identificador = M.RndPalabra(5, "");
      identificador = "";
      var nombre = "G(conex_" + identificador;
      Console.WriteLine($"{nombre} = {Conexiones(tarea)})");
      //var red = $"red_output{M.List2String(tarea.salidas)}_" + identificador;
      //Console.WriteLine($"{red} = ListaConexiones_to_Red({nombre})");
      //Console.WriteLine($"Analizar_Modularidad({red})");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tarea"></param>
    /// <returns></returns>
    public string PrintPython(Tarea tarea) {
      var identificador = M.RndPalabra(5, "");
      identificador = "";
      var nombre = "conex_" + identificador;
      return $"{nombre} = {Conexiones(tarea)}";
      //Console.WriteLine();
      //var red = $"red_output{M.List2String(tarea.salidas)}_" + identificador;
      //Console.WriteLine($"{red} = ListaConexiones_to_Red({nombre})");
      //Console.WriteLine($"Analizar_Modularidad({red})");
    }


    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="g">Generación</param>
    ///// <returns></returns>
    //public float MedirFitness(Tarea tarea) {
    //  int n_efectivos = Depurar(tarea).Count - n_entradas;
    //  Performance(tarea);
    //  var contador = 0;
    //  foreach (var cdi in resultados.Keys) {
    //    if (resultados[cdi] == tarea.G[cdi]) contador++;
    //  }
    //  fit = (float)contador / resultados.Count;
    //  if (n_efectivos > 11) fit -= 0.2f;
    //  return fit;
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //public double MedirDistancia() {
    //  dist = 0d;
    //  var N = nodes.Count + n_entradas + 1;
    //  for (int i = 0; i < nodes.Count; i++) {
    //    var n1 = nodes[i];
    //    foreach (var input in n1.inputs) {
    //      var a1 = Math.Cos((i + n_entradas) * 2 * Math.PI / N);
    //      var a2 = Math.Cos((input + n_entradas) * 2 * Math.PI / N);
    //      if (i != input) {
    //        dist += Math.Acos(Math.Cos(a1) * Math.Cos(a2) + Math.Sin(a1) * Math.Sin(a2));
    //      }
    //    }
    //  }
    //  return dist;
    //}

    /// <summary>
    /// 
    /// </summary>
    public void QueTantoInfluye(Tarea tarea) {
      var efectivos = tarea.Depurar(this);
      var nCondiciones = 100;
      for (int i = 0; i < nodes.Count; i++) {
        var n = nodes[i];
        foreach (var entrada in n.inputs) {
          var efectividad = 0f;
          if (efectivos.Contains(i) && efectivos.Contains(entrada)) {
            for (int b = 0; b < nCondiciones; b++) {
              var e01 = M.CadenaBinaria(nodes.Count, rnd.NextDouble());
              var e02 = M.FlipValorCadenaBin(e01, entrada);
              var e11 = tarea.Ev(e01, this);
              var e12 = tarea.Ev(e02, this);
              efectividad += (e11[i] != e12[i]) ? 1f : 0f;
            }
            Console.WriteLine($"{entrada}->{i} = {efectividad / nCondiciones:0.00}, T = {efectividad}");
          }
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="llEstados"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    public void InfoMut(List<List<string>> llEstados) {
      //var N = nodes.Count;
      //var slkdj = Depurar();
      //var datos_infoMut = new Dictionary<string, string>();
      //for (int i = 0; i < N; i++) {
      //  for (int j = 0; j < N; j++) {
      //    if (!slkdj.Contains(i) || !slkdj.Contains(j)) {
      //      datos_infoMut[$"{i}->{j}"] = "x";
      //    } else {
      //      var infoMut_ij = 0f;
      //      foreach (var l in llEstados) {
      //        var l2 = l.Select(x => x.Substring(4)).ToList();
      //        infoMut_ij += InformacionMutua(l2, i, j);
      //      }
      //      datos_infoMut[$"{i}->{j}"] = $"{infoMut_ij:0.00}";
      //    }
      //  }
      //}

      //var sep = "\t";
      //Console.WriteLine($"nodo{sep}" + M.List2String(M.L(N), sep));
      //for (int i = 0; i < N; i++) {
      //  var renglon = $"{i}{sep}";
      //  for (int j = 0; j < N; j++) {
      //    renglon += $"{datos_infoMut[$"{i}->{j}"]}{sep}";
      //  }
      //  Console.WriteLine(M.QUlt(renglon));
      //}
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public float Lg2(float x) {
      if (x == 0) return 0f;
      return x * (float)Math.Log(x) / (float)Math.Log(2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lEstados"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    public float InformacionMutua(List<string> lEstados, int i, int j) {
      var T = (float)lEstados.Count;
      float pi0, pi1, pj0, pj1;
      pi0 = lEstados.Count(x => x[i] == '0') / T;
      pj0 = lEstados.Count(x => x[j] == '0') / T;
      pi1 = 1 - pi0;
      pj1 = 1 - pj0;
      float p00 = 0f, p01 = 0f, p10 = 0f, p11 = 0f;
      T--;
      for (int t = 0; t < T; t++) {
        var s1 = lEstados[t];
        var s2 = lEstados[t + 1];
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
    /// <param name="p_fB">Probability to choose mutations
    /// for the boolean funcion instead the network's structure</param>
    public void Mutar(float p_fB, Tarea t) {
      var elegido = rnd.Next(nEntradas, nodes.Count);
      var n = nodes[elegido];
      if (M.Intento(p_fB)) {
        nMutF++;
        var clave = M.RndElemento(n.fB.Keys.ToList());
        var val = n.fB[clave];
        n.fB[clave] = (val.Equals("0")) ? "1" : "0";
      } else {
        nMutS++;
        var entrada_idx = rnd.Next(n.inputs.Count);
        n.inputs[entrada_idx] = M.RndConExcepciones(0, nodes.Count, n.inputs[entrada_idx]);
      }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="t"></param>
    ///// <returns></returns>
    //public Network Depurar(Tarea t) {
    //  var redEfectiva = new Network();
    //  var nds_efectiv = new List<int>(t.salidas);
    //  foreach (var salida in t.salidas) {
    //    var revisados = new List<int>();
    //    while (true) {
    //      var por_revisar = new List<int>();
    //      foreach (var z in nds_efectiv.Where(x => !t.entradas.Contains(x) && !revisados.Contains(x)).ToList()) {
    //        var n = nodes[z];
    //        foreach (var e in n.inputs) {
    //          if (!nds_efectiv.Contains(e)) {
    //            nds_efectiv.Add(e);
    //            por_revisar.Add(e);
    //          }
    //        }
    //      }
    //      if (por_revisar.Count == 0) break;
    //      foreach (var x in por_revisar) revisados.Add(x);
    //    }
    //  }
    //  foreach (var et in t.entradas) redEfectiva.nodes.Add(nodes[et]);
    //  foreach (var it in nds_efectiv)
    //    if (!t.entradas.Contains(it) && !t.salidas.Contains(it))
    //      redEfectiva.nodes.Add(nodes[it]);
    //  foreach (var st in t.salidas) redEfectiva.nodes.Add(nodes[st]);
    //  return redEfectiva;
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //public List<int> Depurar2() {
    //  nodos_efectivos = new List<int>() { output };
    //  if (output < 0) return nodos_efectivos;
    //  while (true) {
    //    var contador = 0;
    //    foreach (var z in nodos_efectivos.Where(x => x >= 0).ToList()) {
    //      var n = nodes[z];
    //      foreach (var e in n.inputs) {
    //        if (!nodos_efectivos.Contains(e)) {
    //          nodos_efectivos.Add(e);
    //          contador++;
    //        }
    //      }
    //    }
    //    if (contador == 0) break;
    //  }
    //  return nodos_efectivos;
    //}





  }
}

