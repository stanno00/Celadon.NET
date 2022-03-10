namespace DotNetTribes.Exceptions
{
    public class ErrorJSON
    {
        public string Error { get; }

        public ErrorJSON(string error)
        {
            Error = error;
        }
    }
}