
namespace E_Com.DTO_s.ResponseDTO_s
{
    public class ServiceResopnses<T>
    {
        public bool Success { get; set; } = true;

        public T Data { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
