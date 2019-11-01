using Alexa.NET.Response;

namespace StarWarsPuns.Core
{
  public static class Extensions
  {
    public static SkillResponse Tell(this string phrase, bool shouldEndSession)
    {
      SsmlOutputSpeech speech = new SsmlOutputSpeech();
      speech.Ssml = string.Format("<speak>{0}</speak>", phrase);

      ResponseBody responseBody = new ResponseBody();
      responseBody.OutputSpeech = speech;
      responseBody.ShouldEndSession = shouldEndSession;

      SkillResponse skillResponse = new SkillResponse();
      skillResponse.Response = responseBody;
      skillResponse.Version = "1.0";

      return skillResponse;
    }

    public static SkillResponse TellWithCard(this string phrase, ICard card)
    {
      PlainTextOutputSpeech plainText = new PlainTextOutputSpeech();
      plainText.Text = string.Format("{0}", phrase);

      ResponseBody responseBody = new ResponseBody();
      responseBody.OutputSpeech = plainText;
      responseBody.ShouldEndSession = null;
      responseBody.Card = card;

      SkillResponse skillResponse = new SkillResponse();
      skillResponse.Response = responseBody;
      skillResponse.Version = "1.0";

      return skillResponse;
    }
  }
}