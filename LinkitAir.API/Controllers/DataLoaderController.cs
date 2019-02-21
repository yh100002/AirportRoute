namespace LinkitAir.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LinkitAir.Service.Configurations;
    using LinkitAir.Service.Data;
    using LinkitAir.Service.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [ApiController]
    public class DataLoaderController : ControllerBase
    {      
        private readonly IBulkDataLoadRepository bulkDataLoadRepository;
        private readonly ILogger<DataLoaderController> logger;
        

        public  DataLoaderController(IBulkDataLoadRepository bulkDataLoadRepository, ILogger<DataLoaderController> logger)
        {           
            this.bulkDataLoadRepository = bulkDataLoadRepository;
            this.logger = logger;
        }

        //Actual do not need
        [HttpGet("init")]
        public IActionResult Init()
        {            
            var resultMessage = this.bulkDataLoadRepository.LoadData();            
            return Ok($"{resultMessage}");            
        }
        
    }
}