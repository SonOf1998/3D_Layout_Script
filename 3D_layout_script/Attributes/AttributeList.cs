using System.Collections.Generic;

namespace _3D_layout_script.Attributes
{
    public class AttributeList
    {
        List<Attribute> list;

        public AttributeList()
        {
            list = new List<Attribute>();
        }

        public AttributeList(AttributeList other)
        {
            list = new List<Attribute>(other.list);
        }

        public bool Add(Attribute attr)
        {
            if (!(attr.Name == "rotation-axis" || attr.Name == "rotation-angle"))
            {
                foreach (var elem in list)
                {
                    if (elem.Name == attr.Name)
                    {
                        list.Remove(elem);
                        list.Add(attr);
                        return false;
                    }
                }
            }

            list.Add(attr);
            return true;
        }

        public List<Attribute> GetAttributeList()
        {
            return list;
        }

        /* Az utoljára definiált diff mennyiségű angle attribútumot kiveszi a listából. */ 
        public void FitRotationAngles(int maxNumberOfAngles)
        {
            int foundAngles = 0;
            List<Attribute> toRemove = new List<Attribute>();
            foreach (var attr in list)
            {
                if (attr.Name == "rotation-angle")
                {
                    if (foundAngles == maxNumberOfAngles)
                    {
                        toRemove.Add(attr);
                    }
                    else
                    {
                        foundAngles++;
                    }                    
                }
            }

            foreach (var attrToRemove in toRemove)
            {
                list.Remove(attrToRemove);
            }
        }

        /* Az utoljára definiált diff mennyiségű rotation-axis-hoz hozzárendeli az utolsó definiált angle értéket, vagy ha ilyen nincs 0-át
         * Az összerendelés annyit jelent, hogy felveszünk még diff mennyiségű angle attribútumot a előbb említett értékkel 
         * Az összerendelést majd a DDDObject végzi el magában
        */
        public void FitRotationAxes(int diff)
        {
            Attribute a = null;
            foreach (var attr in list)
            {
                if (attr.Name == "rotation-angle")
                {
                    a = attr;
                }
            }

            if (a == null)
            {
                a = new Attribute("rotation-angle", 0);
            }

            for (int i = 0; i < diff; ++i)
            {
                list.Add(a);
            }
        }

        public IEnumerator<Attribute> GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
