using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oLink
{
    public class ooData
    {
        static public string olinkDBName = "oLink";
        static public string olinkRDSDBName = "rdsadmin";
        //static public string sG10配置Set = @"update [" + olinkDBName + "].[dbo].[G10配置] set S3Id=N'^S3Id^',S3Key=N'^S3Key^',S3Bucket=N'^S3Bucket^',Name=N'^Name^',Note=N'^Note^'";
        static public string sG10配置Set_p = @"update [" + olinkDBName + "].[dbo].[G10配置] set S3Id = @S3Id,S3Key = @S3Key,S3Bucket = @S3Bucket,Name = @Name,Note = @Note";
        
        static public string sG10配置Get = @"SELECT S3Id,S3Key,S3Bucket,Name,Note FROM [" + olinkDBName + "].[dbo].[G10配置]";

        //static public string sG10事物Add = @"if (select count(*) from [" + olinkDBName + "]..[G10事物] where 名称=N'^名称^')=0"
        //    + "insert into [" + olinkDBName + "]..[G10事物](名称) values (N'^名称^')";
        static public string sG10事物Add_p = @"if (select count(*) from [" + olinkDBName + "]..[G10事物] where 名称 = @名称)=0 "
            + "insert into [" + olinkDBName + "]..[G10事物](名称) values (@名称)";

        static public string sG10事物All = @"select 序号,名称,标题,摘要,封面 FROM [" + olinkDBName + "].[dbo].[G10事物] order by 序号 desc";
        
        static public string sG10事物Get = @"select 序号 FROM [" + olinkDBName + "].[dbo].[G10事物] where 名称=N'^名称^'";
        static public string sG10事物Get_p = @"select 序号 FROM [" + olinkDBName + "].[dbo].[G10事物] where 名称 = @名称";

        //static public string sG10事物Set = @"update [" + olinkDBName + "].[dbo].[G10事物] set 标题=N'^标题^',摘要=N'^摘要^',封面=N'^封面^' where 名称=N'^名称^'"
        //+ "select 序号 from [" + olinkDBName + "].[dbo].[G10事物] where 名称=N'^名称^'";
        static public string sG10事物Set_p = @"update [" + olinkDBName + "].[dbo].[G10事物] set 标题 = @标题, 摘要 = @摘要,封面 = @封面 where 名称 = @名称 "
        + "select 序号 from [" + olinkDBName + "].[dbo].[G10事物] where 名称 = @名称 ";

        //static public string sG10事物Del = @"delete [" + olinkDBName + "].[dbo].[G10事物] where 序号=N'^序号^'";
        static public string sG10事物Del_p = @"delete [" + olinkDBName + "].[dbo].[G10事物] where 序号 = @序号 ";

        static public string sG10文件Add = @"if (select count(*) from [" + olinkDBName + "].[dbo].[G10文件] where 名称=N'^名称^')=0 " +
            "insert into [" + olinkDBName + "].[dbo].[G10文件](名称) values (N'^名称^')" +
            "select 序号 from [" + olinkDBName + "].[dbo].[G10文件] where 名称=N'^名称^'";
        static public string sG10文件Add_p = @"if (select count(*) from [" + olinkDBName + "].[dbo].[G10文件] where 名称 = @名称)=0 " +
            "insert into [" + olinkDBName + "].[dbo].[G10文件](名称) values (@名称)" +
            "select 序号 from [" + olinkDBName + "].[dbo].[G10文件] where 名称 = @名称";

        static public string olinkcreatedb = @"USE [master] CREATE DATABASE [" + olinkDBName + "]" +
            "CONTAINMENT = NONE ON  PRIMARY " +
            @"( NAME = N'oLink', FILENAME = N'D:\rdsdbdata\DATA\oLink.mdf' , SIZE = 5120KB , FILEGROWTH = 10%)" +
            @"LOG ON ( NAME = N'oLink_log', FILENAME = N'D:\rdsdbdata\DATA\oLink_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)"
+ "ALTER DATABASE [" + olinkDBName + "] SET ANSI_NULLS OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET ANSI_PADDING OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET ANSI_WARNINGS OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET ARITHABORT OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET AUTO_CLOSE OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET AUTO_SHRINK OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET AUTO_CREATE_STATISTICS ON(INCREMENTAL = OFF) " +
            "ALTER DATABASE [" + olinkDBName + "] SET AUTO_UPDATE_STATISTICS ON " +
            "ALTER DATABASE [" + olinkDBName + "] SET CURSOR_CLOSE_ON_COMMIT OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET CURSOR_DEFAULT  GLOBAL " +
            "ALTER DATABASE [" + olinkDBName + "] SET CONCAT_NULL_YIELDS_NULL OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET NUMERIC_ROUNDABORT OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET QUOTED_IDENTIFIER OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET RECURSIVE_TRIGGERS OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET DISABLE_BROKER " +
            "ALTER DATABASE [" + olinkDBName + "] SET AUTO_UPDATE_STATISTICS_ASYNC OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET DATE_CORRELATION_OPTIMIZATION OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET PARAMETERIZATION SIMPLE " +
            "ALTER DATABASE [" + olinkDBName + "] SET READ_COMMITTED_SNAPSHOT OFF " +
            "ALTER DATABASE [" + olinkDBName + "] SET READ_WRITE " +
            "ALTER DATABASE [" + olinkDBName + "] SET RECOVERY FULL " +
            "ALTER DATABASE [" + olinkDBName + "] SET MULTI_USER " +
            "ALTER DATABASE [" + olinkDBName + "] SET PAGE_VERIFY CHECKSUM " +
            "ALTER DATABASE [" + olinkDBName + "] SET TARGET_RECOVERY_TIME = 60 SECONDS " +
            "ALTER DATABASE [" + olinkDBName + "] SET DELAYED_DURABILITY = DISABLED ";

        static public string olinktablelist = "[G10事物]; [G10文件]; [G10配置]";
        static public string olinkcreatetables = @"SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[G10事物](
[序号] [int] IDENTITY(1,1) NOT NULL,
[名称] [nvarchar](max) NULL,
[标题] [nvarchar](max) NULL,
[摘要] [nvarchar](max) NULL,
[封面] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object: Table [dbo].[G10文件] ******/
CREATE TABLE [dbo].[G10文件](
[序号] [int] IDENTITY(1,1) NOT NULL,
[名称] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object: Table [dbo].[G10配置] ******/
CREATE TABLE [dbo].[G10配置](
[S3Id] [nvarchar](50) NOT NULL, [S3Key] [nvarchar](max) NOT NULL, [S3Bucket] [nchar](10) NOT NULL, [Name] [nvarchar](50) NULL, [Note] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
";
        //AmazonS3FullAccess

    }
}
