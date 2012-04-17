Metrix Library App Integration Steps


Unlike with Mojo, the Enyo app structure is not formally designed. To accomodate this, Metrix will be self contained within a single folder.  For this beta there are a few hardcoded paths for images pointing at "\source\libraries\Metrix\images\".  These images are only used in the Bulletins feature. In future versions we will remove this constraint. For now we recommend you follow the example app setup. 

EXAMPLE APP SETUP:

YourApp/

  source/

    libraries/
    
      Metrix/

        metrix.js
        
        images/

          menu-icon-back.png

          menu-icon-forward.png
    

You will now need to add the Metrix Library to your depends.json file.  Add the following line to the sources.json file:

"source/libraries/Metrix/metrix.js"

You are new ready to utilize the Metrix Library.




Using the Metrix Library


The Metrix object should have global scope. You can do this by instantiating the object in the creation function of your base Enyo object.

Example:

  YourApp = {}; //Global Object

  . 
  .
  .
  create: function() 
  {
      YourApp.Metrix = new Metrix(); //Instantiate Metrix Library
      .
      .
      .
  }
  .
  .
  .
   
You can now use the Metrix Library functions in your app.

Example:

  YourApp.Metrix.functionNameToCall();

  


The Metrix Library Functions


NAME:         postDeviceData()

PARAMS:       n/a

RETURNS:      n/a

DESCRIPTION:  This function is used to send the anonymous device data to the Metrix server.  This function is what is used to generate your
              apps metrics and should be called at each app startup. 

EXAMPLE:

YourApp.Metrix.postDeviceData();


NAME:        isExpired(currentUtcTime, daysAllowed)

PARAMS:      daysAllowed - Number of days allowed before the function returns true.

RETURNS:     Boolean - true if expired and false if not.

DESCRIPTION: This method will determine if an app is being used within the alloted window.  The developer needs to specify the number of days that the app should expire in from first use. The function will then compare the current UTC time to see if it is within the apps first run time stamp and the number of alloted days and return true if it is passed or false if it is within the alloted time. If the Metrix Library has no reference to the app's first run timestamp, then it will create it at that time.

EXAMPLE:

var daysValid = 7;

if(YourApp.Metrix.isExpired(daysValid))
{
  //notify user that app has expired and point them to the full version
}


NAME:         checkBulletinBoard(controller, minBulletinVersion, forceReview)

PARAMS:       minBulletinVersion - All bulletins with a version less than this will be ignored.  This is usefull if notifing users of a new
              version so new users do not get notified.

              forceReview - Optional boolean to tell the system to ingnore the minBulletinVersion and Time requirements. Often used to review bulletins after the user has dismissed them.

RETURNS:      n/a

DESCRIPTION:  This function will check the server every 24 hours for a new bulletin version.  If the version of bulletin is greater than
              the minBulletinVersion passed in, a dialog box will be presented to the user.  This dialog box is scrollable and allows the user to page through multiple bulletins.  The user can dismiss the bulliten so it will not show up again or snooze it to be displayed during the next check (24 hours later). You can manage your bulletins through the web interface.

 

EXAMPLE:

   YourApp.Metrix.checkBulletinBoard(1001, false);

 
NAME:         customCounts (valueGroup, valueName, valueData)

PARAMS:       valueGroup - The name of the collection of metrics.

              valueName – The label for the metric.

              valueData – The number to increment your count by.  This number will be added the current count on the server.

RETURNS:      n/a

DESCRIPTION:  This method will allow you to create a custom metric to count something. What you count is up to you! We ask that you try to be                respectfull of the server and cache the data as much as possible before calling this to post it to the server.  A common use for               this is to count scene views.

EXAMPLE:

              YouApp.Metrix.customCounts(“MyCustomMetric”, “SomethingToCount”, 2);                                                                           