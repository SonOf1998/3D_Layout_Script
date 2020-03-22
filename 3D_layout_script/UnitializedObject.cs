using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_layout_script
{
    class UnitializedObject
    {

        public static UnitializedObject operator -(UnitializedObject uo)
        {
            return uo;
        }

        public override string ToString()
        {
            return "UnitializedObject";
        }
    }
}
