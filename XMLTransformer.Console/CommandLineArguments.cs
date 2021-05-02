using System.Collections.Generic;
using CommandLine;

namespace XMLTransformer.Console
{
    public class CommandLineArguments
    {
        [Option('i', "input", Required = true, HelpText = "The input XML file")]
        public string InputFile { get; set; }

        [Option('t', "transform", Required = true, Separator = ',', HelpText = "A collection of XML transform files to apply on the input file")]
        public IEnumerable<string> TransformFiles { get; set; }
    }
}