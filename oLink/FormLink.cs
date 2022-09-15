using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO.Compression;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Configuration;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using System.Linq;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using Amazon.RDS;
using Amazon;
using Amazon.RDS.Model;
using System.Collections.Specialized;
using Amazon.CloudFront;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using Ganss.XSS;
using Amazon.CloudFront.Model;
using System.Runtime.InteropServices;

namespace oLink
{
    public partial class FormLink : Form
    {
        static private string sAppName = "oLink";
        static private string sVersion = "001";
        static private Point mp;
        static private Random random = new Random();
        static private string sUserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.162 Safari/537.36";
        static public string sWaited = "<div class=\"artn\"><p style=\"text-align:center;\">^wait^</p></div>";
        static public string sNFound = "<div class=\"artn\"><p style=\"text-align:center;\">解析失败</p></div>";
        static public string sConvert = "<div class=\"artn\"><p style=\"text-align:center;\">正在转换</p></div>";

        private static readonly byte[] BMP = { 66, 77 };
        private static readonly byte[] DOC = { 208, 207, 17, 224, 161, 177, 26, 225 };
        private static readonly byte[] EXE_DLL = { 77, 90 };
        private static readonly byte[] GIF = { 71, 73, 70, 56 };
        private static readonly byte[] ICO = { 0, 0, 1, 0 };
        private static readonly byte[] JPG = { 255, 216, 255 };
        private static readonly byte[] MP3 = { 255, 251, 48 };
        private static readonly byte[] OGG = { 79, 103, 103, 83, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] PDF = { 37, 80, 68, 70, 45, 49, 46 };
        private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
        private static readonly byte[] RAR = { 82, 97, 114, 33, 26, 7, 0 };
        private static readonly byte[] SWF = { 70, 87, 83 };
        private static readonly byte[] TIFF = { 73, 73, 42, 0 };
        private static readonly byte[] TORRENT = { 100, 56, 58, 97, 110, 110, 111, 117, 110, 99, 101 };
        private static readonly byte[] TTF = { 0, 1, 0, 0, 0 };
        private static readonly byte[] WAV_AVI = { 82, 73, 70, 70 };
        private static readonly byte[] WMV_WMA = { 48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 };
        private static readonly byte[] ZIP_DOCX = { 80, 75, 3, 4 };

        private static readonly string encrypt_fileext = ".dat";
        public FormLink()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                DataGridViewColumn cc = new DataGridViewCheckBoxColumn();
                cc.Name = cc.DataPropertyName = "Chk";
                cc.HeaderText = "Chk";
                cc.Width = 40;
                dataGridView1.Columns.Add(cc);

                DataGridViewColumn dgvc = new DataGridViewTextBoxColumn();
                dgvc.Name = dgvc.DataPropertyName = "Id";
                dgvc.HeaderText = "Id";
                dgvc.Width = 40;
                dgvc.ReadOnly = true;
                dataGridView1.Columns.Add(dgvc);

                dgvc = new DataGridViewTextBoxColumn();
                dgvc.Name = dgvc.DataPropertyName = "Name";
                dgvc.HeaderText = "Name";
                dgvc.Width = 480;
                dgvc.ReadOnly = true;
                dataGridView1.Columns.Add(dgvc);

                dataGridView1.ReadOnly = false;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.AllowUserToOrderColumns = false;
                dataGridView1.AllowUserToResizeColumns = true;
                dataGridView1.AllowUserToResizeRows = false;
                foreach (DataGridViewColumn col in dataGridView1.Columns) col.SortMode = DataGridViewColumnSortMode.NotSortable;

                //ShowConfig();
                //ShowDataGridView1();

                listprofilenames();
                PrepareResourceFiles4S3();

            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            finally
            {
                ShowhideDataTabs(false);
            }
        }

        private void PrepareResourceFiles4S3()
        {
            OrganizeLocalFolders();
            string localfiledirStr = ConfigurationManager.AppSettings["localfiledir"];

            NameValueCollection resrc_section = (NameValueCollection)ConfigurationManager.GetSection("ResourceList");

            foreach (string keystring in resrc_section.Keys)
            {
                string url_value = resrc_section.GetValues(keystring)[0];
                DownloadHtml(url_value, "", "", localfiledirStr + @"\" + keystring);
            }
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                mp = MousePosition;
            }
            catch (Exception ex) { ShowErrD(ex.Message); }
        }

