namespace Kader.DTOs
{
    public class ReturnDto<T>
    {
        public bool isSuccess {get;}
        public string ErrorMsg {get;}
        public T Result {get;}

        public ReturnDto(bool success, T result, string errorMsg = ""){
            isSuccess = success;
            Result = result;
            ErrorMsg = errorMsg;
        }
    }
}
