[![Readme_CN](https://img.shields.io/badge/DreamExcel-%E4%B8%AD%E6%96%87%E6%96%87%E6%A1%A3-red)](https://github.com/Difficulty-in-naming/DreamExcel/blob/master/README_CN.md)


# CSV2Data

Generate database files and script files from CSV. Staff members only need to operate Excel as usual without clicking any button.

Features
===
- Very easy to use
- No code required
- Completely automated
- Instant error feedback
- Smart prompts when entering functions to display related HelpDoc (currently removed)
- Speed of light (even when the table is large)
- ~~Support for cross-table data export~~
- Support for exporting code in multiple development languages
- Customizable language content for export
- Support for enumerations, support for ignoring column content
- Allows exporting data files in various formats (currently supports ~~Sqlite~~ and Json)

Dependencies
===
- ScriptGenerate https://github.com/pk27602017/ScriptGenerate

Get Started (You can ignore the first four steps if you are using the Release version)
===
- Clone the Git repository
- Manually add the reference of ScriptGenerate.dll under the cloned project
- Run the project
- Copy Config.txt and GenerateTemplate.txt from the generated directory to the Excel file directory
- If you are prompted that sqlite3.dll is not found, please copy sqlite3.dll to the Windows directory (because sqlite3.dll is a dynamic link library, the add-in can't find it) or Excel running directory. You can download sqlite from www.sqlite.org
- Start using happily

How To Use
===
- Fill in the planner's remarks information in the first line
- Fill in the script variable name in the second line
- Fill in the script type in the third line
- Start filling in data afterwards

How to use custom types in Excel
===
In the third row of the script type, you can define it like this **Id[int];Name[string]**
<br />
![index](https://github.com/pk27602017/Excel2Sqlite/raw/master/Image/自定义类型.png)
<br />
<br />
If you want to define an array of custom types, you can define it using **{Id[int];Name[string}**
<br />
![index](https://github.com/pk27602017/Excel2Sqlite/raw/master/Image/自定义类型数组.png)

Basic Configuration
===
The cloned project contains a Config.txt and GenerateTemplate file which you can copy to the same directory as your Excel file. When exporting, it will use the configuration file in the Excel file folder by default, and use the project generated configuration file by default. You can modify the related parameters inside to modify the export content.

Modifying Code Generation Results
===
You can modify the Default function in the CodeGenerate script.

Code Generation
===
The cloned project contains a GenerateTemplate.txt file. Please do not delete all the **{@XXXX}** keywords inside.

How to Fill in Custom Type Data
===
From the fourth line, start filling in the data. You can first input =UnityJson() to generate the Json configuration, and then fill in the content one by one.
![index](https://github.com/pk27602017/Excel2Sqlite/raw/master/Image/智能提示.png)

How to Filter Data in a Column
===
When filling in the table, there is usually a column for remarks or other data you don't need. You can add an asterisk before the variable name in the second row, which is the variable name row, and it will be filtered out, and the data will not be stored in Sqlite.

How to Export Enumerations
===
You can input "enum" in the type column and right-click to add remarks according to the following format
1:Equipment[Equip]
2:Item[Item]
3:Rune[Rune]
The content in the square brackets is the variable name of the exported enumeration.

Exporting Different Types of Data
===
Modify the GeneratorType property in Config to generate different types of data files.

Using MemoryPack in Unity
===
1. Modify the Unity path in Build.cmd/or import Unity yourself and then export the project
2. Modify the export type in Config

FAQ
===
Q: My CSV uses a space as a delimiter. Can this project normally export data files?
A: Yes, you only need to modify the format of the CSVDelimiters property in Config.txt to a space.
