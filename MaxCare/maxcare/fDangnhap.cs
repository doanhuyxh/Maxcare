using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using License;
using maxcare.Helper;
using maxcare.Properties;
using MCommon;
using MetroFramework.Controls;

namespace maxcare
{
	// Token: 0x020000DF RID: 223
	public partial class fDangnhap : Form
	{
		// Token: 0x060008E4 RID: 2276 RVA: 0x0000301A File Offset: 0x0000121A
		public fDangnhap()
		{
			this.InitializeComponent();
			this.ChangeLanguage();
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00114038 File Offset: 0x00112238
		private void ChangeLanguage()
		{
			Language.GetValue(this.lblTitle);

		}

        private void btn_dang_nhap_Click(object sender, EventArgs e)
        {
			String txt_username = this.txt_username.Text.Trim();
			String txt_password = this.txt_password.Text.Trim();
            if (txt_username == "")
            {
				MessageBox.Show("Vui lòng điền tên đăng nhập");
				return;
            }
			if (txt_password == "")
			{
				MessageBox.Show("Vui lòng điền mật khẩu");
				return;
			}
			LicenseInfo licenseInfo = Comon.CheckKeyAndLoginPro(txt_username, txt_password, "1234");
            if (licenseInfo == null)
            {
				MessageBox.Show("Thông tin không đúng, xin vui lòng liên hệ với softway.vn");
            }
            else
            {
				Hide();
				MCommon.Common.ShowForm(new fIntro());
			}

		}

        private void btn_close_Click(object sender, EventArgs e)
        {
			Close();
        }

        private void btn_open_create_account_Click(object sender, EventArgs e)
        {
			System.Diagnostics.Process.Start("https://softway.vn/dang-ky-license-fcare.html");
		}

        private void btn_link_trial_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
			DataTable result = new DataTable();
			string query = "SELECT * FROM config limit 1";
			result = Connector.Instance.ExecuteQuery(query);
			if(result.Rows.Count==0)
            {
				try
				{
					//interacts(uid, timeInteract,hanhDong,cauHinh) VALUES (
					query = "CREATE TABLE IF NOT EXISTS Config(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, data TEXT NOT NULL)";
					Connector2.Instance.ExecuteNonQuery(query);
					MessageBox.Show("sdfsdfds");
					//interacts(uid, timeInteract,hanhDong,cauHinh) VALUES (
					query = "INSERT INTO `Config`(data)  VALUES('222222')";
					Connector2.Instance.ExecuteNonQuery(query);
					MessageBox.Show("sdfsdfds");
				}
				catch(Exception error)
				{
					throw new ArgumentNullException("{0} Exception caught.", error);
				}


				
			}
			LicenseInfo licenseInfo = Comon.CheckKeyAndLoginPro("", "", "1234");
			if (licenseInfo == null)
			{
				MessageBox.Show("Thông tin không đúng, xin vui lòng liên hệ với softway.vn");
			}
			else
			{
				Hide();
				MCommon.Common.ShowForm(new fIntro());
			}
		}
    }
}
