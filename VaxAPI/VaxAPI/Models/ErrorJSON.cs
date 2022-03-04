namespace VaxAPI.Models
{
    public class ErrorJSON
    {
        public string Error { get; set; }

        public ErrorJSON(string error)
        {
            Error = error;
        }
    }
}