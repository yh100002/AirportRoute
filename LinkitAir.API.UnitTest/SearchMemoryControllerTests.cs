using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkitAir.API.Controllers;
using LinkitAir.Service.Dto;
using LinkitAir.Service.Interface;
using LinkitAir.Service.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LinkitAir.API.UnitTest
{
    /*
    I made a simple unit test case as an example for this test project.
    Bu in reality, I believe that the code coverage with SQ should be more than 75% at least.
     */
    public class SearchMemoryControllerTests
    {
        [Fact]
        public async Task SearchSourceAirportAsync_WhenCalledWithVaildAirport_Returns2Airports()
        {
            var param = new AirportSearchDto()
            {
                AirportName = "A1"                
            };

            List<Airport> expected = new List<Airport>()
            {
                new Airport{ AirportId = 1, AirportName = "A1"},
                new Airport{ AirportId = 2, AirportName = "A12"}
            };

            var repositoryMock = new Mock<ISearchMemoryRepository>();
            repositoryMock.Setup(s => s.SearchSourceAirportAsync(It.IsAny<AirportSearchDto>())).ReturnsAsync(expected);

            var target = BuilTarget(repositoryMock.Object);
            var result = await target.SearchSourceAirport(param);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var airports = okResult.Value.Should().BeAssignableTo<IEnumerable<Airport>>().Subject;
            airports.Count().Should().Be(2);
        }

        [Fact]
        public async Task SearchSourceAirportAsync_WhenCalledWithInVaildAirport_Returns0Airports()
        {
            var param = new AirportSearchDto()
            {
                AirportName = "A1"                
            };

            List<Airport> expected = new List<Airport>(){ };

            var repositoryMock = new Mock<ISearchMemoryRepository>();
            repositoryMock.Setup(s => s.SearchSourceAirportAsync(It.IsAny<AirportSearchDto>())).ReturnsAsync(expected);

            var target = BuilTarget(repositoryMock.Object);
            var result = await target.SearchSourceAirport(param);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var airports = okResult.Value.Should().BeAssignableTo<IEnumerable<Airport>>().Subject;
            airports.Count().Should().Be(0);
        }

        [Fact]
        public async Task SearchSourceAirportAsync_WhenCalledWithSomeException_Returns0Airports()
        {
            var param = new AirportSearchDto()
            {
                AirportName = "A1"                
            };            

            var repositoryMock = new Mock<ISearchMemoryRepository>();
            repositoryMock.Setup(s => s.SearchSourceAirportAsync(It.IsAny<AirportSearchDto>())).Throws(new Exception("Some exception"));

            var target = BuilTarget(repositoryMock.Object);
            
            Exception ex = await Assert.ThrowsAsync<Exception>(() => target.SearchSourceAirport(param));
            Assert.Equal(ex.Message, "Some exception");
        }
        
        private Mock<ISearchMemoryRepository> BuildRepositoryMock(List<Airport> expected)
        {
            var repositoryMock = new Mock<ISearchMemoryRepository>();
            repositoryMock.Setup(s => s.SearchSourceAirportAsync(It.IsAny<AirportSearchDto>())).ReturnsAsync(expected);
            return repositoryMock;
        }

        private static SearchMemoryController BuilTarget(ISearchMemoryRepository repository)
        {            
            Mock<ILogger<SearchMemoryController>> mock = new Mock<ILogger<SearchMemoryController>>();

            //All dependent objects should be able to be Mocked and injected
            //This is goo enough reason why we have to make very specifiv interfaces, Which alway are testable.
            return new SearchMemoryController(repository, mock.Object);
        }
    }
}

