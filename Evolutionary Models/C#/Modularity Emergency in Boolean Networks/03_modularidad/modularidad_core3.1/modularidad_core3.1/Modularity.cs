using System;
using System.Collections.Generic;
using System.Linq;

namespace modularidad_core3._1 {
  public class Modularity {




    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="all_labels"></param>
    /// <param name="label_to_number"></param>
    /// <returns></returns>
    static Network Temporal_Network(List<string[]> data, IEnumerable<string> all_labels,
    Dictionary<string, int> label_to_number) {
      var net = new Network();
      for (int i = 0; i < all_labels.Count(); i++) {
        var node = new Node();
        net.nodes.Add(node);
      }
      foreach (var conecction in data) {
        var output = label_to_number[conecction[0]];
        var input = label_to_number[conecction[1]];
        if (!net.nodes[input].outputs.Contains(output)) {
          net.nodes[input].outputs.Add(output);
        }
        if (!net.nodes[output].inputs.Contains(input)) {
          net.nodes[output].inputs.Add(input);
        }
      }
      return net;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static List<string[]> StringConection2Data(string s) {
      var sep = "\t"; // separator
      s = s.Replace("),", sep);
      foreach (var ch in "[]()") {
        s = s.Replace(ch.ToString(), "");
      }
      return s.Split(sep).Distinct().Select(x => x.Split(',')).ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net"></param>
    /// <param name="score"></param>
    /// <param name="path"></param>
    static void ScoresAndPath(Network net, ref Dictionary<int, int> score, ref List<List<int>> path) {
      while (true) {
        var new_set = new List<int>();
        foreach (var z in path.Last()) {
          var n = net.nodes[z];
          foreach (var output in n.outputs) {
            if (!path.SelectMany(x => x).Contains(output)) {
              if (!score.ContainsKey(output)) score[output] = score[z];
              else score[output] += score[z];
              if (!new_set.Contains(output)) new_set.Add(output);
            }
          }
        }
        if (new_set.Count == 0) break;
        path.Add(new_set);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="net"></param>
    /// <param name="int2label"></param>
    /// <returns></returns>
    static List<List<int>> Communities(Network net) {
      var communities = new List<List<int>>();
      for (int i = 0; i < net.nodes.Count; i++) {
        if (!communities.SelectMany(x => x).Contains(i)) {
          var capas = new List<List<int>>() { new List<int> { i } };
          while (true) {
            var capa = new List<int>();
            foreach (var z in capas.Last()) {
              var n = net.nodes[z];
              var c = capas.SelectMany(x => x);
              foreach (var input in n.inputs)
                if (!c.Contains(input)) capa.Add(input);
              foreach (var output in n.outputs)
                if (!c.Contains(output)) capa.Add(output);
            }
            if (capa.Count == 0) break;
            capas.Add(capa.Distinct().ToList());
          }
          communities.Add(capas.SelectMany(x => x).Distinct().ToList());
        }
      }
      return communities;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="seq"></param>
    /// <returns></returns>
    public static T[][] FastPowerSet<T>(T[] seq) {
      var powerSet = new T[1 << seq.Length][];
      powerSet[0] = new T[0]; // starting only with empty set

      for (int i = 0; i < seq.Length; i++) {
        var cur = seq[i];
        int count = 1 << i; // doubling list each time
        for (int j = 0; j < count; j++) {
          var source = powerSet[j];
          var destination = powerSet[count + j] = new T[source.Length + 1];
          for (int q = 0; q < source.Length; q++)
            destination[q] = source[q];
          destination[source.Length] = cur;
        }
      }
      return powerSet;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source">One node of the network.
    /// It is constructed the structure from the source to all the
    /// possible nodes connected in a downstream</param>
    /// <param name="net"></param>
    /// <returns></returns>
    static Dictionary<string, float> Link2Betwnns(int source, Network net) {
      var score = new Dictionary<int, int> { [source] = 1 };
      var path = new List<List<int>>() { new List<int>() { source } };
      ScoresAndPath(net, ref score, ref path);
      var lk2btw = new Dictionary<string, float>();
      for (int i = path.Count - 1; i > 0; i--) {
        foreach (var z in path[i]) {
          var node = net.nodes[z];
          foreach (var input in node.inputs.Where(x => path[i - 1].Contains(x))) {
            var zlab = z;
            var flow = 1 + lk2btw.Where(x => x.Key.Contains($"{zlab}->")).Sum(y => y.Value);
            var link = $"{input}->{zlab}";
            lk2btw[link] = flow * score[input] / score[z];
          }
        }
      }
      return lk2btw;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net"></param>
    /// <param name="disconnections"></param>
    /// <returns></returns>
    static Network NetworkDisconnected(Network net, List<string> disconnections) {
      var net2 = net.Clonar();
      foreach (var s in disconnections) {
        var input = int.Parse(s.Split("->")[0]);
        var output = int.Parse(s.Split("->")[1]);
        net2.nodes[input].outputs.RemoveAll(x => x == output);
        net2.nodes[output].inputs.RemoveAll(x => x == input);
      }
      return net2;
    }

    /// <summary>
    /// "9->10"
    /// </summary>
    /// <param name="s1"></param>
    /// <returns></returns>
    static string RealName(string s1, Dictionary<int, string> int2label) {
      var sss = s1.Split("->").Select(x => int.Parse(x)).ToArray();
      var real = $"{int2label[sss[0]]}->{int2label[sss[1]]}";
      return real;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net2"></param>
    /// <param name="int2label"></param>
    /// <returns></returns>
    public static Dictionary<string, float> EdgeBetweennessCentrality(string conecctions_str) {
      var data = StringConection2Data(conecctions_str);
      data = StringConection2Data(conecctions_str);
      //Console.WriteLine($"data = {data}");
      var all_labels = data.SelectMany(x => x).Distinct();
      var label2int = new Dictionary<string, int>();
      foreach (var label in all_labels) label2int[label] = label2int.Count;
      var int2label = label2int.ToDictionary(x => x.Value, x => x.Key);
      var net2 = Temporal_Network(data, all_labels, label2int);

      var edgeBetweennessCentrality = new Dictionary<string, float>();
      for (int source = 0; source < net2.nodes.Count; source++) {
        foreach (var x in Link2Betwnns(source, net2)) {
          if (!edgeBetweennessCentrality.ContainsKey(x.Key)) {
            edgeBetweennessCentrality[x.Key] = x.Value;
          } else edgeBetweennessCentrality[x.Key] += x.Value;
        }
      }
      edgeBetweennessCentrality = edgeBetweennessCentrality.OrderBy(x => x.Value)
        .ToDictionary(x => x.Key, x => x.Value);


      var edge2 = new Dictionary<string, float>();
      foreach (var k in edgeBetweennessCentrality.Keys) {
        edge2[RealName(k, int2label)] = edgeBetweennessCentrality[k];
      }


      return edge2;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net2"></param>
    /// <param name="int2label"></param>
    /// <returns></returns>
    static string WhichDisconnection(Network net2) {
      var edgeBetweennessCentrality = new Dictionary<string, float>();
      for (int source = 0; source < net2.nodes.Count; source++) {
        foreach (var x in Link2Betwnns(source, net2)) {
          if (!edgeBetweennessCentrality.ContainsKey(x.Key)) {
            edgeBetweennessCentrality[x.Key] = x.Value;
          } else edgeBetweennessCentrality[x.Key] += x.Value;
        }
      }
      edgeBetweennessCentrality = edgeBetweennessCentrality.OrderBy(x => x.Value)
        .ToDictionary(x => x.Key, x => x.Value);

      return edgeBetweennessCentrality.Last().Key;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net"></param>
    /// <returns></returns>
    static float MeasureModularity(Network net, List<List<int>> comms) {
      var q = 0f;
      var m = (float)net.nodes.Sum(n => n.inputs.Count);
      for (int i = 0; i < net.nodes.Count; i++) {
        for (int j = 0; j < net.nodes.Count; j++) {
          if (WhichCommunity(i, comms) == WhichCommunity(j, comms)) {
            var n_i = net.nodes[i];
            var n_j = net.nodes[j];
            var A_ij = (n_i.inputs.Contains(j)) ? 1 : 0;
            var kk = n_i.inputs.Count * n_j.outputs.Count / m;
            q += A_ij - kk;
          }
        }
      }
      return q / m;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="communities"></param>
    /// <returns></returns>
    static int WhichCommunity(int x, List<List<int>> communities) {
      var resultado = -1;
      for (int i = 0; i < communities.Count; i++) {
        if (communities[i].Contains(x)) {
          resultado = i;
          break;
        }
      }
      return resultado;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="net"></param>
    /// <param name="communities"></param>
    /// <returns></returns>
    static float ModularityNoDirigida(Network net, List<List<int>> comm) {
      var q = 0f;
      var m = (float)net.nodes.Sum(n => n.inputs.Count) / 2;
      for (int i = 0; i < net.nodes.Count; i++) {
        for (int j = 0; j < net.nodes.Count; j++) {
          if (WhichCommunity(i, comm) == WhichCommunity(j, comm)) {
            var n_i = net.nodes[i];
            var n_j = net.nodes[j];
            var A_ij = (n_i.inputs.Contains(j)) ? 1 : 0;
            var kk = n_i.inputs.Count * n_j.inputs.Count / (2 * m);
            q += A_ij - kk;
          }
        }
      }
      return q / (2 * m);
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="s"></param>
    ///// <param name="int2label"></param>
    ///// <returns></returns>
    //static string Q(string s, Dictionary<int, string> int2label) {
    //  var input = int2label[int.Parse(s.Split("->")[0])];
    //  var output = int2label[int.Parse(s.Split("->")[1])];
    //  return $"{input}->{output}";
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    static string NoDirigido(string s) {
      var sep = "\t"; // separator
      s = s.Replace("),", sep);
      foreach (var ch in "[]()") {
        s = s.Replace(ch.ToString(), "");
      }
      var lkj = s.Split(sep).ToList();
      foreach (var par in lkj.ToList()) {
        var alreves = $"{par.Split(',')[1]},{par.Split(',')[0]}";
        if (!lkj.Contains(alreves)) lkj.Add(alreves);
      }
      lkj = lkj.OrderByDescending(x => x).ToList();
      //lkj.Sort();
      return "[" + M.List2String(lkj.Select(x => $"({x})")) + "]";
    }

    /// <summary>
    /// 
    /// </summary>
    public static float T04(Network best_net, Tarea t) {

      var conecctions_str = best_net.ImprimirConnections(t);


      var data = StringConection2Data(conecctions_str);
      data = StringConection2Data(conecctions_str);
      //Console.WriteLine($"data = {data}");
      var all_labels = data.SelectMany(x => x).Distinct();
      var label2int = new Dictionary<string, int>();
      foreach (var label in all_labels) label2int[label] = label2int.Count;
      var int2label = label2int.ToDictionary(x => x.Value, x => x.Key);
      var net = Temporal_Network(data, all_labels, label2int);
      var num_communities = Communities(net).Count;
      var list_disconnections = new List<string>();
      var max_modularity = -100f;

      var comunidadMaxMod = new List<List<int>>();

      while (true) {
        var net2 = NetworkDisconnected(net, list_disconnections);
        var comm2 = Communities(net2);
        if (num_communities != comm2.Count) {
          var modularity = MeasureModularity(net, comm2);
          if (modularity > max_modularity) {
            max_modularity = modularity;
            comunidadMaxMod = comm2;
          }
          num_communities = comm2.Count;
        }
        if (comm2.Count >= net.nodes.Count) break;
        list_disconnections.Add(WhichDisconnection(net2));
      }


      best_net.comunidades = comunidadMaxMod.Select(l => l.Select(x => int2label[x]).ToList()).ToList();

      best_net.modularidad = max_modularity;
      return max_modularity;
    }















  }
}

