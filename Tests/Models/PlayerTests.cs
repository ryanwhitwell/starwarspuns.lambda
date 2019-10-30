using System;
using StarWarsPuns.Models;
using Xunit;

namespace StarWarsPuns.Tests.Models
{
  public class PlayerTests
  {
    [Fact]
    public void Constructor_GeneratesInstanceOfClass()
    {
      string expectedName = "TestUser";
      int expectedPoints = 3;

      Player player = new Player();
      player.Name = expectedName;
      player.Points = expectedPoints;

      Assert.IsType<Player>(player);
      Assert.Equal(expectedName, player.Name);
      Assert.Equal(expectedPoints, player.Points);
    }
  }
}