        private void panelTop_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    Location = new Point(Left + MousePosition.X - mp.X, Top + MousePosition.Y - mp.Y);
                    mp = MousePosition;
                }
            }
            catch (Exception ex) { ShowErrD(ex.Message); }
        }

        private void ShowConfig()
        {
            try
            {
                DataSet ds = new DataSet();
                ExecuteSQL(ooData.sG10配置Get, ds);
                textBoxS3Id.Text = ds.Tables[0].Rows[0][0].ToString();
                textBoxS3Key.Text = ds.Tables[0].Rows[0][1].ToString();
                textBoxS3Bucket.Text = ds.Tables[0].Rows[0][2].ToString();
                textBoxName.Text = ds.Tables[0].Rows[0][3].ToString();
                textBoxNote.Text = ds.Tables[0].Rows[0][4].ToString();
                ds.Clear();
                ds = null;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        }

        private void ShowDataGridView1()
        {
            try
            {
                dataGridView1.Rows.Clear();
                DataSet ds = new DataSet();
                ExecuteSQL(ooData.sG10事物All, ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataGridViewRow r = new DataGridViewRow();
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[1].Value = ds.Tables[0].Rows[i][0].ToString();
                    dataGridView1.Rows[n].Cells[2].Value = ds.Tables[0].Rows[i][1].ToString();
                }
                ds.Clear();
                ds = null;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        }

        private void button2_Click(object sender, EventArgs e)
        {//save button on tabpage1, below Note
            try
            {
                string s1 = ooData.sG10配置Set_p;
                List<SqlParameter> sqlparms = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@S3Id", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(textBoxS3Id.Text.Trim())},
                    new SqlParameter() {ParameterName = "@S3Key", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(textBoxS3Key.Text.Trim())},
                    new SqlParameter() {ParameterName = "@S3Bucket", SqlDbType = SqlDbType.NChar, Value =  GetSqlParam(textBoxS3Bucket.Text.Trim())},
                    new SqlParameter() {ParameterName = "@Name", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(textBoxName.Text.Trim())},
                    new SqlParameter() {ParameterName = "@Note", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(textBoxNote.Text.Trim())}
                };
                ExecuteSQL(s1, null, sqlparms);
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //"Add" "Website Url" button
            try
            {
                string s1 = textBox3.Text.Trim();
                if (s1 == "" || (!s1.StartsWith("https://") && !s1.StartsWith("http://"))) return;
                //string s4 = ooData.sG10事物Add
                //    .Replace("^名称^", GetSqlParam(s1));

                string s4 = ooData.sG10事物Add_p;
                List<SqlParameter> sqlparms = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@名称", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(s1)}
                };
                //ExecuteSQL(s4);
                ExecuteSQL(s4, null, sqlparms);
                ShowDataGridView1();
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == false) continue;
                    //string s4 = ooData.sG10事物Del
                    //    .Replace("^序号^", GetSqlParam(dataGridView1.Rows[i].Cells[1].Value.ToString()));

                    string s4 = ooData.sG10事物Del_p;
                    List<SqlParameter> sqlparms = new List<SqlParameter>()
                    {
                        new SqlParameter() {ParameterName = "@序号", SqlDbType = SqlDbType.Int, Value = GetSqlParam(dataGridView1.Rows[i].Cells[1].Value.ToString())}
                    };
                    ExecuteSQL(s4, null, sqlparms);
                }
                ShowDataGridView1();
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Retrieve button
            Thread thread = new Thread(Retrieve);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Retrieve()
        {
            try
            {
                ShowMsgD("Retrieve Start");
                DataSet ds = new DataSet();
                ExecuteSQL(ooData.sG10事物All, ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string s1 = ds.Tables[0].Rows[i][0].ToString();
                    string s2 = ds.Tables[0].Rows[i][1].ToString();
                    ShowMsgD((i + 1) + "/" + ds.Tables[0].Rows.Count + " " + s2);

                    GetArticle(s2, out string s标题, out string s摘要, out string s封面, out string s时间);
                    if (s标题 != "")
                    {
                        if (s封面 != "") s封面 = GetFile(s封面);
                        //string sql = ooData.sG10事物Set
                        //    .Replace("^标题^", GetSqlParam(s标题))
                        //    .Replace("^摘要^", GetSqlParam(s摘要))
                        //    .Replace("^封面^", GetSqlParam(s封面))
                        //    .Replace("^名称^", GetSqlParam(s2));
                        string sql = ooData.sG10事物Set_p;
                        List<SqlParameter> sqlparms = new List<SqlParameter>()
                        {
                            new SqlParameter() {ParameterName = "@标题", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(s标题)},
                            new SqlParameter() {ParameterName = "@摘要", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(s摘要)},
                            new SqlParameter() {ParameterName = "@封面", SqlDbType = SqlDbType.NVarChar, Value =  GetSqlParam(s封面)},
                            new SqlParameter() {ParameterName = "@名称", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(s2)}
                        };
                        string s3 = ExecuteSQLResult(sql, sqlparms);

                        string s4 = GetHtmlPlayer(s2, s封面);
                        s4 = s4.Replace("[链接[", "<a href=\"javascript:void(0);\" onclick=\"javascript:Load(encodeURI('")
                            .Replace("[链接]", "'));\">").Replace("[链接当前]", "'));\">").Replace("]链接]", "</a>");
                        s4 = ooView.sShow.Replace("^文章^", s4);
                        SaveFile(s4, "Site/c" + int.Parse(s3).ToString("D6") + ".htm");
                    }
                }
                ds.Clear();
                ds = null;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            ShowMsgD("Retrieve Done");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Reconstruct);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Reconstruct()
        {
            try
            {
                ShowMsgD("Reconstruct Start");
                string s6 = "";
                DataSet ds = new DataSet();
                ExecuteSQL(ooData.sG10事物All, ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string s1 = ds.Tables[0].Rows[i][0].ToString();
                    string s2 = ds.Tables[0].Rows[i][1].ToString();
                    string s3 = ds.Tables[0].Rows[i][2].ToString();
                    string s4 = ds.Tables[0].Rows[i][3].ToString();
                    string s5 = ds.Tables[0].Rows[i][4].ToString();
                    ShowMsgD((i + 1) + "/" + ds.Tables[0].Rows.Count + " " + s2);
                    s6 += ooView.sCard
                        .Replace("^序号^", int.Parse(s1).ToString("D6"))
                        .Replace("^标题^", GetHtmlParam(s3))
                        .Replace("^摘要^", GetHtmlParam(s4))
                        .Replace("^封面^", GetHtmlParam(s5));
                }
                ds.Clear();
                ds = null;
                s6 = ooView.sHome.Replace("^卡片^", s6)
                    .Replace("^Name^", GetHtmlParam(textBoxName.Text.Trim()))
                    .Replace("^Note^", GetHtmlParam(textBoxNote.Text.Trim()));
                SaveFile(s6, "Site/olHome.htm");
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            ShowMsgD("Reconstruct Done");
        }

        private void GetArticle(string name0, out string sTitle, out string sAdstract, out string sCover, out string sTime)
        {
            sTitle = sAdstract = sCover = sTime = "";
            try
            {
                string r = GetHtml(name0);
                if (name0.StartsWith("https://www.minghui.org/") && r.Contains("<meta http-equiv=\"refresh\""))
                {
                    Match m0 = new Regex("(?<=url=/)([\\S]*?)(?=\")").Match(r);
                    r = GetHtml("https://www.minghui.org/" + m0.Value);
                }
                if (r == "" || r.Contains("访问的页面不存在，点击回到首页")) return; //Soh
                sTitle = Regex.Match(r, @"(?<=property=[""|']og:title[""|']\s*?content=[""|'])([\S\s]*?)(?=[""|'])").Value.Trim();
                if (sTitle == "") sTitle = Regex.Match(r, @"(?<=<title>)([\S\s]*?)(?=</title>)").Value.Trim();
                sTitle = sTitle.Replace(" — 普通話主頁", "");
                if (sTitle == "") return;
                sAdstract = Regex.Match(r, @"(?<=property=[""|']og:description[""|']\s*?content=[""|'])([\S\s]*?)(?=[""|'])").Value.Trim();
                if (sAdstract == "") sAdstract = Regex.Match(r, @"(?<=content=[""|'])([\S]*?)(?=[""|']\s*?property=[""|']og:description[""|'])").Value.Trim();
                if (sAdstract == "") sAdstract = Regex.Match(r, @"(?<=name=[""|']description[""|']\s*?content=[""|'])([\S\s]*?)(?=[""|'])").Value.Trim();
                sCover = Regex.Match(r, @"(?<=property=[""|']og:image[""|']\s*?content=[""|'])([\S\s]*?)(?=[""|'])").Value.Trim();
                if (sCover == "") sCover = Regex.Match(r, @"(?<=content=[""|'])([\S]*?)(?=[""|']\s*?property=[""|']og:image[""|'])").Value.Trim();
                if (sCover == "") sCover = Regex.Match(r, @"(?<=name=[""|']twitter:image[""|']\s*?content=[""|'])([\S\s]*?)(?=[""|'])").Value.Trim();
                if (sCover == "") sCover = Regex.Match(r, @"(?<=name=[""|']twitter:image:src[""|']\s*?content=[""|'])([\S\s]*?)(?=[""|'])").Value.Trim(); //
                //s封面 = s封面.Replace("maxresdefault.jpg", "mqdefault.jpg"); //Ytb
                sTime = Regex.Match(r, @"(?<=property=[""|']article:published_time[""|']\s*?content=[""|'])([\S\s]*?)(?=[""|'])").Value.Trim();
                //Match m6 = new Regex("(?<=<meta name=\"date\" content=\")([\\S\\s]*?)(?=\")").Match(r2);
                if (sTime == "") sTime = Regex.Match(r, @"(?<=""datePublished"":"")([\S\s]*?)(?="")").Value.Trim(); //Abl
                if (sTime != "")
                    if (DateTime.TryParse(sTime, out DateTime dt) && dt < DateTime.Now && dt > DateTime.Parse("1990-1-1"))
                        sTime = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    else sTime = "";
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        }

        private string GetFile(string name)
        {
            try
            {
                //string s1 = ooData.sG10文件Add
                //    .Replace("^名称^", GetSqlParam(name));
                string s1 = ooData.sG10文件Add_p;
                List<SqlParameter> sqlparms = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@名称", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(name)}
                };

                //string s2=ExecuteSQLResult(s1);
                string s2 = ExecuteSQLResult(s1, sqlparms);

                string s4 = "File/" + int.Parse(s2).ToString("D6");
                if (name.EndsWith(".jpg")) s4 += ".jpg";
                if (name.EndsWith(".jpeg")) s4 += ".jpg";
                if (name.EndsWith(".png")) s4 += ".png";
                if (name.EndsWith(".gif")) s4 += ".gif";
                if (name.EndsWith(".webp")) s4 += ".webp";
                if (name.EndsWith(".mp3")) s4 += ".mp3";
                if (name.EndsWith(".mp4")) s4 += ".mp4";
                if (name.EndsWith("@@images/image") || name.EndsWith("@@images/image/social_media")) s4 += ".jpg";
                if (name.EndsWith("@@stream")) s4 += ".mp3";
                DownloadHtml(name, "", "", s4);
                return "../" + s4;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            return "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Upload);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Upload()
        {
            ShowMsgD("Start Upload");
            try
            {
                ClearDangerousFilesinLocalFolders();    //File check before upload 8/2022
                string s0 = Path.GetDirectoryName(Application.ExecutablePath);
                AmazonS3Client s3cli = AWSConnections.getAWSS3Cli(tbxAccKey.Text, tbxSecKey.Text);
                string st_myS3name = AWSConnections.getAWSS3Client(tbxAccKey.Text, tbxSecKey.Text);

                if (s3cli == null)
                {
                    string s = @"set AWS_ACCESS_KEY_ID=^S3Id^
                                set AWS_SECRET_ACCESS_KEY=^S3Key^
                                aws s3 sync C:\oLink\Site s3://^S3Bucket^/Site --acl public-read --region eu-west-1
                                aws s3 sync C:\oLink\File s3://^S3Bucket^/File --acl public-read --region eu-west-1"
                                .Replace("^S3Id^", textBoxS3Id.Text.Trim())
                                .Replace("^S3Key^", textBoxS3Key.Text.Trim())
                                .Replace("^S3Bucket^", textBoxS3Bucket.Text.Trim());
                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
                    p.Start();
                    p.BeginOutputReadLine();
                    p.StandardInput.WriteLine(s);
                    p.StandardInput.WriteLine("exit");
                    p.WaitForExit();
                    if (p.ExitCode != 0) ShowMsgD(p.StandardError.ReadToEnd());
                }
                else
                {
                    string remote_folderPath = "Site";
                    string Local_folderpath = s0 + @"\Site\";// @"C:\oLink\Site";
                    //S3DirectoryInfo directory = new S3DirectoryInfo(s3cli, st_myS3name, remote_folderPath);
                    //directory.MoveFromLocal(Local_folderpath);
                    var transferUtility = new TransferUtility(s3cli);
                    TransferUtilityUploadDirectoryRequest tuudr_request = new TransferUtilityUploadDirectoryRequest()
                    {
                        BucketName = st_myS3name,
                        Directory = Local_folderpath,
                        KeyPrefix = remote_folderPath
                    };
                    transferUtility.UploadDirectory(tuudr_request);


                    remote_folderPath = "File";
                    Local_folderpath = s0 + @"\File\";// @"C:\oLink\File";
                    //directory = new S3DirectoryInfo(s3cli, st_myS3name, remote_folderPath);
                    //directory.MoveFromLocal(Local_folderpath);

                    tuudr_request = new TransferUtilityUploadDirectoryRequest()
                    {
                        BucketName = st_myS3name,
                        Directory = Local_folderpath,
                        KeyPrefix = remote_folderPath
                    };
                    transferUtility.UploadDirectory(tuudr_request);

                    transferUtility.Dispose();
                }


            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            ShowMsgD("Done Upload");
        }

        private void OnOutputDataReceived(object Sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e.Data != null && e.Data != "")
                {
                    string s = e.Data;
                    if (s.StartsWith("upload")) ShowMsgD(e.Data);
                }
            }
            catch { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Generate);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Generate()
        {
            try
            {
                string cf_url = retrievecfurl();
                string url = "";
                string cfsitedn = ConfigurationManager.AppSettings["cfdn"];
                if (!cf_url.ToUpper().Contains(cfsitedn.ToUpper()) || !ckbcloudfront.Checked)
                {
                    url = "https://s3.amazonaws.com/^S3Bucket^/Site/show.htm?ag=olHome&pin=^random^#olHome"
                      .Replace("^S3Bucket^", textBoxS3Bucket.Text.Trim())
                      .Replace("^random^", GetRandom());
                }
                else
                {
                    url = "https://" + cf_url + "/Site/show.htm?ag=olHome&pin=^random^#olHome"
                    .Replace("^random^", GetRandom());
                }
                TogglePublicAccessBlock(ckbcloudfront.Checked);

                string s = url;
                if (ckbShortURL.Checked)
                    s = GetHtmlMethod("GET", "https://is.gd/create.php?format=simple&url=" + EnUrlSymbol(url), "", "", "", "");
                ShowMsgD(s);
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        }

        private string retrievecfurl()
        {
            string awsAccessKey = tbxAccKey.Text;
            string awsSecretAccessKey = tbxSecKey.Text;
            try
            {
                AmazonCloudFrontClient acfcli = new AmazonCloudFrontClient(awsAccessKey, awsSecretAccessKey);

                if (acfcli != null)
                {
                    var dl = acfcli.ListDistributions();
                    if (dl.ContentLength > 0)
                    {
                        foreach (var ditem in dl.DistributionList.Items)
                        {
                            foreach (var dorg in ditem.Origins.Items)
                            {
                                if (dorg.DomainName.ToUpper().Contains(tbxS3buckets.Text.ToUpper()))
                                {
                                    //if (testurlaccessible(ditem.DomainName))
                                    return ditem.DomainName;
                                }
                            }
                        }
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return "";
        }

        private bool testurlaccessible(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;// | SecurityProtocolType.Tls13;
            //ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AlwaysGoodCertificate);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return CertificateValidationCallBack(sender, certificate, chain, errors); };
            HttpClient client = new HttpClient();

            HttpResponseMessage checkingResponse = client.GetAsync(url).Result;
            if (!checkingResponse.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }


        // Player
        static public string GetVideoPlayer(string url, string track = "", string cover = "", string myip = "", bool bDownload = true)
        {
            if (url == "") return "";
            if (url.StartsWith("https://player.vimeo.com/video/")) url = url.Replace("https://player.vimeo.com/", "https://vimeo.com/");
            if (url.StartsWith("https://www.youtube.com/") || url.StartsWith("https://www.youtube.com/embed/"))
                url = url.Replace("https://www.youtube.com/watch?v=", "https://youtu.be/").Replace("https://www.youtube.com/embed/", "https://youtu.be/");

            string sVideoL = ""; string sVideoM = ""; string sVideoH = ""; string sVideoV = ""; string sVideoM2 = ""; string sVideoV2 = "";
            string sAudio140 = ""; string sAudio171 = ""; string sAudio249 = ""; string sAudio250 = ""; string sAudio251 = "";
            string sTrack = "";
            string sTrackZh = "", sTrackEn = "";
            string sTrackZhSrc = "", sTrackEnSrc = "";
            if (track != "" && track.EndsWith(".vtt"))
            {
                sTrackZh = "zh";
                sTrackZhSrc = EnUrl2(track);
                sTrack += "\r\n<track kind=\"subtitles\" label=\"中文\" srclang=\"zh\" src=\"" + sTrackZhSrc + "\" default>";
            }

            url = GetEnUrl(url);
            string id = GetRandomNum();
            string r = @"<div class=""videocontainer"">
    <video id=""v^id^"" controls ^autoplay^ preload=""^preload^"" crossorigin=""anonymous"" width=""100%"" x-webkit-airplay=""true"" airplay=""allow"""
                .Replace("^autoplay^", "")
                .Replace("^preload^", (cover != "" ? "auto" : "auto")) /// none
                + @" src=""" + url + @""" ^poster^>"
                .Replace("^poster^", (cover != "" ? "poster=\"" + cover + "\"" : ""))
                + sTrack
                + @"
    </video>
    <div class=""navv""><div class=""nav"" style=""width:100%; height:28px; position:absolute; top:0; background:rgba(255,255,255,0.8);"">
        <ul class=""nav__menu"" style=""text-align:right;"">";
            if (sTrack != "")
            {
                r += @"
            <li class=""nav__menu-item""><a>字幕</a>
                <ul class=""nav__submenu"">";
                if (sTrackZh != "") r += @"
                    <li class=""nav__submenu-item"" onclick='javascript:if (document.getElementById(""v^id^"").textTracks[^no^].mode!=""showing"") document.getElementById(""v^id^"").textTracks[^no^].mode=""showing""; else document.getElementById(""v^id^"").textTracks[^no^].mode=""hidden"";'><a>^label^</a></li>".Replace("^id^", id).Replace("^no^", "0").Replace("^label^", "中文");
                if (sTrackEn != "") r += @"
                    <li class=""nav__submenu-item"" onclick='javascript:if (document.getElementById(""v^id^"").textTracks[^no^].mode!=""showing"") document.getElementById(""v^id^"").textTracks[^no^].mode=""showing""; else document.getElementById(""v^id^"").textTracks[^no^].mode=""hidden"";'><a>^label^</a></li>".Replace("^id^", id).Replace("^no^", (sTrackZh == "" ? "0" : "1")).Replace("^label^", "外文");
                r += @"
                </ul>
            </li>";
            }
            if (sAudio140 != "") r += @"
            <li class=""nav__menu-item""><a>音频</a>
                <ul class=""nav__submenu"">"
                      + (sAudio140 == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^audio^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>高清</a></li>".Replace("^id^", id).Replace("^audio^", EnUrl2(sAudio140)))
                      + (sAudio171 == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^audio^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>高清</a></li>".Replace("^id^", id).Replace("^audio^", EnUrl2(sAudio171)))
                      + (sAudio249 == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^audio^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>低清</a></li>".Replace("^id^", id).Replace("^audio^", EnUrl2(sAudio249)))
                      + (sAudio250 == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^audio^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>标清</a></li>".Replace("^id^", id).Replace("^audio^", EnUrl2(sAudio250)))
                      + (sAudio251 == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^audio^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>超清</a></li>".Replace("^id^", id).Replace("^audio^", EnUrl2(sAudio251)))
                      + @"
                </ul>
            </li>";
            if (sVideoM != "") r += @"
            <li class=""nav__menu-item""><a>视频</a>
                <ul class=""nav__submenu"">"
                      + (sVideoV == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^videov^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>超清</a></li>".Replace("^id^", id).Replace("^videov^", EnUrl2(sVideoV)))
                      + (sVideoH == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^videoh^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>高清</a></li>".Replace("^id^", id).Replace("^videoh^", EnUrl2(sVideoH)))
                      + (sVideoM == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^videom^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>标清</a></li>".Replace("^id^", id).Replace("^videom^", EnUrl2(sVideoM)))
                      + (sVideoL == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^videol^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>低清</a></li>".Replace("^id^", id).Replace("^videol^", EnUrl2(sVideoL)))
                      + (sVideoV2 == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^videov^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>超二</a></li>".Replace("^id^", id).Replace("^videov^", EnUrl2(sVideoV2)))
                      + (sVideoM2 == "" ? "" : @"
                    <li class=""nav__submenu-item"" onclick='javascript:var ct=document.getElementById(""v^id^"").currentTime; document.getElementById(""v^id^"").src=""^videom^""; document.getElementById(""v^id^"").currentTime=ct; document.getElementById(""v^id^"").play();'><a>标二</a></li>".Replace("^id^", id).Replace("^videom^", EnUrl2(sVideoM2)))
                      + @"
                </ul>
            </li>";
            r += @"
            <li class=""nav__menu-item""><a>速度</a>
                <ul class=""nav__submenu"">
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=0.5;'><a>0.5</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=1;'><a>1</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=1.25;'><a>1.25</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=1.5;'><a>1.5</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=2;'><a>2</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=3;'><a>3</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=4;'><a>4</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=8;'><a>8</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=16;'><a>16</a></li>
                </ul>
            </li>
            <li class=""nav__menu-item""><a>功能</a>
                <ul class=""nav__submenu"">
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").play();'><a>播放</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").pause();'><a>暂停</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").currentTime+=10;'><a>快进</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").currentTime-=10;'><a>快退</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:var v=document.getElementById(""v^id^""); if (v.requestFullscreen) {v.requestFullscreen();} else if (v.webkitRequestFullscreen) {v.webkitRequestFullScreen();} else if (v.mozRequestFullScreen) {v.mozRequestFullScreen();} else if (v.msRequestFullscreen) {v.msRequestFullscreen();} else if (v.oRequestFullscreen) {v.oRequestFullscreen();}'><a>全屏</a></li>
                </ul>
            </li>";
            if (bDownload)
            {
                r += @"
            <li class=""nav__menu-item""><a>下载</a>
                <ul class=""nav__submenu"">
                    <li class=""nav__submenu-item"" onclick='javascript:Save(document.getElementById(""v^id^"").src);'><a>音视</a></li>".Replace("^id^", id);
                if (sTrackZh != "") r += @"
                    <li class=""nav__submenu-item"" onclick='javascript:Save(""^src^"");'><a>中文</a></li>".Replace("^src^", sTrackZhSrc);
                if (sTrackEn != "") r += @"
                    <li class=""nav__submenu-item"" onclick='javascript:Save(""^src^"");'><a>英文</a></li>".Replace("^src^", sTrackEnSrc);
                r += @"
                </ul>
            </li>";
            }
            r += @"
         </ul>
    </div></div>
</div>";
            return r.Replace("^id^", id);
        }

        private static string GetYoutube(string s3)
        {
            return s3;
        }

        private static string GetAudioPlayer(string url, string cover = "")
        {
            url = GetEnUrl(url);
            string id = GetRandomNum();
            return @"<div class='videocontainer'>
    <video id=""v^id^"" controls ^autoplay^ preload=""auto"" crossorigin=""anonymous"" style=""width:100%; height:180px; background:#faf2e1;""
        src=""^url^"" ^poster^>
    </video>
    <div class=""navv""><div class=""nav"" style=""width:100%; height:28px; position:absolute; top:0;"">
        <ul class=""nav__menu"" style=""text-align:right;"">           
            <li class=""nav__menu-item""><a>速度</a>
                <ul class=""nav__submenu"">
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=0.5;'><a>0.5</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=1;'><a>1</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=1.25;'><a>1.25</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=1.5;'><a>1.5</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=2;'><a>2</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=3;'><a>3</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").playbackRate=4;'><a>4</a></li>
                </ul>
            </li>
            <li class=""nav__menu-item""><a>功能</a>
                <ul class=""nav__submenu"">
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").play();'><a>播放</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").pause();'><a>暂停</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").currentTime+=10;'><a>快进</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:document.getElementById(""v^id^"").currentTime-=10;'><a>快退</a></li>
                    <li class=""nav__submenu-item"" onclick='javascript:var v=document.getElementById(""v^id^""); if (v.requestFullscreen) {v.requestFullscreen();} else if (v.webkitRequestFullscreen) {v.webkitRequestFullScreen();} else if (v.mozRequestFullScreen) {v.mozRequestFullScreen();} else if (v.msRequestFullscreen) {v.msRequestFullscreen();} else if (v.oRequestFullscreen) {v.oRequestFullscreen();}'><a>全屏</a></li>
                </ul>
            </li>
            <li class=""nav__menu-item""><a href=""javascript:void(0);"" onclick=""javascript:Save('^url^');"">下载</a></li>
        </ul>
    </div></div>
</div>"
            .Replace("^autoplay^", (cover != "" ? "" : "")).Replace("^url^", url).Replace("^id^", id) /// autoplay
            .Replace("^poster^", (cover != "" ? "poster=\"" + cover + "\"" : ""));
        }

        static private string GetImagePlayerTop(string url)
        {
            url = GetEnUrl(url);
            return @"<div class='videocontainer'>
    <img style='width:100%;' src='^url^'/>
    <div class=""navv""><div class=""nav"" style=""width:100%; height:28px; position:absolute; top:0;"">
        <ul class=""nav__menu"" style=""text-align:right;"">
            <li class=""nav__menu-item""><a href=""javascript:void(0);"" onclick=""javascript:Save('^url^');"">下载</a></li>
        </ul>
    </div></div>
</div>"
            .Replace("^url^", url);
        }

        static private string GetEnUrl(string url)
        {
            return EnUrl2(url);
        }

        public string GetHtmlPlayer(string name, string cover)
        {
            string co = "";
            string coMedia = "";
            try
            {
                name = UrlProc(name);
                co = GetHtml(name.Replace("#", ""), true);
                if (co == "") return sNFound;
                if (name.StartsWith("https://www.minghui.org/") && co.Contains("<meta http-equiv=\"refresh\""))
                {
                    Match m0 = new Regex("(?<=url=/)([\\S]*?)(?=\")").Match(co);
                    co = GetHtml("https://www.minghui.org/" + m0.Value, true);
                    if (co == "") return sNFound;
                }

                if (name.StartsWith("https://www.minghui.org/") && name.EndsWith("#"))
                {
                    Match mm = new Regex("(?<=src=\")([\\S]*?).mp3(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=src=\")([\\S]*?).mp4(?=\")").Match(co);
                    coMedia = mm.Value;
                    co = "";
                }
                else if (name.StartsWith("https://www.mhradio.org/") && name.EndsWith("#"))
                {
                    Match m = new Regex("(?<=href=\")([\\S]*?).mp3(?=\")").Match(co);
                    coMedia = m.Value.StartsWith("http") ? m.Value : "https://www.mhradio.org" + m.Value;
                    co = "";
                }
                else if (name.StartsWith("https://www.zhengjian.org/") && name.EndsWith("#"))
                {
                    Match m = new Regex("(?<=src=\")([\\S]*?).mp3(?=\")").Match(co);
                    coMedia = m.Value.StartsWith("http") ? m.Value : "http://www.zhengjian.org" + m.Value;
                    co = "";
                }
                else if (name.StartsWith("https://www.ntdtv.com/") && name.EndsWith("#"))
                {
                    Match m = new Regex("(?<=src=\")(//www.youmaker.com/assets/player/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!m.Success) m = new Regex("(?<=src=\")(//vs.youmaker.com/assets/player/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!m.Success) m = new Regex("(?<=data-url=\")(//vs.youmaker.com/assets/player/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!m.Success) m = new Regex("(?<=href=\")([\\S]*?).mp4(?=\")").Match(co);
                    if (!m.Success) m = new Regex("(https://www.youtube.com/embed/)([\\S]*?)(?=\")").Match(co);
                    coMedia = m.Value;
                    co = "";
                }
                else if (name.StartsWith("https://www.epochtimes.com/") && name.EndsWith("#"))
                {
                    Match m = new Regex("(?<=src=\")(//www.youmaker.com/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!m.Success) m = new Regex("(?<=href=\")([\\S]*?.mp4)(?=\")").Match(co);
                    if (!m.Success) m = new Regex("(https://www.youtube.com/embed/)([\\S]*?)(?=\")").Match(co);
                    coMedia = m.Value;
                    co = "";
                }
                else if (name.StartsWith("https://www.soundofhope.org/") && name.EndsWith("#"))
                {
                    Match m = new Regex("(?<=audio.media\":\\{\"link\":\")([\\S]*?\\.mp3)(?=\")").Match(co);
                    if (!m.Success) m = new Regex("(https://www.youtube.com/embed/)([\\S]*?)(?=\')").Match(co);
                    coMedia = m.Value;
                    if (coMedia.StartsWith("//")) coMedia = "https:" + coMedia;
                    co = "";
                }
                else if (name.StartsWith("https://www.rfa.org/") && name.EndsWith("#"))
                {
                    Match m = new Regex("(?<=src=\")([\\S]*?).mp3(?=\")").Match(co);
                    coMedia = m.Value;
                    co = "";
                }
                else if (name.StartsWith("https://www.minghui.org/"))
                {
                    Match m1 = new Regex("(<h1>)([\\S\\s]*?)(</h1>)").Match(co);
                    if (!m1.Success) m1 = new Regex("(?<=<div class=\"article_title\">)([\\S\\s]*?)(?=<div)").Match(co);
                    Match m2 = new Regex("(?<=<div id=\"ar_bArticleContent\" class=\"ar_articleContent\">)([\\S\\s]*?)(?=</div>([\\s]*?)<div)").Match(co);
                    if (!m2.Success) m2 = new Regex("(<div id=\"bArticleContent\" class=\"article_content\">)([\\S\\s]*?)(?=<!-- article_content -->)").Match(co);
                    if (!m2.Success) m2 = new Regex("(<div id=\"bArticleContent\" class=\"articleContent\">)([\\S\\s]*?)(?=</div>([\\s]*?)<div)").Match(co);
                    Match m3 = new Regex("(?<=<div class=\"ar_articleAuthor\">)([\\S\\s]*?)(?=</div>)").Match(co);
                    Match mm = new Regex("(?<=src=\")([\\S]*?).mp3(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=src=\")([\\S]*?).mp4(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(https://www.youtube.com/embed/)([\\S]*?)(?=\")").Match(co);
                    if (mm.Success) coMedia = mm.Value;
                    co = m1.Value;
                    if (m3.Success) co += "\r\n" + m3.Value + "<br/><br/>";
                    co += "\r\n" + m2.Value;
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.mhradio.org/"))
                {
                    Match m1 = new Regex("(<div id=\"articlebody\">)([\\S\\s]*?)(<div id=\"relatedarticlesbody\">)").Match(co);
                    Match m = new Regex("(?<=href=\")([\\S]*?).mp3(?=\")").Match(co);
                    if (m.Success) coMedia = m.Value.StartsWith("http") ? m.Value : "https://www.mhradio.org" + m.Value;
                    co = m1.Value;
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.zhengjian.org/"))
                {
                    Match m1 = new Regex("(<h1 class=\"page-header\">)([\\S\\s]*?)(</h1>)").Match(co);
                    Match m2 = new Regex("(<article)([\\S\\s]*?)(</article>)").Match(co);
                    Match mm = new Regex("(?<=src=\")([\\S]*?).mp3(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(https://www.youtube.com/embed/)([\\S]*?)(?=\")").Match(co);
                    if (mm.Success) coMedia = (mm.Value.StartsWith("http") ? mm.Value : "http://www.zhengjian.org" + mm.Value);
                    co = m1.Value + "\r\n" + m2.Value;
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.epochtimes.com/"))
                {
                    Match m1 = new Regex("(<div class=\"arttop arttop2\">)([\\S\\s]*?)(?=<div id=\"artbody\" class=\"column\">)").Match(co);
                    if (!m1.Success) m1 = new Regex("(<h1)([\\S\\s]*?)(</h1>)").Match(co);
                    if (!m1.Success) m1 = new Regex("(<div class=\"arttop mbottom20\">)([\\S\\s]*?)(?=<header role=\"heading\">)").Match(co);
                    Match m2 = new Regex("(?<=<!-- article content begin -->)([\\S\\s]*?)(?=<!-- article content end -->)").Match(co);
                    if (!m2.Success) m2 = new Regex("(?<=<div class=\"art-content\">)([\\S\\s]*?)(?=</div>)").Match(co);
                    Match m3 = new Regex(@"(?<=<div class=""blue16 subtitle mtop10"">)(.*?)(?=</div>)").Match(co); // 作者
                    if (!m3.Success) m3 = new Regex(@"(?<=<h4 class=""author"">)(.*?)(?=</h4>)").Match(co);
                    Match mm = new Regex("(?<=src=\")(//www.youmaker.com/assets/player/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=src=\")(//vs.youmaker.com/assets/player/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=src=\")(//vs.youmaker.com/)([\\S]*?)(?=\\?api=)").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=src=\")(//www.youmaker.com/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!mm.Success) mm = new Regex("(https://www.youtube.com/embed/)([\\S]*?)(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=href=\")([\\S]*?)(.jpg)(?=\")").Match(co);
                    if (mm.Success) coMedia = mm.Value;
                    co = m1.Value + "\r\n<h4>" + m3.Value + "</h4>\r\n" + m2.Value;
                    co = HtmlDel(co, "<video([\\S\\s]*?)</video>");
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("http://ccpsecretorigin.blog.epochtimes.com/"))
                {
                    Match m1 = new Regex("(?<=<div class=\"articletitle\">)([\\S\\s]*?)(?=</div>)").Match(co);
                    Match m2 = new Regex("(?<=<div class=\"articlecontent\">)([\\S\\s]*?)(?=</div>)").Match(co);
                    co = HtmlDel2("<h1>" + m1.Value + "</h1>\r\n" + m2.Value.Replace("<br />\r\n<br />", "</p>\r\n<p>").Replace("<p>", "<p class=\"artl\">"), name);
                }
                else if (name.StartsWith("https://www.ntdtv.com/"))
                {
                    Match m1 = new Regex("(?<=<h1>)([\\S\\s]*?)(?=</h1>)").Match(co);
                    if (!m1.Success) m1 = new Regex("(?<=<div class=\"main_title\">)([\\S\\s]*?)(?=</div>)").Match(co);
                    Match m3 = new Regex("(<div class=\"post_content\")([\\S\\s]*?)(<div class=\"post_related\">)").Match(co);
                    if (!m3.Success) m3 = new Regex("(?<=<div class=\"article_content\">)([\\S\\s]*?)(?=</div>)").Match(co);
                    Match mm = new Regex("(?<=src=\")(//www.youmaker.com/assets/player/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=src=\")(//vs.youmaker.com/assets/player/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=data-url=\")(//vs.youmaker.com/assets/player/)([\\S]*?)(?=\\?r=)").Match(co);
                    if (!mm.Success) mm = new Regex("(https://www.youtube.com/embed/)([\\S]*?)(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=href=\")([\\S]*?).mp4(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=content=\")(https://www.ntdtv.com/assets/)([\\S]*?)(.jpg)(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=content=\")(https://cn.ntdtv.com/assets/)([\\S]*?)(.jpg)(?=\")").Match(co);
                    if (mm.Success && !mm.Value.EndsWith(".flv")) coMedia = mm.Value;
                    co = "<h1>" + m1.Value + "</h1>\r\n" + m3.Value;
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.epochweekly.com/"))
                {
                    Match m2 = null;
                    if (name.EndsWith(".htm"))
                    {
                        m2 = new Regex("(<h1)([\\S\\s]*?)(?=<table>)").Match(co);
                        co = m2.Value;
                    }
                    else
                    {
                        m2 = new Regex("(?<=<!-- Row -->)([\\S\\s]*?)(?=<!-- Row /- -->)").Match(co);
                        co = m2.Value.Replace("href=\"", "href=\"http://www.epochweekly.com").Replace("</span>", "<br/>");
                    }
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.soundofhope.org/"))
                {
                    Match m1 = new Regex("(<h1 class=\"title\">)([\\S\\s]*?)(</h1>)").Match(co);
                    Match m2 = new Regex("(<div class=\"Content__Wrapper)([\\S\\s]*?)(?=<div class=\"SocialFP__SocialFP)").Match(co);
                    Match mm = new Regex("(https://www.youtube.com/embed/)([\\S]*?)(?=\')").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=//cdn.iframe.ly/api/iframe\\?url=)([\\S]*?)(?=&amp;key=)").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=audio.media\":\\{\"link\":\")([\\S]*?\\.mp3)(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=audios.0\":\\{\"link\":\")([\\S]*?\\.mp3)(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=\\<meta property=\"og:image\" content=\")([\\S]*?)(?=\")").Match(co);
                    if (mm.Success) coMedia = mm.Value.Replace("%3A", ":").Replace("%2F", "/").Replace("%3F", "?").Replace("%3D", "=");
                    if (coMedia.StartsWith("//")) coMedia = "https:" + coMedia;
                    co = m1.Value + "\r\n" + m2.Value;
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("http://mob.soundofhope.org/"))
                {
                    Match m1 = new Regex("(<article)([\\S\\s]*?)(</article>)").Match(co);
                    co = HtmlDel2(m1.Value, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.secretchina.com/"))
                {
                    Match m1 = new Regex("(<h1 style=\"padding:)([\\S\\s]*?)(</h1>)").Match(co);
                    Match m2 = new Regex("(?<=<div class=\"article_right\" style=\"fone-color:#000\">)([\\S\\s]*?)(?=<div id='SC-21'>)").Match(co);
                    if (!m2.Success) m2 = new Regex("(?<=<div class=\"article_right\" style=\"fone-color:#000\">)([\\S\\s]*?)(?=<div id='SC-21xxx'>)").Match(co);
                    if (!m2.Success) m2 = new Regex("(?<=<div class=\"article_right\" style=\"fone-color:#000\">)([\\S\\s]*?)(?=<div id='SC-21xx'>)").Match(co);
                    co = m1.Value + "\r\n" + m2.Value;
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.aboluowang.com/"))
                {
                    Match m1 = new Regex("(<h1)([\\S\\s]*?)(</h1>)").Match(co);
                    Match m2 = new Regex("(<article)([\\S\\s]*?)(</article>)").Match(co);
                    if (!m2.Success) m2 = new Regex("(<article)([\\S\\s]*?)(<div id=\"article_bottom\">)").Match(co);
                    Match mm = new Regex("(https://www.youtube.com/embed/)([\\S]*?)(?=\")").Match(co);
                    if (!mm.Success) mm = new Regex("(?<=href=\")(https://t.co/[\\S]*?)(?=\")").Match(co);
                    if (mm.Success) coMedia = mm.Value;
                    co = m1.Value + "\r\n" + m2.Value;
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.renminbao.com/"))
                {
                    Match m1 = new Regex("(?<=<span class=titleBlack>)([\\S\\s]*?)(?=</span>)").Match(co);
                    Match m2 = new Regex("(?<=<td class=\"articlebody\">)([\\S\\s]*?)(?=</td></tr>\\s*?<tr>)").Match(co);
                    co = "<h1>" + m1.Value + "</h1>\r\n<p>" + m2.Value.Replace("<br>\r<br>", "</p><p>") + "</p>";
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.zhuichaguoji.org/"))
                {
                    Match m1 = new Regex("(<h1 class=\"page-header\">)([\\S\\s]*?)(</h1>)").Match(co);
                    Match m2 = new Regex("(<article)([\\S\\s]*?)(</article>)").Match(co);
                    co = m1.Value + "\r\n" + m2.Value;
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://zh.wikipedia.org/"))
                {
                    Match m1 = new Regex("(<body)([\\S\\s]*?)(?=<table cellspacing=\"0\" class=\"navbox\" style=\"border-spacing:0\">)").Match(co);
                    if (!m1.Success) m1 = new Regex("(<body)([\\S\\s]*?)(?=<img src=\"//zh.wikipedia.org/wiki/Special:CentralAutoLogin)").Match(co);
                    co = HtmlDel(m1.Value, "(<div id=\"siteSub\")([\\S\\s]*?)(</div>)");
                    co = HtmlDel(co, "(<a class=\"mw-jump-link\")([\\S\\s]*?)(</a>)");
                    co = HtmlDel(co, "(<span class=\"mw-editsection\">)([\\S\\s]*?)(</span></span>)");
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://zh.wikisource.org/"))
                {
                    Match m1 = new Regex("(<body)([\\S\\s]*?)(?=<img src=\"//zh.wikisource.org/wiki/Special:CentralAutoLogin)").Match(co);
                    co = HtmlDel(m1.Value, "(<div id=\"siteSub\")([\\S\\s]*?)(</div>)");
                    co = HtmlDel(co, "(<a class=\"mw-jump-link\")([\\S\\s]*?)(</a>)");
                    co = HtmlDel(co, "(<span class=\"mw-editsection\">)([\\S\\s]*?)(</span></span>)");
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else if (name.StartsWith("https://www.rfa.org/"))
                {
                    Match m1 = new Regex("(<h1>)([\\S\\s]*?)(</h1>)").Match(co);
                    Match m2 = new Regex("(?<=<div id=\"storytext\">)([\\S\\s]*?)(?=</div> <!-- END storytext-->)").Match(co);
                    Match m = new Regex("(?<=<meta content=\")([\\S]*?)(?=\" property=\"og:audio\"/>)").Match(co);
                    if (!m.Success) m = new Regex(@"(?<=content=[""|'])([\S]*?)(?=[""|']\s*?property=[""|']og:image[""|'])").Match(co);
                    if (m.Success) coMedia = m.Value;
                    co = m1.Value + "\r\n" + m2.Value;
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }
                else
                {
                    co = HtmlDel2(co, name).Replace("<p>", "<p class=\"artl\">");
                }

                co = co.Trim();
                if (co != "") co = "<div class=\"artl\">\r\n" + co + "\r\n</div>";
                if (coMedia != "")
                {
                    coMedia = GetFile(coMedia);
                    if (coMedia.EndsWith(".jpg") || coMedia.EndsWith(".png") || coMedia.EndsWith(".jpeg")
                         || coMedia.EndsWith(".gif") || coMedia.EndsWith(".webp")) co = GetImagePlayerTop(coMedia) + "\r\n" + co;
                    else if (coMedia.EndsWith(".mp3")) co = GetAudioPlayer(coMedia, cover) + "\r\n" + co;
                    else if (coMedia.EndsWith(".mp4")) co = GetVideoPlayer(coMedia, "", cover) + "\r\n" + co;
                    else co = GetVideoPlayer(coMedia, "", cover) + "\r\n" + co;
                }
                return co;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            return "";
        }

        public static string HtmlDel(string content, string regex)
        {
            MatchCollection mc0 = new Regex(regex).Matches(content);
            foreach (Match m in mc0)
            {
                content = content.Replace(m.Value, "");
            }
            return content;
        }

        public string HtmlDel2(string content, string url)
        {
            var sanitizer = new HtmlSanitizer();
            try
            {
                content = sanitizer.Sanitize(content, url);
                content = HtmlDel(content, "(<head)([\\S\\s]*?)(</head>)");
                content = HtmlDel(content, "(<script)([\\S\\s]*?)(</script>)");
                content = HtmlDel(content, "(<noscript)([\\S\\s]*?)(</noscript>)");
                content = HtmlDel(content, "(<style)([\\S\\s]*?)(</style>)");
                content = HtmlDel(content, "(<ins)([\\S\\s]*?)(</ins>)");

                MatchCollection mc1 = new Regex("(<a )([\\S\\s]*?)(</a>)").Matches(content);
                foreach (Match m in mc1)
                {
                    Match m0 = new Regex("(?<=href=\"[\\s]*?)([\\S]*?)(?=[\\s]*?\")").Match(m.Value);
                    if (!m0.Success) m0 = new Regex("(?<=href=)([\\S]*?)(?=[\\s|>])").Match(m.Value);
                    Match m1 = new Regex("(?<=>)([\\S\\s]*?)(?=</a>)").Match(m.Value);
                    if (!m0.Success || !m1.Success) { content = content.Replace(m.Value, ""); continue; }
                    string sHref = HtmlHref(m0.Value, url);
                    //string s1 = ooData.sG10事物Get.Replace("^名称^",GetSqlParam(sHref));
                    string s1 = ooData.sG10事物Get_p;
                    List<SqlParameter> sqlparms = new List<SqlParameter>()
                    {
                        new SqlParameter() {ParameterName = "@名称", SqlDbType = SqlDbType.NVarChar, Value = GetSqlParam(sHref)}
                    };
                    //string s2 = ExecuteSQLResult(s1);
                    string s2 = ExecuteSQLResult(s1, sqlparms);
                    if (s2 != "")
                        content = content.Replace(m.Value, "[链接[c" + int.Parse(s2).ToString("D6") + "[链接]" + m1.Value + "]链接]");
                    else content = content.Replace(m.Value, m1.Value);
                }

                MatchCollection mc0 = new Regex("(<)([\\S\\s]*?)(>)").Matches(content);
                foreach (Match m in mc0)
                {
                    if (m.Value == "<p>" || m.Value == "</p>" || m.Value == "<p class=\"artl\">" || m.Value == "<p class=\"artc\">"
                        || m.Value == "<p class=\"artl\" style=\"text-align:center;\">" || m.Value == "<p style=\"text-align:center;\">"
                        || m.Value == "<b>" || m.Value == "</b>" || m.Value == "<br/>" || m.Value == "<br>" || m.Value == "<br />"
                        || m.Value == "<strong>" || m.Value == "</strong>"
                        || m.Value == "</h1>" || m.Value == "</h2>" || m.Value == "</h3>" || m.Value == "</h4>" || m.Value == "</h5>"
                        || m.Value == "<em>" || m.Value == "</em>" || m.Value == "<sup>" || m.Value == "</sup>") ;
                    else if (m.Value.StartsWith("<h1")) { content = content.Replace(m.Value, "<h1 style=\"text-align:center;\">"); }
                    else if (m.Value.StartsWith("<h2")) { content = content.Replace(m.Value, "<h2 style=\"text-align:center;\">"); }
                    else if (m.Value.StartsWith("<h3")) { content = content.Replace(m.Value, "<h3 style=\"text-align:center;\">"); }
                    else if (m.Value.StartsWith("<h4")) { content = content.Replace(m.Value, "<h4 style=\"text-align:center;\">"); }
                    else if (m.Value.StartsWith("<h5")) { content = content.Replace(m.Value, "<h5 style=\"text-align:center;\">"); }
                    else if (m.Value.StartsWith("<p")) content = content.Replace(m.Value, "<p>");
                    else if (m.Value.StartsWith("<img") || m.Value.StartsWith("<IMG"))
                    {
                        Match m0 = new Regex("(?<=data-src=\")([\\S\\s]*?)(?=\")").Match(m.Value);
                        if (!m0.Success) m0 = new Regex("(?<=src=\")([\\S\\s]*?)(?=\")").Match(m.Value);
                        if (!m0.Success) m0 = new Regex("(?<=src=)([\\S]*?)(?=[\\s|>])").Match(m.Value);
                        if (m0.Value.StartsWith("data:image/")) continue;
                        Match m1 = new Regex("(?<=width=\")([\\S\\s]*?)(?=\")").Match(m.Value);
                        string sHref = GetHtmlParam(HtmlHref(m0.Value, url));
                        int w = 0; int.TryParse(m1.Value, out w);
                        if (m0.Value == "/rmb/images/rmb_donation_200x200.jpg"
                            || sHref == "https://www.rfa.org/++plone++rfa-resources/img/icon-zoom.png") content = content.Replace(m.Value, "");
                        else
                        {
                            sHref = GetFile(sHref);
                            if (w < 45 && w > 0) content = content.Replace(m.Value, "<img src=\"" + EnUrl2(sHref) + "\">");
                            else content = content.Replace(m.Value, "<img class=\"artl\" src=\"" + EnUrl2(sHref) + "\">");
                        }
                    }
                    else if (m.Value.StartsWith("<table")) content = content.Replace(m.Value, "<table>");
                    else if (m.Value.StartsWith("<thead")) content = content.Replace(m.Value, "<thead>");
                    else if (m.Value.StartsWith("<tr")) content = content.Replace(m.Value, "<tr>");
                    else if (m.Value.StartsWith("<td"))
                    {
                        Match m0 = new Regex("( rowspan=\")([\\S\\s]*?)(\")").Match(m.Value);
                        Match m1 = new Regex("( colspan=\")([\\S\\s]*?)(\")").Match(m.Value);
                        content = content.Replace(m.Value, "<td" + m0.Value + m1.Value + ">");
                    }
                    else if (m.Value.StartsWith("<th"))
                    {
                        Match m0 = new Regex("( rowspan=\")([\\S\\s]*?)(\")").Match(m.Value);
                        Match m1 = new Regex("( colspan=\")([\\S\\s]*?)(\")").Match(m.Value);
                        content = content.Replace(m.Value, "<th" + m0.Value + m1.Value + ">");
                    }
                    else if (m.Value == "</th>" || m.Value == "</td>" || m.Value == "</tr>"
                        || m.Value == "<tbody>" || m.Value == "</tbody>"
                        || m.Value == "</thead>" || m.Value == "</table>") ;
                    else if (m.Value.StartsWith("<sup")) content = content.Replace(m.Value, "<sup>");
                    else if (m.Value == "<li>" || m.Value.StartsWith("<li ")) content = content.Replace(m.Value, "<li>");
                    else if (m.Value == "</li>") ;
                    else if (m.Value.StartsWith("<ol")) content = content.Replace(m.Value, "<ol>");
                    else if (m.Value == "</ol>") ;
                    else content = content.Replace(m.Value, "");
                }

                content = content.Replace("\t", "").Replace("\n\n", "\n").Replace("\n\n", "\n").Replace("\n\n", "\n")
                    .Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");
                return content;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            return "";
        }

        static private string HtmlHref(string sHref, string url)
        {
            try
            {
                if (sHref.StartsWith("http")) ;
                else if (sHref.StartsWith("/Ups/")) ;
                else if (sHref.StartsWith("//")) sHref = "http:" + sHref;
                else if (sHref.StartsWith("/")) sHref = url.Substring(0, url.Replace("//", "\\").IndexOf("/") + 1) + sHref;
                else sHref = url.Substring(0, url.LastIndexOf("/") + 1) + sHref;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            return sHref;
        }

        static public string UrlProc(string name)
        {
            try
            {
                if (name.StartsWith("https://www.youtube.com/watch?v=")) name = name.Replace("https://www.youtube.com/watch?v=", "https://youtu.be/");

                if (name.StartsWith("http://www.minghui.org/")) name = name.Replace("http://www.minghui.org/", "https://www.minghui.org/");
                if (name.StartsWith("http://www.mhradio.org/")) name = name.Replace("http://www.mhradio.org/", "https://www.mhradio.org/");
                if (name.StartsWith("http://www.zhengjian.org/")) name = name.Replace("http://www.zhengjian.org/", "https://www.zhengjian.org/");
                if (name.StartsWith("http://www.epochweekly.com/")) name = name.Replace("http://www.epochweekly.com/", "https://www.epochweekly.com/");
                if (name.StartsWith("http://cn.epochtimes.com/")) name = name.Replace("http://cn.epochtimes.com/", "https://cn.epochtimes.com/");
                if (name.StartsWith("http://www.epochtimes.com/")) name = name.Replace("http://www.epochtimes.com/", "https://www.epochtimes.com/");
                if (name.StartsWith("http://cn.ntdtv.com/")) name = name.Replace("http://cn.ntdtv.com/", "https://cn.ntdtv.com/");
                if (name.StartsWith("http://www.ntdtv.com/")) name = name.Replace("http://www.ntdtv.com/", "https://www.ntdtv.com/");
                if (name.StartsWith("http://soundofhope.org/")) name = name.Replace("http://soundofhope.org/", "https://soundofhope.org/");
                if (name.StartsWith("http://www.soundofhope.org/")) name = name.Replace("http://www.soundofhope.org/", "https://www.soundofhope.org/");
                if (name.StartsWith("http://cn.secretchina.com/")) name = name.Replace("http://cn.secretchina.com/", "https://cn.secretchina.com/");
                if (name.StartsWith("http://www.secretchina.com/")) name = name.Replace("http://www.secretchina.com/", "https://www.secretchina.com/");
                if (name.StartsWith("http://www.aboluowang.com/")) name = name.Replace("http://www.aboluowang.com/", "https://www.aboluowang.com/");
                if (name.StartsWith("http://www.renminbao.com/")) name = name.Replace("http://www.renminbao.com/", "https://www.renminbao.com/");
                if (name.StartsWith("http://www.zhuichaguoji.org/")) name = name.Replace("http://www.zhuichaguoji.org/", "https://www.zhuichaguoji.org/");
                if (name.StartsWith("http://huanyuanzhongguo.net/")) name = name.Replace("http://huanyuanzhongguo.net/", "https://huanyuanzhongguo.net/");
                if (name.StartsWith("http://www.huanyuanzhongguo.net/")) name = name.Replace("http://www.huanyuanzhongguo.net/", "https://www.huanyuanzhongguo.net/");

                if (name.StartsWith("https://cn.epochtimes.com/")) name = name.Replace("https://cn.epochtimes.com/", "https://www.epochtimes.com/");
                if (name.StartsWith("https://cn.ntdtv.com/")) name = name.Replace("https://cn.ntdtv.com/", "https://www.ntdtv.com/");
                if (name.StartsWith("https://cn.secretchina.com/")) name = name.Replace("https://cn.secretchina.com/", "https://www.secretchina.com/");
                if (name.StartsWith("https://www.epochtimes.com/b5/")) name = name.Replace("https://www.epochtimes.com/b5/", "https://www.epochtimes.com/gb/");
                if (name.StartsWith("https://www.ntdtv.com/b5/")) name = name.Replace("https://www.ntdtv.com/b5/", "https://www.ntdtv.com/gb/");
                if (name.StartsWith("https://www.epochweekly.com/") && !name.EndsWith("index.htm") && !name.EndsWith("p.htm")) name = name.Replace(".htm", "p.htm");
                if (name.StartsWith("https://soundofhope.org/")) name = name.Replace("https://soundofhope.org/", "https://www.soundofhope.org/");
                if (name.StartsWith("https://huanyuanzhongguo.net/")) name = name.Replace("https://huanyuanzhongguo.net/", "https://www.huanyuanzhongguo.net/");

                if (name.StartsWith("https://www.soundofhope.org/")) name = name.Replace("?lang=b5", "");
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            return name;
        }
        static private string GetHtmlParam(string name)
        {
            return name.Replace("<", "&lt;").Replace(">", "&gt;")
                .Replace("'", "&#39;").Replace("\"", "&quot;");
        }

        // HtmlRsc
        static public string EnUrl2(string name, bool bFirst = false)
        {
            return name;
        }

        static public string EnUrlSymbol(string url)
        {
            return url.Replace(":", "%3A").Replace("/", "%2F").Replace("?", "%3F").Replace("&", "%26").Replace("#", "%23").Replace("=", "%3D");
        }

        // Utility
        static public void Log(string co)
        {
            WriteErr(co);
        }

        static public void Log(string name, string co)
        {
            Log(name + ": " + co);
        }

        static private string sRandomChars = "abcdefghijklmnopqrstuvwxyz";
        static private string sRandomChars2 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        static private object oRandom = new object();
        static public string GetRandom(int len = 12, bool upper = false)
        {
            string randomChars = upper ? sRandomChars2 : sRandomChars;
            string password = "";
            lock (oRandom) for (int i = 0; i < len; i++) password += randomChars[random.Next(randomChars.Length)];
            return password;
        }
        static public string GetRandomNum()
        {
            string password = "";
            lock (oRandom) password = random.Next(10000000, 99999999).ToString();
            return password;
        }
        static private string GetPictParam(string sPict, string sNumb)
        {
            sPict = GetHtmlParam(sPict.Trim());
            if (sPict != "")
                return "><img class='imgh' src='" + EnUrl2(sPict.Replace("#P ", "").Replace(".jpg", "").Replace(".jpeg", "").Replace(".png", "")
                    + ".K.jpg", true) + "'/>";
            else return " style='background:hsla(" + int.Parse(sNumb) % 360 + ",75%,50%,0.5)'>";
        }

        static public string RSAEncrypt(string publickey, string content)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(publickey);
                byte[] cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                return Convert.ToBase64String(cipherbytes);
            }
            catch { return ""; }
        }

        static public string RSADecrypt(string privatekey, string content)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(privatekey);
                byte[] cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
                return Encoding.UTF8.GetString(cipherbytes);
            }
            catch { return ""; }
        }

        // Show
        private delegate void ShowMsgDelegate(string msg);
        private void ShowLabel(string msg)
        {
            labelMsg.Text = msg;
        }
        private void ShowMsg(string msg)
        {
            textBoxMsg.AppendText("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + msg + "\r\n");
            textBoxMsg.ScrollToCaret();
        }
        private void ShowMsgD(string msg)
        {
            //WriteMsg(msg);
            BeginInvoke(new ShowMsgDelegate(ShowMsg), new object[] { msg });
        }
        private void ShowErr(string msg)
        {
            textBoxMsg.AppendText("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + msg + "\r\n");
            textBoxMsg.ScrollToCaret();
        }
        private void ShowErrD(string msg)
        {
            //WriteErr(msg);
            BeginInvoke(new ShowMsgDelegate(ShowErr), new object[] { msg });
        }

        static private void WriteErr(string message)
        {
            try
            {
                StreamWriter swlog = File.AppendText(sAppName + "Err.txt");
                swlog.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + message);
                swlog.Close();
            }
            catch { }
        }
        private void SaveFile(string message, string sFile)
        {
            try
            {
                StreamWriter swlog = File.CreateText(sFile);
                swlog.Write(message);
                swlog.Close();
            }
            catch { }
        }

        // Html Utility
        static public string GetHtml(string sName, bool bFail = false)
        {
            string r = "";
            r = GetHtmlMethod("GET", sName, "", "", "", "");
            if (r.Contains("charset=gb2312")) r = GetHtmlMethod("GET", sName, "", "", "", "gb2312");
            return r;
        }

        static public string GetHtmlMethod(string sMethod, string sName, string sData, string sHeader, string sReferer,
            string sCode)
        {
            return GetHtmlMethod(sMethod, sName, sData, sHeader, sReferer, sCode, "", out string c);
        }

        static public string GetHtmlMethod(string sMethod, string sName, string sData, string sHeader, string sReferer,
            string sCode, string sContentType, out string sCookie)
        {
            sCookie = "";
            string r = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader streamReader = null;
            try
            {
                if (!sName.StartsWith("http://") && !sName.StartsWith("https://")) return "";

                request = (HttpWebRequest)WebRequest.Create(sName);
                request.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return CertificateValidationCallBack(sender, certificate, chain, errors); };
                request.UserAgent = sUserAgent;
                request.Timeout = request.ReadWriteTimeout = 10 * 1000;
                if (sReferer != "") request.Referer = sReferer;
                if (sContentType != "") request.ContentType = sContentType;
                else if (sMethod == "POST") request.ContentType = "application/x-www-form-urlencoded";
                request.Method = sMethod;
                if (sHeader != "") { string[] ss0 = sHeader.Split(','); foreach (string s in ss0) { string[] ss = s.Split(':'); request.Headers[ss[0].Trim()] = ss[1].Trim(); } }
                if (sData != "")
                {
                    byte[] postData = Encoding.UTF8.GetBytes(sData);
                    request.ContentLength = postData.Length;
                    requestStream = request.GetRequestStream();
                    requestStream.Write(postData, 0, postData.Length);
                    requestStream.Close();
                }
                response = (HttpWebResponse)request.GetResponse();
                if (response.Headers.Get("Set-Cookie") != null) { sCookie = response.Headers.Get("Set-Cookie"); }
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 2 * 1024 * 1024
                    && (response.ContentType.StartsWith("text") || response.ContentType.StartsWith("application"))) /// important
                {
                    responseStream = response.GetResponseStream();
                    if (response.ContentEncoding != null && response.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (response.ContentEncoding != null && response.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                    streamReader = new StreamReader(responseStream, Encoding.GetEncoding(sCode == "" ? "utf-8" : sCode));
                    r = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            finally
            {
                if (request != null) request = null;
                if (response != null) { response.Close(); response = null; }
                if (requestStream != null) requestStream.Close();
                if (responseStream != null) responseStream.Close();
                if (streamReader != null) streamReader.Close();
            }
            return r;
        }

        private static bool AlwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        public async Task<string> DownloadFile(string name, string host, string referer, string sFileName)
        {
            // validation
            HttpClient _httpClient = new HttpClient();
            using (var stream = await _httpClient.GetStreamAsync(name))
            {
                using (var fileStream = new FileStream(sFileName, FileMode.CreateNew))
                {
                    await stream.CopyToAsync(fileStream);
                    return fileStream.Name;
                }
            }

            //var fileInfo = new FileInfo(sFileName);

            //Task<HttpResponseMessage> response = _httpClient.GetAsync(name);
            //response.EnsureSuccessStatusCode();
            //Stream ms = await response.Content.ReadAsStreamAsync();
            //FileStream fs = File.Create(fileInfo.FullName);
            //ms.Seek(0, SeekOrigin.Begin);
            //ms.CopyTo(fs);

            //return fileInfo.FullName;




        }
        public bool DownloadHtml(string name, string host, string referer, string sFileName)
        {
            HttpWebRequest request2 = null;
            HttpWebResponse response2 = null;
            try
            {
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AlwaysGoodCertificate);

                //request2 = (HttpWebRequest)WebRequest.Create("https://a57.foxnews.com/static.foxnews.com/foxnews.com/content/uploads/2022/09/1024/576/GettyImages-1402371449.jpg?tl=1&ve=1");
                //response2 = (HttpWebResponse)request2.GetResponse();

                request2 = (HttpWebRequest)WebRequest.Create(name);
                request2.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return CertificateValidationCallBack(sender, certificate, chain, errors); };
                if (host != "") request2.Host = host;
                if (referer != "") request2.Referer = referer;
                request2.UserAgent = sUserAgent;
                response2 = (HttpWebResponse)request2.GetResponse();
                if (response2.StatusCode != HttpStatusCode.OK)
                {
                    if (response2 != null) { response2.Close(); response2 = null; }
                    if (request2 != null) request2 = null;
                    return false;
                }
                if (File.Exists(sFileName))
                {
                    FileInfo fi = new FileInfo(sFileName);
                    if (response2.ContentLength == -1 || fi.Length == response2.ContentLength)
                    {
                        ///BeginInvoke(new ShowMsgDelegate(ShowMsg), new object[] { "Skip" });
                        if (response2 != null) { response2.Close(); response2 = null; }
                        if (request2 != null) request2 = null;
                        return true;
                    }
                }
                if (isMimeAcceptable(response2.ContentType))
                {
                    //L4
                    ShowMsgD("Downloading: " + sFileName);

                    byte[] buffer = new byte[8 * 1024];
                    Stream outStream = File.Create(sFileName);
                    Stream inStream = response2.GetResponseStream();
                    long length = response2.ContentLength;
                    long total = 0;
                    int l = 0;
                    while ((l = inStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        total += l;
                        int progress = (int)(((float)total / length) * 100);
                        BeginInvoke(new ShowMsgDelegate(ShowLabel), new object[] { progress + " %" });
                        outStream.Write(buffer, 0, l);
                        //if (isMimeAcceptable(GetMimeType(buffer))) //screening MIME types
                        //{
                        //    outStream.Write(buffer, 0, l);
                        //}
                        //else
                        //{
                        //    outStream.Close();
                        //    return false;
                        //}
                    }
                    outStream.Close();
                    if (response2.ContentLength != -1 && length != total) { }
                    //BeginInvoke(new ShowMsgDelegate(ShowLabel), new object[] { "Done" });
                    if (response2 != null) { response2.Close(); response2 = null; }
                    if (request2 != null) request2 = null;
                    return true;
                }

            }
            catch (Exception ex) { }
            if (response2 != null) { response2.Close(); response2 = null; }
            if (request2 != null) request2 = null;
            return false;
        }

        // SQL Utility
        public string ExecuteSQLResult(string s, List<SqlParameter> sps)
        {
            string r = "";
            try
            {
                DataSet ds = new DataSet();
                //ExecuteSQL(s, ds);
                ExecuteSQL(s, ds, sps);
                if (ds.Tables[0].Rows.Count > 0) r = ds.Tables[0].Rows[0][0].ToString();
                ds.Clear();
                ds = null;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message + " " + s); }
            return r;
        }
        public string ExecuteSQLResult(string s)
        {
            string r = "";
            try
            {
                DataSet ds = new DataSet();
                ExecuteSQL(s, ds);
                if (ds.Tables[0].Rows.Count > 0) r = ds.Tables[0].Rows[0][0].ToString();
                ds.Clear();
                ds = null;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message + " " + s); }
            return r;
        }

        public bool ExecuteSQL(string s, DataSet ds, List<SqlParameter> sps)
        {
            string tbname = "result";
            try
            {
                string connTusiStr = tbxDbConnString.Text; //ConfigurationManager.AppSettings["DB"];
                SqlConnection conn = new SqlConnection(connTusiStr);
                conn.Open();
                if (ds == null)
                {
                    SqlCommand sqlCmd = new SqlCommand(s, conn);
                    foreach (var sp in sps)
                    {
                        sqlCmd.Parameters.Add(sp);
                    }

                    object o = sqlCmd.ExecuteScalar();
                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter(s, conn);
                    foreach (var sp in sps)
                    {
                        da.SelectCommand.Parameters.Add(sp);
                    }

                    da.Fill(ds, tbname);
                }
                conn.Close();
                return true;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message + " " + s); }
            return false;
        }

        public bool ExecuteSQL(string s)
        {
            return ExecuteSQL(s, null);
        }

        public bool ExecuteSQL(string s, DataSet ds)
        {
            string tbname = "result";
            try
            {
                string connTusiStr = tbxDbConnString.Text; //ConfigurationManager.AppSettings["DB"];
                SqlConnection conn = new SqlConnection(connTusiStr);
                conn.Open();
                if (ds == null)
                {
                    SqlCommand sqlCmd = new SqlCommand(s, conn);
                    object o = sqlCmd.ExecuteScalar();
                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter(s, conn);
                    da.Fill(ds, tbname);
                }
                conn.Close();
                return true;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message + " " + s); }
            return false;
        }

        static public string GetSqlParam(string s)
        {
            return s.Replace("'", "''");
        }

        private static bool CertificateValidationCallBack(
          object sender,
          System.Security.Cryptography.X509Certificates.X509Certificate certificate,
          System.Security.Cryptography.X509Certificates.X509Chain chain,
          System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if (
                          (certificate.Subject == certificate.Issuer) &&
                          (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid.
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                MessageBox.Show("The certificate is invalid. Certificate Error Code: " + status.Status.ToString());
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                MessageBox.Show("The certificate is invalid.");
                return false;
            }
        }

        private bool loginawsiam(string bucketname, string accesskeyid, string seckey)
        {
            using (AmazonRDSClient rdsClient = new AmazonRDSClient(RegionEndpoint.USEast1))
            {

            }

            return false;
        }
        private bool verifys3bucket(string bucketname, string accesskeyid, string seckey)
        {
            AmazonS3Client s3Client = new AmazonS3Client(accesskeyid, seckey);
            ListBucketsResponse buckets = s3Client.ListBuckets();
            foreach (var bucket in buckets.Buckets)
            {
                if (bucket.BucketName.ToUpper() == bucketname.ToUpper())
                    return true;
            }

            return false;
        }

        private void OrganizeLocalFolders()
        {
            string localfiledirStr = ConfigurationManager.AppSettings["localfiledir"];
            string localsitedirStr = ConfigurationManager.AppSettings["localsitedir"];
            string appdirStr = Application.StartupPath;

            localfiledirStr = appdirStr + "\\" + localfiledirStr;
            localsitedirStr = appdirStr + "\\" + localsitedirStr;
            if (!Directory.Exists(localsitedirStr))
            {
                DirectoryInfo siteDi = Directory.CreateDirectory(localsitedirStr);

                if (siteDi is null)
                {
                    ShowMsg(localsitedirStr + " not exists.");
                }
            }

            if (!Directory.Exists(localfiledirStr))
            {
                DirectoryInfo siteDi = Directory.CreateDirectory(localfiledirStr);

                if (siteDi is null)
                {
                    ShowMsg(localfiledirStr + " not exists.");
                }
            }
        }
        private void ClearDangerousFilesinLocalFolders()
        {
            //beginning byte[] pattern determins file/MIME type, 8.2022
            string localfiledirStr = ConfigurationManager.AppSettings["localfiledir"];
            string localsitedirStr = ConfigurationManager.AppSettings["localsitedir"];
            if (localfiledirStr == null) localfiledirStr = "File";
            if (localsitedirStr == null) localsitedirStr = "Site";

            OrganizeLocalFolders();

            string appdirStr = Application.StartupPath;

            localfiledirStr = appdirStr + "\\" + localfiledirStr;
            localsitedirStr = appdirStr + "\\" + localsitedirStr;
            ClearDangerousFilesinLocalFolder(localfiledirStr);
            ClearDangerousFilesinLocalFolder(localsitedirStr);
        }
        private void ClearDangerousFilesinLocalFolder(string foldername)
        {
            if (!Directory.Exists(foldername))
            {
                ShowMsgD("Directory Not Found: " + foldername);
                return;
            }

            string fileext_checked = "";
            string[] filePaths = Directory.GetFiles(foldername);
            for (int i = 0; i < filePaths.Length; i++)
            {
                if (File.Exists(filePaths[i]))
                {
                    string fileext = Path.GetExtension(filePaths[i]);
                    if (fileext.ToLower() == fileext_checked.ToLower()) continue;

                    byte[] file = File.ReadAllBytes(filePaths[i]);
                    //binary file mime check: based on a mime and byte match library
                    string mime = GetMimeType(file, filePaths[i]);
                    if (!isMimeAcceptable(mime))
                        mime = getMimeFromFile(filePaths[i]);   //based on the query in to "urlmon.dll" in windows system
                    if (isMimeAcceptable(mime))
                    {
                        ShowMsgD("Acceptable Type (" + mime + "): " + Path.GetFileName(filePaths[i]));
                        fileext_checked = fileext;
                        continue;
                    }

                    //the dll should do the work most likely
                    if (isInFileTypeList(fileext))
                    {
                        ShowMsgD("Acceptable Type (" + mime + "): " + Path.GetFileName(filePaths[i]));
                        fileext_checked = fileext;
                        continue;
                    }

                    FileAttributes fas = File.GetAttributes(filePaths[i]);
                    ShowMsgD("Delete (File Type Error " + mime + "): " + Path.GetFileName(filePaths[i]));
                    File.Delete(filePaths[i]);

                }
            }

        }

        // !isInFileTypeList(Path.GetExtension(filePaths[i]))
        private bool isInFileTypeList(string fileext)
        {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(fileext.ToLower());
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                string mimeType;
                mimeType = regKey.GetValue("Content Type").ToString();
                //Console.WriteLine(mimeType);

                return isMimeAcceptable(mimeType);
            }
            return false;
        }
        private void textBoxS3Id_TextChanged(object sender, EventArgs e)
        {
            //when start changing credentials, enable bucket entry
            //textBoxS3Bucket.Enabled = true;
        }

        private void textBoxS3Key_TextChanged(object sender, EventArgs e)
        {
            //textBoxS3Bucket.Enabled = true;
        }

        private bool isMimeAcceptable(string mimestring)
        {
            if (mimestring.StartsWith("application/") && mimestring.Contains("javascript")
                || mimestring.StartsWith("image/")
                || mimestring.StartsWith("audio/")
                || mimestring.StartsWith("video/")
                || mimestring.StartsWith("application/octet-stream") && (mimestring.Contains("javascript"))
                || mimestring.StartsWith("text/plain")
                || mimestring.StartsWith("text/css")
                || mimestring.StartsWith("text/html")
                ) return true;
            return false;
        }
        private string GetMimeType(byte[] filebytes)
        {

            string mime = "application/octet-stream"; //DEFAULT UNKNOWN MIME TYPE

            //Get the MIME Type
            if (filebytes.Take(2).SequenceEqual(BMP))
            {
                mime = "image/bmp";
            }
            else if (filebytes.Take(8).SequenceEqual(DOC))
            {
                mime = "application/msword";
            }
            else if (filebytes.Take(2).SequenceEqual(EXE_DLL))
            {
                mime = "application/x-msdownload"; //both use same mime type
            }
            else if (filebytes.Take(4).SequenceEqual(GIF))
            {
                mime = "image/gif";
            }
            else if (filebytes.Take(4).SequenceEqual(ICO))
            {
                mime = "image/x-icon";
            }
            else if (filebytes.Take(3).SequenceEqual(JPG))
            {
                mime = "image/jpeg";
            }
            else if (filebytes.Take(3).SequenceEqual(MP3))
            {
                mime = "audio/mpeg";
            }
            else if (filebytes.Take(14).SequenceEqual(OGG))
            {
                mime = "video/ogg";
            }
            else if (filebytes.Take(7).SequenceEqual(PDF))
            {
                mime = "application/pdf";
            }
            else if (filebytes.Take(16).SequenceEqual(PNG))
            {
                mime = "image/png";
            }
            else if (filebytes.Take(7).SequenceEqual(RAR))
            {
                mime = "application/x-rar-compressed";
            }
            else if (filebytes.Take(3).SequenceEqual(SWF))
            {
                mime = "application/x-shockwave-flash";
            }
            else if (filebytes.Take(4).SequenceEqual(TIFF))
            {
                mime = "image/tiff";
            }
            else if (filebytes.Take(11).SequenceEqual(TORRENT))
            {
                mime = "application/x-bittorrent";
            }
            else if (filebytes.Take(5).SequenceEqual(TTF))
            {
                mime = "application/x-font-ttf";
            }
            else if (filebytes.Take(4).SequenceEqual(WAV_AVI))
            {
                mime = "media/avi-wav";//extension == ".AVI" ? "video/x-msvideo" : "audio/x-wav";
            }
            else if (filebytes.Take(16).SequenceEqual(WMV_WMA))
            {
                mime = "media/wma-=wmv";// extension == ".WMA" ? "audio/x-ms-wma" : "video/x-ms-wmv";
            }
            else if (filebytes.Take(4).SequenceEqual(ZIP_DOCX))
            {
                mime = "application/word-zip";
                //extension == ".DOCX" ? "application/vnd.openxmlformats-officedocument.wordprocessingml.document" : "application/x-zip-compressed";
            }

            return mime;
        }
        private string GetMimeType(byte[] file, string fileName)
        {

            string mime = "application/octet-stream"; //DEFAULT UNKNOWN MIME TYPE

            //Ensure that the filename isn't empty or null
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return mime;
            }

            //Get the file extension
            string extension = Path.GetExtension(fileName) == null
                                   ? string.Empty
                                   : Path.GetExtension(fileName).ToUpper();

            //Get the MIME Type
            if (file.Take(2).SequenceEqual(BMP))
            {
                mime = "image/bmp";
            }
            else if (file.Take(8).SequenceEqual(DOC))
            {
                mime = "application/msword";
            }
            else if (file.Take(2).SequenceEqual(EXE_DLL))
            {
                mime = "application/x-msdownload"; //both use same mime type
            }
            else if (file.Take(4).SequenceEqual(GIF))
            {
                mime = "image/gif";
            }
            else if (file.Take(4).SequenceEqual(ICO))
            {
                mime = "image/x-icon";
            }
            else if (file.Take(3).SequenceEqual(JPG))
            {
                mime = "image/jpeg";
            }
            else if (file.Take(3).SequenceEqual(MP3))
            {
                mime = "audio/mpeg";
            }
            else if (file.Take(14).SequenceEqual(OGG))
            {
                if (extension == ".OGX")
                {
                    mime = "application/ogg";
                }
                else if (extension == ".OGA")
                {
                    mime = "audio/ogg";
                }
                else
                {
                    mime = "video/ogg";
                }
            }
            else if (file.Take(7).SequenceEqual(PDF))
            {
                mime = "application/pdf";
            }
            else if (file.Take(16).SequenceEqual(PNG))
            {
                mime = "image/png";
            }
            else if (file.Take(7).SequenceEqual(RAR))
            {
                mime = "application/x-rar-compressed";
            }
            else if (file.Take(3).SequenceEqual(SWF))
            {
                mime = "application/x-shockwave-flash";
            }
            else if (file.Take(4).SequenceEqual(TIFF))
            {
                mime = "image/tiff";
            }
            else if (file.Take(11).SequenceEqual(TORRENT))
            {
                mime = "application/x-bittorrent";
            }
            else if (file.Take(5).SequenceEqual(TTF))
            {
                mime = "application/x-font-ttf";
            }
            else if (file.Take(4).SequenceEqual(WAV_AVI))
            {
                mime = extension == ".AVI" ? "video/x-msvideo" : "audio/x-wav";
            }
            else if (file.Take(16).SequenceEqual(WMV_WMA))
            {
                mime = extension == ".WMA" ? "audio/x-ms-wma" : "video/x-ms-wmv";
            }
            else if (file.Take(4).SequenceEqual(ZIP_DOCX))
            {
                mime = extension == ".DOCX" ? "application/vnd.openxmlformats-officedocument.wordprocessingml.document" : "application/x-zip-compressed";
            }

            return mime;
        }
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
        System.UInt32 pBC,
        [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
        [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
        System.UInt32 cbSize,
        [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
        System.UInt32 dwMimeFlags,
        out System.UInt32 ppwzMimeOut,
        System.UInt32 dwReserverd
    );

        public static string getMimeFromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename + " not found");

            byte[] buffer = new byte[256];
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }
            try
            {
                System.UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                System.IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception e)
            {
                return "unknown/unknown";
            }
        }

        private void listprofilenames()
        {
            List<CredentialProfile> aws_credlist = loadProfileFromCredFile();

            if (aws_credlist.Count > 0)
            {
                List<string> profnames = new List<string>();
                foreach (var pf in aws_credlist)
                {
                    profnames.Add(pf.Name);
                }

                cbxProfiles.DataSource = profnames;
                btnSaveCred.Text = "Use Profile";
                EnableProfileEntries(false);
                //cbxProfiles.SelectedIndex = -1;
                return;
            }

            btnSaveCred.Text = "Create Profile";
            EnableProfileEntries(true);
            ShowhideDataTabs(false);
        }
        private void ShowhideDataTabs(bool bshow)
        {
            if (!bshow)
            {
                tabEntries.TabPages.Remove(tabPage1);
                tabEntries.TabPages.Remove(tabPage2);
                tabEntries.TabPages.Remove(tabPage3);
                tabEntries.TabPages.Remove(tabPage4);
            }
            else
            {
                tabEntries.TabPages.Add(tabPage1);
                tabEntries.TabPages.Add(tabPage2);
                tabEntries.TabPages.Add(tabPage3);
                tabEntries.TabPages.Add(tabPage4);
            }
        }

        private void ShowhideCredsTab(bool bshow)
        {
            if (!bshow)
            {
                tabEntries.TabPages.Remove(tpIAM);
            }
            else
            {
                tabEntries.TabPages.Add(tpIAM);
            }
        }
        private void EnableProfileEntries(bool benable)
        {
            tbxProfName.Enabled = benable;
            tbxAccKey.Enabled = benable;
            tbxSecKey.Enabled = benable;
            tbxDbPwdP.Enabled = benable;
            tbxDbUserP.Enabled = benable;
        }

        private void EnableDbLogins(bool benable)
        {
            tbxDbPwdP.Enabled = benable;
            tbxDbUserP.Enabled = benable;
        }

        private void EnableEntryTabs(bool benable)
        {
            for (int i = 0; i < this.tabEntries.TabCount; i++)
            {
                if (tabEntries.TabPages[i].Name != "tpIAM")
                {
                    tabEntries.TabPages[i].Size = Size.Empty;
                }
            }
        }

        private string protectText(string texttoencrypt)
        {
            var sharedFile = new SharedCredentialsFile();
            if (!File.Exists(sharedFile.FilePath)) return "";


            string readText = File.ReadAllText(sharedFile.FilePath);


            FileStream fStream = new FileStream(sharedFile.FilePath + encrypt_fileext, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);

            int length = 0;
            byte[] toEncrypt = UnicodeEncoding.ASCII.GetBytes(readText);
            byte[] entropy = CreateRandomEntropy();
            byte[] encrptedData = ProtectedData.Protect(toEncrypt, entropy, DataProtectionScope.CurrentUser);
            // Write the encrypted data to a stream.
            if (fStream.CanWrite && encrptedData != null)
            {
                fStream.Write(encrptedData, 0, encrptedData.Length);

                length = encrptedData.Length;
            }

            fStream.Close();
            return "";
        }

        private string saveCredFile()
        {
            var sharedFile = new SharedCredentialsFile();
            if (!File.Exists(sharedFile.FilePath)) return "";


            string readText = File.ReadAllText(sharedFile.FilePath);
            FileStream fStream = new FileStream(sharedFile.FilePath + encrypt_fileext, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            int length = 0;
            byte[] toEncrypt = UnicodeEncoding.ASCII.GetBytes(readText);
            byte[] entropy = CreateRandomEntropy();
            byte[] encrptedData = ProtectedData.Protect(toEncrypt, entropy, DataProtectionScope.CurrentUser);
            // Write the encrypted data to a stream.
            if (fStream.CanWrite && encrptedData != null)
            {
                fStream.Write(encrptedData, 0, encrptedData.Length);
            }

            fStream.Close();
            return sharedFile.FilePath + encrypt_fileext;
        }

        private List<CredentialProfile> loadProfileFromCredFile()
        {
            List<CredentialProfile> credlist = new List<CredentialProfile>();
            if (ExistsCredFile())
            {
                var crdfile = OpenCredFile();
                //var sharedFile = new SharedCredentialsFile();
                if (crdfile != null)
                {
                    credlist = crdfile.ListProfiles();// .ListProfileNames();
                    closeShareProfileFile();    //protect the credential info
                }

            }

            return credlist;
        }

        private void closeShareProfileFile()
        {
            var sharedFile = new SharedCredentialsFile();
            string credfilepath = sharedFile.FilePath;
            if (File.Exists(credfilepath)) File.Delete(credfilepath);   //remove it so no one can open it with notepad, since it's not encrypted
        }

        private bool ExistsCredFile()
        {
            var sharedFile = new SharedCredentialsFile();
            string encrypt_file = sharedFile.FilePath + encrypt_fileext;
            if (File.Exists(encrypt_file))
            {
                return true;
            }
            return false;
        }
        private SharedCredentialsFile OpenCredFile()
        {
            var sharedFile = new SharedCredentialsFile();
            string encrypt_file = sharedFile.FilePath + encrypt_fileext;
            byte[] toDecrypt;
            if (File.Exists(encrypt_file))
            {
                FileStream fStream = new FileStream(sharedFile.FilePath + encrypt_fileext, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (fStream.CanRead)
                {
                    toDecrypt = new byte[fStream.Length];
                    fStream.Read(toDecrypt, 0, (int)fStream.Length);

                    byte[] entropy = CreateRandomEntropy();
                    byte[] decrypted_bytes = ProtectedData.Unprotect(toDecrypt, entropy, DataProtectionScope.CurrentUser);
                    string decrypted_readtext = UnicodeEncoding.ASCII.GetString(decrypted_bytes);

                    FileStream fStream1 = new FileStream(sharedFile.FilePath, FileMode.OpenOrCreate);

                    if (fStream1.CanWrite && decrypted_bytes != null)
                    {
                        fStream1.Write(decrypted_bytes, 0, decrypted_bytes.Length);
                        fStream1.Close();
                    }
                }
            }

            return sharedFile;
        }

        private string unprotectText(string texttodecrypt)
        {
            var sharedFile = new SharedCredentialsFile();
            if (!File.Exists(sharedFile.FilePath + encrypt_fileext)) return "";

            FileStream fStream = new FileStream(sharedFile.FilePath + encrypt_fileext, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            string unprotectedstring = string.Empty;
            // Read the encrypted data from a stream.
            if (fStream.CanRead)
            {
                byte[] inBuffer = new byte[fStream.Length];
                byte[] outBuffer;
                byte[] entropy = CreateRandomEntropy();

                fStream.Read(inBuffer, 0, Convert.ToInt32(fStream.Length));

                outBuffer = ProtectedData.Unprotect(inBuffer, entropy, DataProtectionScope.CurrentUser);
                unprotectedstring = UnicodeEncoding.ASCII.GetString(outBuffer);
            }
            else
            {
                throw new IOException("Could not read the stream.");
            }
            return unprotectedstring;
        }

        public static byte[] CreateRandomEntropy()
        {
            // Create a byte array to hold the random value.
            //byte[] entropy = new byte[16];

            // Create a new instance of the RNGCryptoServiceProvider.
            // Fill the array with a random value.
            //new RNGCryptoServiceProvider().GetBytes(entropy);
            byte[] s_additionalEntropy = { 9, 8, 7, 6, 5 };
            // Return the array.
            return s_additionalEntropy;
        }

        private string createEmptyCredentialFile()
        {
            CredentialProfileOptions options = new CredentialProfileOptions
            {
                AccessKey = ""
            };
            string dumstr = "dummy";
            var profile = new CredentialProfile(dumstr, options);
            var sharedFile = new SharedCredentialsFile();
            sharedFile.RegisterProfile(profile);
            //sharedFile.UnregisterProfile(dumstr);
            string filepath = sharedFile.FilePath;
            return filepath;
        }
        private void WriteProfile(string profileName, string keyId, string secret, string acctExternalId, string dbadmusr, string dbadmpwd)
        {
            var sharedFile = new SharedCredentialsFile();
            CredentialProfileOptions options = new CredentialProfileOptions
            {
                AccessKey = keyId,
                SecretKey = secret
            };
            var profile = new CredentialProfile(profileName, options);

            if (ExistsCredFile())
            {
                sharedFile = OpenCredFile();
                sharedFile.RegisterProfile(profile);
            }
            else
            {
                sharedFile.RegisterProfile(profile);
            }

            saveCredFile();
            closeShareProfileFile();    //protect the credential info

        }

        private void btnSaveCred_Click(object sender, EventArgs e)
        {
            if (btnSaveCred.Text.ToUpper().Contains("USE PROFILE"))
            {
                string oodbendpoint = string.Empty;
                oodbendpoint = AWSConnections.getAWSRDSCleint(tbxAccKey.Text, tbxSecKey.Text, tbxRDSsf.Text);
                tbxDBep.Text = oodbendpoint;


                string oos3name = string.Empty;
                oos3name = AWSConnections.getAWSS3Client(tbxAccKey.Text, tbxSecKey.Text);
                tbxS3buckets.Text = oos3name;

                string oousernmae = string.Empty;
                oousernmae = AWSConnections.LoginDB(tbxDbUserP.Text, tbxDbPwdP.Text, tbxDBep.Text, ooData.olinkDBName);

            }
            else if (btnSaveCred.Text.ToUpper().Contains("CREATE PROFILE"))
            {

                string oodbendpoint = string.Empty;
                oodbendpoint = AWSConnections.getAWSRDSCleint(tbxAccKey.Text, tbxSecKey.Text, tbxRDSsf.Text);
                tbxDBep.Text = oodbendpoint;

                string oos3name = string.Empty;
                oos3name = AWSConnections.getAWSS3Client(tbxAccKey.Text, tbxSecKey.Text);
                tbxS3buckets.Text = oos3name;

                string oousernmae = string.Empty;
                oousernmae = AWSConnections.LoginDB(tbxDbUserP.Text, tbxDbPwdP.Text, tbxDBep.Text, ooData.olinkDBName);

                if (oousernmae != tbxDbUserP.Text)
                {
                    ShowMsgD("Database Login Failed. Please try again.");
                    return;  //login failed
                }
                //bool b_dbtbl_create = AWSConnections.CreateooTables(ooData.olinkDBName, tbxDbUserP.Text, tbxDbPwdP.Text, tbxDBep.Text);
                //if (!AWSConnections.checkooDbExists(ooData.olinkDBName, tbxDbUserP.Text, tbxDbPwdP.Text, tbxDBep.Text, ooData.olinkRDSDBName))
                //{
                //    bool b_dbtb//l_create = AWSConnections.CreateooDb(ooData.olinkDBName, tbxDbUserP.Text, tbxDbPwdP.Text, tbxDBep.Text, ooData.olinkRDSDBName);
                //}

                WriteProfile(tbxProfName.Text, tbxAccKey.Text, tbxSecKey.Text, "", tbxDbUserP.Text, tbxDbPwdP.Text);
                listprofilenames();
            }
            else if (btnSaveCred.Text.ToUpper().Contains("LOG IN"))
            {
                string oodbendpoint = string.Empty;
                oodbendpoint = AWSConnections.getAWSRDSCleint(tbxAccKey.Text, tbxSecKey.Text, tbxRDSsf.Text);
                tbxDBep.Text = oodbendpoint;

                string oousernmae = string.Empty;
                oousernmae = AWSConnections.LoginDB(tbxDbUserP.Text, tbxDbPwdP.Text, tbxDBep.Text, ooData.olinkDBName);
                if (oousernmae != tbxDbUserP.Text)
                {
                    ShowMsgD("Database server Login Failed. Please try again.");
                    return;  //login failed
                }
                ShowMsgD("Remote database server login done.");

                if (!AWSConnections.checkooDbExists(ooData.olinkDBName, tbxDbUserP.Text, tbxDbPwdP.Text, tbxDBep.Text, ooData.olinkRDSDBName))
                {
                    ShowMsgD("No oLink database has been created to proceed. Please try again.");
                    return;
                }
                ShowMsgD("Default database to function has been found.");

                if (!AWSConnections.ExistsooTables(ooData.olinkDBName, tbxDbUserP.Text, tbxDbPwdP.Text, tbxDBep.Text))
                {
                    ShowMsgD("Not all default oLink database tables have been created to proceed. Please try again.");
                    return;
                }
                ShowMsgD("Default database tables to function have been found.");

                string conn = AWSConnections.RDSConnString(tbxDbUserP.Text, tbxDbPwdP.Text, tbxDBep.Text, ooData.olinkDBName);
                tbxDbConnString.Text = conn;

                ShowhideDataTabs(true);
                ShowhideCredsTab(false);
                ShowDataGridView1();
            }

        }

        private CredentialProfile retrieveByProfileName(string profilename)
        {
            List<CredentialProfile> pl = loadProfileFromCredFile();
            var profile = pl.Find(p => p.Name == profilename);
            return profile;
        }
        private void cbxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxProfiles.SelectedIndex > -1)
            {
                CredentialProfile profile = retrieveByProfileName(cbxProfiles.Text);

                if (profile != null && profile.Options != null)
                {
                    tbxAccKey.Text = profile.Options.AccessKey;
                    tbxSecKey.Text = profile.Options.SecretKey;
                    EnableDbLogins(true);
                    btnSaveCred.Enabled = true;
                    btnSaveCred.Text = "Log In";
                    tbxProfName.Text = cbxProfiles.Text;
                    tbxcurUser.Text = AWSConnections.getAWSUserId(tbxAccKey.Text, tbxSecKey.Text);
                    tbxS3buckets.Text = AWSConnections.getAWSS3Client(tbxAccKey.Text, tbxSecKey.Text);
                    tbxDBep.Text = AWSConnections.getAWSRDSCleint(tbxAccKey.Text, tbxSecKey.Text, tbxRDSsf.Text);
                }
            }
        }

        private void tabEntries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabEntries.SelectedIndex >= 0)
            {//Management Console
                textBoxS3Id.Text = tbxAccKey.Text;
                textBoxS3Key.Text = tbxSecKey.Text;
                textBoxS3Bucket.Text = tbxS3buckets.Text;
            }
        }

        private void ckbcloudfront_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckbcloudfront.Checked)
            {
                DialogResult confirmResult = MessageBox.Show("Uncheck Cloudfront will make your S3 bucket public",
                                     "Confirm Uncheck!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.No)
                {
                    ckbcloudfront.Checked = true;
                }
            }
        }

        private void TogglePublicAccessBlock(bool toblock)
        {
            try
            {
                //When using CF: Turn on Block All Public Access" & Deny all others from GetS3Object (Permission Policy)
                //When ot using CF: Turn off "Block All Public Access" & Allow all others from GetS3Object (Permission Policy)
                // Create a client
                AmazonS3Client s3Client = AWSConnections.getAWSS3Cli(tbxAccKey.Text, tbxSecKey.Text);

                string st_myS3name = AWSConnections.getAWSS3Client(tbxAccKey.Text, tbxSecKey.Text);

                GetPublicAccessBlockRequest request = new GetPublicAccessBlockRequest();
                request.BucketName = st_myS3name;
                GetPublicAccessBlockResponse response = s3Client.GetPublicAccessBlock(request);

                PutPublicAccessBlockRequest request1 = new PutPublicAccessBlockRequest();
                request1.BucketName = st_myS3name;
                request1.PublicAccessBlockConfiguration = new PublicAccessBlockConfiguration
                { BlockPublicAcls = toblock, BlockPublicPolicy = toblock, IgnorePublicAcls = toblock, RestrictPublicBuckets = toblock };
                PutPublicAccessBlockResponse response1 = s3Client.PutPublicAccessBlock(request1);
                string msg_x = toblock ? "private." : "public.";
                ShowMsgD("S3 Bucket is made " + msg_x);
                s3Client.Dispose();

            }
            catch (Exception exp)
            {
                ShowMsgD("S3 Bucketsetting change failed. Please manually modify on your account if necessary. ");
            }

        }
    }
}
