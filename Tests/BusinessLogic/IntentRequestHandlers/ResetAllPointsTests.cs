using StarWarsPuns.BusinessLogic;
using Xunit;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using StarWarsPuns.Models;
using Alexa.NET.Response;
using StarWarsPuns.BusinessLogic.IntentRequestHandlers;

namespace StarWarsPuns.Tests.BusinessLogic.IntentRequestHandlers
{
  public class ResetAllPointsTests
  {
    private static SkillRequest GenerateValidSkillRequest(Request request)
    { 
      SkillRequest skillRequest = new SkillRequest()
      {
        Context = new Context()
        {
          System = new AlexaSystem()
          {
            ApiEndpoint = "http://localhost",
            ApiAccessToken = "xx508xx63817x752xx74004x30705xx92x58349x5x78f5xx34xxxxx51",
            User = new User()
            {
             UserId = "TestUserId", 
AccessToken = "TestAccessToken"
            }
          }
        },
        Request = request
      };

      return skillRequest;
    }
    
    [Fact]
    public void Ctor_ShouldReturnInstanceOfClass_WhenDependenciesAreValid()
    {
      Mock<ILogger<ResetAllPoints>> mockLogger = new Mock<ILogger<ResetAllPoints>>();
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();

      ResetAllPoints sut = new ResetAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      Assert.IsType<ResetAllPoints>(sut);
    }

    [Fact]  
    public void Ctor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();

      Assert.Throws<ArgumentNullException>(() => new ResetAllPoints(null, mockSkillRequestValidator.Object));
    }

    [Fact]  
    public void Ctor_ShouldThrowArgumentNullException_WhenSkillRequestValidatorIsNull()
    {
      Mock<ILogger<ResetAllPoints>> mockLogger = new Mock<ILogger<ResetAllPoints>>();
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();

      Assert.Throws<ArgumentNullException>(() => new ResetAllPoints(mockLogger.Object, null));
    }

    [Fact]
    public void Handle_ShouldReturnSkillResponse_WhenCalled()
    {
      Mock<ILogger<ResetAllPoints>> mockLogger = new Mock<ILogger<ResetAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);
      
      ResetAllPoints sut = new ResetAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      Assert.IsType<ResetAllPoints>(sut);
    }

    [Fact]
    public void Handle_ShouldReturnSkillResponse_WhenCalledWithValidInputs()
    {
      Mock<ILogger<ResetAllPoints>> mockLogger = new Mock<ILogger<ResetAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);
      
      ResetAllPoints sut = new ResetAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      SkillRequest skillRequest = GenerateValidSkillRequest(new IntentRequest()
      { 
        RequestId ="TestRequestId", 
        Locale = "en-US", 
        Type = "IntentRequest", 
        Intent = new Intent() 
        { 
          ConfirmationStatus = "CONFIRMED",
          Name = "ResetAllPoints",
          Slots = new Dictionary<string, Slot>()
          {
            {
              "player", 
              new Slot()
              {
                Name = "player",
                Value = "blue",
                ConfirmationStatus = "NONE"
              }
            }
          }
        } 
      });

      TokenUser tokenUser = new TokenUser() { Players = new List<Player>() };

      SkillResponse skillResponse = sut.Handle(skillRequest, tokenUser);

      Assert.IsType<SkillResponse>(skillResponse);
    }

    [Fact]
    public void Handle_ShouldThrowArgumentNullException_WhenCalledWithInvalidSkillRequest()
    {
      Mock<ILogger<ResetAllPoints>> mockLogger = new Mock<ILogger<ResetAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(false);
      
      ResetAllPoints sut = new ResetAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      TokenUser tokenUser = new TokenUser() { Players = new List<Player>() };

      SkillRequest skillRequest = new SkillRequest();

      Assert.Throws<ArgumentNullException>(() => sut.Handle(skillRequest, tokenUser));
    }

    [Fact]
    public void Handle_ShouldThrowArgumentNullException_WhenTokenUserIsNull()
    {
      Mock<ILogger<ResetAllPoints>> mockLogger = new Mock<ILogger<ResetAllPoints>>();
      
      Mock<ISkillRequestValidator> mockSkillRequestValidator = new Mock<ISkillRequestValidator>();
      mockSkillRequestValidator.Setup(x => x.IsValid(It.IsAny<SkillRequest>())).Returns(true);
      
      ResetAllPoints sut = new ResetAllPoints(mockLogger.Object, mockSkillRequestValidator.Object);

      SkillRequest skillRequest = new SkillRequest();

      Assert.Throws<ArgumentNullException>(() => sut.Handle(skillRequest, null));
    }
  }
}