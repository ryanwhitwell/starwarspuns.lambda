using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using StarWarsPuns.BusinessLogic;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Data.Interfaces;
using Xunit;
using System;
using StarWarsPuns.Models;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET;
using Alexa.NET.InSkillPricing;
using Alexa.NET.Response;
using StarWarsPuns.Core;
using Amazon.Lambda.Core;
using StarWarsPuns.Data;

namespace StarWarsPuns.Tests.BusinessLogic
{
  public class RequestBusinessLogicTests
  {
    private static readonly SkillRequest ValidSkillRequest = new SkillRequest()
    {
      Context = new Context()
      {
        System = new AlexaSystem()
        {
          ApiEndpoint = "http://localhost",
          ApiAccessToken = "TestApiAccessToken",
          User = new User()
          {
            UserId = "TestUserId",
            AccessToken = "TestAccessToken"
          }
        }
      },
      Request = new IntentRequest()
      {
        RequestId = "TestRequestId",
        Type = "IntentRequest"
      }
    };
    
    [Fact]
    public void Ctor_ShouldReturnInstanceOfClass_WhenInputIsValid()
    {
      Mock<ISkillRequestValidator>        mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ISkillProductsClientAdapter>   mockSkillProductsAdapter  = new Mock<ISkillProductsClientAdapter>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger                = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper         = new Mock<IRequestMapper>();
      Mock<ITokenUserData>                mockTokenUserData         = new Mock<ITokenUserData>();
      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      Assert.IsType<RequestBusinessLogic>(sut);
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenSkillProductsAdapterIsNull()
    {
      Mock<ISkillRequestValidator>        mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger                = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper         = new Mock<IRequestMapper>();
      Mock<ITokenUserData>                mockTokenUserData         = new Mock<ITokenUserData>();
      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();

      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, null, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenSkillRequestValidatorIsNull()
    {
      Mock<ISkillProductsClientAdapter>         mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger               = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper        = new Mock<IRequestMapper>();
      Mock<ITokenUserData>                mockTokenUserData        = new Mock<ITokenUserData>();
      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();

      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockUserProfileClient.Object, null, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ISkillProductsClientAdapter>  mockSkillProductsAdapter  = new Mock<ISkillProductsClientAdapter>();
      Mock<IRequestMapper>         mockRequestMapper         = new Mock<IRequestMapper>();
      Mock<ITokenUserData>         mockTokenUserData         = new Mock<ITokenUserData>();
      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();

      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, null, mockRequestMapper.Object, mockTokenUserData.Object));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenREquestMapperIsEmpty()
    {
      Mock<ISkillRequestValidator>        mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ISkillProductsClientAdapter>         mockSkillProductsAdapter  = new Mock<ISkillProductsClientAdapter>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger                = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<ITokenUserData>                mockTokenUserData         = new Mock<ITokenUserData>();
      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();

      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, null, mockTokenUserData.Object));
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenTokenDataIsNull()
    {
      Mock<ISkillRequestValidator>        mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      Mock<ISkillProductsClientAdapter>         mockSkillProductsAdapter  = new Mock<ISkillProductsClientAdapter>();
      Mock<ILogger<RequestBusinessLogic>> mockLogger                = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper         = new Mock<IRequestMapper>();
      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();

      Assert.Throws<ArgumentNullException>(() => new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, null));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GenerateEmptyTokenUser_ShouldThrowArgumentNullException_WhenIdIsNotProvided(string id)
    {
      Assert.Throws<ArgumentNullException>(() => RequestBusinessLogic.GenerateEmptyTokenUser(id));
    }

    [Fact]
    public void GenerateEmptyTokenUser_ShouldReturnATokenUser_WhenIdIsProvided()
    {
      string id = "abc123";

      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = id,
        Players = new List<Player>()
      };

      TokenUser tokenUser = RequestBusinessLogic.GenerateEmptyTokenUser(id);

      Assert.IsType<TokenUser>(tokenUser);
      Assert.Equal(expectedTokenUser.Id, tokenUser.Id);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnTrue_WhenUserHasPointsPersistenceAndIsEntitled()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(
        new InSkillProductsResponse()
        {
          Products = new InSkillProduct[] { new InSkillProduct() { Entitled = Entitlement.Entitled, ProductId = "amzn1.adg.product.467f7ca4-91dd-48d3-b831-040673e7066c" }}
        }
      ));

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger        = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper = new Mock<IRequestMapper>();
      Mock<ITokenUserData>                mockTokenUserData = new Mock<ITokenUserData>();

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      SkillRequest skillRequest = ValidSkillRequest;

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(skillRequest);

      Assert.True(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserHasPointsPersistenceAndIsNotEntitled()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(
        new InSkillProductsResponse()
        {
          Products = new InSkillProduct[] { new InSkillProduct() { Entitled = Entitlement.NotEntitled, ProductId = "amzn1.adg.product.467f7ca4-91dd-48d3-b831-040673e7066c" }}
        }
      ));

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper = new Mock<IRequestMapper>();
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(ValidSkillRequest);

