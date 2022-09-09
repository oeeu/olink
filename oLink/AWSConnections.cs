using Amazon;
using Amazon.IdentityManagement;
using Amazon.RDS;
using Amazon.RDS.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oLink
{
    public class AWSConnections
    {
        /*
         * credentials is the default AWS credential file, which can be open with a text editor
         * credentials.dat will be created as an encrypted file
         * If there is profile created in the .dat file, user will need to enter info below to register a profile
         * 1) Profile Name, as user likes
         * 2) Profile Id, "aws_access_key_id", a user account will be created for any new user under the administrator, AWS gives the user a new "_id" and "_key"
         * 3) Profile Key, "aws_secret_access_key"
         * 4) DB User Name, by default, it's the user name for the user account, which is not saved in the credential file; if the user likes, admin can create a new user name
         * 5) DB Password
         * The administrator can be olink team if a client prefers or the client's own administrator (Root user)
         * 
         */
        public static string awsusername = string.Empty;
        public static string S3_suffix = "-S3";
        public static string RDS_suffix = "-RDB";
        public static string getAWSUserId(string acckey, string seckey)
        {
            string curusername = string.Empty;
            try
            {
                var iamClient = new AmazonIdentityManagementServiceClient(acckey, seckey);
                curusername = iamClient.GetUser().User.UserName; //GetUserpolicy has been created and added as a permission for this user
                //DB name: usernmae + "-rdb"
                //S3 name: usernmae + "-s3"
            }
            catch (Exception ex)
            {
                
            }

            return curusername;
        }
        public static string getAWSRDSCleint(string acckey, string seckey, string st_RDS_suffix)
        {
            return getAWSRDSCleint(acckey, seckey, RegionEndpoint.USEast1, st_RDS_suffix);
        }
        public static string getAWSRDSCleint(string acckey, string seckey, RegionEndpoint region, string st_RDS_suffix)
        {
            //REMOTE DB: a sql server database manually created for a client (RFA, SOH, etc)
            string oodbendpoint = string.Empty;
            try
            {
                AmazonRDSClient rds = new AmazonRDSClient(acckey, seckey, region);

                var request = new DescribeDBInstancesRequest();
                var response = rds.DescribeDBInstances(request);

                
                awsusername = AWSConnections.getAWSUserId(acckey, seckey);
                string rdbname_look = awsusername + st_RDS_suffix;

                if (response.DBInstances.Count > 0)
                {
                    DBInstance fnddb = response.DBInstances.Find(x => x.DBInstanceIdentifier.ToUpper() == rdbname_look.ToUpper());
                    if (fnddb != null)
                    {
                        oodbendpoint = fnddb.Endpoint.Address + ", " + fnddb.Endpoint.Port;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return oodbendpoint;
        }

        public static string getAWSS3Client(string acckey, string seckey)
        {
            return getAWSS3Client(acckey, seckey, RegionEndpoint.USEast1);
        }
        public static string getAWSS3Client(string acckey, string seckey, RegionEndpoint region)
        {
            //acckey is associated with an AWS user
            AmazonS3Client s3Client = new AmazonS3Client(acckey, seckey, region);
            ListBucketsResponse buckets = s3Client.ListBuckets();
            string oos3name = string.Empty;
            awsusername = AWSConnections.getAWSUserId(acckey, seckey);
            string s3name_look = awsusername + S3_suffix;
            if (buckets != null && buckets.Buckets != null && buckets.Buckets.Count > 0)
            {
                //oos3name = buckets.Buckets[0].BucketName;
                S3Bucket bkt = buckets.Buckets.Find(b => b.BucketName.ToUpper() == s3name_look.ToUpper());
                if (bkt != null)
                {
                    oos3name = bkt.BucketName;
                }
            }
            return oos3name;
        }

        public static AmazonS3Client getAWSS3Cli(string acckey, string seckey)
        {
            return getAWSS3Cl(acckey, seckey, RegionEndpoint.USEast1);
        }
        public static AmazonS3Client getAWSS3Cl(string acckey, string seckey, RegionEndpoint region)
        {
            //acckey is associated with an AWS user
            AmazonS3Client s3Client = new AmazonS3Client(acckey, seckey, region);
            ListBucketsResponse buckets = s3Client.ListBuckets();
            string oos3name = string.Empty;
            awsusername = AWSConnections.getAWSUserId(acckey, seckey);
            string s3name_look = awsusername + S3_suffix;
            if (buckets != null && buckets.Buckets != null && buckets.Buckets.Count > 0)
            {
                //oos3name = buckets.Buckets[0].BucketName;
                S3Bucket bkt = buckets.Buckets.Find(b => b.BucketName.ToUpper() == s3name_look.ToUpper());
                if (bkt != null)
                {
                    oos3name = bkt.BucketName;
                }
            }
            return s3Client;
        }

        public static string RDSConnString(string oousername, string oopwd, string dbserver, string oodefaultdb)
        {
            string connetionString;
            string str_username = string.Empty;
            SqlConnection cnn;
            connetionString = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
            connetionString = string.Format(connetionString, dbserver, oodefaultdb, oousername, oopwd);
            
            return connetionString;
        }

        public static string LoginDB(string oousername, string oopwd, string RDSendpoint, string oodefaultdb)
        {
           

            string connetionString;
            string str_username = string.Empty;
            SqlConnection cnn;
            connetionString = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
            connetionString = string.Format(connetionString, RDSendpoint, ooData.olinkRDSDBName, oousername, oopwd);
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                cnn.Close();
                if (!checkooDbExists(ooData.olinkDBName, oousername, oopwd, RDSendpoint, ""))
                {
                    CreateooDb(ooData.olinkDBName, oousername, oopwd, RDSendpoint, "");
                }
                return oousername;
            }
            catch (Exception ex)
            {
                //ShowMsg(ex.Message);
            }
            finally
            {
                cnn.Close();
            }
            return "";
        }

        public static bool checkooDbExists(string ootargetdb, string oousername, string oopwd, string dbserver, string oodefaultdb)
        {
            string sqlDBQuery;
            bool result = false;

            string connetionString;
            connetionString = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
            connetionString = string.Format(connetionString, dbserver, ooData.olinkRDSDBName, oousername, oopwd);
            SqlConnection tmpConn = new SqlConnection(connetionString);
            try
            {
                sqlDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", ootargetdb);
        
                using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlDBQuery, tmpConn))
                    {
                        tmpConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        tmpConn.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {

                result = false;
            }
            finally
            {
                if (tmpConn.State == ConnectionState.Open)
                {
                    tmpConn.Close();
                }
            }
            return result;
        }

        public static bool CreateooDb (string ootargetdb, string oousername, string oopwd, string dbserver, string oodefaultdb)
        {
            string connetionString;
            connetionString = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
            connetionString = string.Format(connetionString, dbserver, ooData.olinkRDSDBName, oousername, oopwd);
            SqlConnection myConn = new SqlConnection(connetionString);
            SqlCommand myCommand = new SqlCommand(ooData.olinkcreatedb, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                myConn.Close();

                bool btablecreated = CreateooTables(ootargetdb, oousername, oopwd, dbserver);
                return true && btablecreated;
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public static bool CreateooTables(string ootargetdb, string oousername, string oopwd, string dbserver)
        {
            string connetionString;
            connetionString = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
            connetionString = string.Format(connetionString, dbserver, ootargetdb, oousername, oopwd);
            SqlConnection myConn = new SqlConnection(connetionString);
            SqlCommand myCommand = new SqlCommand(ooData.olinkcreatetables, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public static bool ExistsooTables(string ootargetdb, string oousername, string oopwd, string dbserver)
        {
            string connetionString;
            bool result = false;
            connetionString = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
            connetionString = string.Format(connetionString, dbserver, ootargetdb, oousername, oopwd);
            SqlConnection myConn = new SqlConnection(connetionString);
            //Nvarchar comparison
            string[] tnarray = ooData.olinktablelist.Split(';').Select(n => "N'" + n.Trim().Replace("[", "").Replace("]", "") + "'").ToArray();

            string sqlDBQuery = "SELECT count(*) FROM sys.tables WHERE name in (" + string.Join(",", tnarray) + ")";
            SqlCommand myCommand = new SqlCommand(sqlDBQuery, myConn);
            try
            {
                using (myConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlDBQuery, myConn))
                    {
                        myConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int foundtablecnt = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out foundtablecnt);
                        }

                        myConn.Close();

                        result = (foundtablecnt == ooData.olinktablelist.Split(';').Length);
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return result;
        }
    }
}
