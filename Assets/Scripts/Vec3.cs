using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FranMath
{
    [System.Serializable]

    public class Vec3 : IEquatable<Vec3>
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
    }

}
