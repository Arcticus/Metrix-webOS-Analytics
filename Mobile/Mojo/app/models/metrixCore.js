/*
Copyright (c) 2010 Syntactix, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

Metrix.prototype.coreInit = function()
{ 
  this.initializeCookie();
};

Metrix.prototype.postDeviceData = function(versionCtrl)
{  
  if(versionCtrl === true && this.lastAppRunVersion === Mojo.appInfo.version && this.lastWebosBuild === Mojo.Environment.build)
  {
    return;
  }
  
  if(!this.postDeviceDataOnce) //Only post this once per app session.
  {
  	this.postDeviceDataOnce = true;    
  }  
  else
  {
  	return;
  }
  
  if((Date.parse(new Date()) - 43200) > this.lastUpdateTimestamp) // only post once every 12 hours
  {
    var idRequest = this.ServiceRequest.request('palm://com.palm.preferences/systemProperties', {
                                                  method:"Get",
                                                  parameters:{"key": "com.palm.properties.nduid" },
                                                  onSuccess: this.gotDeviceId.bind(this)
                                              });
  }

  
};

Metrix.prototype.gotDeviceId = function(response)
{
  var deviceId = response["com.palm.properties.nduid"];
  var appId = Mojo.appInfo.id;
  var appVersion = Mojo.appInfo.version;
  var webOsBuildNumber = Mojo.Environment.build;
  var resolution = Mojo.Environment.DeviceInfo.screenWidth + " x " + Mojo.Environment.DeviceInfo.screenHeight;
  var deviceName = Mojo.Environment.DeviceInfo.modelNameAscii;
  var carrierName = Mojo.Environment.DeviceInfo.carrierName;
  var webOsVersion = Mojo.Environment.DeviceInfo.platformVersion;
  var companyName = Mojo.appInfo.vendor;
  var locale = Mojo.Locale.getCurrentLocale();
  
  var request = this.AjaxRequest.request("http://metrix.webosroundup.com/MetrixInterface.asmx/DeviceDataPostV2", {
                                          method: "get",
                                          evalJSON: "false",
                                          parameters: {deviceId: deviceId, companyName: companyName, packageId: appId, appVersion: appVersion, resolution: resolution, webOsBuildNumber: webOsBuildNumber, webOsVersion: webOsVersion, carrier: carrierName, deviceName: deviceName, locale: locale},
                                          onSuccess: this.postDeviceDataSuccess.bind(this),
                                          onFailure: this.postDeviceDataFailure,
                                          on0: this.postDeviceDataFailure
                                        });

};
  
Metrix.prototype.postDeviceDataSuccess = function(transport)
{
  var temp = transport.responseXML.getElementsByTagName("creationTimestamp");
  
  if(temp.length > 0)
  {
    this.creationTimestamp = temp.item(0).textContent;
  }
  else
  {
    return;
  }

  temp = transport.responseXML.getElementsByTagName("lastUpdateTimestamp");
  if(temp.length > 0)
  {
    this.lastUpdateTimestamp = temp.item(0).textContent;
  }
  else
  {
    return;
  }

  this.lastAppRunVersion = Mojo.appInfo.version;
  this.lastWebosBuild  = Mojo.Environment.build;

  this.storeCookie();
};

Metrix.prototype.postDeviceDataFailure = function(transport)
{
  Mojo.Log.warn("Metrix Response Failed",transport.status);
  Mojo.Log.warn("Metrix Error=",transport.responseText);
};

Metrix.prototype.initializeCookie = function()
{
  this.cookieData = new Mojo.Model.Cookie(this.cookieName);

  var oldMetrixCookie = this.cookieData.get();

  if(oldMetrixCookie)
  {      
    if(oldMetrixCookie.metrixVersion === this.metrixVersion)
    {
      this.creationTimestamp = oldMetrixCookie.creationTimestamp;
      this.lastUpdateTimestamp = oldMetrixCookie.lastUpdateTimestamp;
      this.lastAppRunVersion = oldMetrixCookie.lastAppRunVersion;
      this.lastWebOsBuild = oldMetrixCookie.lastWebOsBuild;
      this.lastBulletinTime = oldMetrixCookie.lastBulletinTime;
      this.bulletinVersion = oldMetrixCookie.bulletinVersion;
    }
    else
    {
      this.creationTimestamp = oldMetrixCookie.creationTimestamp;
      this.lastUpdateTimestamp = oldMetrixCookie.lastUpdateTimestamp;
      this.lastAppRunVersion = oldMetrixCookie.lastAppRunVersion;
      this.lastWebOsBuild = oldMetrixCookie.lastWebOsBuild;
      this.lastBulletinTime = oldMetrixCookie.lastBulletinTime;
      this.bulletinVersion = oldMetrixCookie.bulletinVersion;
    }
  }
  else
  {
    this.creationTimestamp = null;
    this.lastUpdateTimestamp = null;
    this.lastAppRunVersion = null;
    this.lastWebOsBuild = null;
    this.lastBulletinTime = null;
    this.bulletinVersion = 0;
  }

  this.storeCookie();
};

Metrix.prototype.storeCookie = function(lastBulletinTime,bulletinVersion)
{
  if(lastBulletinTime)
  {
    
    this.lastBulletinTime = lastBulletinTime;
  }
  
  if(bulletinVersion)
  {
    this.bulletinVersion = bulletinVersion;
  }
  
  this.cookieData.put({
												creationTimestamp: this.creationTimestamp,
												lastUpdateTimestamp: this.lastUpdateTimestamp,
												lastAppRunVersion: this.lastAppRunVersion,
												lastWebOsBuild: this.lastWebOsBuild,
												metrixVersion: this.metrixVersion,
												lastBulletinTime: this.lastBulletinTime,
												bulletinVersion: this.bulletinVersion
											}
										 );
};
  
Metrix.prototype.isExpired = function(currentUtcTime, daysAllowed)
{
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
};
  
Metrix.prototype.checkBulletinBoard = function(controller,minBulletinVersion, forceReview, url)
{
  if(minBulletinVersion)
  {    
    if(minBulletinVersion > this.bulletinVersion)
    {
      this.bulletinVersion = minBulletinVersion;
    }
  }
  
  if(!forceReview)
  {
    forceReview = false;
  }
  
  if(!url)
  {
    url = "http://metrix.webosroundup.com/MetrixInterface.asmx/GetBulletinBoard?packageId=" + Mojo.appInfo.id;
  }
  
  var result = this.ServiceRequest.request('palm://com.palm.systemservice/time', {
                                          method: 'getSystemTime',
                                          parameters: {},
                                          onSuccess: this.bulletinTimeCheck.bind(this,controller, forceReview, url),
                                          onFailure: function(){return;}
                                        });
};

Metrix.prototype.bulletinTimeCheck = function(controller, forceReview, url, response)
{
  var timeUTC = response.utc;

  if(response.utc > (this.lastBulletinTime + 86400) || forceReview === true)
  {
    var requestBulletin = this.AjaxRequest.request(url, {
                                                    method: "get",
                                                    evalJSON: "false",
                                                    onSuccess: this.checkBulletinSuccess.bind(this, timeUTC,controller, forceReview),
                                                    onFailure: this.checkBulletinFailure.bind(this),
                                                    on0: this.checkBulletinFailure.bind(this)
                                                  });
  }
};
  
Metrix.prototype.checkBulletinSuccess = function(timeUTC,controller, forceReview, transport)
{
  var version = transport.responseXML.getElementsByTagName("version").item(0).textContent;
  var msgArray = [];
  
  if(version > this.bulletinVersion || forceReview === true)
  {
      var bulletins = transport.responseXML.getElementsByTagName("announcement");

      for(var i = 0; i < bulletins.length; i++)
      {
        msgArray.push({title: bulletins[i].getElementsByTagName("title").item(0).textContent,text: bulletins[i].getElementsByTagName("message").item(0).textContent});
      }
      
      if(msgArray.length > 0)
      {
        controller.showDialog({template: "metrix/displayBulletin-dialog",assistant: new BulletinAssistant(this, controller, msgArray, timeUTC,version),preventCancel:true});
      }
  }
};

Metrix.prototype.checkBulletinFailure = function(transport)
{
};

Metrix.prototype.customCounts = function(valueGroup,valueName,valueData)
{
	var result = 0;
	
	if(!valueGroup || !valueName || !valueData || isNaN(valueData))
	{
		return -1;
	}
	
	if(!this.customCountsLock)
	{
  	this.customCountsLock = setTimeout(function(){this.customCountsLock = null;}.bind(this),5000);
  	
  	var url = "http://metrix.webosroundup.com/MetrixInterface.asmx/UpdateCustomCounts";
  	
  	this.AjaxRequest.request(url, {
  																method: "get",
  																evalJSON: "false",
  																parameters: {packageId: Mojo.appInfo.id, valueGroup: valueGroup, valueName: valueName, valueData: valueData},
  																onSuccess: this.customCountsSuccess.bind(this),
  																onFailure: this.customCountsFailure.bind(this),
  																on0: this.customCountsFailure.bind(this)
  															});
	}
	else
	{
		result = -1;
	}
	
	return result;
};

Metrix.prototype.customCountsSuccess = function(transport)
{
	
};

Metrix.prototype.customCountsFailure = function(transport)
{
	
};


function BulletinAssistant(sceneAssistant, controller, msgArray, timeUTC, version)
{
  this.sceneAssistant = sceneAssistant;
  this.sceneAssistant.controller = controller;
	this.msgArray = msgArray;
	this.utc = timeUTC;
	this.version = version;
}

BulletinAssistant.prototype.setup = function(widget)
{
  this.widget = widget;

	this.msgTitleElement = this.sceneAssistant.controller.get("bulletinTitle");
	this.msgTextElement = this.sceneAssistant.controller.get("msgContent");

	this.sceneAssistant.controller.get("bltnAckSelectorHitTarget").observe("mousedown",this.highlighter.bind(this, "bltnAckSelector_source", "bltnAckSelectorTitle","down","txt"));
	this.sceneAssistant.controller.get("bltnAckSelectorHitTarget").observe("mouseup",this.highlighter.bind(this, "bltnAckSelector_source", "bltnAckSelectorTitle","up","txt"));

	this.sceneAssistant.controller.get("bltnSnoozeSelectorHitTarget").observe("mousedown",this.highlighter.bind(this, "bltnSnoozeSelector_source", "bltnSnoozeSelectorTitle","down","txt"));
	this.sceneAssistant.controller.get("bltnSnoozeSelectorHitTarget").observe("mouseup",this.highlighter.bind(this, "bltnSnoozeSelector_source", "bltnSnoozeSelectorTitle","up","txt"));

	this.sceneAssistant.controller.get("bltnPrevSelectorHitTarget").observe("mousedown",this.highlighter.bind(this, "bltnPrevSelector_source", "imgPrev","down","img"));
	this.sceneAssistant.controller.get("bltnPrevSelectorHitTarget").observe("mouseup",this.highlighter.bind(this, "bltnPrevSelector_source", "imgPrev","up","img"));

	this.sceneAssistant.controller.get("bltnNextSelectorHitTarget").observe("mousedown",this.highlighter.bind(this, "bltnNextSelector_source", "imgNext","down","img"));
	this.sceneAssistant.controller.get("bltnNextSelectorHitTarget").observe("mouseup",this.highlighter.bind(this, "bltnNextSelector_source", "imgNext","up","img"));

	this.menuBarHandler = this.menuBarTouch.bindAsEventListener(this);
	this.sceneAssistant.controller.listen("bulletin_view_header", Mojo.Event.tap, this.menuBarHandler);

	if(this.msgArray.length > 1)
	{
	  var srcString = this.sceneAssistant.controller.get("imgNext").src;
    srcString = srcString.substr(0,srcString.length - 7) + "upp.png";
    this.sceneAssistant.controller.get("imgNext").src = srcString;
	}

	this.msgIndex = 0;

	this.msgTitleElement.innerHTML = this.msgArray[this.msgIndex].title;
  this.msgTextElement.innerHTML = this.msgArray[this.msgIndex].text;

};

BulletinAssistant.prototype.cleanup = function()
{
	this.sceneAssistant.controller.get("bltnAckSelectorHitTarget").stopObserving("mousedown");
	this.sceneAssistant.controller.get("bltnAckSelectorHitTarget").stopObserving("mouseup");
	this.sceneAssistant.controller.get("bltnSnoozeSelectorHitTarget").stopObserving("mousedown");
	this.sceneAssistant.controller.get("bltnSnoozeSelectorHitTarget").stopObserving("mouseup");
	this.sceneAssistant.controller.get("bltnPrevSelectorHitTarget").stopObserving("mousedown");
	this.sceneAssistant.controller.get("bltnPrevSelectorHitTarget").stopObserving("mouseup");
	this.sceneAssistant.controller.get("bltnNextSelectorHitTarget").stopObserving("mousedown");
	this.sceneAssistant.controller.get("bltnNextSelectorHitTarget").stopObserving("mouseup");
	
  this.sceneAssistant.controller.stopListening("bulletin_view_header", Mojo.Event.tap, this.menuBarHandler);
};

BulletinAssistant.prototype.highlighter = function(btnSourceElement, btnTitleElement,mouseAction, btnType)
{
  var srcString = "";
  
  if(mouseAction === "down")
  {
    this.sceneAssistant.controller.get(btnSourceElement).style["-webkit-border-image"] = "url(" + Mojo.appPath + "/images/Metrix/header-button-inverse.png) 0 10 0 10 stretch stretch";

    if(btnType === "txt")
    {
      this.sceneAssistant.controller.get(btnTitleElement).style["color"] = "white";
    }
    else if (btnType === "img")
    {
      srcString = this.sceneAssistant.controller.get(btnTitleElement).src;

      if(srcString.substr(srcString.length - 7,7) !== "bnk.png")
      {
        srcString = srcString.substr(0,srcString.length - 7) + "dwn.png";
        this.sceneAssistant.controller.get(btnTitleElement).src = srcString;
      }
    }
  }
  else if(mouseAction === "up")
  {
    this.sceneAssistant.controller.get(btnSourceElement).style["-webkit-border-image"] = "url(" + Mojo.appPath + "/images/Metrix/header-button.png) 0 10 0 10 stretch stretch";

    if(btnType === "txt")
    {
      this.sceneAssistant.controller.get(btnTitleElement).style.color = "black";
    }
    else if (btnType === "img")
    {
      srcString = this.sceneAssistant.controller.get(btnTitleElement).src;

      if(srcString.substr(srcString.length - 7,7) !== "bnk.png")
      {
        srcString = srcString.substr(0,srcString.length - 7) + "upp.png";
        this.sceneAssistant.controller.get(btnTitleElement).src = srcString;
      }
    }
  }
};

BulletinAssistant.prototype.activate = function()
{

};

BulletinAssistant.prototype.handleCommand = function(event)
{
	if(event.type == Mojo.Event.back)
	{
		event.stop();
		
    this.sceneAssistant.storeCookie(this.utc, this.version);

		this.widget.mojo.close();
	}
};

BulletinAssistant.prototype.menuBarTouch = function(event)
{
  var target = event.target.id;
  var srcString = "";
  
  switch(target)
  {
    case "bltnAckSelectorHitTarget":
      this.sceneAssistant.storeCookie(this.utc, this.version);

      this.widget.mojo.close();
    break;

    case "bltnPrevSelectorHitTarget":
      if(this.msgIndex > 0)
      {
        this.msgIndex--;
        this.msgTitleElement.innerHTML = this.msgArray[this.msgIndex].title;
        this.msgTextElement.innerHTML = this.msgArray[this.msgIndex].text;
      }

      if(this.msgIndex === 0)
      {
        srcString = this.sceneAssistant.controller.get("imgPrev").src;
        srcString = srcString.substr(0,srcString.length - 7) + "bnk.png";
        this.sceneAssistant.controller.get("imgPrev").src = srcString;
      }

      if(this.msgArray.length > 1)
      {
        srcString = this.sceneAssistant.controller.get("imgNext").src;
        srcString = srcString.substr(0,srcString.length - 7) + "upp.png";
        this.sceneAssistant.controller.get("imgNext").src = srcString;
      }
    break;

    case "bltnNextSelectorHitTarget":
      if(this.msgIndex < this.msgArray.length - 1)
      {
        this.msgIndex++;
        this.msgTitleElement.innerHTML = this.msgArray[this.msgIndex].title;
        this.msgTextElement.innerHTML = this.msgArray[this.msgIndex].text;
      }

      if(this.msgIndex === this.msgArray.length - 1)
      {
        srcString = this.sceneAssistant.controller.get("imgNext").src;
        srcString = srcString.substr(0,srcString.length - 7) + "bnk.png";
        this.sceneAssistant.controller.get("imgNext").src = srcString;
      }

      if(this.msgIndex > 0)
      {
        srcString = this.sceneAssistant.controller.get("imgPrev").src;
        srcString = srcString.substr(0,srcString.length - 7) + "upp.png";
        this.sceneAssistant.controller.get("imgPrev").src = srcString;
      }
    break;

    case "bltnSnoozeSelectorHitTarget":      
      this.sceneAssistant.storeCookie(this.utc);

      this.widget.mojo.close();
    break;

    default:

    break;
  }

};

Mojo.Log.debug = function()
{
  Mojo.Log._logImplementation(Mojo.Log.LOG_LEVEL_ERROR, $A(arguments));
};

Mojo.Log._logRemoteDownloadImplementation = function(args)
{
  var stringToLog;
	
	var formatString = args.shift();
	
	if (formatString) 
	{
	  // make sure the format string is in fact a string
	  formatString = "" + formatString;
		
		var nextArgument = function(stringToReplace) 
		{
			var target;
			if (stringToReplace === "%%") 
			{
				return  "%";
			}
			
			target = args.shift();
			switch (stringToReplace) 
			{
  			case "%o":
  				return Object.inspect(target);
  			case "%j":
  				return Object.toJSON(target);
  			default:
  			break;
			}

			return target;
		};
		
		var resultString = formatString.replace(/%[jsdfio%]/g, nextArgument);
		
		stringToLog = [resultString].concat(args).join(" ");
/*		
		var loggingFunction, banners = {};
		
		var makeBanners = function(label) 
		{
			var appTitle = Mojo.appInfo.title || "foo";
			var loggingPrefix = label + ": ";
			return {loggingPrefix: loggingPrefix};
		};
		
		if (messageLevel <= Mojo.Log.LOG_LEVEL_ERROR) 
		{
			loggingFunction = "error";
			banners = makeBanners("Error");
		} 
		else if (messageLevel <= Mojo.Log.LOG_LEVEL_WARNING) 
		{
			loggingFunction = "warn";
			banners = makeBanners("Warning");
		} 
		else 
		{
			loggingFunction = "info";
			banners = makeBanners("Info");
		} 
		
		if (console[loggingFunction]) 
		{
			if (Mojo.Host.current !== Mojo.Host.browser && banners.loggingPrefix) 
			{
				stringToLog = banners.loggingPrefix + stringToLog;
				
				if (banners.loggingSuffix) 
				{
					stringToLog += banners.loggingSuffix;
				}
			}
			console[loggingFunction](stringToLog);				
		}
*/		
	}
		
	return stringToLog;
};
