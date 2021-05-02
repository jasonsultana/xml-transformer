# xml-transformer
A cross platform program that can apply transform files to XML configuration without doing a build or publish.

G'day guys!

As the name of the repo suggests, this is an XML Transformer tool. There are 2 runnable projects.

## GUI

The GUI version of this project is probably ideal for those who aren't comfortable with a terminal, or are working primarily within one directory. The GUI also supports a preview option, and reverting, which is not supported in the CLI.

### Usage

1. Select a source file (your original XML)
2. Select up to 2 XML transform files.
3. Preview or apply the transform files.

#### Preview
Preview will save the resulting _transformed_ XML to a temporary xml file on your local filesystem and open it in your system default XML viewer.

#### Transform
This will _apply_ the transform XML files and overwrite the original source file. The changes can be undone by clicking Revert.



## CLI

The CLI option was born out of a frustration I had, navigating between different folders using the GUI.

### Usage

The command line transform program requires at least two arguments; an input file (specified with the `i` parameter name) and at least one transform file (denoted by `t`). An unlimited number of transform files can be run sequentially. Just include a space between transform file paths, or a comma.
```
 ./XMLTransformer.Console -i "../../../../Samples/sample.config" -t "../../../../Samples/sample.transform.config" "../../../../Samples/sample.transform-2.config"
```
Unlike the GUI, there is no preview or revert option.

## Misc
### Why multiple transform files?

If you're working with .NET Framework (which I assume you are, if you're reading this), you'll likely have a source config file, and various XML transform files
per region, per environment. This tool was originally made to provide a convenient way to apply those transformations locally, instead of having to do a 
build or publish.

However, I soon found that I often needed to make a small tweak on the resulting transformed XML. Eg: using a different data source or security credential for local
debugging. To make this easier, you can store all of your local _tweaks_ in a non-source controlled transform file, and select that file as the 2nd transform file
to use in this tool.

Happy to receive suggestions or feedback, though this tool was mainly made for personal use (and for use by my peers) so it's not meant to be robust enough to 
cover everyone's use cases. Anyways, happy coding and catch ya later!
