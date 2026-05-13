namespace BackpackApp
{
    public class BackpackOverflowException : Exception
    {
        public BackpackOverflowException(string message) : base(message) { }
    }
}
