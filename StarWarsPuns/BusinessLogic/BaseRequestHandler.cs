using System;
using Microsoft.Extensions.Logging;

namespace StarWarsPuns.BusinessLogic
{
  public abstract class BaseRequestHandler<T>
  {
    private ILogger<T> _logger;

    public BaseRequestHandler(ILogger<T> logger)
    {
      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }

      _logger = logger;
    }

    public ILogger<T> logger { get { return _logger; } }
  }
}