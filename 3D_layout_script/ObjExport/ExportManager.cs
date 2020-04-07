using _3D_layout_script.Objects;
using System.Collections.Generic;

namespace _3D_layout_script.ObjExport
{
    // Ha több fájltípusba is akarjuk az exportot valamikor
    public abstract class ExportManager
    {
        protected string filename;

        public ExportManager(string filename)
        {
            this.filename = filename;
        }

        public abstract void Export(List<DDDObject> objects);
    }
}
