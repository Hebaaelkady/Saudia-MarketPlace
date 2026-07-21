using Microsoft.AspNetCore.Mvc;

namespace Kader.Models
{
    public class JsonResultModel
    {
        public bool isSuccess { get; }
        public string Message { get; }

        public JsonResultModel(bool isSuccess, string Message)
        {
            this.isSuccess = isSuccess;
            this.Message = Message;
        }
        public JsonResult Serialize()
        {
            return new JsonResult(this);
        }
    }
}
