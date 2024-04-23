using API_Project.Models;
using API_Project.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace IntegrationTesting
{
    public class IntegrationCustomer : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public IntegrationCustomer(WebApplicationFactory<Program> factory)
        {

            _factory = factory;

        }
        [Theory]
        [InlineData("https://localhost:44361/api/Customer/Getcustomer")]
        [InlineData("https://localhost:44361/api/Product/Getproduct")]
        public async Task IntegrationTestType1(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            var result = response.IsSuccessStatusCode;
            if (result)
                Assert.True(result);
            else
                Assert.False(result);
        }
        [Theory]
        [InlineData("https://localhost:44333/api/Product/GetAllProducts")]
        public async Task IntegrationTestType2(string url)
        {
            var client = _factory.CreateClient();
            var result = await client.GetAsync(url);
            var responseContent = await result.Content.ReadAsStringAsync();
            var actualCustomer = JsonConvert.DeserializeObject<Customer>(responseContent);
            Assert.IsType<Customer>(actualCustomer);
            //var result = await client.GetAsync(url);
            //var content = await result.Content.ReadFromJsonAsync<Customer>();
            //Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            //Assert.Equal("test", actualCustomer?.);

        }


    }

}