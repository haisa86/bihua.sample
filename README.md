# 说明
基于 https://github.com/haisa86/bihua.db 库的简单应用例子

包含一个WebApi工程，和两个客户端应用例子

1. Bihua.WebApi:使用.NetCore 2.1构建的WebApi工程，运行前需要先部署好 https://github.com/haisa86/bihua.db 中的mongodb

2. Bihua.Client.WPF:使用.Net Framework4.5构建的WPF工程，调用Bihua.WebApi工程提供的api
![image](https://github.com/haisa86/bihua.sample/blob/master/Bihua.Client.WPF/preview.gif)

2. Bihua.Client.Html:使用html+js构建的客户端，调用Bihua.WebApi工程提供的api
![image](https://github.com/haisa86/bihua.sample/blob/master/Bihua.Client.Html/preview.gif)