      Assert.False(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserDoesNotHavePointsPersistence()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(
        new InSkillProductsResponse()
        {
          Products = new InSkillProduct[] { new InSkillProduct() { Entitled = Entitlement.NotEntitled, ProductId = "NotPointsPersistence" }}
        }
      ));

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper = new Mock<IRequestMapper>();
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(ValidSkillRequest);

      Assert.False(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserDoesNotHaveAnySkillProducts()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(
        new InSkillProductsResponse()
        {
          Products = null
        }
      ));

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper = new Mock<IRequestMapper>();
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(ValidSkillRequest);

      Assert.False(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserHasEmptySkillProducts()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(null));

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper = new Mock<IRequestMapper>();
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);

      bool hashPointsPersistence = await sut.HasPointsPersistence(ValidSkillRequest);

      Assert.False(hashPointsPersistence);
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldThrowArgumentNullException_WhenKillRequestIsNotValid()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(false);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Returns(Task.FromResult<InSkillProductsResponse>(null));

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper = new Mock<IRequestMapper>();
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);

      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.HasPointsPersistence(ValidSkillRequest));
    }

    [Fact]
    public async Task HasPointsPersistence_ShouldReturnFalse_WhenUserHasPointsPersistenceCannotReadTheUsersSkillProducts()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).Throws(new Exception());

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper = new Mock<IRequestMapper>();
      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      bool hashPointsPersistence = await sut.HasPointsPersistence(ValidSkillRequest);

      Assert.False(hashPointsPersistence);
    }

    [Fact]
    public async Task GetUserApplicationState_ShouldReturnTokenUser_WhenExistingUserIsFound()
    {
      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = "existingTokenUser",
        Players = new List<Player>()
      };
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).ReturnsAsync(new InSkillProductsResponse(){ Products = new InSkillProduct[0] });

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper = new Mock<IRequestMapper>();

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();
      mockTokenUserData.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(expectedTokenUser);

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      
      TokenUser tokenUser = await sut.GetUserApplicationState(ValidSkillRequest);

      Assert.Equal(expectedTokenUser, tokenUser);
    }

    [Fact]
    public async Task GetUserApplicationState_ShouldThrowArgumentNullException_WhenSkillRequestIsInvalid()
    {
      TokenUser expectedTokenUser = new TokenUser()
      {
        Id = "existingTokenUser",
        Players = new List<Player>()
      };
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(false);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).ReturnsAsync(new InSkillProductsResponse(){ Products = new InSkillProduct[0] });

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      Mock<IRequestMapper>                mockRequestMapper = new Mock<IRequestMapper>();

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();
      mockTokenUserData.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(expectedTokenUser);

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      
      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetUserApplicationState(ValidSkillRequest));
    }

    [Fact]
    public async Task GetSkillResponse_ShouldReturnSkillResponse_WhenInputIsValid()
    {
      TokenUser tokenUser = new TokenUser()
      {
        Id = "existingTokenUser",
        Players = new List<Player>()
      };

      SkillResponse expectedSkillResponse = new SkillResponse();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).ReturnsAsync(new InSkillProductsResponse(){ Products = new InSkillProduct[0] });

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();

      Mock<IRequestRouter> mockIntentRequestRouter = new Mock<IRequestRouter>();
      mockIntentRequestRouter.Setup(x => x.GetSkillResponse(It.IsAny<SkillRequest>(), It.IsAny<TokenUser>())).ReturnsAsync(expectedSkillResponse);
      mockIntentRequestRouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      Mock<IRequestMapper> mockRequestMapper = new Mock<IRequestMapper>();
      mockRequestMapper.Setup(x => x.GetRequestHandler(It.IsAny<SkillRequest>())).Returns(mockIntentRequestRouter.Object);

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();
      mockTokenUserData.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(tokenUser);

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      
      SkillResponse skillResponse = await sut.GetSkillResponse(ValidSkillRequest, tokenUser);
      
      Assert.IsType<SkillResponse>(skillResponse);
    }

    [Fact]
    public async Task GetSkillResponse_ShouldThrowArgumentNullException_WhenSkillRequestIsInvalid()
    {
      TokenUser tokenUser = new TokenUser()
      {
        Id = "existingTokenUser",
        Players = new List<Player>()
      };

      SkillResponse expectedSkillResponse = new SkillResponse();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(false);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).ReturnsAsync(new InSkillProductsResponse(){ Products = new InSkillProduct[0] });

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      
     Mock<IRequestRouter> mockIntentRequestRouter = new Mock<IRequestRouter>();
      mockIntentRequestRouter.Setup(x => x.GetSkillResponse(It.IsAny<SkillRequest>(), It.IsAny<TokenUser>())).ReturnsAsync(expectedSkillResponse);
      mockIntentRequestRouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      Mock<IRequestMapper> mockRequestMapper = new Mock<IRequestMapper>();
      mockRequestMapper.Setup(x => x.GetRequestHandler(It.IsAny<SkillRequest>())).Returns(mockIntentRequestRouter.Object);

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();
      mockTokenUserData.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(tokenUser);

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      
      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetSkillResponse(ValidSkillRequest, tokenUser));
    }

    [Fact]
    public async Task GetSkillResponse_ShouldThrowArgumentNullException_WhenTokenUserIsNull()
    {
      SkillResponse expectedSkillResponse = new SkillResponse();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).ReturnsAsync(new InSkillProductsResponse(){ Products = new InSkillProduct[0] });

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      
      Mock<IRequestRouter> mockIntentRequestRouter = new Mock<IRequestRouter>();
      mockIntentRequestRouter.Setup(x => x.GetSkillResponse(It.IsAny<SkillRequest>(), It.IsAny<TokenUser>())).ReturnsAsync(expectedSkillResponse);
      mockIntentRequestRouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      Mock<IRequestMapper> mockRequestMapper = new Mock<IRequestMapper>();
      mockRequestMapper.Setup(x => x.GetRequestHandler(It.IsAny<SkillRequest>())).Returns(mockIntentRequestRouter.Object);

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      
      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetSkillResponse(ValidSkillRequest, null));
    }

    [Fact]
    public async Task HandleSkillRequest_ShouldThrowArgumentNullException_WhenSkillRequestIsInvalid()
    {
      Mock<ILambdaContext> mockLambdaContext = new Mock<ILambdaContext>();

      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(false);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).ReturnsAsync(new InSkillProductsResponse(){ Products = new InSkillProduct[0] });

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      
      Mock<IRequestMapper> mockRequestMapper = new Mock<IRequestMapper>();

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      
      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.HandleSkillRequest(ValidSkillRequest, mockLambdaContext.Object));
    }

    [Fact]
    public async Task HandleSkillRequest_ShouldThrowArgumentNullException_WhenLambdaContextIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).ReturnsAsync(new InSkillProductsResponse(){ Products = new InSkillProduct[0] });

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      
      Mock<IRequestMapper> mockRequestMapper = new Mock<IRequestMapper>();

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      
      await Assert.ThrowsAsync<ArgumentNullException>(() => sut.HandleSkillRequest(ValidSkillRequest, null));
    }

    [Fact]
    public async Task HandleSkillRequest_ShouldReturnSkillResponse_WhenInputIsValid()
    {
      SkillResponse expectedSkillResponse = new SkillResponse()
      {
        Version = "TestVersion",
        Response = new ResponseBody()
      };

      Mock<ILambdaContext> mockLambdaContext = new Mock<ILambdaContext>();

      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);

      Mock<SkillProductsClient> mockInSkillProductsClient = new Mock<SkillProductsClient>(MockBehavior.Loose);
      mockInSkillProductsClient.Setup(x => x.GetProducts()).ReturnsAsync(new InSkillProductsResponse(){ Products = new InSkillProduct[0] });

      Mock<ISkillProductsClientAdapter> mockSkillProductsAdapter = new Mock<ISkillProductsClientAdapter>(MockBehavior.Loose);
      mockSkillProductsAdapter.Setup(x => x.GetClient(It.IsAny<SkillRequest>())).Returns(mockInSkillProductsClient.Object);
      
      Mock<ILogger<RequestBusinessLogic>> mockLogger = new Mock<ILogger<RequestBusinessLogic>>();
      
      Mock<IRequestRouter> mockIntentRequestRouter = new Mock<IRequestRouter>();
      mockIntentRequestRouter.Setup(x => x.GetSkillResponse(It.IsAny<SkillRequest>(), It.IsAny<TokenUser>())).ReturnsAsync(expectedSkillResponse);
      mockIntentRequestRouter.Setup(x => x.RequestType).Returns(RequestType.IntentRequest);

      Mock<IRequestMapper> mockRequestMapper = new Mock<IRequestMapper>();
      mockRequestMapper.Setup(x => x.GetRequestHandler(It.IsAny<SkillRequest>())).Returns(mockIntentRequestRouter.Object);

      Mock<ITokenUserData> mockTokenUserData = new Mock<ITokenUserData>();
      mockTokenUserData.Setup(x => x.Save(It.IsAny<TokenUser>())).Returns(Task.FromResult(true));

      Mock<IUserProfileClient>            mockUserProfileClient     = new Mock<IUserProfileClient>();
      mockUserProfileClient.Setup(x => x.GetUserId(It.IsAny<string>())).ReturnsAsync("TestProfileUserId");

      RequestBusinessLogic sut = new RequestBusinessLogic(mockUserProfileClient.Object, mockSkillRequestValidator.Object, mockSkillProductsAdapter.Object, mockLogger.Object, mockRequestMapper.Object, mockTokenUserData.Object);
      
      SkillResponse skillResponse = await sut.HandleSkillRequest(ValidSkillRequest, mockLambdaContext.Object);

      Assert.IsType<SkillResponse>(skillResponse);
    }
  }
}
