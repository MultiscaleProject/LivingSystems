using System;

namespace ActiveMatter {
	/// <summary>
	/// Clase que representa un vector en dos dimensiones.
	/// </summary>
	public class Vector {
		/// <summary>
		/// Componente x del vector.
		/// </summary>
		public float X;

		/// <summary>
		/// Componente y del vector.
		/// </summary>
		public float Y;

		/// <summary>
		/// Constructor de la clase Vector.
		/// </summary>
		/// <param name="x">Valor de la componente x del vector.</param>
		/// <param name="y">Valor de la componente y del vector.</param>
		public Vector(float x, float y) {
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Crea una copia superficial del objeto Vector.
		/// </summary>
		/// <returns>Una copia del objeto Vector.</returns>
		public Vector Clone() {
			return new Vector(this.X, this.Y);
		}

		/// <summary>
		/// Sobrecarga del operador + para sumar dos vectores.
		/// </summary>
		/// <param name="v1">Vector a sumar.</param>
		/// <param name="v2">Vector a sumar.</param>
		/// <returns>Un nuevo vector que es la suma de los dos vectores de entrada.</returns>
		public static Vector operator +(Vector v1, Vector v2) {
			return new Vector(v1.X + v2.X, v1.Y + v2.Y);
		}

		/// <summary>
		/// Sobrecarga del operador - para restar dos vectores.
		/// </summary>
		/// <param name="v1">Vector minuendo.</param>
		/// <param name="v2">Vector sustraendo.</param>
		/// <returns>Un nuevo vector que es la diferencia entre los dos vectores de entrada.</returns>
		public static Vector operator -(Vector v1, Vector v2) {
			return new Vector(v1.X - v2.X, v1.Y - v2.Y);
		}

		/// <summary>
		/// Sobrecarga del operador * para multiplicar un vector por un escalar.
		/// </summary>
		/// <param name="v">Vector a multiplicar.</param>
		/// <param name="s">Escalar por el cual multiplicar el vector.</param>
		/// <returns>Un nuevo vector que es el resultado de multiplicar el vector de entrada por el escalar.</returns>
		public static Vector operator *(Vector v, float s) {
			return new Vector(v.X * s, v.Y * s);
		}

		/// <summary>
		/// Sobrecarga del operador / para dividir un vector por un escalar.
		/// </summary>
		/// <param name="v">Vector a dividir.</param>
		/// <param name="s">Escalar por el cual dividir el vector.</param>
		/// <returns>Un nuevo vector que es el resultado de dividir el vector de entrada por el escalar.</returns>
		public static Vector operator /(Vector v, float s) {
			return new Vector(v.X / s, v.Y / s);
		}

		/// <summary>
		/// Calcula el producto escalar de dos vectores.
		/// </summary>
		/// <param name="v2">Vector con el cual calcular el producto escalar.</param>
		/// <returns>El producto escalar de los dos vectores.</returns>
		public float Dot(Vector v2) {
			return (X * v2.X) + (Y * v2.Y);
		}

		/// <summary>
		/// Calcula la magnitud del vector.
		/// </summary>
		/// <returns>La magnitud del vector.</returns>
		public float Mag() {
			return (float)Math.Sqrt(X * X + Y * Y);
		}

		/// <summary>
		/// Calcula la normalización del vector.
		/// </summary>
		/// <returns>El vector normalizado.</returns>
		public Vector Normalize() {
			float magnitude = Mag();
			return new Vector(X / magnitude, Y / magnitude);
		}

		/// <summary>
		/// Calcula el ángulo en radianes del vector.
		/// </summary>
		/// <returns>Ángulo en radianes del vector.</returns>
		public float GetAngle() {
			// Utiliza la función Atan2 de la clase Math para calcular el ángulo en
			// radianes del vector a partir de sus componentes x e y.
			return (float)Math.Atan2(Y, X);
		}

	}
}





//using System;

//namespace ActiveMatter
//{
//	public class Vector : ICloneable
//	{
//		public float x;
//		public float y;

//		public Vector(float x, float y)
//		{
//			this.x = x;
//			this.y = y;
//		}

//		public object Clone()
//		{
//			return this.MemberwiseClone();
//		}

//		public static Vector operator +(Vector v1, Vector v2)
//		{
//			return new Vector(v1.x + v2.x, v1.y + v2.y);
//		}

//		public static Vector operator -(Vector v1, Vector v2)
//		{
//			return new Vector(v1.x - v2.x, v1.y - v2.y);
//		}

//		public static Vector operator *(Vector v, float s)
//		{
//			return new Vector(v.x * s, v.y * s);
//		}

//		public static Vector operator /(Vector v, float s)
//		{
//			return new Vector(v.x / s, v.y / s);
//		}

//		public float Dot(Vector v2)
//		{
//			return (x * v2.x) + (y * v2.y);
//		}

//		public float Mag()
//		{
//			return (float)Math.Sqrt(x * x + y * y);
//		}

//		public Vector Normalize()
//		{
//			float magnitude = Mag();
//			return new Vector(x / magnitude, y / magnitude);
//		}
//	}
//}





//using System;
//namespace ActiveMatter
//{
//	public class Vector
//	{

//		public float x;
//		public float y;

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="x_"></param>
//		/// <param name="y_"></param>
//		public Vector(float x_, float y_)
//		{
//			x = x_;
//			y = y_;
//		}
//		public Vector(double x_, double y_)
//		{
//			x = (float)x_;
//			y = (float)y_;
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <returns></returns>
//		public Vector Clone()
//		{
//			return new Vector(x, y);
//		}


//		public static float Dot(Vector v1, Vector v2)
//		{
//			return v1.x * v2.x + v1.y + v2.y;
//		}

//		public static Vector Mult(Vector v, float s)
//		{
//			return new Vector(s * v.x, s * v.y);
//		}

//		public float Dot(Vector v2) => (x * v2.x) + (y * v2.y);

//		public Vector Mult(float s) => new Vector(s * x, s * y);

//		public Vector Div(float s) => new Vector(x / s, y / s);

//		public Vector Add(Vector v2) => new Vector(x + v2.x, y + v2.y);

//		public Vector Sub(Vector v2) => new Vector(x - v2.x, y - v2.y);

//		public float Mag() => (float)Math.Sqrt(x * x + y * y);

//		public Vector Normilze() => new Vector(x / Mag(), y / Mag());





//	}
//}

