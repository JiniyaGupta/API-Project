using API_Project.Controllers;
using API_Project.Models;
using API_Project.Repository;
using FakeItEasy;
using LazyCache.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using Assert = Xunit.Assert;

namespace UnitTest
{
    [TestClass]
    public class CustomerControllerTest
    {
        [Fact]
        public void CheckAllCustomerController()
        {
            var data = new Mock<ICustomer>();
            data.Setup(r => r.GetAllCustomers()).ReturnsAsync(new List<Customer>());
            var allController = new CustomerController(data.Object);
            var okObjectResult = allController.GetCustomers();
            OkObjectResult okresult = okObjectResult.Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(okresult);
        }
        [Theory]
        [InlineData("Jiniya", "478438973", "jini@gmail.com")]
        public async void PostCustomerCheck(string customerName, string phone, string email)
        {
            // Arrange
            Customer customers = new Customer
            {
                CustomerName = customerName,
                CustomerPhone = phone,
                CustomerEmail = email
            };

            var mockCustomerRepository = new Mock<ICustomer>();
            mockCustomerRepository.Setup(r => r.AddCustomer(It.IsAny<Customer>()));

            var controller = new CustomerController(mockCustomerRepository.Object);
            var result = await controller.PostCustomer(customers);

            //assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(customers, createdAtActionResult.Value);

        }
        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "jini", "8963511", "j@gmail.com")]
        public async Task PutCustomer_Check(Guid id, string customerName, string phone, string email)
        {
            // Arrange
            var customerToUpdate = new Customer { CustomerId = id, CustomerName = customerName, CustomerPhone = phone, CustomerEmail = email };

            var mockCustomerRepository = new Mock<ICustomer>();
            mockCustomerRepository.Setup(r => r.UpdateCustomer(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            var controller = new CustomerController(mockCustomerRepository.Object);

            // Act
            var result = await controller.PutCustomer(id, customerToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public async Task DeleteCustomer_Check(Guid id)
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomer>();
            mockCustomerRepository.Setup(r => r.DeleteCustomer(id)).Returns(Task.CompletedTask);

            var controller = new CustomerController(mockCustomerRepository.Object);

            // Act
            var result = await controller.DeleteCustomer(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }



}

    
