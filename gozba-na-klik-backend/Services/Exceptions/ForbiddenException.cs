namespace gozba_na_klik_backend.Services.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }
}