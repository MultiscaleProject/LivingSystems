using System;
using System.Collections.Generic;

// Namespace declaration
namespace BooleanNetwork {

	// Program class containing the main method
	class Program {

		// A read-only random number generator
		public static readonly Random rnd = new Random(DateTime.Now.Millisecond);

		// The main method
		static void Main(string[] args) {

			// Number of nodes in the network
			var num_nodes = 30;

			// Connectivity of the network
			var connectivity = 2;

			// Create an instance of the Network class
			var net = new Network();

			// Create the network with num_nodes and connectivity
			net.CreateNetwork(num_nodes, connectivity);

			// Randomly initialize the state of the network
			var network_state = RandomString(num_nodes);

			// The number of time steps to run the simulation
			var time_steps = 50;

			// Loop through time steps
			for (int i = 0; i < time_steps; i++) {

				// Print the current time step and network state
				Console.WriteLine($"t = {i}\t{network_state}");

				// Update the network state
				network_state = net.Update(network_state);
			}
		}

		// Generates a random binary string of length l with probability p of generating a "1"
		public static string RandomString(int l, double p = 0.5) {
			var cadena = "";
			for (int i = 0; i < l; i++) {
				cadena += (rnd.NextDouble() < p) ? "1" : "0";
			}
			return cadena;
		}
	}

	// Network class to represent a Boolean network
	public class Network {

		// List of nodes in the network
		public List<Node> nodes = new List<Node>();

		// Method to create the network with nnds nodes and k connections per node
		public void CreateNetwork(int nnds, int k) {

			nodes.Clear(); // Clear the list of nodes

			// Loop through nodes to add
			for (int i = 0; i < nnds; i++) {
				var n = new Node();
				n.AddInputs(nnds, k); // Add inputs to the node
				n.CreateBF(); // Create the Boolean function for the node
				nodes.Add(n); // Add the node to the network
			}
		}

		// Update method updates the network state and returns the updated state
		public string Update(string s0) {
			var s1 = ""; // Initialize the new network state
			foreach (var n in nodes) {
				var config = ""; // Initialize the configuration of inputs for current node n
				foreach (var input in n.inputs) {
					// Concatenate the input state of the current node n
					config += s0[input];
				}
				// Get the new state of the current node n based on its Boolean function fB
				s1 += n.fB[config];
			}
			// Return the updated network state
			return s1;
		}

	}

	public class Node {
		// List to store input nodes for current node
		public List<int> inputs = new List<int>();
		// Dictionary to store Boolean function of the node
		public Dictionary<string, string> fB = new Dictionary<string, string>();

		// Int2Bin method converts an integer to binary string
		public static string Int2Bin(int z, int l) {
			var s = Convert.ToString(z, 2); // Convert integer to binary string
			return new string('0', l - s.Length) + s; // Pad with zeros to make length equal to l
		}

		// AddInputs method adds inputs to the current node
		public void AddInputs(int nnds, int k) {
			while (inputs.Count < k) {
				var selected = Program.rnd.Next(nnds); // Select a random input node
				inputs.Add(selected); // Add selected node to inputs list
			}
		}

		// CreateBF method creates Boolean function for the current node
		public void CreateBF() {
			fB.Clear(); // Clear the dictionary fB
			int k = inputs.Count; // Number of inputs for the current node
			for (int i = 0; i < Math.Pow(2, k); i++) {
				// Create binary string representation of the input configuration
				var key = Int2Bin(i, k);
				// Assign a random binary value to the input configuration
				fB[key] = $"{Program.rnd.Next(2)}";
			}
		}
	}

}

