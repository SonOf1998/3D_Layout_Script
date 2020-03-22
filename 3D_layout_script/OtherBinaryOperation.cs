namespace _3D_layout_script
{
    class OtherBinaryOperation
    {
        private enum Operation
        {
            PLUS_ASSIGN,
            MINUS_ASSIGN,
            MULTI_ASSIGN,
            DIV_ASSIGN,
        }

        Operation op;
        public string ErrorMsg { get; private set; }
        private string warningMsg;
        public string WarningMsg {
            get
            {
                string copy = warningMsg;
                warningMsg = null;
                return copy;
            }
            set
            {
                warningMsg = value;
            }
        }


        public OtherBinaryOperation(string opStr)
        {
            switch (opStr)
            {
                case "+=":
                    op = Operation.PLUS_ASSIGN;
                    break;
                case "-=":
                    op = Operation.MINUS_ASSIGN;
                    break;
                case "*=":
                    op = Operation.MULTI_ASSIGN;
                    break;
                case "/=":
                    op = Operation.DIV_ASSIGN;
                    break;
            }
        }

        public double Calculate(double a, double b)
        {
            switch (op)
            {
                case Operation.PLUS_ASSIGN:
                    return a + b;
                case Operation.MINUS_ASSIGN:
                    return a - b;
                case Operation.MULTI_ASSIGN:
                    return a * b;
                case Operation.DIV_ASSIGN:
                    if (b == 0.0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return double.MinValue;
                    }

                    return a / b;
            }

            return 0;
        }

        public double Calculate(double a, int b)
        {
            switch (op)
            {
                case Operation.PLUS_ASSIGN:
                    return a + b;
                case Operation.MINUS_ASSIGN:
                    return a - b;
                case Operation.MULTI_ASSIGN:
                    return a * b;
                case Operation.DIV_ASSIGN:
                    if (b == 0.0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return double.MinValue;
                    }
                    return a / b;
            }

            return 0;
        }
        

        public int Calculate(int a, int b)
        {
            switch (op)
            {
                case Operation.PLUS_ASSIGN:
                    return a + b;
                case Operation.MINUS_ASSIGN:
                    return a - b;
                case Operation.MULTI_ASSIGN:
                    return a * b;
                case Operation.DIV_ASSIGN:
                    if (b == 0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return (int)(a / 0.0);
                    }
                    return a / b;
            }


            return 0;
        }

        public vec3 Calculate(vec3 a, vec3 b)
        {
            switch (op)
            {
                case Operation.PLUS_ASSIGN:
                    return a + b;
                case Operation.MINUS_ASSIGN:
                    return a - b;
            }

            ErrorMsg = "Only addition and subtraction is allowed between Vec3 and Vec3";
            return null;
        }

        public vec3 Calculate(vec3 a, double b)
        {
            switch (op)
            {
                case Operation.MULTI_ASSIGN:
                    return a * b;
                case Operation.DIV_ASSIGN:
                    if (b == 0.0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return new vec3(double.MinValue, double.MinValue, double.MinValue);
                    }
                    return a / b;
            }

            ErrorMsg = "Only multiplication and division is allowed between Vec3 and float";
            return null;
        }

        public object Calculate(double a, vec3 b)
        {
            ErrorMsg = "Cannot use += -= *= /= %= between Float and Vec3";
            return null;
        }

        public vec3 Calculate(vec3 a, int b)
        {
            switch (op)
            {
                case Operation.MULTI_ASSIGN:
                    return a * b;
                case Operation.DIV_ASSIGN:
                    if (b == 0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return new vec3(double.MinValue, double.MinValue, double.MinValue);
                    }
                    return a / b;
            }

            ErrorMsg = "Only multiplication and division is allowed between Vec3 and float";
            return null;
        }

        public object Calculate(int a, vec3 b)
        {
            ErrorMsg = "Cannot use += -= *= /= %= between Int and Vec3";
            return null;
        }
    }
}
