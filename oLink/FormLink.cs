using Ganss.XSS;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace oLink
{
    public partial class FormLink : Form
    {
        private static string sAppName = "oLink";
        private static string sVersion = "001";
        private static Point mp;
        private static Random random = new Random();
        private static string sUserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.162 Safari/537.36";
        public static string sWaited = "<div class=\"artn\"><p style=\"text-align:center;\">^wait^</p></div>";
        public static string sNFound = "<div class=\"artn\"><p style=\"text-align:center;\">解析失败</p></div>";
        public static string sConvert = "<div class=\"artn\"><p style=\"text-align:center;\">正在转换</p></div>";

        public FormLink()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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

                ShowConfig();
                ShowDataGridView1();
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
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
        {
            try
            {
                string s1 = ooData.sG10配置Set
                    .Replace("^S3Id^", GetSqlParam(textBoxS3Id.Text.Trim()))
                    .Replace("^S3Key^", GetSqlParam(textBoxS3Key.Text.Trim()))
                    .Replace("^S3Bucket^", GetSqlParam(textBoxS3Bucket.Text.Trim()))
                    .Replace("^Name^", GetSqlParam(textBoxName.Text.Trim()))
                    .Replace("^Note^", GetSqlParam(textBoxNote.Text.Trim()));
                ExecuteSQL(s1);
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string s1 = textBox3.Text.Trim();
                if (s1 == "" || (!s1.StartsWith("https://") && !s1.StartsWith("http://"))) return;
                string s4 = ooData.sG10事物Add
                    .Replace("^名称^", GetSqlParam(s1));
                ExecuteSQL(s4);
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
                    string s4 = ooData.sG10事物Del
                        .Replace("^序号^", GetSqlParam(dataGridView1.Rows[i].Cells[1].Value.ToString()));
                    ExecuteSQL(s4);
                }
                ShowDataGridView1();
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Retrieve);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Retrieve()
        {
            try
            {
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
                        string sql = ooData.sG10事物Set
                            .Replace("^标题^", GetSqlParam(s标题))
                            .Replace("^摘要^", GetSqlParam(s摘要))
                            .Replace("^封面^", GetSqlParam(s封面))
                            .Replace("^名称^", GetSqlParam(s2));
                        string s3 = ExecuteSQLResult(sql);

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
            ShowMsgD("Done");
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
            ShowMsgD("Done");
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
                string s1 = ooData.sG10文件Add
                    .Replace("^名称^", GetSqlParam(name));
                string s2 = ExecuteSQLResult(s1);
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
            ShowMsgD("Start");
            try
            {
                string s0 = Path.GetDirectoryName(Application.ExecutablePath);
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
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            ShowMsgD("Done");
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
                string url = "https://s3.amazonaws.com/^S3Bucket^/Site/show.htm?ag=olHome&pin=^random^#olHome"
                    .Replace("^S3Bucket^", textBoxS3Bucket.Text.Trim())
                    .Replace("^random^", GetRandom());
                string s = GetHtmlMethod("GET", "https://is.gd/create.php?format=simple&url=" + EnUrlSymbol(url), "", "", "", "");
                ShowMsgD(s);
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        }

        // Player
        public static string GetVideoPlayer(string url, string track = "", string cover = "", string myip = "", bool bDownload = true)
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
            //if (track != "" && track.EndsWith(".vtt"))
            //{
            //    sTrackZh = "zh";
            //    sTrackZhSrc = EnUrl2(track);
            //    sTrack += "\r\n<track kind=\"subtitles\" label=\"中文\" srclang=\"zh\" src=\"" + sTrackZhSrc + "\" default>";
            //}

            //if (url.StartsWith("https://www.youtube.com/") || url.StartsWith("https://youtu.be/") || url.StartsWith("https://www.youtube.com/embed/"))
            //{
            //    url = url.Replace("https://www.youtube.com/watch?v=", "").Replace("https://youtu.be/", "").Replace("https://www.youtube.com/embed/", "");
            //    if (url.Length > 11) url = url.Substring(0, 11);

            //    string sGetYtb = "";
            //    string[] ssGetYtb = sGetYtb.Split(',');
            //    string uinfo = "https://www.youtube.com/get_video_info?eurl=&sts=^sSts^&video_id=".Replace("^sSts^", ssGetYtb[0]) + url;
            //    string s1 = GetHtml(uinfo, true);
            //    if (s1.Contains("%22status%22%3A%22UNPLAYABLE%22")) s1 = GetHtml(uinfo + "&el=detailpage", true); // "&el=embedded", "&el=vevo", ""
            //    if (s1 == "") return sNFound;

            //    // Wait
            //    Match mWait = new Regex("(?<=%22status%22%3A%22LIVE_STREAM_OFFLINE%22%2C%22reason%22%3A%22)([\\S\\s]*?)(?=%22)").Match(s1);
            //    if (mWait.Success) return sWaited.Replace("^wait^", HttpUtility.UrlDecode(mWait.Value)
            //        + "<img class='artl' src='" + EnUrl2("https://i.ytimg.com/vi/" + url + "/maxresdefault.jpg") + "'>");

            //    // m3u8
            //    Match mM3u8 = new Regex("(?<=%22hlsManifestUrl%22%3A%22)([\\S\\s]*?)(?=%22)").Match(s1);
            //    if (mM3u8.Success)
            //    {
            //        string sM3u8 = GetHtml(mM3u8.Value.Replace("%3A", ":").Replace("%2F", "/").Replace("%252C", ",").Replace("%253D", "="));
            //        Match mM3u8_360 = new Regex("(?<=RESOLUTION=640x[\\S\\s]*?)(https[\\S]*?)(?=\\s)").Match(sM3u8);
            //        if (!mM3u8_360.Success) return sNFound;
            //        string m3u8 = GetHtml(mM3u8_360.Value);
            //        string[] m3u8s = m3u8.Split('\n'); //Log(url + " " + m3u8s.Length+" "+ mM3u8_360.Value);
            //        if (m3u8s.Length > 100 || m3u8 == "") return sConvert;
            //        sM3u8 = "http://or9a.odisk.org/oo.aspx?name=get_m3u8&ag=" + HttpUtility.UrlEncode(mM3u8_360.Value)
            //            + "&myip=" + myip + "&type=play.m3u8";
            //        return GetM3u8Player(sM3u8);
            //    }

            //    Match mTrackList = new Regex("(?<=%22captionTracks%22%3A%5B)([\\S\\s]*?)(?=%5D)").Match(s1);
            //    string sTrackList = mTrackList.Value
            //        .Replace("%3A", ":").Replace("%2F", "/").Replace("%3F", "?").Replace("%26", "&")
            //        .Replace("%3D", "=").Replace("%2C", ",").Replace("%2B", "+")
            //        .Replace("%7B", "{").Replace("%7D", "}").Replace("%22", "\"")
            //        .Replace("%252C", ",").Replace("%5C", "\\").Replace("\\u0026", "&");
            //    MatchCollection mc = new Regex("\"name\":\\{([\\S\\s]*?)\\}").Matches(sTrackList);
            //    foreach (Match m in mc)
            //        sTrackList = sTrackList.Replace(m.Value, m.Value.Replace("\"name\":{", "").Replace("}", ""));
            //    if
            //            (sTrackList.Contains("\"vssId\":\".zh-CN\"")) sTrackZh = ".zh-CN"; // lang_code
            //    else if (sTrackList.Contains("\"vssId\":\".zh\"")) sTrackZh = ".zh";
            //    else if (sTrackList.Contains("\"vssId\":\".zh-Hans\"")) sTrackZh = ".zh-Hans";
            //    else if (sTrackList.Contains("\"vssId\":\".zh-HK\"")) sTrackZh = ".zh-HK";
            //    else if (sTrackList.Contains("\"vssId\":\".zh-TW\"")) sTrackZh = ".zh-TW";
            //    else if (sTrackList.Contains("\"vssId\":\".zh-SG\"")) sTrackZh = ".zh-SG"; // 新加坡
            //    else if (sTrackList.Contains("\"vssId\":\".zh-Hant\"")) sTrackZh = ".zh-Hant";
            //    else if (sTrackList.Contains("\"vssId\":\".zh-Hans.GZolFkXAZLc\"")) sTrackZh = ".zh-Hans.GZolFkXAZLc";
            //    else if (sTrackList.Contains("\"vssId\":\".nan-TW\"")) sTrackZh = ".nan-TW"; // Min Nan Chinese (Taiwan)
            //    if
            //            (sTrackList.Contains("\"vssId\":\".en\"")) sTrackEn = ".en";
            //    else if (sTrackList.Contains("\"vssId\":\".en-GB\"")) sTrackEn = ".en-GB";
            //    else if (sTrackList.Contains("\"vssId\":\".en-US\"")) sTrackEn = ".en-US";
            //    else if (sTrackList.Contains("\"vssId\":\"a.en\"")) sTrackEn = "a.en";
            //    else if (sTrackList.Contains("\"vssId\":\"a.ja\"")) sTrackEn = "a.ja";
            //    else if (sTrackList.Contains("\"vssId\":\"a.de\"")) sTrackEn = "a.de";
            //    else if (sTrackList.Contains("\"vssId\":\"a.ru\"")) sTrackEn = "a.ru";
            //    else if (sTrackList.Contains("\"vssId\":\"a.fr\"")) sTrackEn = "a.fr"; // 法
            //    mc = new Regex("\\{([\\S\\s]*?)\\}").Matches(sTrackList);
            //    if (sTrackZh != "" && sTrackZhSrc == "")
            //        foreach (Match m in mc)
            //            if (m.Value.Contains("\"vssId\":\"" + sTrackZh + "\""))
            //            {
            //                Match m2 = new Regex("(?<=\"baseUrl\":\")([\\S\\s]*?)(?=\")").Match(m.Value);
            //                sTrackZhSrc = EnUrl2(m2.Value + "&fmt=vtt&type=.vtt");
            //                sTrack += "\r\n<track kind=\"subtitles\" label=\"^origin^\" srclang=\"^code^\" src=\"^src^\" default>"
            //                .Replace("^origin^", "原始中文").Replace("^code^", sTrackZh).Replace("^src^", sTrackZhSrc);
            //            }
            //    if (sTrackEn != "")
            //        foreach (Match m in mc)
            //            if (m.Value.Contains("\"vssId\":\"" + sTrackEn + "\""))
            //            {
            //                Match m2 = new Regex("(?<=\"baseUrl\":\")([\\S\\s]*?)(?=\")").Match(m.Value);
            //                if (sTrackZh == "")
            //                {
            //                    sTrackZh = "zh-Hans";
            //                    sTrackZhSrc = EnUrl2(m2.Value + "&fmt=vtt&tlang=zh-Hans&type=.vtt");
            //                    sTrack += "\r\n<track kind=\"subtitles\" label=\"^origin^\" srclang=\"zh\" src=\"^src^\" default>"
            //                    .Replace("^origin^", "自动中文").Replace("^src^", sTrackZhSrc);
            //                }
            //                sTrackEnSrc = EnUrl2(m2.Value + "&fmt=vtt&type=.vtt");
            //                sTrack += "\r\n<track kind=\"subtitles\" label=\"^origin^\" srclang=\"en\" src=\"^src^\">"
            //                .Replace("^origin^", (sTrackEn == "a.en" ? "自动外文" : "原始外文")).Replace("^src^", sTrackEnSrc);
            //            }

            //    // video & audio
            //    Match m1 = new Regex("(?<=%22formats%22%3A%5B)([\\S]*?)(?=%5D)").Match(s1);
            //    if (!m1.Success) return sNFound;
            //    string s2 = m1.Value
            //        .Replace("%3A", ":").Replace("%2F", "/").Replace("%3F", "?").Replace("%26", "&")
            //        .Replace("%3D", "=").Replace("%2C", ",").Replace("%2B", "+")
            //        .Replace("%7B", "{").Replace("%7D", "}").Replace("%22", "\"");
            //    string[] ss1 = s2.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
            //    foreach (string s in ss1) if (s.Contains("\"itag\":22")) { sVideoV = GetYoutube(s); break; }
            //    foreach (string s in ss1) if (s.Contains("\"itag\":18")) { sVideoM = GetYoutube(s); break; }
            //    foreach (string s in ss1) if (s.Contains("\"itag\":45")) { sVideoV2 = GetYoutube(s); break; }
            //    foreach (string s in ss1) if (s.Contains("\"itag\":43")) { sVideoM2 = GetYoutube(s); break; }
            //    m1 = new Regex("(?<=%22adaptiveFormats%22%3A%5B)([\\S]*?)(?=%5D)").Match(s1);
            //    s2 = m1.Value
            //        .Replace("%3A", ":").Replace("%2F", "/").Replace("%3F", "?").Replace("%26", "&")
            //        .Replace("%3D", "=").Replace("%2C", ",").Replace("%2B", "+")
            //        .Replace("%7B", "{").Replace("%7D", "}").Replace("%22", "\"");
            //    ss1 = s2.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
            //    foreach (string s in ss1) if (s.Contains("\"itag\":140")) { sAudio140 = GetYoutube(s); break; }
            //    foreach (string s in ss1) if (s.Contains("\"itag\":171")) { sAudio171 = GetYoutube(s); break; }
            //    foreach (string s in ss1) if (s.Contains("\"itag\":249")) { sAudio249 = GetYoutube(s); break; }
            //    foreach (string s in ss1) if (s.Contains("\"itag\":250")) { sAudio250 = GetYoutube(s); break; }
            //    foreach (string s in ss1) if (s.Contains("\"itag\":251")) { sAudio251 = GetYoutube(s); break; }
            //    url = EnUrl2(sVideoM);
            //}

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

    //    private static string GetSoundPlayer(string url, string cover = "")
    //    {
    //        url = EnUrl2(url);
    //        return @"
    //<video controls ^autoplay^ preload=""auto"" crossorigin=""anonymous"" style=""width:100%; height:180px; background:#faf2e1;""
    //    src=""^src^"" ^poster^>
    //</video>"
    //        .Replace("^autoplay^", (cover != "" ? "" : "")).Replace("^src^", GetHtmlParam(url)) /// autoplay
    //        .Replace("^poster^", (cover != "" ? "poster='" + GetHtmlParam(cover) + "'" : ""));
    //    }

        private static string GetM3u8Player(string url)
        {
            url = GetEnUrl(url);
            return @"
    <video id=""p@id"" class=""video-js"" controls autoplay width=""100%"" data-setup=""{}"">
        <source src=""@src"" type=""application/x-mpegURL"">
    </video>".Replace("@id", GetRandomNum()).Replace("@src", url); /// no poster= poster=""""
        }

//        private static string GetFlashPlayer(string url)
//        {
//            url = EnUrl2(url);
//            return @"
//<link rel=""stylesheet"" href=""/src/functional.css"">
//<style> .flowplayer {background-color:#ffffd0;} </style>
//<script src=""/src/flowplayer.min.js""></script>
//<div class=""flowplayer"">
//    <video>
//        <source type=""video/flash"" src=""@src"">
//    </video>
//</div>".Replace("@src", GetHtmlParam(url));
//        }

        private static string GetImagePlayerTop(string url)
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

//        private static string GetImagePlayer(string url)
//        {
//            url = EnUrl2(url);
//            return @"<div class='videocontainer' style='max-width:580px; margin:6px auto;'>
//    <img style='width:100%;' src='^url^'/>
//    <div class=""navv""><div class=""nav"" style=""width:100%; height:28px; position:absolute; top:0;"">
//        <ul class=""nav__menu"" style=""text-align:right;"">
//            <li class=""nav__menu-item""><a href=""javascript:void(0);"" onclick=""javascript:Save('^url^');"">下载</a></li>
//        </ul>
//    </div></div>
//</div>"
//            .Replace("^url^", GetHtmlParam(url));
//        }

        //private static string GetDownloadPlayer(string url)
        //{
        //    url = EnUrl2(url);
        //    return "\r\n  <div class='buttoncontainer'><a href='javascript:void(0);' onclick=\"javascript:Save('"
        //        + GetHtmlParam(url) + "');\"><div class='button'>点击下载</div></a></div><div class='lisw'></div>";
        //}

        //private static string GetTwitter(string co, string cover, string title)
        //{
        //    string r = "";
        //    try
        //    {
        //        string[] ss = co.Split(new string[] { "[分隔[时间]分隔]" }, StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string s in ss)
        //        {
        //            string[] ss2 = s.Split(new string[] { "[分隔[推文]分隔]" }, StringSplitOptions.None);
        //            r +=
        //              "\r\n    <div class='lisu'>"
        //            + "\r\n      <div class='picv'><img class='imgh' src='" + cover + "'></div>"
        //            + "\r\n      <div class='artv'><p class='namv'>" + "<font class='count'>" + title + " " + ss2[0] + "</font></p>"
        //            + "\r\n      </div>"
        //            + "\r\n    </div>";
        //        }
        //    }
        //    catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name, ex.Message); }
        //    return r;
        //}

        private static string GetEnUrl(string url)
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
            // filter example:
            //content += @"<><<>img src=x onerror=""alert('img tag/script bypassed filtering'); "">";
            var sanitizer = new HtmlSanitizer();
            try
            {
                content = sanitizer.Sanitize(content, url);
                return content;
            }
            catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            return "";
            //try
            //{
            //    content = HtmlDel(content, "(<head)([\\S\\s]*?)(</head>)");
            //    content = HtmlDel(content, "(<script)([\\S\\s]*?)(</script>)");
            //    content = HtmlDel(content, "(<noscript)([\\S\\s]*?)(</noscript>)");
            //    content = HtmlDel(content, "(<style)([\\S\\s]*?)(</style>)");
            //    content = HtmlDel(content, "(<ins)([\\S\\s]*?)(</ins>)");

            //    MatchCollection mc1 = new Regex("(<a )([\\S\\s]*?)(</a>)").Matches(content);
            //    foreach (Match m in mc1)
            //    {
            //        Match m0 = new Regex("(?<=href=\"[\\s]*?)([\\S]*?)(?=[\\s]*?\")").Match(m.Value);
            //        if (!m0.Success) m0 = new Regex("(?<=href=)([\\S]*?)(?=[\\s|>])").Match(m.Value);
            //        Match m1 = new Regex("(?<=>)([\\S\\s]*?)(?=</a>)").Match(m.Value);
            //        if (!m0.Success || !m1.Success) { content = content.Replace(m.Value, ""); continue; }
            //        string sHref = HtmlHref(m0.Value, url);
            //        string s1 = ooData.sG10事物Get.Replace("^名称^",GetSqlParam(sHref));
            //        string s2 = ExecuteSQLResult(s1);
            //        if (s2 != "")
            //            content = content.Replace(m.Value, "[链接[c" + int.Parse(s2).ToString("D6") + "[链接]" + m1.Value + "]链接]");
            //        else content = content.Replace(m.Value, m1.Value);
            //    }

            //    MatchCollection mc0 = new Regex("(<)([\\S\\s]*?)(>)").Matches(content);
            //    foreach (Match m in mc0)
            //    {
            //        if (m.Value == "<p>" || m.Value == "</p>" || m.Value == "<p class=\"artl\">" || m.Value == "<p class=\"artc\">"
            //            || m.Value == "<p class=\"artl\" style=\"text-align:center;\">" || m.Value == "<p style=\"text-align:center;\">"
            //            || m.Value == "<b>" || m.Value == "</b>" || m.Value == "<br/>" || m.Value == "<br>" || m.Value == "<br />"
            //            || m.Value == "<strong>" || m.Value == "</strong>"
            //            || m.Value == "</h1>" || m.Value == "</h2>" || m.Value == "</h3>" || m.Value == "</h4>" || m.Value == "</h5>"
            //            || m.Value == "<em>" || m.Value == "</em>" || m.Value == "<sup>" || m.Value == "</sup>") ;
            //        else if (m.Value.StartsWith("<h1")) { content = content.Replace(m.Value, "<h1 style=\"text-align:center;\">"); }
            //        else if (m.Value.StartsWith("<h2")) { content = content.Replace(m.Value, "<h2 style=\"text-align:center;\">"); }
            //        else if (m.Value.StartsWith("<h3")) { content = content.Replace(m.Value, "<h3 style=\"text-align:center;\">"); }
            //        else if (m.Value.StartsWith("<h4")) { content = content.Replace(m.Value, "<h4 style=\"text-align:center;\">"); }
            //        else if (m.Value.StartsWith("<h5")) { content = content.Replace(m.Value, "<h5 style=\"text-align:center;\">"); }
            //        else if (m.Value.StartsWith("<p")) content = content.Replace(m.Value, "<p>");
            //        else if (m.Value.StartsWith("<img") || m.Value.StartsWith("<IMG"))
            //        {
            //            Match m0 = new Regex("(?<=data-src=\")([\\S\\s]*?)(?=\")").Match(m.Value);
            //            if (!m0.Success) m0 = new Regex("(?<=src=\")([\\S\\s]*?)(?=\")").Match(m.Value);
            //            if (!m0.Success) m0 = new Regex("(?<=src=)([\\S]*?)(?=[\\s|>])").Match(m.Value);
            //            if (m0.Value.StartsWith("data:image/")) continue;
            //            Match m1 = new Regex("(?<=width=\")([\\S\\s]*?)(?=\")").Match(m.Value);
            //            string sHref = GetHtmlParam(HtmlHref(m0.Value, url));
            //            int w = 0; int.TryParse(m1.Value, out w);
            //            if (m0.Value == "/rmb/images/rmb_donation_200x200.jpg"
            //                || sHref == "https://www.rfa.org/++plone++rfa-resources/img/icon-zoom.png") content = content.Replace(m.Value, "");
            //            else
            //            {
            //                sHref = GetFile(sHref);
            //                if (w < 45 && w > 0) content = content.Replace(m.Value, "<img src=\"" + EnUrl2(sHref) + "\">");
            //                else content = content.Replace(m.Value, "<img class=\"artl\" src=\"" + EnUrl2(sHref) + "\">");
            //            }
            //        }
            //        else if (m.Value.StartsWith("<table")) content = content.Replace(m.Value, "<table>");
            //        else if (m.Value.StartsWith("<thead")) content = content.Replace(m.Value, "<thead>");
            //        else if (m.Value.StartsWith("<tr")) content = content.Replace(m.Value, "<tr>");
            //        else if (m.Value.StartsWith("<td"))
            //        {
            //            Match m0 = new Regex("( rowspan=\")([\\S\\s]*?)(\")").Match(m.Value);
            //            Match m1 = new Regex("( colspan=\")([\\S\\s]*?)(\")").Match(m.Value);
            //            content = content.Replace(m.Value, "<td" + m0.Value + m1.Value + ">");
            //        }
            //        else if (m.Value.StartsWith("<th"))
            //        {
            //            Match m0 = new Regex("( rowspan=\")([\\S\\s]*?)(\")").Match(m.Value);
            //            Match m1 = new Regex("( colspan=\")([\\S\\s]*?)(\")").Match(m.Value);
            //            content = content.Replace(m.Value, "<th" + m0.Value + m1.Value + ">");
            //        }
            //        else if (m.Value == "</th>" || m.Value == "</td>" || m.Value == "</tr>"
            //            || m.Value == "<tbody>" || m.Value == "</tbody>"
            //            || m.Value == "</thead>" || m.Value == "</table>") ;
            //        else if (m.Value.StartsWith("<sup")) content = content.Replace(m.Value, "<sup>");
            //        else if (m.Value == "<li>" || m.Value.StartsWith("<li ")) content = content.Replace(m.Value, "<li>");
            //        else if (m.Value == "</li>") ;
            //        else if (m.Value.StartsWith("<ol")) content = content.Replace(m.Value, "<ol>");
            //        else if (m.Value == "</ol>") ;
            //        else content = content.Replace(m.Value, "");
            //    }

            //    content = content.Replace("\t", "").Replace("\n\n", "\n").Replace("\n\n", "\n").Replace("\n\n", "\n")
            //        .Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");
            //    return content;
            //}
            //catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
            //return "";
        }

        private static string HtmlHref(string sHref, string url)
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

        public static string UrlProc(string name)
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

        private static string GetHtmlParam(string name)
        {
            return name.Replace("<", "&lt;").Replace(">", "&gt;")
                .Replace("'", "&#39;").Replace("\"", "&quot;");
        }

        // HtmlRsc
        public static string EnUrl2(string name, bool bFirst = false)
        {
            return name;
        }

        public static string EnUrlSymbol(string url)
        {
            return url.Replace(":", "%3A").Replace("/", "%2F").Replace("?", "%3F").Replace("&", "%26").Replace("#", "%23").Replace("=", "%3D");
        }

        // Utility
        public static void Log(string co)
        {
            WriteErr(co);
        }

        public static void Log(string name, string co)
        {
            Log(name + ": " + co);
        }

        private static string sRandomChars = "abcdefghijklmnopqrstuvwxyz";
        private static string sRandomChars2 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static object oRandom = new object();

        public static string GetRandom(int len = 12, bool upper = false)
        {
            string randomChars = upper ? sRandomChars2 : sRandomChars;
            string password = "";
            lock (oRandom) for (int i = 0; i < len; i++) password += randomChars[random.Next(randomChars.Length)];
            return password;
        }

        public static string GetRandomNum()
        {
            string password = "";
            lock (oRandom) password = random.Next(10000000, 99999999).ToString();
            return password;
        }

        //private static string GetTxt部分(string s摘要, string s链接, int i长度)
        //{
        //    if (s摘要.Length > i长度)
        //    {
        //        string s = "";
        //        string[] ss = s摘要.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
        //        for (int i1 = 0; i1 < ss.Length; i1++)
        //            if (s.Length + ss[i1].Length <= i长度 && !ss[i1].StartsWith("#{"))
        //                s += (s == "" ? "" : "\n") + ss[i1];
        //            else break;
        //        s摘要 = s + "\n[链接[" + s链接 + "[链接][语言[全文]语言]]链接]";
        //    }
        //    return s摘要;
        //}

        //private static string GetString千万(string page)
        //{
        //    if (page.Length >= 5) return page.Substring(0, page.Length - 4) + "万";
        //    else if (page.Length >= 4) return page.Substring(0, page.Length - 3) + "千";
        //    else return page;
        //}

        //private static string GetString最长(string sIn, int iLen)
        //{
        //    if (sIn.Length <= iLen) return sIn;
        //    return sIn.Substring(0, iLen - 1) + "…";
        //}

        //private static string Get一行(string sIn)
        //{
        //    return Get换行(sIn).Replace("\n", " ");
        //}

        private static string Get换行(string sIn)
        {
            return sIn.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        //private static string Get时距Param(DateTime dt)
        //{
        //    TimeSpan tsNow = DateTime.Now - dt;
        //    if (tsNow.TotalMinutes < 1.0) return "[语言[刚刚]语言]";
        //    if (tsNow.TotalHours < 1.0) return Math.Floor(tsNow.TotalMinutes) + "[语言[分前]语言]";
        //    if (tsNow.TotalDays < 1.0) return Math.Floor(tsNow.TotalHours) + "[语言[时前]语言]";
        //    return Math.Floor(tsNow.TotalDays) + "[语言[天前]语言]";
        //}

        //private static string GetPict中(string sPict)
        //{
        //    return GetHtmlParam(sPict.Trim()).Replace("#P ", "").Replace(".jpg", "").Replace(".jpeg", "").Replace(".png", "") + ".M.jpg";
        //}

        //private static string GetPictParam(string sPict, string sNumb)
        //{
        //    sPict = GetHtmlParam(sPict.Trim());
        //    if (sPict != "")
        //        return "><img class='imgh' src='" + EnUrl2(sPict.Replace("#P ", "").Replace(".jpg", "").Replace(".jpeg", "").Replace(".png", "")
        //            + ".K.jpg", true) + "'/>";
        //    else return " style='background:hsla(" + int.Parse(sNumb) % 360 + ",75%,50%,0.5)'>";
        //}

        //private static string Get时间(string s)
        //{
        //    string r = "";
        //    try
        //    {
        //        r = DateTime.Parse(s).ToString("yyyy-MM-dd HH:mm");
        //    }
        //    catch { }
        //    return r;
        //}

        //public static string HtmlEn(string input, string password)
        //{
        //    string r = "";
        //    if (input.Length == 0) return "";
        //    r += string.Format("{0:x4}", ((int)input[0]));// + input.Length));
        //    for (int i = 1; i < input.Length; i++) r += string.Format("{0:x4}", ((int)input[i]));// + (int)input[i - 1]);
        //    return r;
        //}

        //public static string RSAEncrypt(string publickey, string content)
        //{
        //    try
        //    {
        //        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //        rsa.FromXmlString(publickey);
        //        byte[] cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
        //        return Convert.ToBase64String(cipherbytes);
        //    }
        //    catch { return ""; }
        //}

        //public static string RSADecrypt(string privatekey, string content)
        //{
        //    try
        //    {
        //        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //        rsa.FromXmlString(privatekey);
        //        byte[] cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
        //        return Encoding.UTF8.GetString(cipherbytes);
        //    }
        //    catch { return ""; }
        //}

        // Show
        private delegate void ShowMsgDelegate(string msg);

        private void ShowLabel(string msg)
        {
            labelMsg.Text = msg;
        }

        //private void ShowLabelD(string msg)
        //{
        //    BeginInvoke(new ShowMsgDelegate(ShowLabel), new object[] { msg });
        //}

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

        //private static void WriteMsg(string message)
        //{
        //    try
        //    {
        //        StreamWriter swlog = File.AppendText(sAppName + "Msg.txt");
        //        swlog.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + message);
        //        swlog.Close();
        //    }
        //    catch { }
        //}

        private static void WriteErr(string message)
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
        public static string GetHtml(string sName, bool bFail = false)
        {
            string r = "";
            r = GetHtmlMethod("GET", sName, "", "", "", "");
            if (r.Contains("charset=gb2312")) r = GetHtmlMethod("GET", sName, "", "", "", "gb2312");
            return r;
        }

        //public static string PostHtml(string sName, string sData)
        //{
        //    return GetHtmlMethod("POST", sName, sData, "", "", "");
        //}

        public static string GetHtmlMethod(string sMethod, string sName, string sData, string sHeader, string sReferer,
            string sCode)
        {
            return GetHtmlMethod(sMethod, sName, sData, sHeader, sReferer, sCode, "", out string c);
        }

        public static string GetHtmlMethod(string sMethod, string sName, string sData, string sHeader, string sReferer,
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

        //public static string GetHtmlMethod(string sMethod, string sName, string sData, string sHeader, string sReferer,
        //    string sContentType, string sHost, string sCode)
        //{
        //    string r = "";
        //    HttpWebRequest request = null;
        //    HttpWebResponse response = null;
        //    Stream requestStream = null;
        //    Stream responseStream = null;
        //    StreamReader streamReader = null;
        //    try
        //    {
        //        request = (HttpWebRequest)WebRequest.Create(sName);
        //        request.UserAgent = sUserAgent;
        //        if (sReferer != "") request.Referer = sReferer;
        //        if (sHost != "") request.Host = sHost;
        //        if (sContentType != "") request.ContentType = sContentType;
        //        else if (sMethod == "POST") request.ContentType = "application/x-www-form-urlencoded";
        //        request.Method = sMethod;
        //        if (sHeader != "")
        //        {
        //            string[] ss0 = sHeader.Split(',');
        //            foreach (string s in ss0)
        //            {
        //                string s0 = s.Substring(0, s.IndexOf(':'));
        //                string s1 = s.Substring(s.IndexOf(':') + 1);
        //                request.Headers[s0.Trim()] = s1.Trim();
        //            }
        //        }
        //        if (sData != "")
        //        {
        //            byte[] postData = Encoding.UTF8.GetBytes(sData);
        //            request.ContentLength = postData.Length;
        //            requestStream = request.GetRequestStream();
        //            requestStream.Write(postData, 0, postData.Length);
        //            requestStream.Close();
        //        }
        //        response = (HttpWebResponse)request.GetResponse();
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            responseStream = response.GetResponseStream();
        //            if (response.ContentEncoding != null && response.ContentEncoding.ToLower().Contains("gzip"))
        //                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
        //            else if (response.ContentEncoding != null && response.ContentEncoding.ToLower().Contains("deflate"))
        //                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
        //            streamReader = new StreamReader(responseStream, Encoding.GetEncoding(sCode == "" ? "utf-8" : sCode));
        //            r = streamReader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //    }
        //    catch (Exception ex) { }
        //    finally
        //    {
        //        if (request != null) request = null;
        //        if (response != null) { response.Close(); response = null; }
        //        if (requestStream != null) requestStream.Close();
        //        if (responseStream != null) responseStream.Close();
        //        if (streamReader != null) streamReader.Close();
        //    }
        //    return r;
        //}

        //public static string GetHtmlMethod(string sMethod, string sName, string sData, string sHeader, string sReferer,
        //    string sContentType, string sCode)
        //{
        //    string r = "";
        //    HttpWebRequest request = null;
        //    HttpWebResponse response = null;
        //    Stream requestStream = null;
        //    Stream responseStream = null;
        //    StreamReader streamReader = null;
        //    try
        //    {
        //        request = (HttpWebRequest)WebRequest.Create(sName);
        //        request.UserAgent = sUserAgent;
        //        if (sReferer != "") request.Referer = sReferer;
        //        if (sContentType != "") request.ContentType = sContentType;
        //        else if (sMethod == "POST") request.ContentType = "application/x-www-form-urlencoded";
        //        request.Method = sMethod;
        //        if (sHeader != "")
        //        {
        //            string[] ss0 = sHeader.Split(',');
        //            foreach (string s in ss0)
        //            {
        //                string s0 = s.Substring(0, s.IndexOf(':'));
        //                string s1 = s.Substring(s.IndexOf(':') + 1);
        //                request.Headers[s0.Trim()] = s1.Trim();
        //            }
        //        }
        //        if (sData != "")
        //        {
        //            byte[] postData = Encoding.UTF8.GetBytes(sData);
        //            request.ContentLength = postData.Length;
        //            requestStream = request.GetRequestStream();
        //            requestStream.Write(postData, 0, postData.Length);
        //            requestStream.Close();
        //        }
        //        response = (HttpWebResponse)request.GetResponse();
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            responseStream = response.GetResponseStream();
        //            if (response.ContentEncoding != null && response.ContentEncoding.ToLower().Contains("gzip"))
        //                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
        //            else if (response.ContentEncoding != null && response.ContentEncoding.ToLower().Contains("deflate"))
        //                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
        //            streamReader = new StreamReader(responseStream, Encoding.GetEncoding(sCode == "" ? "utf-8" : sCode));
        //            r = streamReader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //    }
        //    catch (Exception ex) { }
        //    finally
        //    {
        //        if (request != null) request = null;
        //        if (response != null) { response.Close(); response = null; }
        //        if (requestStream != null) requestStream.Close();
        //        if (responseStream != null) responseStream.Close();
        //        if (streamReader != null) streamReader.Close();
        //    }
        //    return r;
        //}

        //public static string GetHtmlMethod(string sMethod, string sName, string sData, string sHeader, string sReferer)
        //{
        //    string r = "";
        //    HttpWebRequest request = null;
        //    HttpWebResponse response = null;
        //    Stream requestStream = null;
        //    Stream responseStream = null;
        //    StreamReader streamReader = null;
        //    try
        //    {
        //        request = (HttpWebRequest)WebRequest.Create(sName);
        //        request.UserAgent = sUserAgent;
        //        if (sReferer != "") request.Referer = sReferer;
        //        else if (sMethod == "POST") request.ContentType = "application/x-www-form-urlencoded";
        //        request.Method = sMethod;
        //        if (sHeader != "")
        //        {
        //            string[] ss0 = sHeader.Split(',');
        //            foreach (string s in ss0)
        //            {
        //                string s0 = s.Substring(0, s.IndexOf(':'));
        //                string s1 = s.Substring(s.IndexOf(':') + 1);
        //                request.Headers[s0.Trim()] = s1.Trim();
        //            }
        //        }
        //        if (sData != "")
        //        {
        //            byte[] postData = Encoding.UTF8.GetBytes(sData);
        //            request.ContentLength = postData.Length;
        //            requestStream = request.GetRequestStream();
        //            requestStream.Write(postData, 0, postData.Length);
        //            requestStream.Close();
        //        }
        //        response = (HttpWebResponse)request.GetResponse();
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            responseStream = response.GetResponseStream();
        //            if (response.ContentEncoding != null && response.ContentEncoding.ToLower().Contains("gzip"))
        //                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
        //            else if (response.ContentEncoding != null && response.ContentEncoding.ToLower().Contains("deflate"))
        //                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
        //            streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
        //            r = streamReader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //    }
        //    catch (Exception ex) { }
        //    finally
        //    {
        //        if (request != null) request = null;
        //        if (response != null) { response.Close(); response = null; }
        //        if (requestStream != null) requestStream.Close();
        //        if (responseStream != null) responseStream.Close();
        //        if (streamReader != null) streamReader.Close();
        //    }
        //    return r;
        //}

        //public static string GetHtmlMethodOrigin(string sName)
        //{
        //    string r = "";
        //    HttpWebRequest request = null;
        //    HttpWebResponse response = null;
        //    try
        //    {
        //        if (!sName.StartsWith("http://") && !sName.StartsWith("https://")) return "";
        //        request = (HttpWebRequest)WebRequest.Create(sName);
        //        request.UserAgent = sUserAgent;
        //        request.Timeout = request.ReadWriteTimeout = 10 * 1000;
        //        request.AllowAutoRedirect = false;
        //        response = (HttpWebResponse)request.GetResponse();
        //        r = response.Headers["Location"];
        //    }
        //    catch (Exception ex) { Log(MethodBase.GetCurrentMethod().Name + ": " + ex.Message); }
        //    finally
        //    {
        //        if (request != null) request = null;
        //        if (response != null) { response.Close(); response = null; }
        //    }
        //    return r;
        //}

        public bool DownloadHtml(string name, string host, string referer, string sFileName)
        {
            HttpWebRequest request2 = null;
            HttpWebResponse response2 = null;
            try
            {
                request2 = (HttpWebRequest)WebRequest.Create(name);
                if (host != "") request2.Host = host;
                if (referer != "") request2.Referer = referer;
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
                }
                outStream.Close();
                if (response2.ContentLength != -1 && length != total) { }
                //BeginInvoke(new ShowMsgDelegate(ShowLabel), new object[] { "Done" });
                if (response2 != null) { response2.Close(); response2 = null; }
                if (request2 != null) request2 = null;
                return true;
            }
            catch (Exception ex) { }
            if (response2 != null) { response2.Close(); response2 = null; }
            if (request2 != null) request2 = null;
            return false;
        }

        //public bool CheckHtml(string sName)
        //{
        //    HttpWebRequest request = null;
        //    HttpWebResponse response = null;
        //    bool r = false;
        //    try
        //    {
        //        if (!sName.StartsWith("http://") && !sName.StartsWith("https://")) return r;
        //        request = (HttpWebRequest)WebRequest.Create(sName);
        //        request.UserAgent = sUserAgent;
        //        request.Timeout = request.ReadWriteTimeout = 30 * 1000;
        //        response = (HttpWebResponse)request.GetResponse();
        //        if (response.StatusCode == HttpStatusCode.OK) r = true;
        //        else ShowMsgD(response.StatusCode + "");
        //    }
        //    catch (Exception ex) { ShowMsgD(ex.Message); }
        //    finally
        //    {
        //        if (response != null) { response.Close(); response = null; }
        //        if (request != null) request = null;
        //    }
        //    return r;
        //}

        // SQL Utility
        public static string ExecuteSQLResult(string s)
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

        public static bool ExecuteSQL(string s)
        {
            return ExecuteSQL(s, null);
        }

        public static bool ExecuteSQL(string s, DataSet ds)
        {
            string tbname = "result";
            try
            {
                string connTusiStr = ConfigurationManager.AppSettings["DB"];
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

        public static string GetSqlParam(string s)
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
    }
}