/* Vektor osztály
 * operátorok átdefiniálva
 * 
 */ 
namespace _3D_layout_script
{
    public class vec3
    {
        public double x;
        public double y;
        public double z;

        public vec3() : this(0,0,0)
        {

        }

        public vec3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return "Vec3";
        }

        public string PrintValue()
        {
            return $"{x}, {y}, {z}";
        }

        public static vec3 operator -(vec3 v)
        {
            return new vec3(-v.x, -v.y, -v.z);
        }

        public static vec3 operator +(vec3 v1, vec3 v2)
        {
            return new vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static vec3 operator -(vec3 v1, vec3 v2)
        {
            return new vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static vec3 operator *(vec3 vec, double f)
        {
            return new vec3(vec.x * f, vec.y * f, vec.z * f);
        }

        public static vec3 operator *(double f, vec3 vec)
        {
            return vec * f;
        }

        public static vec3 operator /(vec3 vec, double f)
        {
            return vec * (1 / f);
        }

        public static implicit operator string(vec3 vec)
        {
            return $"{vec.x},{vec.y},{vec.z}";
        }

        public static bool operator==(vec3 a, dynamic b)
        {
            if (b is vec3)
            {
                return (a.x == b.x) && (a.y == b.y) && (a.z == b.z);
            }
            else
            {
                return false;
            }            
        }

        public static bool operator!=(vec3 a, dynamic b)
        {
            return !(a == b);
        }



        // skaláris szorzat
        public static double Dot(vec3 a, vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        // vektorszorzat
        public static vec3 Cross(vec3 a, vec3 b)
        {
            return new vec3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }
    }
}
