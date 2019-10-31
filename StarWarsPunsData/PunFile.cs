using System;
using System.Collections.Generic;
using System.IO;
using StarWarsPuns.Models;

namespace StarWarsPunData.Main
{
  public class PunFile
  {
    private string filePath;
    
    public PunFile(string filePath)
    {
      if (string.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException("filePath");
      }
      
      this.filePath = filePath;
    }
    
    public IEnumerable<StarWarsPun> GetPuns()
    {
      List<StarWarsPun> puns = new List<StarWarsPun>();

      int counter = 0;  
      string question;
      StreamReader file = new StreamReader(this.filePath);  

      while((question = file.ReadLine()) != null)  
      {  
        string answer = file.ReadLine();

        if (answer != null)
        {
          StarWarsPun pun = new StarWarsPun(counter, question, answer);
          puns.Add(pun);
        }

        counter++;
        
        Console.WriteLine("Q: {0}\nA: {1}", question, answer);
      }  
        
      file.Close();  
      System.Console.WriteLine("There were {0} puns.", counter);  

      return puns;
    }
  }
}