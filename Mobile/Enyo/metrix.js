enyo.kind({
	name: "Metrix",
	kind: enyo.Component,
	components: [
		{name: "sendDeviceInfo", kind: "WebService", url: "http://metrix.webosroundup.com/MetrixInterface.asmx/DeviceDataPostV2", onSuccess: "deviceInfoSuccess", onFailure: "deviceInfoFailure"},
		{name: "sendCustomCounts", kind: "WebService", url: "http://metrix.webosroundup.com/MetrixInterface.asmx/UpdateCustomCounts", onSuccess: "customCountsSuccess", onFailure: "customCountsFailure"},
		{name: "sendBulletinBoardCheck", kind: "WebService", onSuccess: "checkBulletinBoardSuccess", onFailure: "checkBulletinBoardFailure"},
		{name: "getNUID", kind: "PalmService", service: "palm://com.palm.preferences/systemProperties", method: "Get", params: {"key": "com.palm.properties.nduid" }, onSuccess: "gotNUIDsuccess", onFailure: "gotNUIDfailed"},
		{name: "bulletinBoard", kind: "Dialog", autoClose:false, dismissWithEscape: false, dismissWithClick:false, components: [
			{name: "titleBar", allowHtml: true, content: "", style: "padding-left: 12px;"},
			{name: "messageContainer", kind: "HFlexBox", components: [
				{name: "messageScroller", kind: "Scroller", flex: 1, style: "height: 350px;", components: [
					{name: "message", kind: "HtmlContent", onLinkClick: "htmlContentLinkClick", style: "padding-left: 12px;"}
				]}
			]},
			{name: "navBar",kind: "Toolbar", components: [
				{caption: "Snooze", onclick: "snoozeDialog"},
				{kind: "Spacer"},
				{name: "goBck", kind: "ToolButton", icon: "source/libraries/Metrix/images/menu-icon-back.png", onclick: "goBckTap"},
				{name: "pages", kind: "Control", content: "0 of 0", style:"color: #FFFFFF;"},
				{name: "goFwd", kind: "ToolButton", icon: "source/libraries/Metrix/images/menu-icon-forward.png", onclick: "goFwdTap"},
				{kind: "Spacer"},
				{caption: "Close", onclick: "dialogDone"}
			]}
		]}
	],
	constructor: function (appInfoPath)
	{
		this.inherited(arguments);

		this.creationTimestamp = null;
		this.lastUpdateTimestamp = null;
		this.lastBulletinTime = null;
		this.bulletinVersion = 0;
		this.bulletinData = null;
		this.bulletinIndex = 0;

		this.cookieName = "metrixCookie";
		this.metrixVersion = "0.5.3";
		
		if(appInfoPath)
		{			
			this.appInfoPath = appInfoPath
			
			if(this.appInfoPath.indexOf("appinfo.json") != -1)
			{
				if(this.appInfoPath.charAt(this.appInfoPath.length - 1) != "/")
				{
					this.appInfoPath += "/";
				}
				
				this.appInfoPath += "appinfo.json";
			}
		}
		else
		{
			this.AppInfoPath = enyo.fetchAppRootPath() + "../appinfo.json";
		}

		this.initCookie();
	},
	postDeviceData: function ()
	{
		this.$.getNUID.call();
	},
	gotNUIDsuccess: function (f, result)
	{
		var nuid = result["com.palm.properties.nduid"];
		var deviceInfo = enyo.fetchDeviceInfo();
		//var appInfo = enyo.fetchAppInfo();
		var appInfo = enyo.fetchConfigFile(this.appInfoPath);
		var locale = enyo.g11n.currentLocale();
		var params = null; 

		if(deviceInfo === undefined || deviceInfo === null)
		{
			params = {
				deviceId: nuid,
				companyName: appInfo.vendor,
				packageId: appInfo.id,
				appVersion: appInfo.version,
				resolution: screen.width + " x " + screen.height,
				webOsBuildNumber: "N/A",
				webOsVersion: "3.X.X",
				carrier: "ROW",
				deviceName: "Browser",
				locale: locale.locale
			};
		}
		else
		{
			params = {
				deviceId: nuid,
				companyName: appInfo.vendor,
				packageId: appInfo.id,
				appVersion: appInfo.version,
				resolution: deviceInfo.screenWidth + " x " + deviceInfo.screenHeight,
				webOsBuildNumber: "Unavailable",
				webOsVersion: deviceInfo.platformVersion,
				carrier: deviceInfo.carrierName,
				deviceName: deviceInfo.modelName,
				locale: locale.locale
			};
		}

		this.$.sendDeviceInfo.call(params);
	},
	gotNUIDfailed: function (f, error)
	{
		enyo.warn(error);
	},
	deviceInfoSuccess: function (f, response, transport)
	{
		if(transport.xhr.status !== 200)
		{
			enyo.warn("deviceInfoSucces received error: ", transport.xhr.status);
			return;
		}

		var temp = transport.xhr.responseXML.getElementsByTagName("creationTimestamp");

		if(temp.length > 0)
		{
			this.creationTimestamp = temp.item(0).textContent;
		}
		else
		{
			return;
		}

		temp = transport.xhr.responseXML.getElementsByTagName("lastUpdateTimestamp");
		if(temp.length > 0)
		{
			this.lastUpdateTimestamp = temp.item(0).textContent;
		}
		else
		{
			return;
		}

		this.storeCookie();
	},
	deviceInfoFailure: function (f, response, transport)
	{
		enyo.warn("Metrix Response Failed", transport.xhr.status);
		enyo.warn("Metrix Error=", response);
	},
	isExpired: function (daysAllowed)
	{
		//Date.UTC(year,month,day,hours,minutes,seconds,ms)
		var currentDate = new Date();
		var currentUtcTime = Date.UTC(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate()) / 10000;

		var daysUtc = daysAllowed * 86400;

		var result = false;

		if(this.creationTimestamp !== null)
		{
			if((currentUtcTime - this.creationTimestamp) > daysUtc)
			{
				result = true;
			}
		}
		else //this.creationTimestamp === null
		{
			//some reason we don't have a creationTime use the time passed in and store it. Once the post happens the real time will over write this one
			this.creationTimestamp = currentUtcTime;
			this.storeCookie();
		}

		return result;
	},
	initCookie: function ()
	{
		var cookieJar = enyo.getCookie(this.cookieName);

		if(cookieJar === undefined || cookieJar === null)
		{
			this.creationTimestamp = null;
			this.lastUpdateTimestamp = null;
			this.lastBulletinTime = null;
			this.bulletinVersion = 0;
		}
		else
		{
			var oldMetrixCookie = enyo.json.parse(cookieJar);

			if(oldMetrixCookie.metrixVersion === this.metrixVersion)
			{
				this.creationTimestamp = oldMetrixCookie.creationTimestamp;
				this.lastUpdateTimestamp = oldMetrixCookie.lastUpdateTimestamp;
				this.lastBulletinTime = oldMetrixCookie.lastBulletinTime;
				this.bulletinVersion = oldMetrixCookie.bulletinVersion;
			}
			else
			{
				this.creationTimestamp = oldMetrixCookie.creationTimestamp;
				this.lastUpdateTimestamp = oldMetrixCookie.lastUpdateTimestamp;
				this.lastBulletinTime = oldMetrixCookie.lastBulletinTime;
				this.bulletinVersion = oldMetrixCookie.bulletinVersion;
			}
		}

		this.storeCookie();
	},
	storeCookie: function ()
	{
		var cookie = {};

		cookie.creationTimestamp = this.creationTimestamp;
		cookie.lastUpdateTimestamp = this.lastUpdateTimestamp;
		cookie.lastBulletinTime = this.lastBulletinTime;
		cookie.bulletinVersion = this.bulletinVersion;
		cookie.metrixVersion = this.metrixVersion;

		enyo.setCookie(this.cookieName, enyo.json.stringify(cookie));
	},
	customCounts: function (valueGroup, valueName, valueData)
	{
		var result = 0;

		if(!valueGroup || !valueName || !valueData || isNaN(valueData))
		{
			result = -1;
		}
		else
		{
			var appInfo = enyo.fetchAppInfo();

			var params = {
				packageId: appInfo.id, 
				valueGroup: valueGroup, 
				valueName: valueName, 
				valueData: valueData
			};

			this.$.sendCustomCounts.call(params);
		}

		return result;
	},
	customCountsSuccess: function ()
	{

	},
	customCountsFailure: function ()
	{

	},
	checkBulletinBoard: function (filter, forceReview)
	{
		var d = new Date();
		var utc = Date.UTC(d.getFullYear(), d.getMonth(), d.getDate(),d.getHours(),d.getMinutes()) / 10000;

		if(filter)
		{    
			if(filter > this.bulletinVersion)
			{
				this.bulletinVersion = minBulletinVersion;
			}
		}
		
		if(forceReview)
		{
			filter = 0;
		}

		//we only check the bulletin board every 12 hours
		if(utc > (this.lastBulletinTime + 43200) || forceReview)
		{
			var appInfo = enyo.fetchAppInfo();

			var url = "http://metrix.webosroundup.com/MetrixInterface.asmx/GetBulletinBoard?packageId=" + appInfo.id; 
			
			this.$.sendBulletinBoardCheck.setUrl(url); 
			this.$.sendBulletinBoardCheck.call();
		}

	},
	checkBulletinBoardSuccess: function (f, response, transport)
	{
		if(transport.xhr.status !== 200)
		{
			enyo.warn("checkBulletinBoardSuccess received error: ", transport.xhr.status);
		}
		else
		{
			var version = transport.xhr.responseXML.getElementsByTagName("version").item(0).textContent;
			var msgArray = [];

			if(version > this.bulletinVersion)
			{        
				var bulletins = transport.xhr.responseXML.getElementsByTagName("announcement");

				for(var i = 0; i < bulletins.length; i++)
				{
					msgArray.push({title: bulletins[i].getElementsByTagName("title").item(0).textContent, text: bulletins[i].getElementsByTagName("message").item(0).textContent});
				}

				if(msgArray.length > 0)
				{
					this.bulletinData = msgArray;

					this.showBulletins();
				}
			}
		}	  
	},
	checkBulletinBoardFailure: function (f, response, transport)
	{

	},
	showBulletins: function()
	{		
		this.$.bulletinBoard.open();
		this.renderBulletin();
	},
	renderBulletin: function(direction)
	{
		if(direction == "forward")
		{
			this.bulletinIndex++;
		}
		else if(direction == "backward")
		{
			this.bulletinIndex--;
		}

		if(this.bulletinIndex < 1)
		{
			this.bulletinIndex = 0;
			this.$.goBck.hide();

			if(this.bulletinData.length > 1)
			{
				this.$.goFwd.show();
			}
		}
		else if(this.bulletinIndex > (this.bulletinData.length - 2))
		{
			this.bulletinIndex = (this.bulletinData.length -1);

			this.$.goBck.show();
			this.$.goFwd.hide();
		}
		else
		{
			this.$.goBck.show();
			this.$.goFwd.show();
		}

		this.$.titleBar.setContent("<strong>" + this.bulletinData[this.bulletinIndex].title + "</strong>");
		this.$.message.setContent(this.bulletinData[this.bulletinIndex].text);
		var page = this.bulletinIndex+1 + " of " + this.bulletinData.length;
		this.$.pages.setContent(page);
	},
	goFwdTap: function ()
	{
		this.renderBulletin("forward");
	},
	goBckTap: function ()
	{
		this.renderBulletin("backward");
	},
	dialogDone: function ()
	{
		this.bulletinIndex = 0;

		var d = new Date();
		var utc = Date.UTC(d.getFullYear(), d.getMonth(), d.getDate(),d.getHours(),d.getMinutes()) / 10000; 

		this.lastBulletinTime = utc;	

		this.storeCookie();
		this.$.bulletinBoard.close();
	},
	snoozeDialog: function ()
	{
		this.bulletinIndex = 0;

		var d = new Date();
		var utc = Date.UTC(d.getFullYear(), d.getMonth(), d.getDate(),d.getHours(),d.getMinutes()) / 10000; 

		this.lastBulletinTime = utc - 43200;	//simply set it back 12 hours to rerun the next time.

		this.storeCookie();
		this.$.bulletinBoard.close();
	},
	htmlContentLinkClick: function(inSender, inUrl) 
	{
		window.open(inUrl);
  }
	getXmlContent: function (element, defaultValue) 
	{
		defaultValue = (defaultValue === null || defaultValue === undefined) ? "" : defaultValue;

		if(element.length > 0 && element.item(0).textContent.length > 0)
		{
			return element.item(0).textContent;
		}

		return defaultValue;
	}
});
