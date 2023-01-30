using System;
using System.Collections.Generic;
using System.Linq;

namespace modularidad_core3._1 {
  public class Ciclos {
    public List<string> valoresDeEntrada = new List<string>();
    public List<string> valoresDeSalida = new List<string>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="valoresDeEntrada_"></param>
    /// <param name="valoresDeSalida_"></param>
    public static Ciclos Set(List<string> valoresDeEntrada_, List<string> valoresDeSalida_) {
      var cy = new Ciclos();
      cy.valoresDeEntrada = new List<string>(valoresDeEntrada_);
      cy.valoresDeSalida = new List<string>(valoresDeSalida_);
      return cy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Ciclos Clonar() {
      var c = new Ciclos();
      c.valoresDeEntrada = new List<string>(valoresDeEntrada);
      c.valoresDeSalida = new List<string>(valoresDeSalida);
      return c;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public static List<Ciclos> LcyLogic(Func<List<bool>, bool> func, List<int> entradas) {
      var lcy = new List<Ciclos>();
      for (int i = 0; i < Math.Pow(2, entradas.Count); i++) {
        var e = M.Int2Bin(i, entradas.Count);
        var s = func(e.Select(x => x == '1').ToList());
        var condicion = new List<string>() { e };
        var salida = new List<string>() { s ? "1" : "0" };
        var cy = new Ciclos();
        cy.valoresDeEntrada = condicion;
        cy.valoresDeSalida = salida;
        lcy.Add(cy);
      }
      return lcy;
    }

  }


  public class Tarea {






    public string name;

    //public Network net = new Network();

    public List<int> entradas = new List<int>();
    public List<int> salidas = new List<int>();

    public Dictionary<int, int> dicEnt = new Dictionary<int, int>();

    //public Dictionary<string, int> G = new Dictionary<string, int>();

    //public Ciclos vvv = new Ciclos();

    public List<Ciclos> lcy = new List<Ciclos>();

    public Tarea() {
      //entradas = entradas_;
      //salidas = salidas_;
      //name = name_;
      //lcy = lcy_;
      //dicEnt = M.L(entradas.Count).ToDictionary(x => entradas[x], x => x);
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
          //if (resultf1f2 == "00") continue;
          var cyc1 = new Ciclos();
          cyc1.valoresDeEntrada = new List<string> { valentrada };
          cyc1.valoresDeSalida = new List<string> { $"{H[resultf1f2]}" };
          lcy.Add(cyc1);
        }
      }
      return lcy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entradas_"></param>
    /// <param name="salidas_"></param>
    /// <param name="lcy_"></param>
    /// <param name="name_"></param>
    /// <returns></returns>
    public static Tarea CrearTarea(List<int> entradas_, List<int> salidas_,
    string name_, Dictionary<string, string> f1, Dictionary<string, string> f2,
    Dictionary<string, string> H) {
      var t = new Tarea();
      t.entradas = entradas_;
      t.salidas = salidas_;
      t.name = name_;
      t.lcy = ConsructCycleList(f1, f2, H);
      t.dicEnt = M.L(t.entradas.Count).ToDictionary(x => t.entradas[x], x => x);
      return t;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="func"></param>
    ///// <param name="name_"></param>
    //public static Tarea CrearTarea(List<int> entradas, List<int> salidas,
    //Func<List<bool>, int> func, string name_) {
    //  var t = new Tarea {
    //    entradas = new List<int>(entradas),
    //    salidas = new List<int>(salidas),
    //    name = name_
    //  };
    //  var nentr = t.entradas.Count;
    //  for (int i = 0; i < Math.Pow(2, nentr); i++) {
    //    var cadena = M.Int2Bin(i, nentr);
    //    var l_b = cadena.Select(x => x == '1').ToList();
    //    t.G[cadena] = func(l_b);
    //  }
    //  //t.net = RedEfectiva(red, t);
    //  return t;
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="red0"></param>
    /// <param name="tarea"></param>
    /// <returns></returns>
    public Network RedEfectiva(Network red0) {
      var id_depurados = Depurar(red0);
      return RedDepurada(id_depurados, red0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public List<int> Depurar(Network red0) {
      // The effective nodes contains all the possible task's outputs
      var nds_efectiv = new List<int>(salidas);
      foreach (var salida in salidas) {
        var revisados = new List<int>();
        while (true) {
          var newEffectiveNodes = new List<int>();
          foreach (var z in nds_efectiv.Where(x => !entradas.Contains(x) &&
          !revisados.Contains(x)).ToList()) {
            foreach (var e in red0.nodes[z].inputs) {
              if (!nds_efectiv.Contains(e) && !newEffectiveNodes.Contains(e))
                newEffectiveNodes.Add(e);
            }
            revisados.Add(z);
          }
          if (newEffectiveNodes.Count == 0) break;
          newEffectiveNodes.ForEach(x => nds_efectiv.Add(x));
        }
      }
      return nds_efectiv;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e0"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public string Ev(string e0, Network net0) {
      var depurados = Depurar(net0);
      var e1 = "";
      for (int i = 0; i < net0.nodes.Count; i++) {
        if (entradas.Contains(i) || !depurados.Contains(i)) e1 += e0[i];
        else {
          //var conf = net.nodes[i].inputs.Select(x => e0[x]).ToString();
          var conf = "";
          foreach (var c in net0.nodes[i].inputs) conf += e0[c];
          e1 += net0.nodes[i].fB[conf];
        }
      }
      return e1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="condIni"></param>
    /// <param name="net0"></param>
    /// <returns></returns>
    public string SuperCondnIni(string condIni, Network net0) {
      var e1 = "";
      for (int i = 0; i < net0.nodes.Count; i++) {
        e1 += entradas.Contains(i) ? $"{condIni[dicEnt[i]]}" : "0";
      }
      return e1;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="e0"></param>
    /// <param name="net0"></param>
    /// <param name="new_entradas"></param>
    /// <returns></returns>
    string CambiarEntradas(string e0, Network net0, string new_entradas) {
      var e1 = "";
      for (int i = 0; i < net0.nodes.Count; i++) {
        e1 += entradas.Contains(i) ? new_entradas[dicEnt[i]] : e0[i];
      }
      return e1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e0"></param>
    /// <param name="net0"></param>
    /// <returns></returns>
    string BuscarRepetido(Ciclos cy, Network net0) {
      var e0 = SuperCondnIni(cy.valoresDeEntrada[0], net0);
      var local = new HashSet<string>() { e0 };
      for (int t = 1; t <= 10000; t++) {
        e0 = Ev(e0, net0);
        if (local.Contains(e0)) return e0;
        local.Add(e0);
        var tt = t % cy.valoresDeEntrada.Count;
        e0 = CambiarEntradas(e0, net0, cy.valoresDeEntrada[tt]);
      }
      return "Error al buscar repetido";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<List<string>> Transitorios(Network net0) {
      var transitorios = new List<List<string>>();
      foreach (var cy in lcy) {
        var e0 = SuperCondnIni(cy.valoresDeEntrada[0], net0);
        var transitorio = new List<string>() { e0 };
        for (int t = 0; t < 10000; t++) {
          e0 = Ev(e0, net0);
          if (transitorio.Contains(e0)) {
            transitorio.Add(e0);
            break;
          }
          transitorio.Add(e0);
          var tt = t % cy.valoresDeEntrada.Count;
          e0 = CambiarEntradas(e0, net0, cy.valoresDeEntrada[tt]);
        }
        transitorios.Add(transitorio);
      }
      return transitorios;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="semilla"></param>
    /// <param name="net0"></param>
    /// <returns></returns>
    List<string> Semilla2Ciclo(string semilla, Network net0) {
      var ciclo = new List<string>();
      while (!ciclo.Contains(semilla)) {
        ciclo.Add(semilla);
        semilla = Ev(semilla, net0);
      }
      return ciclo;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cycle"></param>
    /// <returns></returns>
    List<string> OutputsValue(List<string> cycle) {
      var valoresSalidas = new List<string>();
      foreach (var e in cycle) {
        valoresSalidas.Add(M.List2String(salidas.Select(s => e[s]), ""));
      }
      return valoresSalidas;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net0"></param>
    /// <returns></returns>
    public void Performance(Network net0) {
      net0.lcy.Clear();
      foreach (var cy in lcy) {
        var semilla = BuscarRepetido(cy, net0);
        var atractor = Semilla2Ciclo(semilla, net0);
        var cicloResultado = OutputsValue(atractor);

        var ncy = new Ciclos();
        ncy.valoresDeEntrada = new List<string>(cy.valoresDeEntrada);
        ncy.valoresDeSalida = new List<string>(cicloResultado);
        net0.lcy.Add(ncy);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="net0"></param>
    public float Fitness(Network net0) {
      Performance(net0);
      var exito = 0f;
      for (int i = 0; i < lcy.Count; i++) {
        exito += IgualdadEntreCiclos(net0.lcy[i].valoresDeSalida, lcy[i].valoresDeSalida);
      }
      var ndsDepurados = Depurar(net0);
      net0.fit = exito / lcy.Count;
      if (net0.fit > T.treshold) net0.fit -= 0.000001f * ndsDepurados.Count;
      return net0.fit;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <returns></returns>
    public float IgualdadEntreCiclos(List<string> c1, List<string> c2) {
      var min = Math.Min(c1.Count, c2.Count);
      var max = 2 * Math.Max(c1.Count, c2.Count); // importante multiplicar por 2 para garantizar
                                                  // que los ciclos realmente sean iguales
      var cymin = c1;
      var cymax = c2;
      if (c1.Count > c2.Count) {
        cymin = c2;
        cymax = c1;
      }
      var mayor_similitud = 0f;
      for (int i = 0; i < min; i++) {
        var similitud = 0f;
        var ciclopermutado = M.L(max).Select(x => cymin[(i + x) % min]).ToList();
        for (int j = 0; j < max; j++) similitud += Antihamming(ciclopermutado[j], cymax[j % cymax.Count]);
        if (similitud > mayor_similitud) mayor_similitud = similitud;
        if (mayor_similitud == max) break;
      }
      return mayor_similitud / max;
    }

    /// <summary>
    /// This method measures how similiar are two strings. When both are the
    /// same returns the value 1.
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    float Antihamming(string s1, string s2) {
      if (s1.Length != s2.Length) {
        throw new ArgumentException("Problema Hamming a7nb25");
      }
      var l = s1.Length;
      return M.L(l).Count(x => s1[x] == s2[x]) / (float)l; ;
    }





    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static List<int> Depurar299999999(Network red0, List<int> entradas, List<int> salidas) {
      // The effective nodes contains all the possible task's outputs
      var ndsEffctiv = new List<int>(salidas);
      foreach (var salida in salidas) {
        var revisados = new List<int>();
        while (true) {
          var newEfNds = new List<int>();
          foreach (var z in ndsEffctiv.Where(x => !entradas.Contains(x) &&
          !revisados.Contains(x)).ToList()) {
            foreach (var e in red0.nodes[z].inputs) {
              if (!ndsEffctiv.Contains(e) && !newEfNds.Contains(e)) newEfNds.Add(e);
            }
            revisados.Add(z);
          }
          if (newEfNds.Count == 0) break;
          newEfNds.ForEach(x => ndsEffctiv.Add(x));
        }
      }
      return ndsEffctiv;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="efctvs">Lista de nodos efectivos</param>
    /// <param name="t">tarea</param>
    /// <param name="red">Red original</param>
    /// <returns></returns>
    public Network RedDepurada(List<int> efctvs, Network red) {
      var redDep = new Network();
      foreach (var et in entradas) redDep.nodes.Add(red.nodes[et]);
      foreach (var it in efctvs)
        if (!entradas.Contains(it) && !salidas.Contains(it))
          redDep.nodes.Add(red.nodes[it]);
      foreach (var st in salidas) redDep.nodes.Add(red.nodes[st]);
      return redDep;
    }













  }
}

