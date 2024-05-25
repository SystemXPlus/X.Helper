# X.Helper.Common

## ConfigHelper

- web.config or app.config
```
X.Helper.Common.ConfigHelper.GetAppsetting("key");
X.Helper.Common.ConfigHelper.GetAppsetting("key", "defaultValue");
```
- appsettings.json
```
X.Helper.Common.ConfigHelper.GetAppsetting("root:node");
X.Helper.Common.ConfigHelper.GetAppsetting("root:node", "defaultValue");

//Get a setting model with defaultvalue
var settingModel = new SettingModel { Value = DefaultValue };
X.Helper.Common.ConfigHelper.GetAppsetting<SettingModel>("root:node", settingModel);

//Get a setting model in return value
var settingModel = X.Helper.Common.ConfigHelper.GetAppsetting<SettingModel>("root:node", new SettingModel());
var settingModel = X.Helper.Common.ConfigHelper.GetAppsetting<SettingModel>("root:node", new SettingModel{ Value = DefaultValue });
```