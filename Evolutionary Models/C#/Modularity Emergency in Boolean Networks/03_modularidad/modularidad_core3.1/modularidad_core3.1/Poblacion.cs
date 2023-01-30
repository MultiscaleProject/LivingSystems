using System;
using System.Collections.Generic;
using System.Linq;

namespace modularidad_core3._1 {
  public class Poblacion {



    static readonly Random rnd = new Random(DateTime.Now.Millisecond);

    public List<Network> redes = new List<Network>();

    public Poblacion() {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net0"></param>
    /// <returns></returns>
    public Network LimpiarRed(Network net0, Tarea t) {
      var net = net0.Clonar();

      var listaRedes = new List<Network>();
      for (int i = 0; i < 100; i++) {
        listaRedes.Add(net0.Clonar());
      }

      for (int g = 0; g < 100; g++) {
        for (int id = 0; id < listaRedes.Count; id++) {
          var r0 = listaRedes[id];
          var n0 = t.Depurar(r0).Count;

          var r1 = r0.Clonar();
          r1.Mutar(p_fB: 0, t);
          t.Fitness(r1);
          var n1 = t.Depurar(r1).Count;

          if (r1.fit > T.treshold & n1 <= n0) listaRedes[id] = r1;
        }
      }

      return listaRedes.OrderBy(r => t.Depurar(r).Count).First();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="S"></param>
    public void ConstruirRedesDistintas(int S, Tarea tarea, int nNds) {
      while (redes.Count < S) {
        Network net = new Network();
        net.ConstruirRed(nnds: nNds + 1);
        net.nEntradas = tarea.entradas.Count;
        foreach (var entnet in tarea.entradas) {
          var ninnet = net.nodes[entnet];
          ninnet.inputs.Clear();
        }
        //net = Network.ImportStructure("net_no_modular.txt");
        if (RedPuedeAdaptarse(net, tarea)) {
          tarea.Fitness(net);
          redes.Add(net);
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="S"></param>
    public void ConstruirApartirDeUna(int S, int ndeseado, Tarea tarea) {
      var nx = 1;
      Network net = new Network();
      var entris = new List<int>() { -1, -2, -3, -4 };
      while (true) {
        net = new Network();
        net.ConstruirRed();
        var depurados = tarea.Depurar(net);

        var dep = tarea.Depurar(net);

        var todasLasEntradas = !entris.Any(x => !depurados.Contains(x));
        nx = depurados.Count - tarea.entradas.Count;

        net.fit = tarea.Fitness(net);
        if (RedPuedeAdaptarse(net, tarea)) break;
      }
      tarea.Fitness(net);
      for (int i = 0; i < S; i++) redes.Add(net.Clonar());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net"></param>
    /// <returns></returns>
    bool RedPuedeAdaptarse(Network net, Tarea tarea) {
      //if (net.output < 0) return false;
      //var entris = new List<int>() { -1, -2, -3, -4 };
      var depurados = tarea.Depurar(net);
      var nx = depurados.Count - tarea.entradas.Count;


      // Que contenga a todas las entradas de la red
      if (tarea.entradas.Any(e => !depurados.Contains(e))) return false;


      //if (nx != ndeseado) return false;
      //if (entris.Any(x => !depurados.Contains(x))) return false;
      //var nodoOutput = net.nodes[net.output];
      //if (nodoOutput.inputs.Any(x => entris.Contains(x))) return false;
      ////if (!RedAdaptable(net, Network.G1)) return false;
      ////if (!RedAdaptable(net, Network.G2)) return false;
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net"></param>
    /// <param name="tarea"></param>
    /// <returns></returns>
    bool RedAdaptable(Network net, Tarea tarea) {
      var p = new Poblacion();
      int S = 1000;
      int L = 300;
      int G = 100;
      for (int i = 0; i < S; i++) p.redes.Add(net.Clonar());
      for (int g = 0; g < G; g++) {
        p.Seleccion(L, tarea, 1f);
        if (p.redes[0].fit >= T.treshold) return true;
      }
      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="L"></param>
    /// <param name="tarea"></param>
    /// <param name="p_fb"></param>
    public void Seleccion(int L, Tarea tarea, float p_fb) {
      var p_m = 0.7;
      var redes_ordnds = new List<Network>();

      redes.ForEach(r => tarea.Fitness(r));
      redes_ordnds = redes.OrderByDescending(r => r.fit).ToList();

      if (redes.Count(r => r.fit > T.treshold) < 0.75 * redes.Count) {
        // Conservar las L redes sin mutar
        var nuevas_redes = M.L(L).Select(x => redes_ordnds[x]).ToList();

        // Las mismas L mejores redes se copian y mutan en la poblacion
        // De esta manera se descartan las redes con la peor interpretacion
        for (int i = 0; nuevas_redes.Count < redes.Count; i++) {
          var net = redes[i].Clonar();
          if (rnd.NextDouble() < p_m) {
            net.Mutar(p_fb, tarea);
            tarea.Fitness(net);
          }
          nuevas_redes.Add(net);
        }

        redes = nuevas_redes;
        redes = redes.OrderByDescending(r => r.fit).ToList();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<string> DNAsExitosos(Tarea t) => redes.Where(r => r.fit > T.treshold)
      .Select(r => r.PrintDNA()).Distinct().ToList();


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, Network> RedesExitosas(bool indexar) {
      if (indexar) Indexar();
      var bestfitnes = redes.Max(r => r.fit);
      var redes2 = redes.Where(r => r.indice > 0).GroupBy(r => r.indice)
        .Select(g => g.First()).ToList();

      return redes2.Where(r => r.fit - bestfitnes < 0.00001f && r.fit > T.treshold)
        .ToDictionary(r => r.indice, r => r.Clonar());
    }



    /// <summary>
    /// 
    /// </summary>
    public void Indexar() {
      for (int i = 0; i < redes.Count; i++) {
        var red = redes[i];
        if (red.fit >= T.treshold) red.indice = i;
        else red.indice = -1;
      }
    }



  }
}

