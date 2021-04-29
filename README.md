# xml-transformer
A cross platform desktop UI that can apply transform files to XML configuration

G'day guys!

As the name of the repo suggests, this is an XML Transformer tool. It's a desktop app written in C# / Avalonia UI in .NET 5, which makes it cross platform. 
You will need to have the .NET 5 SDK and a compiler available though. This means typically either VS 2019 or Rider.

## Usage

1. Select a source file (your original XML)
2. Select up to 2 XML transform files.
3. Preview or apply the transform files.

### Preview
Preview will save the resulting _transformed_ XML to a temporary xml file on your local filesystem and open it in your system default XML viewer.

### Transform
This will _apply_ the transform XML files and overwrite the original source file. The changes can be undone by clicking Revert.

### Why 2 transform files?

If you're working with .NET Framework (which I assume you are, if you're reading this), you'll likely have a source config file, and various XML transform files
per region, per environment. This tool was originally made to provide a convenient way to apply those transformations locally, instead of having to do a 
build or publish.

However, I soon found that I often needed to make a small tweak on the resulting transformed XML. Eg: using a different data source or security credential for local
debugging. To make this easier, you can store all of your local _tweaks_ in a non-source controlled transform file, and select that file as the 2nd transform file
to use in this tool.

Happy to receive suggestions or feedback, though this tool was mainly made for personal use (and for use by my peers) so it's not meant to be robust enough to 
cover everyone's use cases. Anyways, happy coding and catch ya later!
