namespace Kader.DTOs
{
    public class JobReturnDto
    {
        public bool isSuccess {get;}
        public string ErrorMsg {get;}
        public JobReturnDto(bool success, string errorMsg = ""){
            isSuccess = success;
            ErrorMsg = errorMsg;
        }
    }
}