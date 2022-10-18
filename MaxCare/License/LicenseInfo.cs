using System;
using System.Net;
using System.Text;
using License.RNCryptor;

namespace License
{
	// License.License
	using System;
	using System.Net;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Web.Script.Serialization;
	[DataContract]
	public class LicenseInfo
	{
		[DataMember]
		public string Full_Name
		{
			get; set;
		}
		[DataMember]
		public string Last_Name
		{
			get; set;
		}
		[DataMember]
		public string Email
		{
			get; set;
		}
		[DataMember]
		public Boolean Status
		{
			get; set;
		}

		[DataMember]
		public string Created_Date
		{
			get; set;
		}
		[DataMember]
		public string Modified_Date
		{
			get; set;
		}
		[DataMember]
		public string Package
		{
			get; set;
		}
		[DataMember]
		public DateTime Start_Date
		{
			get; set;
		}
		[DataMember]
		public DateTime End_Date
		{
			get; set;
		}
		public LicenseInfo()
		{
			
		}
		public static LicenseInfo Parse(string jsonString)
        {
			JavaScriptSerializer jss = new JavaScriptSerializer();
			LicenseInfo licenseInfo = jss.Deserialize<LicenseInfo>(jsonString);
			return licenseInfo;

		}
	}
    
}