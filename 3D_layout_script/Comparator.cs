namespace _3D_layout_script
{
    class Comparator
    {
        private enum ComparatorFunc
        {
            LT,
            GT,
            LEQ,
            GEQ,
            EQ,
            NEQ
        }

        ComparatorFunc func;
        private string errorMsg;
        public bool HasErrorMsg { get; private set; }
        public string ErrorMsg
        {
            get
            {
                string copy = errorMsg;
                errorMsg = null;
                HasErrorMsg = false;
                return copy;
            }
            set
            {
                HasErrorMsg = true;
                errorMsg = value;
            }
        }

        public Comparator(string symbol)
        {
            switch (symbol)
            {
                case "<":
                    func = ComparatorFunc.LT;
                    break;
                case ">":
                    func = ComparatorFunc.GT;
                    break;
                case "<=":
                    func = ComparatorFunc.LEQ;
                    break;
                case ">=":
                    func = ComparatorFunc.GEQ;
                    break;
                case "==":
                    func = ComparatorFunc.EQ;
                    break;
                case "!=":
                    func = ComparatorFunc.NEQ;
                    break;
            }
        }
                      
        public bool Compare(int a, int b)
        {
            switch(func)
            {
                case ComparatorFunc.LT:
                    return a < b;
                case ComparatorFunc.GT:
                    return a > b; 
                case ComparatorFunc.LEQ:
                    return a <= b;
                case ComparatorFunc.GEQ:
                    return a >= b;
                case ComparatorFunc.EQ:
                    return a == b;
                case ComparatorFunc.NEQ:
                    return a != b;
            }

            return false;
        }
       
        public bool Compare(int a, double b)
        {
            switch (func)
            {
                case ComparatorFunc.LT:
                    return a < b;
                case ComparatorFunc.GT:
                    return a > b;
                case ComparatorFunc.LEQ:
                    return a <= b;
                case ComparatorFunc.GEQ:
                    return a >= b;
                case ComparatorFunc.EQ:
                    return a == b;
                case ComparatorFunc.NEQ:
                    return a != b;
            }

            return false;
        }

        public bool Compare(double a, int b)
        {
            return Compare(b, a);
        }

        public bool Compare(double a, double b)
        {
            switch (func)
            {
                case ComparatorFunc.LT:
                    return a < b;
                case ComparatorFunc.GT:
                    return a > b;
                case ComparatorFunc.LEQ:
                    return a <= b;
                case ComparatorFunc.GEQ:
                    return a >= b;
                case ComparatorFunc.EQ:
                    return a == b;
                case ComparatorFunc.NEQ:
                    return a != b;
            }

            return false;
        }
       
        public bool Compare(int a, vec3 b)
        {
            ErrorMsg = "Cannot compare types Int and Vec3";
            return false;
        }

        public bool Compare(vec3 a, int b)
        {
            ErrorMsg = "Cannot compare types Vec3 and Int";
            return false;
        }

        public bool Compare(double a, vec3 b)
        {
            ErrorMsg = "Cannot compare types Float and Vec3";
            return false;
        }

        public bool Compare(vec3 a, double b)
        {
            ErrorMsg = "Cannot compare types Vec3 and Float";
            return false;
        }

        public bool Compare(vec3 a, vec3 b)
        {
            switch (func)
            {
                case ComparatorFunc.EQ:
                    return a == b;
                case ComparatorFunc.NEQ:
                    return a != b;
            }

            ErrorMsg = "Can only use equality check between Vec3 and Vec3";
            return false;
        }

    }
}
