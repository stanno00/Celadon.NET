namespace VaxAPI.Models
{
    public class SuccessJSON
    {
        public string Success { get; set; }

        public SuccessJSON(string success)
        {
            Success = success;
        }
    }
}