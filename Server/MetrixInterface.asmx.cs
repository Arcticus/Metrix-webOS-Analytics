using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using CommandLibrary;
using MongoCommand;
using Norm;
using System.Text;
using System.Security.Cryptography;

namespace Dev
{
    /// <summary>
    /// Summary description for MetrixInterface
    /// </summary>
    [WebService(Namespace = "http://metrix.webosroundup.com")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MetrixInterface : System.Web.Services.WebService
    {
        public DBContext db = new DBContext();
        public MetrixGeneralDeviceData generalDataTable = new MetrixGeneralDeviceData();
        public MetrixCustomCount customCountTable = new MetrixCustomCount();
        public SHA1Managed metrixHash = new SHA1Managed();
        
        public const String MetrixVersion = "0.3.2";
				
		[WebMethod]
		public String TestMe()
		{
            Uri uri = HttpContext.Current.Request.UrlReferrer;

            if (uri != null)
            {
    			String MyReferrer = uri.ToString();
                return MyReferrer;
            }

            return null;
		}
				
        [WebMethod]
        public XmlDocument DeviceDataPost(String deviceId, String packageId, String companyName, String appVersion, String webOsBuildNumber, String resolution, String carrier, String webOsVersion, String deviceName)
        {
            return DeviceDataPostV2(deviceId, packageId, companyName, appVersion, webOsBuildNumber, resolution, carrier, webOsVersion, deviceName, "Unknown");
        }

        [WebMethod]
        public XmlDocument DeviceDataPostV2(String deviceId, String packageId, String companyName, String appVersion, String webOsBuildNumber, String resolution, String carrier, String webOsVersion, String deviceName, String locale)
        {            
            return DeviceDataPostV3(deviceId, packageId, companyName, appVersion, webOsBuildNumber, resolution, carrier, webOsVersion, deviceName, locale);
        }

        public XmlDocument DeviceDataPostV3(String deviceId, String packageId, String companyName, String appVersion, String webOsBuildNumber, String resolution, String carrier, String webOsVersion, String deviceName, String locale)
        {
            byte[] deviceIdAsByteArray = Encoding.UTF8.GetBytes(deviceId);
            String hashId = Convert.ToBase64String(metrixHash.ComputeHash(deviceIdAsByteArray));

            var q = from p in db.MetrixGeneralDeviceDatas
                    where p.HashId == hashId && p.PackageID == packageId
                    select p;           

            if (q.Count() == 0)
            {
                //generalDataTable.DeviceId = deviceId;         //Unique Id of a phone, this is not necessary a reflection of unique users since devices can be replaced but it is the closest we can get for now.
                generalDataTable.HashId = hashId;
                generalDataTable.PackageID = packageId;        //Unique name of the app
                generalDataTable.CreationTimeStamp = DateTime.Now;     //Stamped only the first time a deviceId checks in
                generalDataTable.LastUpdateTimeStamp = DateTime.Now;     //Stamped only when the appVersion changes           
                generalDataTable.CompanyName = companyName;      //Company that produced the app
                generalDataTable.AppVersion = appVersion;       //Version number of the app
                generalDataTable.WebOsBuildNumber = webOsBuildNumber; //This is the build number of WebOs on the device               
                generalDataTable.ScreenResolution = resolution;       //Screen resolution for the device. I.E. Pre = 320x480, Pixie = 320x400         
                generalDataTable.CarrierName = carrier;          //Telephone company the phone is assigned to
                generalDataTable.WebOsVersion = webOsVersion;     //This is the version of WebOs on the device           
                generalDataTable.DeviceName = deviceName;       //This is the string name for the device.
                generalDataTable.LastCheckInTimeStamp = DateTime.Now;     //This is stamped everytime the device checks in
                generalDataTable.Locale = locale;

                db.MetrixGeneralDeviceDatas.InsertOnSubmit(generalDataTable);
                db.SubmitChanges();
            }
            else //already on file just update record
            {
                generalDataTable = q.Single();

                // hit the db if something changed or we haven't checked in for 12 hours.
                if (generalDataTable.AppVersion != appVersion ||
                    generalDataTable.HashId == null ||
                    generalDataTable.CompanyName != companyName ||
                    generalDataTable.WebOsBuildNumber != webOsBuildNumber ||
                    generalDataTable.CarrierName != carrier ||
                    generalDataTable.ScreenResolution != resolution ||
                    generalDataTable.WebOsVersion != webOsVersion ||
                    generalDataTable.DeviceName != deviceName ||
                    generalDataTable.Locale != locale ||
                    generalDataTable.LastCheckInTimeStamp < DateTime.Now.AddHours(-12)
                   )
                {
                    if (generalDataTable.AppVersion != appVersion)
                    {
                        generalDataTable.LastUpdateTimeStamp = DateTime.Now;
                    }

                    if (generalDataTable.HashId == null)
                    {
                        generalDataTable.HashId = hashId;
                    }

                    generalDataTable.CompanyName = companyName;
                    generalDataTable.AppVersion = appVersion;
                    generalDataTable.WebOsBuildNumber = webOsBuildNumber;
                    generalDataTable.CarrierName = carrier;
                    generalDataTable.ScreenResolution = resolution;
                    generalDataTable.WebOsVersion = webOsVersion;
                    generalDataTable.DeviceName = deviceName;
                    generalDataTable.LastCheckInTimeStamp = DateTime.Now;
                    generalDataTable.Locale = locale;

                    db.SubmitChanges();
                }
            }

            XmlDocument xmlDoc = new XmlDocument();
            String result = "<postGeneralDataResult><creationTimestamp>" + convertToUTC(generalDataTable.CreationTimeStamp).ToString() + "</creationTimestamp><lastUpdateTimestamp>" + convertToUTC(generalDataTable.LastUpdateTimeStamp).ToString() + "</lastUpdateTimestamp></postGeneralDataResult>";
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        public XmlDocument DeviceDataPostV4(String deviceId, String packageId, String companyName, String appVersion, String webOsBuildNumber, String resolution, String carrier, String webOsVersion, String deviceName, String locale)
        {
            byte[] deviceIdAsByteArray = Encoding.UTF8.GetBytes(deviceId);
            String hashId = Convert.ToBase64String(metrixHash.ComputeHash(deviceIdAsByteArray));
            
            var q = from p in db.MetrixGeneralDeviceDatas
                    where p.HashId == hashId && p.PackageID == packageId
                    select p;

            if (q.Count() == 0)
            {
                //generalDataTable.DeviceId = deviceId;         //Unique Id of a phone, this is not necessary a reflection of unique users since devices can be replaced but it is the closest we can get for now.
                generalDataTable.HashId = hashId;
                generalDataTable.PackageID = packageId;        //Unique name of the app
                generalDataTable.CreationTimeStamp = DateTime.Now;     //Stamped only the first time a deviceId checks in
                generalDataTable.LastUpdateTimeStamp = DateTime.Now;     //Stamped only when the appVersion changes           
                generalDataTable.CompanyName = companyName;      //Company that produced the app
                generalDataTable.AppVersion = appVersion;       //Version number of the app
                generalDataTable.WebOsBuildNumber = webOsBuildNumber; //This is the build number of WebOs on the device               
                generalDataTable.ScreenResolution = resolution;       //Screen resolution for the device. I.E. Pre = 320x480, Pixie = 320x400         
                generalDataTable.CarrierName = carrier;          //Telephone company the phone is assigned to
                generalDataTable.WebOsVersion = webOsVersion;     //This is the version of WebOs on the device           
                generalDataTable.DeviceName = deviceName;       //This is the string name for the device.
                generalDataTable.LastCheckInTimeStamp = DateTime.Now;     //This is stamped everytime the device checks in
                generalDataTable.Locale = locale;

                db.MetrixGeneralDeviceDatas.InsertOnSubmit(generalDataTable);
                db.SubmitChanges();
            }
            else //already on file just update record
            {
                generalDataTable = q.Single();

                // hit the db if something changed or we haven't checked in for 12 hours.
                if (generalDataTable.AppVersion != appVersion ||
                    generalDataTable.HashId == null ||
                    generalDataTable.CompanyName != companyName ||
                    generalDataTable.WebOsBuildNumber != webOsBuildNumber ||
                    generalDataTable.CarrierName != carrier ||
                    generalDataTable.ScreenResolution != resolution ||
                    generalDataTable.WebOsVersion != webOsVersion ||
                    generalDataTable.DeviceName != deviceName ||
                    generalDataTable.Locale != locale ||
                    generalDataTable.LastCheckInTimeStamp < DateTime.Now.AddHours(-12)
                   )
                {
                    if (generalDataTable.AppVersion != appVersion)
                    {
                        generalDataTable.LastUpdateTimeStamp = DateTime.Now;
                    }

                    if (generalDataTable.HashId == null)
                    {
                        generalDataTable.HashId = hashId;
                    }

                    generalDataTable.CompanyName = companyName;
                    generalDataTable.AppVersion = appVersion;
                    generalDataTable.WebOsBuildNumber = webOsBuildNumber;
                    generalDataTable.CarrierName = carrier;
                    generalDataTable.ScreenResolution = resolution;
                    generalDataTable.WebOsVersion = webOsVersion;
                    generalDataTable.DeviceName = deviceName;
                    generalDataTable.LastCheckInTimeStamp = DateTime.Now;
                    generalDataTable.Locale = locale;

                    db.SubmitChanges();
                }
            }

            MetrixDeviceInfo mdd = new MetrixDeviceInfo();
            MetrixAppStructure mas = new MetrixAppStructure();

            var mongo = MDB.Instance().GetMongo();
            var device = mongo.GetCollection<MetrixDeviceInfo>();

            if (device.Count() > 0)
                mongo.Database.DropCollection("MetrixDeviceInfo");

            device = mongo.GetCollection<MetrixDeviceInfo>();

            var qm = from p in device.AsQueryable()
                     where p.HashId == hashId
                     select p;

            if (q.Count() > 0)
            {
                mdd = qm.Single();

                var a = from p in mdd.Apps
                        where p.PackageId == packageId
                        select p;

                if (a.Count() > 0)
                {

                    mas = a.Single();

                    mas.Company = companyName;
                    mas.Creation = mas.Creation;

                    mas.LastCheckIn = DateTime.Now;

                    if (mas.AppVersion != appVersion)
                    {
                        mas.LastUpdate = DateTime.Now;
                    }
                    else
                    {
                        mas.LastUpdate = mas.LastUpdate;
                    }

                    mas.AppVersion = appVersion;
                }
                else
                {
                    if (mdd.Apps.Count() == 0)
                    {
                        mas.ID = 0;
                    }
                    else
                    {
                        mas.ID = mdd.Apps.AsQueryable().OrderByDescending(i => i.ID).First().ID + 1;
                    }

                    mas.PackageId = packageId;
                    mas.Company = companyName;
                    mas.AppVersion = appVersion;
                    mas.Creation = DateTime.Now;
                    mas.LastCheckIn = DateTime.Now;
                    mas.LastUpdate = DateTime.Now;

                    mdd.Apps.Add(mas);
                }
            }
            else
            {
                if (device.Count() == 0)
                {
                    mdd.ID = 0;
                }
                else
                {
                    mdd.ID = device.AsQueryable().OrderByDescending(i => i.ID).First().ID + 1;
                }

                mdd.HashId = hashId;
                mas.PackageId = packageId;
                mas.Company = companyName;
                mas.AppVersion = appVersion;
                mas.Creation = DateTime.Now;
                mas.LastCheckIn = DateTime.Now;
                mas.LastUpdate = DateTime.Now;

                mdd.Apps = new List<MetrixAppStructure>();
                mdd.Apps.Add(mas);
            }

            mdd.Device = deviceName;
            mdd.Carrier = carrier;
            mdd.Screen = resolution;
            mdd.Locale = locale;
            mdd.WebOsBuild = webOsBuildNumber;
            mdd.WebOsVersion = webOsVersion;

            device.Save(mdd);

            XmlDocument xmlDoc = new XmlDocument();
            String result = "<postGeneralDataResult><creationTimestamp>" + convertToUTC(generalDataTable.CreationTimeStamp).ToString() + "</creationTimestamp><lastUpdateTimestamp>" + convertToUTC(generalDataTable.LastUpdateTimeStamp).ToString() + "</lastUpdateTimestamp></postGeneralDataResult>";
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public int GetNewCount(long timeSince, String packageId)
        {
           DateTime dt = convertFromUTC(timeSince);

            var q = from p in db.MetrixGeneralDeviceDatas
                    where p.PackageID == packageId && p.CreationTimeStamp > dt
                    select p;

            return q.Count();
        }

        [WebMethod]
        public int GetTotalCount(String packageId)
        {
            var q = from p in db.MetrixGeneralDeviceDatas
                    where p.PackageID == packageId
                    select p;

            return q.Count();
        }

        [WebMethod]
        public int GetConversionCount(String packageIdTrial, String packageIdPaid)
        {
           var q = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageIdTrial
                     select p.HashId)
                    .Intersect
                    (from r in db.MetrixGeneralDeviceDatas
                     where r.PackageID == packageIdPaid
                     select r.HashId);

            return q.Count();
        }

        [WebMethod]
        public String GetCompanyPackageIds(String company)
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     where p.CompanyName == company
                     select p.PackageID).Distinct();

            foreach (String s in q)
            {
                result += s + "|";
            }

            return result;
        }

        [WebMethod]
        public String GetAppHeuristicData(String packageId)
        {
            String result = "";

            var q = from p in db.MetrixGeneralDeviceDatas
                    where p.PackageID == packageId
                    group p by new
                    {
                        p.CreationTimeStamp.Date
                    }
                        into r
                        orderby r.Key.Date descending
                        select new
                        {
                            date = r.Key.Date.ToShortDateString(),
                            downloadsPerDay = r.Count()
                        };

            foreach (var item in q)
            {
                String dlDate;
                String[] temp = item.date.Split('/');

                if (temp[0].Length == 1)
                {
                    dlDate = "0" + item.date;
                }
                else
                {
                    dlDate = item.date;
                }

                if (temp[1].Length == 1)
                {
                    dlDate = dlDate.Substring(0, 3) + "0" + dlDate.Substring(3, dlDate.Length - 3);
                }

                result += "\r" + dlDate + "," + item.downloadsPerDay;
            }
            
            result += "\r";
            
            return result;
        }

        [WebMethod]
        public String GetGlobalAppHeuristicData()
        {
            String result = "";

            var q = from p in db.MetrixGeneralDeviceDatas                    
                    group p by new
                    {
                        p.CreationTimeStamp.Date
                    }
                        into r
                        orderby r.Key.Date descending
                        select new
                        {
                            date = r.Key.Date.ToShortDateString(),
                            downloadsPerDay = r.Count()
                        };

            foreach (var item in q)
            {
                String dlDate;
                String[] temp = item.date.Split('/');

                if (temp[0].Length == 1)
                {
                    dlDate = "0" + item.date;
                }
                else
                {
                    dlDate = item.date;
                }

                if (temp[1].Length == 1)
                {
                    dlDate = dlDate.Substring(0, 3) + "0" + dlDate.Substring(3, dlDate.Length - 3);
                }

                result += "\r" + dlDate + "," + item.downloadsPerDay;
            }

            result += "\r";

            return result;
        }

        [WebMethod]
        public int GetVersionCount(String packageId, String version)
        {
           var q = from p in db.MetrixGeneralDeviceDatas
                    where p.PackageID == packageId && p.AppVersion == version
                    select p;

            return q.Count();
        }

        [WebMethod]
        public XmlDocument GetVersionBreakDown(String packageId)
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.AppVersion).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var version in q)
            {
                result += "<value xid=\"" + i + "\">" + version + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var version in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.AppVersion == version && c.PackageID == packageId
                        orderby c.AppVersion
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetWebOsVersionBreakDown(String packageId)
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.WebOsVersion).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var version in q)
            {
                result += "<value xid=\"" + i + "\">" + version + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var version in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.WebOsVersion == version
                        orderby c.WebOsVersion
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetGlobalWebOsVersionBreakDown()
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     select p.WebOsVersion).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var version in q)
            {
                result += "<value xid=\"" + i + "\">" + version + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var version in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.WebOsVersion == version
                        orderby c.WebOsVersion
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetWebOsBuildNumberBreakDown(String packageId)
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.WebOsBuildNumber).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var version in q)
            {
                result += "<value xid=\"" + i + "\">" + version + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var version in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.WebOsBuildNumber == version
                        orderby c.WebOsVersion
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetGlobalWebOsBuildNumberBreakDown()
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas

                     select p.WebOsBuildNumber).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var version in q)
            {
                result += "<value xid=\"" + i + "\">" + version + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var version in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.WebOsBuildNumber == version
                        orderby c.WebOsVersion
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetCarrierBreakDown(String packageId)
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.CarrierName).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var carrier in q)
            {
                result += "<value xid=\"" + i + "\">" + carrier + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var carrier in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.CarrierName == carrier
                        orderby c.CarrierName
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetGlobalCarrierBreakDown()
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     select p.CarrierName).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var carrier in q)
            {
                result += "<value xid=\"" + i + "\">" + carrier + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var carrier in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.CarrierName == carrier
                        orderby c.CarrierName
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetDeviceNameBreakDown(String packageId)
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.DeviceName).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var deviceName in q)
            {
                result += "<value xid=\"" + i + "\">" + deviceName + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var deviceName in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.DeviceName == deviceName
                        orderby c.DeviceName
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetGlobalDeviceNameBreakDown()
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     select p.DeviceName).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var deviceName in q)
            {
                result += "<value xid=\"" + i + "\">" + deviceName + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var deviceName in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.DeviceName == deviceName
                        orderby c.DeviceName
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetScreenResolutionBreakDown(String packageId)
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.ScreenResolution).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var resolution in q)
            {
                result += "<value xid=\"" + i + "\">" + resolution + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var resolution in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.ScreenResolution == resolution
                        orderby c.ScreenResolution
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetGlobalScreenResolutionBreakDown()
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas                     
                     select p.ScreenResolution).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var resolution in q)
            {
                result += "<value xid=\"" + i + "\">" + resolution + "</value>";

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var resolution in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.ScreenResolution == resolution
                        orderby c.ScreenResolution
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetLocaleBreakDown(String packageId)
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.Locale).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var locale in q)
            {
                if (locale == null)
                {
                    result += "<value xid=\"" + i + "\">Unknown</value>";
                }
                else
                {
                    result += "<value xid=\"" + i + "\">" + locale + "</value>";
                }

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var locale in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.Locale == locale
                        orderby c.Locale
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetGlobalLocaleBreakDown()
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas
                     select p.Locale).Distinct().OrderBy(a => a);

            int i = 0;

            result += "<chart><series>";

            foreach (var locale in q)
            {
                if (locale == null)
                {
                    result += "<value xid=\"" + i + "\">Unknown</value>";
                }
                else
                {
                    result += "<value xid=\"" + i + "\">" + locale + "</value>";
                }

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var locale in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.Locale == locale
                        orderby c.Locale
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetBulletinBoard(String packageId)
        {
            String result = "";

            var v = from p in db.MetrixBulletinBoards
                    where p.PackageID == packageId
                    select p.BulletinBoardVersion;

            if (v.Count() > 0)
            {
                result = "<bulletins><version>" + v.Single() + "</version>";

                var q = from p in db.MetrixBulletinBoardAnnouncements
                        where p.PackageID == packageId
                        select p;

                String announcements = "";

                foreach (var announcment in q)
                {
                    announcements = "<announcement><title>" + announcment.Title + "</title><message><![CDATA[<div class=\"bulletin\">" + announcment.Message + "</div>]]></message></announcement>" + announcements;
                }

                result += announcements;
                result += "</bulletins>";
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetCurrentActivityBreakDown(String packageId)
        {
            String result = "<pie>";

            var q1 = from p in db.MetrixGeneralDeviceDatas
                    where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(1))
                    select p;

            result += "<slice title=\"Last 24 Hours\">" + q1.Count() + "</slice>";

            var q2 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(3)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(1))
                     select p;

            result += "<slice title=\"Between 72 and 24 Hours\">" + q2.Count() + "</slice>";

            var q3 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(7)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(3))
                     select p;

            result += "<slice title=\"Between 1 Week and 72 Hours\">" + q3.Count() + "</slice>";

            var q4 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(14)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(7))
                     select p;

            result += "<slice title=\"Between 2 Weeks and 1 Week\">" + q4.Count() + "</slice>";

            var q5 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(30)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(14))
                     select p;

            result += "<slice title=\"Between 1 Month and 2 Weeks\">" + q5.Count() + "</slice>";

            var q6 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(30))
                     select p;

            result += "<slice title=\"Greater than 1 Month\">" + q6.Count() + "</slice>";

            result += "</pie>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetGlobalCurrentActivityBreakDown()
        {
            String result = "<pie>";

            var q1 = from p in db.MetrixGeneralDeviceDatas
                     where p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(1))
                     select p;

            result += "<slice title=\"Last 24 Hours\">" + q1.Count() + "</slice>";

            var q2 = from p in db.MetrixGeneralDeviceDatas
                     where p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(3)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(1))
                     select p;

            result += "<slice title=\"Between 72 and 24 Hours\">" + q2.Count() + "</slice>";

            var q3 = from p in db.MetrixGeneralDeviceDatas
                     where p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(7)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(3))
                     select p;

            result += "<slice title=\"Between 1 Week and 72 Hours\">" + q3.Count() + "</slice>";

            var q4 = from p in db.MetrixGeneralDeviceDatas
                     where p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(14)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(7))
                     select p;

            result += "<slice title=\"Between 2 Weeks and 1 Week\">" + q4.Count() + "</slice>";

            var q5 = from p in db.MetrixGeneralDeviceDatas
                     where p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(30)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(14))
                     select p;

            result += "<slice title=\"Between 1 Month and 2 Weeks\">" + q5.Count() + "</slice>";

            var q6 = from p in db.MetrixGeneralDeviceDatas
                     where p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(30))
                     select p;

            result += "<slice title=\"Greater than 1 Month\">" + q6.Count() + "</slice>";

            result += "</pie>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument GetGlobalCounts()
        {
            String result = "";

            var q = (from p in db.MetrixGeneralDeviceDatas                     
                     select p.PackageID).Distinct();

            int i = 0;

            result += "<chart><series>";

            foreach (var pId in q)
            {                
                result += "<value xid=\"" + i + "\">" + pId + "</value>";
                
                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var pId in q)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == pId
                        select c;

                result += "<value xid=\"" + i + "\">" + d.Count() + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public long UpdateCustomCounts(String packageId, String valueGroup, String valueName, long valueData)
        {
            long result = valueData;

            var c = from p in db.MetrixCustomCounts
                    where p.PackageID == packageId && p.ValueGroup == valueGroup && p.ValueName == valueName
                    select p;

            if (c.Count() == 0)
            {
                customCountTable.PackageID = packageId;
                customCountTable.ValueGroup = valueGroup;
                customCountTable.ValueName = valueName;
                customCountTable.ValueData = valueData;

                db.MetrixCustomCounts.InsertOnSubmit(customCountTable);
            }
            else
            {
                customCountTable = c.Single();
                customCountTable.ValueData += valueData;
                result = customCountTable.ValueData;
            }

            db.SubmitChanges();

            return result;
        }

        [WebMethod]
        public XmlDocument getCustomCounterAsPie(String packageId, String valueGroup)
        {
            String result = "<pie>";

            var q =  from p in db.MetrixCustomCounts
                     where p.PackageID == packageId && p.ValueGroup == valueGroup
                     select p;
            
            foreach (var data in q)
            {                
                result += "<slice title=\"" + data.ValueName + "\">" + data.ValueData + "</slice>";                
            }
         
            result += "</pie>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public XmlDocument getCustomCounterAsBar(String packageId, String valueGroup)
        {
            String result = "";

            var q = from p in db.MetrixCustomCounts
                    where p.PackageID == packageId && p.ValueGroup == valueGroup
                    select p;

            int i = 0;

            result += "<chart><series>";

            foreach (var data in q)
            {
                if (data == null)
                {
                    result += "<value xid=\"" + i + "\">Unknown</value>";
                }
                else
                {
                    result += "<value xid=\"" + i + "\">" + data.ValueName + "</value>";
                }

                i++;
            }

            result += "</series><graphs><graph gid=\"0\">";

            i = 0; //reset counter

            foreach (var data in q)
            {
                result += "<value xid=\"" + i + "\">" + data.ValueData + "</value>";

                i++;
            }

            result += "</graph></graphs></chart>";


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        [WebMethod]
        public void ExportData(String packageId)
        {
            string attachment = "attachment; filename=MetrixData.csv";
            
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");


            HttpContext.Current.Response.Write("Date, DL Count");
            HttpContext.Current.Response.Write(Environment.NewLine);

            var qTimeline = from p in db.MetrixGeneralDeviceDatas
                    where p.PackageID == packageId
                    group p by new
                    {
                        p.CreationTimeStamp.Date
                    }
                        into r
                        orderby r.Key.Date ascending
                        select new
                        {
                            date = r.Key.Date.ToShortDateString(),
                            downloadsPerDay = r.Count()
                        };

            foreach (var item in qTimeline)
            {
                String dlDate;
                String[] temp = item.date.Split('/');

                if (temp[0].Length == 1)
                {
                    dlDate = "0" + item.date;
                }
                else
                {
                    dlDate = item.date;
                }

                if (temp[1].Length == 1)
                {
                    dlDate = dlDate.Substring(0, 3) + "0" + dlDate.Substring(3, dlDate.Length - 3);
                }                

                StringBuilder stringBuilder = new StringBuilder();
                AddComma(dlDate, stringBuilder);
                AddComma(item.downloadsPerDay.ToString(), stringBuilder);

                HttpContext.Current.Response.Write(stringBuilder.ToString());
                HttpContext.Current.Response.Write(Environment.NewLine);
            }

            HttpContext.Current.Response.Write(Environment.NewLine);
            HttpContext.Current.Response.Write(Environment.NewLine);

            HttpContext.Current.Response.Write("App Version, DL Count");
            HttpContext.Current.Response.Write(Environment.NewLine);

            var qAppVer = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.AppVersion).Distinct().OrderBy(a => a);

            foreach (var version in qAppVer)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.AppVersion == version
                        orderby c.AppVersion
                        select c;

                StringBuilder stringBuilder = new StringBuilder();
                AddComma(version, stringBuilder);
                AddComma(d.Count().ToString(), stringBuilder);
                
                HttpContext.Current.Response.Write(stringBuilder.ToString());
                HttpContext.Current.Response.Write(Environment.NewLine);
                
            }

            HttpContext.Current.Response.Write(Environment.NewLine);
            HttpContext.Current.Response.Write(Environment.NewLine);

            HttpContext.Current.Response.Write("WebOS Version, DL Count");
            HttpContext.Current.Response.Write(Environment.NewLine);



            var qOsVer = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.WebOsVersion).Distinct().OrderBy(a => a);


            foreach (var version in qOsVer)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.WebOsVersion == version
                        orderby c.WebOsVersion
                        select c;

                StringBuilder stringBuilder = new StringBuilder();
                AddComma(version, stringBuilder);
                AddComma(d.Count().ToString(), stringBuilder);

                HttpContext.Current.Response.Write(stringBuilder.ToString());
                HttpContext.Current.Response.Write(Environment.NewLine);
            }

            HttpContext.Current.Response.Write(Environment.NewLine);
            HttpContext.Current.Response.Write(Environment.NewLine);

            HttpContext.Current.Response.Write("Build Version, DL Count");
            HttpContext.Current.Response.Write(Environment.NewLine);

            var qPlatformVer = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.WebOsBuildNumber).Distinct().OrderBy(a => a);


            foreach (var version in qPlatformVer)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.WebOsBuildNumber == version
                        orderby c.WebOsVersion
                        select c;

                StringBuilder stringBuilder = new StringBuilder();
                AddComma(version, stringBuilder);
                AddComma(d.Count().ToString(), stringBuilder);

                HttpContext.Current.Response.Write(stringBuilder.ToString());
                HttpContext.Current.Response.Write(Environment.NewLine);
            }

            HttpContext.Current.Response.Write(Environment.NewLine);
            HttpContext.Current.Response.Write(Environment.NewLine);

            HttpContext.Current.Response.Write("Carrier, DL Count");
            HttpContext.Current.Response.Write(Environment.NewLine);

            var qCarrier = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.CarrierName).Distinct().OrderBy(a => a);


            foreach (var carrier in qCarrier)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.CarrierName == carrier
                        orderby c.CarrierName
                        select c;

                StringBuilder stringBuilder = new StringBuilder();
                AddComma(carrier, stringBuilder);
                AddComma(d.Count().ToString(), stringBuilder);

                HttpContext.Current.Response.Write(stringBuilder.ToString());
                HttpContext.Current.Response.Write(Environment.NewLine);
            }

            HttpContext.Current.Response.Write(Environment.NewLine);
            HttpContext.Current.Response.Write(Environment.NewLine);

            HttpContext.Current.Response.Write("Device Name, DL Count");
            HttpContext.Current.Response.Write(Environment.NewLine);

            var qDeviceName = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.DeviceName).Distinct().OrderBy(a => a);


            foreach (var deviceName in qDeviceName)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.DeviceName == deviceName
                        orderby c.DeviceName
                        select c;

                StringBuilder stringBuilder = new StringBuilder();
                AddComma(deviceName, stringBuilder);
                AddComma(d.Count().ToString(), stringBuilder);

                HttpContext.Current.Response.Write(stringBuilder.ToString());
                HttpContext.Current.Response.Write(Environment.NewLine);
            }

            HttpContext.Current.Response.Write(Environment.NewLine);
            HttpContext.Current.Response.Write(Environment.NewLine);

            HttpContext.Current.Response.Write("Resolution, DL Count");
            HttpContext.Current.Response.Write(Environment.NewLine);

            var qResolution = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.ScreenResolution).Distinct().OrderBy(a => a);


            foreach (var resolution in qResolution)
            {
                var d = from c in db.MetrixGeneralDeviceDatas
                        where c.PackageID == packageId && c.ScreenResolution == resolution
                        orderby c.ScreenResolution
                        select c;

                StringBuilder stringBuilder = new StringBuilder();
                AddComma(resolution, stringBuilder);
                AddComma(d.Count().ToString(), stringBuilder);

                HttpContext.Current.Response.Write(stringBuilder.ToString());
                HttpContext.Current.Response.Write(Environment.NewLine);
            }

            HttpContext.Current.Response.Write(Environment.NewLine);
            HttpContext.Current.Response.Write(Environment.NewLine);

            HttpContext.Current.Response.Write("Locale, DL Count");
            HttpContext.Current.Response.Write(Environment.NewLine);

            var qLocale = (from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId
                     select p.Locale).Distinct().OrderBy(a => a);


            foreach (var locale in qLocale)
            {
                if (locale != null)
                {
                    var d = from c in db.MetrixGeneralDeviceDatas
                            where c.PackageID == packageId && c.Locale == locale
                            orderby c.Locale
                            select c;

                    StringBuilder stringBuilder = new StringBuilder();
                    AddComma(locale, stringBuilder);
                    AddComma(d.Count().ToString(), stringBuilder);

                    HttpContext.Current.Response.Write(stringBuilder.ToString());
                    HttpContext.Current.Response.Write(Environment.NewLine);
                }
            }

            HttpContext.Current.Response.Write(Environment.NewLine);
            HttpContext.Current.Response.Write(Environment.NewLine);

            HttpContext.Current.Response.Write("Usage, DL Count");
            HttpContext.Current.Response.Write(Environment.NewLine);

            StringBuilder usageBuilderq1 = new StringBuilder();
            
            var q1 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(1))
                     select p;

            AddComma("Last 24 Hours", usageBuilderq1);
            AddComma(q1.Count().ToString(), usageBuilderq1);
            HttpContext.Current.Response.Write(usageBuilderq1.ToString());
            HttpContext.Current.Response.Write(Environment.NewLine);

            StringBuilder usageBuilderq2 = new StringBuilder();

            var q2 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(3)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(1))
                     select p;

            AddComma("Between 72 and 24 Hours", usageBuilderq2);
            AddComma(q2.Count().ToString(), usageBuilderq2);
            HttpContext.Current.Response.Write(usageBuilderq2.ToString());
            HttpContext.Current.Response.Write(Environment.NewLine);

            StringBuilder usageBuilderq3 = new StringBuilder();

            var q3 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(7)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(3))
                     select p;

            AddComma("Between 1 Week and 72 Hours", usageBuilderq3);
            AddComma(q3.Count().ToString(), usageBuilderq3);
            HttpContext.Current.Response.Write(usageBuilderq3.ToString());
            HttpContext.Current.Response.Write(Environment.NewLine);

            StringBuilder usageBuilderq4 = new StringBuilder();

            var q4 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(14)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(7))
                     select p;

            AddComma("Between 2 Weeks and 1 Week", usageBuilderq4);
            AddComma(q4.Count().ToString(), usageBuilderq4);
            HttpContext.Current.Response.Write(usageBuilderq4.ToString());
            HttpContext.Current.Response.Write(Environment.NewLine);

            StringBuilder usageBuilderq5 = new StringBuilder();

            var q5 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp >= DateTime.Now.Subtract(TimeSpan.FromDays(30)) && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(14))
                     select p;

            AddComma("Between 1 Month and 2 Weeks", usageBuilderq5);
            AddComma(q5.Count().ToString(), usageBuilderq5);
            HttpContext.Current.Response.Write(usageBuilderq5.ToString());
            HttpContext.Current.Response.Write(Environment.NewLine);

            StringBuilder usageBuilderq6 = new StringBuilder();

            var q6 = from p in db.MetrixGeneralDeviceDatas
                     where p.PackageID == packageId && p.LastCheckInTimeStamp < DateTime.Now.Subtract(TimeSpan.FromDays(30))
                     select p;

            AddComma("Greater than 1 Month", usageBuilderq6);
            AddComma(q6.Count().ToString(), usageBuilderq6);
            HttpContext.Current.Response.Write(usageBuilderq6.ToString());
            HttpContext.Current.Response.Write(Environment.NewLine);

            var q = (from p in db.MetrixCustomCounts
                    where p.PackageID == packageId
                    select p.ValueGroup).Distinct();

            foreach (var groupValue in q)
            {
                HttpContext.Current.Response.Write(Environment.NewLine);
                HttpContext.Current.Response.Write(Environment.NewLine);

                string fn = groupValue + ", Count";
                HttpContext.Current.Response.Write(fn);
                HttpContext.Current.Response.Write(Environment.NewLine);

                var qData = from p in db.MetrixCustomCounts
                        where p.PackageID == packageId && p.ValueGroup == groupValue
                        select p;


                foreach (var data in qData)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    AddComma(data.ValueName, stringBuilder);
                    AddComma(data.ValueData.ToString(), stringBuilder);

                    HttpContext.Current.Response.Write(stringBuilder.ToString());
                    HttpContext.Current.Response.Write(Environment.NewLine);
                }
            }
                       
            HttpContext.Current.Response.End();
        }

        [WebMethod]
        public String GetMetrixCoreVersion()
        {
            return MetrixVersion;
        }

        [WebMethod]
        public XmlDocument GetMetrixCoreCRC(String metrixCoreVersion)
        {
            String result;
            String crc;

            switch (metrixCoreVersion)
            {
                case "0.3.2":
                    crc = "364524970";
                    break;

                default:
                    crc = "Unknown";
                    break;
            }

            XmlDocument xmlDoc = new XmlDocument();
            result = "<metrixCoreCRC><crc>" + crc + "</crc></metrixCoreCRC>";
            xmlDoc.LoadXml(result);
            return xmlDoc;
        }

        public long convertToUTC(DateTime dt)
        {
            return ((dt.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }

        public DateTime convertFromUTC(long utc)
        {
            DateTime dt = new DateTime(((utc * 10000000) + 621355968000000000));
            return dt;
        }

        private static void AddComma(string value, StringBuilder stringBuilder)
        {
            stringBuilder.Append(value.Replace(',', ' '));
            stringBuilder.Append(", ");
        }
    }
}
