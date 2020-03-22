using _3D_layout_script;

namespace Extensions
{
    public static class Extensions
    {
        public static string ToString(int i)
        {
            return "Int";
        }

        public static string ToString(double d)
        {
            return "Float";
        }

        public static string ToString(vec3 _)
        {
            return "Vec3";
        }

        public static string ToString(ErrorObject errorObject)
        {
            return "ErrorType";
        }
    }
}
