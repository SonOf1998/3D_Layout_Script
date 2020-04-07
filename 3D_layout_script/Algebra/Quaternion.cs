using static System.Math;
using static _3D_layout_script.vec3;

namespace _3D_layout_script
{
    public class Quaternion
    {
        double scalar;
        vec3 vec;
        double angle;

        private Quaternion(double scalar, vec3 vec)
        {
            this.scalar = scalar;
            this.vec = vec;
        }

        private Quaternion(vec3 point, vec3 axis, double angle)
        {
            scalar = Cos(angle);
            vec = axis * Sin(angle);
            this.angle = angle;
        }

        public vec3 ToVec3()
        {
            return vec;
        }

        private Quaternion inverse()
        {
            return new Quaternion(scalar, -vec);
        }

        public static Quaternion operator * (Quaternion a, Quaternion b)
        {
            return new Quaternion(a.scalar * b.scalar - Dot(a.vec, b.vec), b.vec * a.scalar + a.vec * b.scalar + Cross(a.vec, b.vec));
        }

        public static vec3 Rotate(vec3 point, vec3 axis, double angle)
        {
            Quaternion q = new Quaternion(point, axis, angle);
            Quaternion qinv = q.inverse();
            Quaternion v = new Quaternion(0, point);

            Quaternion resultQuat = q * v * qinv;
            return resultQuat.ToVec3();
        }
    }
}
