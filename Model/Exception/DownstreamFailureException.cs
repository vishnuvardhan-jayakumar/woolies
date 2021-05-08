namespace Woolies.Model.Exception
{
    public class DownstreamFailureException : System.Exception
    {
        
        public DownstreamFailureException(string message) : base(message)
        {
        }

        public DownstreamFailureException(string message, System.Exception innerException) : base(message, innerException)
        {
            
        }
    }
}