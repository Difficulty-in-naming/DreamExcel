[![Readme](https://img.shields.io/badge/DreamExcel-English-red)](https://github.com/Difficulty-in-naming/DreamExcel/blob/master/README.md)


# CSV2Data

在CSV中生成出数据库文件和脚本文件,工作人员只需要像平常一样操作Excel,不需要点击任何按钮.

Features
===
- 非常简单的使用
- 不需要任何代码
- 完全自动化
- 即时的反馈错误
- 智能提示,当输入函数时显示出相关的HelpDoc(目前已移出该功能)
- 光的速度(即使在表格很庞大的情况下)
- ~~支持跨表格链接数据导出~~
- 支持导出多种开发语言版本的代码
- 自定义导出语言内容
- 支持枚举,支持忽略列内容
- 允许导出各种格式的数据文件(目前支持~~Sqlite~~ 和 Json)

依赖库
===
- ScriptGenerate https://github.com/pk27602017/ScriptGenerate

Get Started(如果你使用的是Release版本则可以忽略前四步)
===
- Clone Git 仓库
- 手动添加克隆项目下的ScriptGenerate.dll引用
- 运行项目
- 将生成目录下的Config.txt和GenerateTemplate.txt拷贝到Excel文件目录
- 如果提示找不到sqlite3.dll的话,请复制sqlite3.dll到Windows目录下(因为sqlite3.dll是动态链接库,add-in没办法找到他)或者Excel运行目录下,sqlite可以到www.sqlite.org上面进行下载
- 开始愉快的使用吧

How To Use
===
- 第一行填写策划的备注信息
- 第二行填写脚本变量名称
- 第三行填写脚本类型
- 之后开始填写数据

如何在Excel中使用自定义类型
===
在第三行脚本类型中你可以这样定义**Id[int];Name[string]**
<br />
![index](https://github.com/pk27602017/Excel2Sqlite/raw/master/Image/自定义类型.png)
<br />
<br />
如果你希望定义自定义类型数组你可以使用 **{Id[int];Name[string}** 来进行定义
<br />
![index](https://github.com/pk27602017/Excel2Sqlite/raw/master/Image/自定义类型数组.png)

基础配置
===
在克隆下来的项目里包含了一个Config.txt和GenerateTemplate文件你可以复制到你的Excel文件夹相同目录中.这样导出的时候就会使用Excel文件夹下面的配置文件,默认使用项目生成的配置文件,你可以在里面修改相关的参数达到修改导出内容的目的

修改代码生成结果
===
你可以修改CodeGenerate脚本中的Default函数.

代码生成
===
在克隆下来的项目里包含了一个GenerateTemplate.txt文件,请不要删除里面所有的 **{@XXXX}** 这样的关键字

如何填写自定义类型的数据
===
从第四行开始填写数据你可以先输入=UnityJson()生成出Json配置,然后将内容逐一填写即可
![index](https://github.com/pk27602017/Excel2Sqlite/raw/master/Image/智能提示.png)

如何过滤某一列的数据
===
当我们在填写表格的时候通常需要有一列作为备注用的数据或其他你不需要的数据,你可以在第二行也就是变量名那一行,在变量名前面加一个*号就会被过滤,数据将不会存储在Sqlite当中

如何导出枚举类型
===
你可以在类型那一栏中输入enum并且右键添加备注按以下格式填写   
1:装备[Equip]   
2:物品[Item]   
3:属性石[Rune]   
方括号内为导出枚举的变量名称

导出各种类型的数据
===
在Config中修改GeneratorType的属性去生成不同的类型的数据文件

Unity中使用MemoryPack
===
1.修改Build.cmd中的Unity路径/或者自己导入Unity然后将项目导出
2.修改Config的导出类型

FAQ
===
Q：我的CSV使用的是空格作为分隔符.使用这个项目可以正常导出数据文件吗
A：可以,你只需要修改Config.txt中的CSVDelimiters属性的格式为空格就可以了