using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FranMath;
using System.Linq;

//Las meshes estan formadas por triangulos, para la deteccion se calcula la normal hacia dentro de cada triangulo de ambos meshes, si corren en sentido opuesto, una mesh se encuentra dentro de otra

public class MeshDetection : MonoBehaviour
{
    [SerializeField] private Transform pointToCollide;
    private MeshFilter mf;
    private int[] indexes;
    Vec3 left;
    Vec3 right;
    Vec3 up;
    Vec3 down;
    Vec3 front;
    Vec3 back;
    List<FranPlane> planesFromMesh = new List<FranPlane>();
    List<Vec3> pointsFromPointCollide = new List<Vec3>();

    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        indexes = mf.mesh.GetIndices(0);
    }

    private void Update()
    {
        if (CreateAndDetectCollisionWithBoundingBox(mf.mesh.vertices, pointToCollide))
        {
            transform.GetComponent<MeshRenderer>().material.color = Color.yellow;
            if (IsPointCollidingWithMesh(mf.mesh, pointToCollide))
            {
                transform.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
        else
        {
            transform.GetComponent<MeshRenderer>().material.color = Color.red;
        }

    }

    private bool IsPointCollidingWithMesh(Mesh objectMesh, Transform pointToCollide)
    {
        Vec3 pos = new Vec3(transform.position);
        planesFromMesh.Clear();

        for (int i = 0; i < indexes.Length; i += 3)
        {
            Vec3 v1 = new Vec3(objectMesh.vertices[indexes[i]]);
            Vec3 v2 = new Vec3(objectMesh.vertices[indexes[i + 1]]);
            Vec3 v3 = new Vec3(objectMesh.vertices[indexes[i + 2]]);


            FranPlane plane = new FranPlane(v1, v2, v3);
            plane.FirstVector = FromLocalToWolrd(v1, transform);
            plane.SecondVector = FromLocalToWolrd(v2, transform);
            plane.ThirdVector = FromLocalToWolrd(v3, transform);
            plane.Normal = Vec3.Cross(plane.SecondVector - plane.FirstVector, plane.ThirdVector - plane.FirstVector).normalized + pos;
            plane.Distance = -Vec3.Dot(plane.Normal, plane.FirstVector);

            planesFromMesh.Add(plane);
        }

        planesFromMesh = planesFromMesh.OrderByDescending(plane => (Vec3.Cross(plane.FirstVector - plane.SecondVector, plane.FirstVector - plane.ThirdVector).magnitude) * 0.5f).ToList();

        pointsFromPointCollide.Clear();

        for (int i = 0; i < pointToCollide.GetComponent<MeshFilter>().mesh.vertices.Length; i++)
        {
            Vec3 point = new Vec3(pointToCollide.GetComponent<MeshFilter>().mesh.vertices[i]);

            pointsFromPointCollide.Add(point);
        }

        for (int j = 0; j < pointsFromPointCollide.Count; j++)
        {
            int planesCollided = 0;
            for (int i = 0; i < planesFromMesh.Count; i++)
            {
                if (!planesFromMesh[i].SameSide(FromLocalToWolrd(pointsFromPointCollide[j], pointToCollide), planesFromMesh[i].Normal))
                {
                    planesCollided++;
                }
            }
            if (planesCollided == planesFromMesh.Count)
            {
                return true;
            }
        }
        return false;
    }

    private Vec3 FromLocalToWolrd(Vec3 point, Transform transformRef)
    {
        Vector3 result = Vector3.zero;

        result = new Vector3(point.x * transformRef.localScale.x, point.y * transformRef.localScale.y, point.z * transformRef.localScale.z);

        result = transform.rotation * (Vector3)result;

        Vec3 finalResult = new Vec3(result);

        return finalResult + new Vec3(transformRef.position);
    }

    private bool CreateAndDetectCollisionWithBoundingBox(Vector3[] points, Transform pointToCollide)
    {
        Vec3[] pointsTransformed = new Vec3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            pointsTransformed[i] = new Vec3(points[i].x * transform.localScale.x, points[i].y * transform.localScale.y, points[i].z * transform.localScale.z);
            pointsTransformed[i] = new Vec3(transform.localRotation * pointsTransformed[i]);
        }

        left = Vec3.Zero;
        right = Vec3.Zero;
        up = Vec3.Zero;
        down = Vec3.Zero;
        front = Vec3.Zero;
        back = Vec3.Zero;

        for (int i = 0; i < pointsTransformed.Length; i++)
        {
            if (pointsTransformed[i].x < left.x)
            {
                left = -pointsTransformed[i].x * Vec3.Left;
            }
            if (pointsTransformed[i].x > right.x)
            {
                right = pointsTransformed[i].x * Vec3.Right;
            }
            if (pointsTransformed[i].y < down.y)
            {
                down = -pointsTransformed[i].y * Vec3.Down;
            }
            if (pointsTransformed[i].y > up.y)
            {
                up = pointsTransformed[i].y * Vec3.Up;
            }
            if (pointsTransformed[i].z < back.z)
            {
                back = -pointsTransformed[i].z * Vec3.Back;
            }
            if (pointsTransformed[i].z > front.z)
            {
                front = pointsTransformed[i].z * Vec3.Forward;
            }
        }

        Vec3 pos = new Vec3(transform.position);

        List<Vec3> pointsFromPointCollide = new List<Vec3>();

        for (int i = 0; i < pointToCollide.GetComponent<MeshFilter>().mesh.vertices.Length; i++)
        {
            Vec3 point = FromLocalToWolrd(new Vec3(pointToCollide.GetComponent<MeshFilter>().mesh.vertices[i]), pointToCollide);

            pointsFromPointCollide.Add(point);
        }

        for (int i = 0; i < pointsFromPointCollide.Count; i++)
        {
            if (pointsFromPointCollide[i].x > left.x + pos.x && pointsFromPointCollide[i].y > down.y + pos.y && pointsFromPointCollide[i].z > back.z + pos.z && pointsFromPointCollide[i].x < right.x + pos.x && pointsFromPointCollide[i].y < up.y + pos.y && pointsFromPointCollide[i].z < front.z + pos.z)
            {
                return true;
            }
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vec3(Vec3.Distance(left, right), Vec3.Distance(down, up), Vec3.Distance(back, front)));
        Gizmos.color = Color.black;
        foreach (FranPlane plane in planesFromMesh)
        {
            Gizmos.DrawSphere(plane.Normal, 0.05f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(plane.FirstVector, 0.01f);
            Gizmos.DrawSphere(plane.SecondVector, 0.01f);
            Gizmos.DrawSphere(plane.ThirdVector, 0.01f);
            Gizmos.color = Color.black;
        }
        foreach (Vec3 pointFromPointCollide in pointsFromPointCollide)
        {
            Gizmos.DrawSphere(pointFromPointCollide, 0.001f);
        }
    }
#endif
}