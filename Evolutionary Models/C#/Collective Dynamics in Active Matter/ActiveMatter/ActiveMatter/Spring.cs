using System;

namespace ActiveMatter {
	public class Spring {
		// Propiedad para la longitud inicial l0
		public float InitialLength { get; set; }
		// Propiedad para la constante de resorte k
		public float SpringConstant { get; set; }

		// Propiedades para los agentes
		public Agent Agent1 { get; set; }
		public Agent Agent2 { get; set; }

		public Spring(Agent agent1, Agent agent2) {
			// Validación de entradas
			if (agent1 == null || agent2 == null) {
				throw new ArgumentNullException("Los agentes no pueden ser nulos.");
			}

			Agent1 = agent1;
			Agent2 = agent2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public float Distancia() {
			var vector_diferencia = Agent1.Position - Agent2.Position;
			return vector_diferencia.Mag();
		}

		// Método para actualizar la fuerza en los agentes involucrados en la primavera
		public void Update() {
			// Obtiene la distancia entre los radios de los dos agentes
			var distance = Agent2.Position_R() - Agent1.Position_R();

			// Normaliza la distancia para obtener la dirección del vector
			var direction = distance.Normalize();

			// Calcula la fuerza a aplicar en cada agente
			var force = direction * (SpringConstant) * (distance.Mag() - InitialLength);

			// Aplica la fuerza en cada agente
			Agent1.ApplyForce(force);
			force = force * (-1);
			Agent2.ApplyForce(force);
		}
	}
}






//using System;
//namespace ActiveMatter
//{
//	public class Spring
//	{

//		public float l0;
//		public float k;

//		public Agent agent_i;
//		public Agent agent_j;

//		public Spring(Agent ai, Agent aj)
//		{
//			agent_i = ai;
//			agent_j = aj;
//		}

//		public void Update()
//		{
//			var rij = agent_j.Position_R() - agent_i.Position_R();

//			var dir_rij = rij.Normalize();

//			var force = dir_rij * (k / l0) * (rij.Mag() - l0);

//			agent_i.ApplyForce(force);
//			force = force * (-1);
//			agent_j.ApplyForce(force);
//		}

//	}
//}

