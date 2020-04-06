using _3D_layout_script.Objects;
using System.Collections.Generic;

namespace _3D_layout_script.ObjExport
{
    public abstract class ExportManager
    {
        public abstract void Export(List<DDDObject> objects);
    }
}
