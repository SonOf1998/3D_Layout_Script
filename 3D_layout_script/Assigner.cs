namespace _3D_layout_script
{
    static class Assigner
    {
        public static string ErrorMsg;
        
        public static bool CanAssign(string s, dynamic d)
        {
            switch(s)
            {
                case "Int":
                    return CanAssign(1, d);
                case "Float":
                    return CanAssign(1.0, d);
                case "Vec3":
                    return CanAssign(new vec3(), d);
            }

            if (d is ErrorObject)
            {
                ErrorMsg = "Right hand side has invalid value";
                return false;
            }

            // Unknown típushoz minden mehet
            return true;
        }
        

        public static bool CanAssign(double d1, double d2)
        {
            return true;
        }

        public static bool CanAssign(double d, int i)
        {
            return true;
        }

        public static bool CanAssign(double d, vec3 v)
        {
            ErrorMsg = "Cannot assign Vec3 to Float";
            return false;
        }

        public static bool CanAssign(vec3 v1, vec3 v2)
        {
            return true;
        }

        public static bool CanAssign(vec3 v, double d)
        {
            ErrorMsg = "Cannot assign Float to Vec3";
            return false;
        }

        public static bool CanAssign(vec3 v, int i)
        {
            ErrorMsg = "Cannot assign Int to Vec3";
            return false;
        }

        public static bool CanAssign(int i1, int i2)
        {
            return true;
        }

        public static bool CanAssign(int i, double d)
        {
            ErrorMsg = "Cannot assign Float to Int";
            return false;
        }

        public static bool CanAssign(int i, vec3 v)
        {
            ErrorMsg = "Cannot assign Vec3 to Int";
            return false;
        }

        // üres jobboldalt (nem inicializált jobboldal) hozzá lehet kötni a változóhot mindig
        public static bool CanAssign(int o, UnitializedObject uo)
        {
            return true;
        }

        public static bool CanAssign(double o, UnitializedObject uo)
        {
            return true;
        }

        public static bool CanAssign(vec3 o, UnitializedObject uo)
        {
            return true;
        }

    }
}
