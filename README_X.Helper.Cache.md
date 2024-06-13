# X.Helper.Cache

### Special Explanation

if you are plainning to need ServiceStackRedisHelper, you need to review following description
In .NetFramework 4.6.1 referenced ServiceStack.Redis Version is 3.9.71 (the last free version)
In .Net Standard 2.0/2.1 or .Net 6.0 referenced ServiceStack.Redis Version is 8.2.2.
You may need to purchase a license for this version.
Please go to https://servicestack.net for more details.


### Config File

#### App.Config
```
	<appSettings>
		<add key="RedisServer" value="10.0.0.254"/>	<!--SERVER IP ADDRESS-->
		<add key="RedisPort" value="6379"/>		<!--SERVER PORT-->
		<add key="RedisPassword" value=""/>		<!--SERVER PASSWORD-->
	</appSettings>
```

#### appsettings.json
```
  "cache": {
    "redisserver": "10.0.0.254",	//SERVER IP ADDRESS
    "redisport": "6379",		//SERVER PORT
    "redispassword": ""			//ERVER PASSWORD
  }
```

### General Configration

```
Config.DefaultTimeout = 20		//Minute
Config.DefaultDatabaseIndex = 0		//0-15
```

### User ServiceExchange.Redis
```
//default database index uses the value from Config.DefaultDatabaseIndex 
ICachaHelper helper = new X.Helper.Cache.ServiceStackRedisHelper()
//use the specified database index
ICachaHelper helper = new X.Helper.Cache.ServiceStackRedisHelper(dbIndex)
```

### Use ServiceStack.Redis
```
//default database index uses the value from Config.DefaultDatabaseIndex 
ICacheHelper helper = new X.Helper.Cache.StackExchangeRedisHelper()
//use the specified database index
ICachaHelper helper = new X.Helper.Cache.StackExchangeRedisHelper(dbIndex)
```


### Switch database index
```
helper = helper.GetDatabase(dbIndex)
```

