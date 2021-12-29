using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oLink
{
    class ooData
    {
        static public string sG10配置Set = @"update [oLink].[dbo].[G10配置] set S3Id=N'^S3Id^',S3Key=N'^S3Key^',S3Bucket=N'^S3Bucket^',Name=N'^Name^',Note=N'^Note^'";
        static public string sG10配置Get = @"SELECT S3Id,S3Key,S3Bucket,Name,Note FROM [oLink].[dbo].[G10配置]";

        static public string sG10事物Add = @"if (select count(*) from [oLink].[dbo].[G10事物] where 名称=N'^名称^')=0
            insert into [oLink].[dbo].[G10事物](名称) values(N'^名称^')";
        static public string sG10事物All = @"select 序号,名称,标题,摘要,封面 FROM [oLink].[dbo].[G10事物] order by 序号 desc";
        static public string sG10事物Get = @"select 序号 FROM [oLink].[dbo].[G10事物] where 名称=N'^名称^'";
        static public string sG10事物Set = @"update [oLink].[dbo].[G10事物] set 标题=N'^标题^',摘要=N'^摘要^',封面=N'^封面^' where 名称=N'^名称^'
        select 序号 from [oLink].[dbo].[G10事物] where 名称=N'^名称^'";
        static public string sG10事物Del = @"delete [oLink].[dbo].[G10事物] where 序号=N'^序号^'";

        static public string sG10文件Add = @"if (select count(*) from [oLink].[dbo].[G10文件] where 名称=N'^名称^')=0
            insert into [oLink].[dbo].[G10文件](名称) values(N'^名称^')
        select 序号 from [oLink].[dbo].[G10文件] where 名称=N'^名称^'";
    }
}
