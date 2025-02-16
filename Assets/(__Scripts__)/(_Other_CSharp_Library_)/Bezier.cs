using System;
using UnityEngine;

namespace HSCL
{
    public class Bezier
    {
        #region Lerp
        public static Vector2 Lerp2D(Vector2 p0, Vector2 p1, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p1;

            float tPrim = 1.0f - t;
            return tPrim * p0 + t * p1;

        }
        public static Vector2 Lerp2D(ref Vector2 p0, ref Vector2 p1, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p1;

            float tPrim = 1.0f - t;
            return tPrim * p0 + t * p1;

        }
        public static Vector3 Lerp3D(Vector3 p0, Vector3 p1, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p1;

            float tPrim = 1.0f - t;
            return tPrim * p0 + t * p1;
        }
        public static Vector3 Lerp3D(ref Vector3 p0, ref Vector3 p1, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p1;

            float tPrim = 1.0f - t;
            return tPrim * p0 + t * p1;
        }
        #endregion Lerp

        #region Quadratic
        public static Vector2 Quadratic2D(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p2;

            // outP = seg1 + seg2 + seg3
            float tPrim = 1.0f - t;
            Vector2 seg1 = MathF.Pow(tPrim, 2) * p0;
            Vector2 seg2 = 2.0f * tPrim * t * p1;
            Vector2 seg3 = MathF.Pow(t, 2) * p2;

            return seg1 + seg2 + seg3;
        }
        public static Vector2 Quadratic2D(ref Vector2 p0, ref Vector2 p1, ref Vector2 p2, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p2;

            // outP = seg1 + seg2 + seg3
            float tPrim = 1.0f - t;
            Vector2 seg1 = MathF.Pow(tPrim, 2) * p0;
            Vector2 seg2 = 2.0f * tPrim * t * p1;
            Vector2 seg3 = MathF.Pow(t, 2) * p2;

            return seg1 + seg2 + seg3;
        }
        public static Vector3 Quadratic3D(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p2;

            // outP = seg1 + seg2 + seg3
            float tPrim = 1.0f - t;
            Vector2 seg1 = MathF.Pow(tPrim, 2) * p0;
            Vector2 seg2 = 2.0f * tPrim * t * p1;
            Vector2 seg3 = MathF.Pow(t, 2) * p2;

            return seg1 + seg2 + seg3;
        }
        public static Vector3 Quadratic3D(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p2;

            // outP = seg1 + seg2 + seg3
            float tPrim = 1.0f - t;
            Vector2 seg1 = MathF.Pow(tPrim, 2) * p0;
            Vector2 seg2 = 2.0f * tPrim * t * p1;
            Vector2 seg3 = MathF.Pow(t, 2) * p2;

            return seg1 + seg2 + seg3;
        }
        #endregion Quadratic

        #region Cubic
        public static Vector2 Cubic2D(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p3;

            // outP = seg1 + seg2 + seg3 + seg4
            float tPrim = 1.0f - t;
            Vector2 seg1 = MathF.Pow(tPrim, 3) * p0;
            Vector2 seg2 = 3.0f * MathF.Pow(tPrim, 2) * t * p1;
            Vector2 seg3 = 3.0f * MathF.Pow(t, 2) * tPrim * p2;
            Vector2 seg4 = MathF.Pow(t, 3) * p3;

            return seg1 + seg2 + seg3 + seg4;
        }
        public static Vector2 Cubic2D(ref Vector2 p0, ref Vector2 p1, ref Vector2 p2, ref Vector2 p3, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p3;

            // outP = seg1 + seg2 + seg3 + seg4
            float tPrim = 1.0f - t;
            Vector2 seg1 = MathF.Pow(tPrim, 3) * p0;
            Vector2 seg2 = 3.0f * MathF.Pow(tPrim, 2) * t * p1;
            Vector2 seg3 = 3.0f * MathF.Pow(t, 2) * tPrim * p2;
            Vector2 seg4 = MathF.Pow(t, 3) * p3;

            return seg1 + seg2 + seg3 + seg4;
        }
        public static Vector3 Cubic3D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p3;

            // outP = seg1 + seg2 + seg3 + seg4
            float tPrim = 1.0f - t;
            Vector2 seg1 = MathF.Pow(tPrim, 3) * p0;
            Vector2 seg2 = 3.0f * MathF.Pow(tPrim, 2) * t * p1;
            Vector2 seg3 = 3.0f * MathF.Pow(t, 2) * tPrim * p2;
            Vector2 seg4 = MathF.Pow(t, 3) * p3;

            return seg1 + seg2 + seg3 + seg4;
        }
        public static Vector3 Cubic3D(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3, float t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException("float");
            }

            if (t == 0) return p0;
            if (t == 1) return p3;

            // outP = seg1 + seg2 + seg3 + seg4
            float tPrim = 1.0f - t;
            Vector2 seg1 = MathF.Pow(tPrim, 3) * p0;
            Vector2 seg2 = 3.0f * MathF.Pow(tPrim, 2) * t * p1;
            Vector2 seg3 = 3.0f * MathF.Pow(t, 2) * tPrim * p2;
            Vector2 seg4 = MathF.Pow(t, 3) * p3;

            return seg1 + seg2 + seg3 + seg4;
        }
        #endregion Cubic

        #region De_Casteljau
        public static Vector2 De_Casteljau2D(Vector2[] points, float t)
        {
            int pointsNumber = points.Length;

            // Vector2[] pointsHolder = new Vector2[pointsNumber];
            Span<Vector2> pointsHolder = stackalloc Vector2[pointsNumber];

            // copy input vector array to pointsHolder array:
            for (int i = 0; i < pointsNumber; i++)
            {
                pointsHolder[i] = new Vector2(points[i].x, points[i].y);
            }

            // calculate 
            for (int i = pointsNumber - 1; i > 0; i--)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    pointsHolder[j] = Lerp2D(pointsHolder[j], pointsHolder[j + 1], t);
                }
            }
            return pointsHolder[0];
        }
        public static Vector3 De_Casteljau3D(Vector3[] points, float t)
        {
            int pointsNumber = points.Length;

            // Vector3[] pointsHolder = new Vector3[pointsNumber];
            Span<Vector3> pointsHolder = stackalloc Vector3[pointsNumber];

            // copy input vector array to pointsHolder array:
            for (int i = 0; i < pointsNumber; i++)
            {
                pointsHolder[i] = new Vector3(points[i].x, points[i].y);
            }

            // calculate 
            for (int i = pointsNumber - 1; i > 0; i--)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    pointsHolder[j] = Lerp3D(pointsHolder[j], pointsHolder[j + 1], t);
                }
            }
            return pointsHolder[0];
        }

        #endregion De_Casteljau   
    }
}
