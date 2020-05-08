using System;
using System.IO;
using System.Linq;
using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MergeJson
{
  internal static class Program
  {
    private static int Main(string[] args)
    {
      try
      {
        return Parser.Default.ParseArguments<AppParameters>(args)
          .MapResult(Do, errs => 1);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return 2;
      }
    }

    private static int Do(AppParameters parameters)
    {
      FileInfo file = new FileInfo(parameters.JsonFileName);
      if (!file.Exists)
        throw new Exception($"File {parameters.JsonFileName} doesn't exists.");

      dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(parameters.JsonFileName));
      if (json == null)
        throw new Exception("Could not parse JSON file.");
      
      string[] chunks = parameters.JsonPath.Split('/');

      JToken target = chunks.Aggregate<string, JToken>(null, (current, chunk) => current == null ? json[chunk] : current[chunk]);

      if (target == null)
        throw new Exception($"Could not navigate to JSON path {parameters.JsonPath}");

      ((JValue)target).Value = parameters.JsonValue;
      
      File.WriteAllText(parameters.JsonFileName, JsonConvert.SerializeObject(json, Formatting.Indented));

      return 0;
    }
  }
}