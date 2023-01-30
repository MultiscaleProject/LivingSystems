using System;

namespace ActiveMatter {
	public class Agent {
		// Propiedad para el índice del agente
		public int Index { get; set; }

		// Propiedades para los parámetros alpha, beta, dTheta y chi
		public float Alpha { get; set; }
		public float Beta { get; set; }
		public float DTheta { get; set; }
		public float Chi { get; set; }

		// Propiedad para la posición del agente
		public Vector Position { get; set; }

		// Propiedades para el ángulo theta, la velocidad v0 y el radio r
		public float Theta { get; set; }
		public float V0 { get; set; }
		public float R { get; set; }

		// Constante para el intervalo de tiempo delta_t
		public const float DELTA_T = 0.004f;

		// Propiedades para indicar si las coordenadas x e y están fijas o no
		public bool XFixed { get; set; }
		public bool YFixed { get; set; }

		// Propiedad para la fuerza aplicada al agente
		public Vector F { get; set; }

		// Propiedad para la velocidad del agente
		//public Vector Velocity { get; set; }
		public Vector Velocity = new Vector(0, 0);


		public Agent() {
			// Inicializa la fuerza en (0, 0)
			F = new Vector(0, 0);
		}

		// Implementación para clonar el agente
		public Agent Clone() {
			var a = new Agent();
			a.Index = Index;
			a.Alpha = Alpha;
			a.Beta = Beta;
			a.DTheta = DTheta;
			a.Chi = Chi;
			a.Position = Position.Clone();
			a.Theta = Theta;
			a.V0 = V0;
			a.R = R;
			a.XFixed = XFixed;
			a.YFixed = YFixed;
			a.F = F.Clone();
			a.Velocity = new Vector(Velocity.X, Velocity.Y);
			return a;
		}

		// Método para obtener el vector normal del agente
		public Vector GetNormalVector() {
			return new Vector((float)Math.Cos(Theta), (float)Math.Sin(Theta));
		}

		// Método para obtener el vector normal ortogonal del agente
		public Vector GetOrthogonalNormalVector() {
			return new Vector(-(float)Math.Sin(Theta), (float)Math.Cos(Theta));
		}

		// Método para actualizar la posición y velocidad del agente
		public void Update() {
			Theta += (Beta * (F.Dot(GetOrthogonalNormalVector())) +
				(DTheta / (float)Math.Sqrt(DELTA_T)) * M.RndNormal(0, 1)) * DELTA_T;

			Velocity = GetNormalVector() * V0 * DELTA_T;
			Velocity += GetNormalVector() * Alpha * (1 + Chi) * F.Dot(GetNormalVector()) * DELTA_T;
			Velocity += GetOrthogonalNormalVector() * Alpha * (1 - Chi) * F.Dot(GetOrthogonalNormalVector()) * DELTA_T;

			if (!XFixed && !YFixed) Position += Velocity;
			if (!XFixed || !YFixed) {
				if (XFixed) Position.Y += Velocity.Y;
				if (YFixed) Position.X += Velocity.X;
			}

			// Es necesario limpiar la fuerza después de actualizar la posición
			F = new Vector(0, 0);
		}

		// Método para aplicar una fuerza al agente
		public void ApplyForce(Vector force) {
			F = F + force;
		}

		// Método para obtener la posición del radio del agente
		public Vector Position_R() {
			return new Vector(Position.X + R * (float)Math.Cos(Theta),
								Position.Y + R * (float)Math.Sin(Theta));
		}

		// Método para calcular el momento angular del agente con respecto a un punto dado
		public float GetAngularMomentum(Vector pointOfReference, Vector centerOfMass) {
			var r_cm = Position - centerOfMass;
			var v_cm = Velocity - centerOfMass;
			return (r_cm.X * v_cm.Y) - (r_cm.Y * v_cm.X);
		}

	}
}





//using System;
//namespace ActiveMatter
//{
//	public class Agent
//	{
//		public int ind;

//		public float alpha;
//		public float beta;
//		public float dTheta;
//		public float chi;

//		public Vector position;

//		public float theta;
//		public float v0;
//		public float r;

//		public static float delta_t;


//		public bool x_fixed;
//		public bool y_fixed;

//		public Vector f;


//		public Vector velocity = new Vector(0, 0); // Se calcula después


//		public Agent()
//		{
//			f = new Vector(0, 0);
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <returns></returns>
//		public Agent Clone()
//		{
//			var a = new Agent();
//			a.ind = ind;
//			a.alpha = alpha;
//			a.beta = beta;
//			a.dTheta = dTheta;
//			a.chi = chi;
//			a.position = position.Clone();
//			a.theta = theta;
//			a.v0 = v0;
//			a.r = r;
//			a.x_fixed = x_fixed;
//			a.y_fixed = y_fixed;
//			a.f = f.Clone();
//			a.velocity = velocity.Clone();
//			return a;
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <returns></returns>
//		public Vector N()
//		{
//			return new Vector((float)Math.Cos(theta), (float)Math.Sin(theta));
//		}

//		public Vector N_ort()
//		{
//			return new Vector(-(float)Math.Sin(theta), (float)Math.Cos(theta));
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		public void Update()
//		{
//			theta += (beta * (f.Dot(N_ort())) +
//				(dTheta / (float)Math.Sqrt(delta_t)) * M.RndNormal(0, 1)) * delta_t;

//			velocity = N() * v0 * delta_t;
//			velocity += N() * alpha * (1 + chi) * f.Dot(N()) * delta_t;
//			velocity += N_ort() * alpha * (1 - chi) * f.Dot(N_ort()) * delta_t;

//			if (!x_fixed && !y_fixed) position = position + velocity;
//			if (!x_fixed || !y_fixed)
//			{
//				if (x_fixed) position.Y += velocity.Y;
//				if (y_fixed) position.X += velocity.X;
//			}

//			f = new Vector(0, 0); // It is necessary to clear the force after updating the position
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="force"></param>
//		public void ApplyForce(Vector force)
//		{
//			f = f + force;
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <returns></returns>
//		public Vector Position_R()
//		{
//			return new Vector(position.X + r * (float)Math.Cos(theta),
//								position.Y + r * (float)Math.Sin(theta));
//		}


//		public float AngularMomentum(Vector LM, Vector CM)
//		{
//			var r_cm = position - CM;
//			var v_cm = velocity - CM;
//			return (r_cm.X * v_cm.Y) - (r_cm.Y * v_cm.X);
//		}






//	}
//}

