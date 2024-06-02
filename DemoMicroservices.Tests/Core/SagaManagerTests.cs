using DemoMicroservices.Core.Saga;
using DemoMicroservices.OrderService;
using NSubstitute;

namespace DemoMicroservices.Tests.Core
{
    public class Tests
    {
        private IServiceProvider _serviceProvider;
        private SagaManager<CreateOrderSagaData> _sagaManager;
        private ISagaInstanceRepository _sagaInstanceRepository;

        [SetUp]
        public void Setup()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            _sagaInstanceRepository = Substitute.For<ISagaInstanceRepository>();
            _serviceProvider.GetService(typeof(ISaga<CreateOrderSagaData>)).Returns(new CreateOrderSaga());
            _serviceProvider.GetService(typeof(ISagaInstanceRepository)).Returns(_sagaInstanceRepository);
            _sagaManager = new SagaManager<CreateOrderSagaData>(_serviceProvider);
        }

        [Test]
        public async Task CreateAsync_WhenCalled_ShouldCreateNewSagaInstance()
        {
            //Arrange  
            //create new #CreateOrderSagaData and initialize its properties
            var createOrderSagaData = new CreateOrderSagaData
            {
                OrderId = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString()
            };
            //Act
            var result = await _sagaManager.CreateAsync(createOrderSagaData);
            //Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public async Task HandleReplyAsync_SagaStep1LocalStep2Remote_ShouldReturnCommands()
        {
            //Arrange
            //create new #CreateOrderSagaData and initialize its properties
            var createOrderSagaData = new CreateOrderSagaData
            {
                OrderId = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString()
            };
            //Act
            var sagaInstance = await _sagaManager.CreateAsync(createOrderSagaData);
            //Assert
            Assert.IsTrue(sagaInstance.GetCommands() is { Count: >0});
        }
    }
}