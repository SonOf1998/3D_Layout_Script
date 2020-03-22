namespace Alert
{
    public sealed class Error : Alert
    {
        public Error(int lineNumber, string msg) : base(lineNumber, msg)
        {

        }
        public override string GetAlertType()
        {
            return "Error";
        }
    }
}
