using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FranMath
{
    [System.Serializable]

    public class Vec3
    {
        #region Variables
        public float x;
        public float y;
        public float z;

        public float sqrMagnitude { get { return (x * x + y * y + z * z); } }
        public Vec3 normalized { get { return new Vec3(x / Mathf.Sqrt(x * x + y * y + z * z), y / Mathf.Sqrt(x * x + y * y + z * z), z / Mathf.Sqrt(x * x + y * y + z * z)); } }
        public float magnitude { get { return (float)Math.Sqrt(sqrMagnitude); } }
        #endregion

        #region Constants
        public const float epsilon = 1e-05f;
        #endregion

        #region Default Values
        public static Vec3 Zero { get { return new Vec3(0.0f, 0.0f, 0.0f); } }
        public static Vec3 One { get { return new Vec3(1.0f, 1.0f, 1.0f); } }
        public static Vec3 Forward { get { return new Vec3(0.0f, 0.0f, 1.0f); } }
        public static Vec3 Back { get { return new Vec3(0.0f, 0.0f, -1.0f); } }
        public static Vec3 Right { get { return new Vec3(1.0f, 0.0f, 0.0f); } }
        public static Vec3 Left { get { return new Vec3(-1.0f, 0.0f, 0.0f); } }
        public static Vec3 Up { get { return new Vec3(0.0f, 1.0f, 0.0f); } }
        public static Vec3 Down { get { return new Vec3(0.0f, -1.0f, 0.0f); } }
        public static Vec3 PositiveInfinity { get { return new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); } }
        public static Vec3 NegativeInfinity { get { return new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); } }
        #endregion

        #region Constructors
        public Vec3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0.0f;
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vec3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }
        #endregion

        #region Operators
        public static bool operator ==(Vec3 a, Vec3 b)
        {
            //Se calcula la diferencia entre cada componente
            float xDifference = a.x - b.x;
            float yDifference = a.y - b.y;
            float zDifference = a.z - b.z;
            //Se realiza la suma de los cuadrados de cada componente$
            float sqr = xDifference * xDifference + yDifference * yDifference + zDifference * zDifference;
            return sqr < epsilon * epsilon;
            //Si la suma de los cuadrados es menor al cuadrado de epsilon, true, si no, false
        }
        public static bool operator !=(Vec3 a, Vec3 b)
        {
            //Se utiliza el == para chequear lo opuesto
            return !(a == b);
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            //Se suman los componentes de cada vector con su correspondiente
            return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            //Se restan los componentes de cada vector con su correspondiente
            return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vec3 operator -(Vec3 a)
        {
            //Se invierte cada componente
            return new Vec3(-a.x, -a.y, -a.z);
        }

        public static Vec3 operator *(Vec3 a, float scalar)
        {
            //Se multiplica cada componente del vector por un numero en particular
            return new Vec3(a.x * scalar, a.y * scalar, a.z * scalar);
        }
        public static Vec3 operator *(float scalar, Vec3 a)
        {
            //Se realiza lo mismo (propiedad conmutativa)
            return new Vec3(a.x * scalar, a.y * scalar, a.z * scalar);
        }

        public static Vec3 operator /(Vec3 a, float scalar)
        {
            //Se divide cada componente del vector por un numero en particular
            return new Vec3(a.x / scalar, a.y / scalar, a.z / scalar);
        }

        public static implicit operator Vector3(Vec3 a)
        {
            return new Vector3(a.x, a.y, a.z);
        }

        public static implicit operator Vector2(Vec3 a)
        {
            return new Vector3(a.x, a.y, 0);
        }
        #endregion

        #region Functions

        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
        }

        public static float Angle(Vec3 from, Vec3 to)
        {
            //Formula = Cos @ = producto punto entre los vectores/ producto punto entre las normalizadas de los vectores
            return ((float)Math.Acos((from.x * to.x + from.y * to.y + from.z * to.z) / ((Math.Sqrt(from.x * from.x + from.y * from.y + from.z * from.z)) * (Math.Sqrt(to.x * to.x + to.y * to.y + to.z + to.z)))) / (float)Math.PI * 180);
        }

        public static Vec3 ClampMagnitude(Vec3 vec, float maxLength)
        {
            float normalized = (float)Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z); //Se normaliza el vector
            float scalar = Math.Min(normalized, maxLength) / normalized; //se obtiene un escalar a partir del minimo entre el vector normalizado y la longitud maxima.
            //Se multiplica cada componente por el escalar para obtener el tamaño de vector deseado.
            return new Vec3(scalar * vec.x, scalar * vec.y, scalar * vec.z);
        }

        public static float Magnitude(Vec3 vector)
        {
            //Magnitud = Raiz cuadrada de la suma de los cuadrados de cada componente
            return (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            //Se realiza el producto cruz (a.x, a.y, a.z)
            //                          * (b.x, b.y, b.z)
            return new Vec3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public static float Distance(Vec3 a, Vec3 b)
        {
            //Distancia = diferencia de magnitudes entre vectores
            return Magnitude(a - b);
        }

        public static float Dot(Vec3 a, Vec3 b)
        {
            //El famoso producto punto = producto entre componentes correspondientes entre vectores.
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
        {
            //T representa el tiempo y siempre se clampea en un rango de 0 a 1
            if (t < 0)
            {
                t = 0;
            }
            if (t > 1)
            {
                t = 1;
            }
            return a + (b - a) * t;
        }

        public static Vec3 LerpUnclamped(Vec3 a, Vec3 b, float t)
        {
            //Lo mismo que el Lerp pero sin el clamp del tiempo
            return (a + (b - a) * t);
        }

        public static Vec3 Max(Vec3 a, Vec3 b)
        {
            //Se saca el valor maximo entre los componentes de cada vector
            return new Vec3(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
        }

        public static Vec3 Min(Vec3 a, Vec3 b)
        {
            //Se saca el valor minimo entre los componentes de cada vector
            return new Vec3(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
        }

        public static float SqrMagnitude(Vec3 vector)
        {
            //Suma de los cuadrados
            return (vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        public static Vec3 Project(Vec3 vector, Vec3 onNormal)
        {
            //Se obtiene el angulo entre los dos vectores, para calcular luego la magnitud del vector proyectado sobre el vector onNormal
            return new Vec3((Dot(vector, onNormal) / Magnitude(onNormal)) * (onNormal / Magnitude(onNormal)));
        }

        public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal)
        {
            return -2F * Dot(inNormal, inDirection) * inNormal + inDirection;
        }

        public void Scale(Vec3 scale)
        {
            //Se multiplica cada componente por un escalar en comun.
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        public void Normalize()
        {
            //Se divide cada componente por la magnitud del vector
            float aux = (float)Math.Sqrt(x * x + y * y + z * z); ;
            x = x / aux;
            y = y / aux;
            z = z / aux;
        }
        #endregion
    }

}
