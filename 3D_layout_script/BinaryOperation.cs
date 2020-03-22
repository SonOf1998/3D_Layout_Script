/* Összeg, Kivonás, Szorzás, Osztás, Modulus bármely két típuson belül. 
 * 
*/

namespace _3D_layout_script
{
    class BinaryOperation
    {
        private enum Operation
        {
            PLUS,
            MINUS,
            MULTI,
            DIV,
            MOD
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

        public BinaryOperation(string symbol)
        {
            switch (symbol)
            {
                case "+":
                    op = Operation.PLUS;
                    break;
                case "-":
                    op = Operation.MINUS;
                    break;
                case "*":
                    op = Operation.MULTI;
                    break;
                case "/":
                    op = Operation.DIV;
                    break;
                case "%":
                    op = Operation.MOD;
                    break;
            }
        }

        public double? Calculate(double a, double b)
        {
            switch (op)
            {
                case Operation.PLUS:
                    return a + b;
                case Operation.MINUS:
                    return a - b;
                case Operation.MULTI:
                    return a * b;
                case Operation.DIV:
                    if (b == 0.0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return double.MinValue;
                    }
                    return a / b;
            }

            ErrorMsg = "Invalid operation (%) between Float and Float";
            return null;
        }

        public double? Calculate(double a, int b)
        {
            switch (op)
            {
                case Operation.PLUS:
                    return a + b;
                case Operation.MINUS:
                    return a - b;
                case Operation.MULTI:
                    return a * b;
                case Operation.DIV:
                    if (b == 0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return double.MinValue;

                    }
                    return a / b;
            }

            ErrorMsg = "Invalid operation (%) between Float and Int";
            return null;
        }

        public double? Calculate(int a, double b)
        {
            switch (op)
            {
                case Operation.PLUS:
                    return a + b;
                case Operation.MINUS:
                    return a - b;
                case Operation.MULTI:
                    return a * b;
                case Operation.DIV:
                    if (b == 0.0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return double.MinValue;
                    }
                    return a / b;
            }

            ErrorMsg = "Invalid operation (%) between Int and Float";
            return null;
        }

        public int? Calculate(int a, int b)
        {
            switch (op)
            {
                case Operation.PLUS:
                    return a + b;
                case Operation.MINUS:
                    return a - b;
                case Operation.MULTI:
                    return a * b;
                case Operation.DIV:
                    if (b == 0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return (int)(a / 0.0);
                    }


                    return a / b;
                case Operation.MOD:
                    return a % b;
            }

            return null;
        }

        public vec3 Calculate(double a, vec3 b)
        {
            switch (op)
            {
                case Operation.MULTI:
                    return a * b;
            }

            ErrorMsg = "Only multiplication is allowed between Float and Vec3";
            return null;
        }

        public vec3 Calculate(vec3 a, double b)
        {
            switch (op)
            {
                case Operation.MULTI:
                    return a * b;
                case Operation.DIV:
                    if (b == 0.0)
                    {
                        WarningMsg = "Dividing by zero can cause unexpected results";
                        return new vec3(double.MinValue, double.MinValue, double.MinValue);
                    }

                    return a / b;
            }

            ErrorMsg = "Only multiplication and division is allowed between Vec3 and Float";
            return null;
        }

        public vec3 Calculate(vec3 a, vec3 b)
        {
            switch (op)
            {
                case Operation.PLUS:
                    return a + b;
                case Operation.MINUS:
                    return a - b;
            }

            ErrorMsg = "Only addition and subtraction is allowed between Vec3 and Vec3";
            return null;
        }

        public UnitializedObject Calculate(UnitializedObject uo, object o)
        {
            return uo;
        }

        public UnitializedObject Calculate(object o, UnitializedObject uo)
        {
            return uo;
        }

        public ErrorObject Calculate(ErrorObject eo, object o)
        {
            return eo;
        }

        public ErrorObject Calculate(object o, ErrorObject eo)
        {
            return eo;
        }
    }
}
