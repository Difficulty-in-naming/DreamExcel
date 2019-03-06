# DreamExcel

在Excel中生成出数据库文件和脚本文件,工作人员只需要像平常一样操作Excel,不需要点击任何按钮.

Features
===
- 非常简单的使用(只需要简单的配置)
- 不需要任何代码
- 完全自动化
- 即时的反馈错误
- 智能提示,当输入函数时显示出相关的HelpDoc
- 光的速度(即使在表格很庞大的情况下)
- 支持跨表格链接数据导出
- 支持导出多种开发语言版本的代码
- 自定义导出语言内容
- 支持枚举,支持忽略导出(备注)
- 允许导出各种格式的数据文件(目前支持Sqlite 和 Json)

依赖库
===
- ScriptGenerate https://github.com/GaLeGayGay/ScriptGenerate

Environment
===
- 请使用Excel 2003以上的版本
- 使用VS2013/VS2015/2017进行编译

Get Started(如果你使用的是Release版本则可以忽略前四步)
===
- Clone Git 仓库
- 手动添加克隆项目下的ScriptGenerate.dll引用
- 编译脚本
- 打开生成目录下的TestExcel_Auto.xlsx或者新建一个Excel文件
- 如果提示找不到sqlite3.dll的话,请复制sqlite3.dll到Windows目录下(因为sqlite3.dll是动态链接库,add-in没办法找到他)或者Excel运行目录下,sqlite可以到www.sqlite.org上面进行下载
- 按下快捷键Alt+t,i打开面板选择浏览加入生成目录下的两个.xll文件
- 开始愉快的使用吧

How To Use
===
- 第一行填写策划的备注信息
- 第二行填写脚本变量名称
- 第三行填写脚本类型(目前仅支持操作11种类型 int,float,string,bool,int[],float[],string[],bool[],long,long[],enum)
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
如果你定义的类型为自定义类型.你可以选中该列下方的任意一处单元格并按下快捷键Ctrl + Shift + E进入Json编辑界面

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

导出不同的脚本类型
===
你可以下载源代码然后在DreamExcel程序集中建立一个脚本,脚本内包含public static Action<Generate.Info, Serializer> Defalut(List<GenerateConfigTemplate> customClass, GenerateConfigTemplate core)这样的方法,且标记有[UseGenerate]标记后就可以了

导出各种类型的数据
===
在Config中修改GeneratorType的属性去生成不同的类型的数据文件

正在计划
===
- 使用字节流导出sqlite
