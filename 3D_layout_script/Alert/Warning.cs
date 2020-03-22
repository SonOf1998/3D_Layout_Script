namespace Alert
{
    public sealed class Warning : Alert
    {
        public Warning(int lineNumber, string msg) : base(lineNumber, msg)
        {

        }
        public override string GetAlertType()
        {
            return "Warning";
        }
    }
}
