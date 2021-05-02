using System;
using System.IO;
using System.Linq;
using System.Net;
using CommandLine;
using XMLTransformer.Shared;

namespace XMLTransformer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (!args.Any())
                    throw new ArgumentException("Arguments not specified. --help for supported arguments.");
                
                Parser.Default.ParseArguments<CommandLineArguments>(args)
                    .WithParsed((args) =>
                    {
                        if (string.IsNullOrWhiteSpace(args.InputFile))
                            throw new ArgumentException(nameof(args.InputFile) + " must not be empty.");

                        if (!args.TransformFiles.Any())
                            throw new ArgumentException("At least one transform file must be provided.");

                        var sourceXml = File.ReadAllText(args.InputFile);
                        foreach (var transformFile in args.TransformFiles)
                        {
                            var transformXml = File.ReadAllText(transformFile);
                            sourceXml = new XmlTransformService().Transform(sourceXml, transformXml);
                        }
                        
                        File.WriteAllText(args.InputFile, sourceXml);
                    });
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.ToString());
            }
        }
    }
}