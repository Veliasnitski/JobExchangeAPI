using Data.Models;

namespace JobExchangeAPI.Models.ResponseModels
{
    public class ResponseModel<T> : BaseResponseModel
    {
        public List<T> Data { get; set; }    
    }
}
