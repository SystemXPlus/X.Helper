# X.Helper.Common

## ConfigHelper

-	web.config or app.config
```
X.Helper.Common.ConfigHelper.GetAppsetting("key");
X.Helper.Common.ConfigHelper.GetAppsetting("key", "defaultValue");
```
- appsettings.json
```
X.Helper.Common.ConfigHelper.GetAppsetting("root:node");
X.Helper.Common.ConfigHelper.GetAppsetting("root:node", "defaultValue");
```