using System;
using System.Collections.Generic;
using System.Linq;
using StarWarsPuns.Models;
using Xunit;

namespace StarWarsPuns.Tests.Models
{
  public class TokenUserTests
  {
    [Fact]
    public void Constructor_ReturnsInstanceOfClass()
    {
      string expectedId = "TestId";
      DateTime expectedCreateDate = DateTime.Now;
      DateTime expectedUpdateDate = DateTime.Now;
      string expectedPasswordHash = "TestPasswordHash";
      bool expectedHasPointsPersistence = false;
      long expectedTTL = 123456789;
      List<Player> expectedPlayers = new List<Player>() { new Player() { Name = "TestPlayerName", Points = 3 } };

      TokenUser tokenUser = new TokenUser();
      tokenUser.Id = expectedId;
      tokenUser.CreateDate = expectedCreateDate;
      tokenUser.UpdateDate = expectedUpdateDate;
      tokenUser.PasswordHash = expectedPasswordHash;
      tokenUser.HasPointsPersistence = expectedHasPointsPersistence;
      tokenUser.TTL = expectedTTL;
      tokenUser.Players = expectedPlayers;

      Assert.IsType<TokenUser>(tokenUser);
      Assert.Equal(tokenUser.Id, expectedId);
      Assert.Equal(tokenUser.CreateDate, expectedCreateDate);
      Assert.Equal(tokenUser.UpdateDate, expectedUpdateDate);
      Assert.Equal(tokenUser.PasswordHash, expectedPasswordHash);
      Assert.Equal(tokenUser.HasPointsPersistence, expectedHasPointsPersistence);
      Assert.Equal(tokenUser.TTL, expectedTTL);
      Assert.True(tokenUser.Players.SequenceEqual(expectedPlayers));
    }
  }
}