using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data;
using Data.Interfaces;
using Data.Repositories;
using Data.Models;
using JobExchangeAPI.Models.ResponseModels;
using System.Net;

namespace JobExchangeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly IJobExchangeRepository _jobExchangeRepository;

        public CompaniesController(ILogger<CompaniesController> logger, IJobExchangeRepository jobExchangeRepository)
        {
            _logger = logger;
            _jobExchangeRepository = jobExchangeRepository;
        }

        [HttpGet]
        public async Task<ResponseModel<Сompany>> Get() {
            try 
            {
                return new ResponseModel<Сompany>
                {
                    Code = (int)HttpStatusCode.OK,
                    Data = await _jobExchangeRepository.GetCompaniesAsync()
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Сompany>
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
