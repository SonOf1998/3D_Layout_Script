namespace Alert
{
    /* Errorok és Warningok ősosztálya.
     * Tartalmazza az üzenetet illetve a sorszámot.
     * 
     * További fejlesztési ötletként tartalmazhatna opcionálisan egy highlight párost.
     * Így a hibákat nem csak sorszinten tudjuk jelezni, hanem konkrétan a hibás részeket
     * meg tudjuk világítani.
     */ 

    public abstract class Alert
    {
        public string Msg { get; private set; }
        public int LineNumber { get; private set; }

        public abstract string GetAlertType();


        public Alert(int lineNumber, string msg)
        {
            LineNumber = lineNumber;
            Msg = msg;
        }

        public override bool Equals(object o)
        {
            if (o is Alert)
            {
                if (Msg == ((Alert)o).Msg && LineNumber == ((Alert)o).LineNumber)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Msg.GetHashCode() ^ LineNumber.GetHashCode();
        }
    }


    
}
