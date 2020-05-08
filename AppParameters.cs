using CommandLine;

namespace MergeJson
{
  public class AppParameters
  {
    [Option('f', "json-file-name", Required = true)]
    public string JsonFileName { get; set; }

    [Option('p', "path", Required = true)]
    public string JsonPath { get; set; }

    [Option('v', "value", Required = true)]
    public string JsonValue { get; set; }
  }
}
