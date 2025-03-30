- Client使用链式编程配置参数
- - ~~新增CustomClient在创建代理时使用新实例化的对象（需要测试频繁实例化异常的情况）~~
- Handler使用链式编程配置参数
- 提交数据类型：文本对象、文件、文本对象文件混合
- 区分文本方式和二进制方式提交
- 返回数据类型：文本对象、文件、文本对象文件混合
- 返回图片？？？

## COOKIEHELPER
- ~~COOKIEHELPER中增加public CookieCollection GetCookieDictionary(string cookies)~~

## 请求参数处理
- ~~HTTPVERSION 默认1.1 请求HTTPS自动设置为1.0？~~
- ~~20250327 Client增加将HttpResponseMessage中的Content读取到Result的StreamContent（MemoryStream）中的方法
后续所有内容都在StreamContent基础上操作，最后再根据CONTENT-TYPE进行处理~~
- 20250330 Client Result中的StreamContent移至Client中，RESULT中只保留输出正文、文件等内容，StreamContent由Client进行处理
- 请求HEADER处理
- 请求COOKIE处理
- 请求携带证书处理

## 请求返回结果处理
- ~~返回COOKIE处理~~
- ~~返回HEADER处理~~
- 读取RESPONSE时先读取到STREAM中，再根据CONTENT-TYPE进行处理？
- 获取返回结果编码，根据编码进行处理？
- 获取二进制字节，考虑gzip压缩？