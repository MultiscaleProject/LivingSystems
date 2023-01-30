using System;
using System.Collections.Generic;
using System.Linq;

namespace modularidad_core3._1 {
  public class Node {


    static readonly Random rnd = new Random(DateTime.Now.Millisecond);

    public string gate_type, first_address, second_address, estado;

    public List<int> inputs = new List<int>();
    // public List<double> pesos = new List<double>();
    public List<int> outputs = new List<int>();


    public Dictionary<string, string> fB = new Dictionary<string, string>();

    public static Dictionary<string, string> RNDFB2 = new Dictionary<string, string>() {
      ["00"] = $"{rnd.Next(2)}",
      ["01"] = $"{rnd.Next(2)}",
      ["10"] = $"{rnd.Next(2)}",
      ["11"] = $"{rnd.Next(2)}"
    };

    public Node Clonar() {
      var n2 = new Node();
      n2.gate_type = gate_type;
      n2.first_address = first_address;
      n2.second_address = second_address;
      n2.estado = estado;
      n2.inputs = new List<int>(inputs);
      n2.outputs = new List<int>(outputs);
      foreach (var k in fB.Keys) n2.fB[k] = fB[k];
      return n2;
    }

    ///// <summary>
    ///// Cada nodo posee dos inputs. Para saber cuáles son se asignan
    ///// dos address a cada nodo. Cada address es una cadena binaria de
    ///// longitud 4. Pero primero se indica el tipo de puerta lógica.
    ///// En esta simulación nunca cambia el tipo de puerta (NAND).
    ///// </summary>
    ///// <returns></returns>
    //public void ConstruirGen() {
    //  first_address = M.CadenaBinaria(Network.digits);
    //  inputs.Add(Network.direcciones[first_address] - Network.n_entradas);
    //  second_address = M.CadenaBinaria(Network.digits);
    //  inputs.Add(Network.direcciones[second_address] - Network.n_entradas);
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="k"></param>
    public void AgregarInputs(int nnds, int k) {
      while (inputs.Count < k) {
        var elegido = rnd.Next(nnds);
        inputs.Add(elegido);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string PrintFbValues() {
      var mensaje = "";
      var sortedKeys = fB.Keys.OrderBy(x => x);
      foreach (var k in sortedKeys) {
        mensaje += $"{k}";
      }
      return mensaje;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public void Mutar2() {
    //  if (M.Intento()) {
    //    first_address = M.CadenaBinaria(Network.digits);
    //  } else {
    //    second_address = M.CadenaBinaria(Network.digits);
    //  }
    //}

    /* 202201051418_ 
     * Para representar una puerta lógica NAND es necesario
     * suponer que cada nodo posee únicamente 2 reguladores.
     * Además hay que fijar los valores de la función booleana,
     * de tal manera que únicamente sea igual a cero cuando ambos 
     * reguladores estén encendidos.*/
    public void ConstruirBf_NAND() {
      fB["00"] = "1";
      fB["01"] = "1";
      fB["10"] = "1";
      fB["11"] = "0";
    }

    public void ConsturiBf_RND() {
      fB.Clear();

      //while (fB.Count == 0 || fB.Values.Count(v => v == "0") != 2) {
      //  fB["00"] = $"{rnd.Next(2)}";
      //  fB["01"] = $"{rnd.Next(2)}";
      //  fB["10"] = $"{rnd.Next(2)}";
      //  fB["11"] = $"{rnd.Next(2)}";
      //}

      //fB["00"] = $"{rnd.Next(2)}";
      //fB["01"] = $"{rnd.Next(2)}";
      //fB["10"] = $"{rnd.Next(2)}";
      //fB["11"] = $"{rnd.Next(2)}";

      int k = inputs.Count;
      for (int i = 0; i < Math.Pow(2, k); i++) {
        var key = M.Int2Bin(i, k);
        fB[key] = $"{rnd.Next(2)}";
      }



      //fB = RNDFB2;
    }



  }
}

