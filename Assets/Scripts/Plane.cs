using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FranMath
{
    public class FranPlane
    {
        private Vec3 normal;
        private float distance;
        private Vec3 firstVector;
        private Vec3 secondVector;
        private Vec3 thirdVector;

        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public Vec3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }

        public Vec3 FirstVector
        {
            get { return firstVector; }
            set { firstVector = value; }
        }

        public Vec3 SecondVector
        {
            get { return secondVector; }
            set { secondVector = value; }
        }

        public Vec3 ThirdVector
        {
            get { return thirdVector; }
            set { thirdVector = value; }
        }

        public FranPlane flipped
        {
            get { return new FranPlane(-normal, -distance); }
        }

        public FranPlane(Vec3 inNormal, Vec3 inPoint)
        {
            normal = inNormal.normalized;
            distance = -Vec3.Dot(inNormal.normalized, inPoint);
            firstVector = Vec3.Zero;
            secondVector = Vec3.Zero;
            thirdVector = Vec3.Zero;
        }

        public FranPlane(Vec3 inNormal, float d)
        {
            normal = inNormal.normalized;
            distance = d;
            firstVector = Vec3.Zero;
            secondVector = Vec3.Zero;
            thirdVector = Vec3.Zero;
        }

        public FranPlane(Vec3 a, Vec3 b, Vec3 c)
        {
            normal = (Vec3.Cross(b - a, c - a)).normalized;
            distance = -Vec3.Dot(normal, a);
            firstVector = a;
            secondVector = b;
            thirdVector = c;
        }

        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            float dot = Vec3.Dot(normal, point) + distance;
            return point - normal * dot;
        }

        public void Flip()
        {
            normal = -normal;
            distance = -distance;
        }

        public float GetDistanceToPoint(Vec3 point)
        {
            return Vec3.Dot(normal, point) + distance;
        }

        public bool GetSide(Vec3 point)
        {
            return (double)Vec3.Dot(normal, point) + (double)distance > 0f;
        }

        public bool SameSide(Vec3 inPoint, Vec3 inPoint1)
        {
            float dToPoint = GetDistanceToPoint(inPoint);
            float dToPoint1 = GetDistanceToPoint(inPoint1);
            return (double)dToPoint > 0.0f && (double)dToPoint1 > 0.0f || (double)dToPoint < 0.0f && (double)dToPoint1 < 0.0f;
        }

        public void Set3Points(Vec3 vecA, Vec3 vecB, Vec3 vecC)
        {
            normal = (Vec3.Cross(vecB - vecA, vecC - vecA)).normalized;
            distance = -Vec3.Dot(normal, vecA);
        }

        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            normal = inNormal.normalized;
            distance = -Vec3.Dot(inNormal.normalized, inPoint);
        }

        public void Translate(Vec3 translation)
        {
            distance += Vec3.Dot(normal, translation);
        }

        public static FranPlane Translate(FranPlane plane, Vec3 translation)
        {
            return new FranPlane(plane.normal, plane.distance += Vec3.Dot(plane.normal, translation));
        }
    }

}
