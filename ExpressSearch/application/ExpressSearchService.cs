using ExpressSearch.code;
using ExpressSearch.Code;
using ExpressSearch.domain;
using ExpressSearch.Helpers;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ExpressSearch.application
{
    /// <summary>
    /// 快递查询
    /// </summary>
    public class ExpressSearchService
    {
        private IExpressSearchRepository service = new Repository.ExpressSearchRepository();
        private Models.ApplicationDbContext dbContext = new Models.ApplicationDbContext();
        private const string host = "https://ali-deliver.showapi.com";
        private const string path = "/showapi_expInfo";
        private const string method = "GET";
        private const string appcode = "696a5aa296cc4771a7c71fba3681285d";

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        public List<Models.SysExpressSearch> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<Models.SysExpressSearch>();
            var queryParam = queryJson.ToJObject();
            DateTime dateTime = DateTime.Now;
            if (!queryParam["mailNo"].IsEmpty())
            {
                string mailNo = queryParam["mailNo"].ToString();
                expression = expression.And(t => t.MailNo == mailNo);
            }
            if (!queryParam["expSpellName"].IsEmpty())
            {
                string expSpellName = queryParam["expSpellName"].ToString();
                expression = expression.And(t => t.ExpSpellName == expSpellName);
            }

            return service.FindList(expression, pagination);
        }

        /// <summary>
        /// 查询详细列表
        /// </summary>
        public List<Models.SysExpressSearchInfo> GetListInfo(string queryJson)
        {
            var info = from i in dbContext.expressSearchInfo select i;

            var queryParam = queryJson.ToJObject();
            if (!queryParam["mailNo"].IsEmpty())
            {
                string mailNo = queryParam["mailNo"].ToString();
                info = info.Where(u => u.mailNo == mailNo);
            }
            if (!queryParam["AddTime"].IsEmpty())
            {
                DateTime dt = Convert.ToDateTime((queryParam["AddTime"]));
                info = info.Where(t => t.AddTime == dt);
            }
            if (!queryParam["context"].IsEmpty())
            {
                string context = queryParam["context"].ToString();
                info = info.Where(t => t.context == context);
            }
            info = info.OrderByDescending(o => o.AddTime);
            return info.ToList();
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        public int Insert(Models.SysExpressSearch model)
        {
            return service.Insert(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        public int InsertInfo(Models.SysExpressSearchInfo model)
        {
            Models.ApplicationDbContext dbContext = new Models.ApplicationDbContext();
            dbContext.expressSearchInfo.Add(model);
            return dbContext.SaveChanges();
        }
        #endregion

        #region 实体
        /// <summary>
        /// 实体
        /// </summary>
        public Models.SysExpressSearch FindEntity(object id)
        {
            return service.FindEntity(id);
        }
        /// <summary>
        /// 实体
        /// </summary>
        public Models.SysExpressSearch FindEntity(string queryJson)
        {
            var expression = ExtLinq.True<Models.SysExpressSearch>();
            var queryParam = queryJson.ToJObject();
            DateTime dateTime = DateTime.Now;
            if (!queryParam["MailNo"].IsEmpty())
            {
                string mailNo = queryParam["MailNo"].ToString();
                expression = expression.And(t => t.MailNo == mailNo);
            }
            if (!queryParam["ExpSpellName"].IsEmpty())
            {
                string expSpellName = queryParam["ExpSpellName"].ToString();
                expression = expression.And(t => t.ExpSpellName == expSpellName);
            }

            return service.FindEntity(expression);
        }

        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="userEntity"></param>
        public int UpdateForm(Models.SysExpressSearch entity)
        {
            return service.Update(entity);
        }
        #endregion

        #region 订单号查询
        /// <summary>
        /// 快递查询
        /// </summary>
        /// <param name="com">快递公司</param>
        /// <param name="nu">订单号</param>
        /// <returns></returns>
        public string ExpInfo(string com, string nu)
        {
            #region 成功/失败数据
            //成功例子
            //{
            //  "showapi_res_code": 0,//showapi平台返回码,0为成功,其他为失败
            //	"showapi_res_error": "",//showapi平台返回的错误信息
            //	"showapi_res_body": {
            //                    "mailNo": "968018776110",//快递单号
            //		"update": 1466926312666,//数据最后查询的时间
            //		"updateStr": "2016-06-26 15:31:52",//数据最后更新的时间
            //		"ret_code": 0,//接口调用是否成功,0为成功,其他为失败
            //		"flag": true,//物流信息是否获取成功
            //		"status": 4,-1 待查询 0 查询异常 1 暂无记录 2 在途中 3 派送中 4 已签收 5 用户拒签 6 疑难件 7 无效单
            // 8 超时单 9 签收失败 10 退回

            //        "tel": "400-889-5543",//快递公司电话
            //		"expSpellName": "shentong",//快递字母简称
            //		"data": [//具体快递路径信息
            //			{
            //				"time": "2016-06-26 12:26",
            //				"context": "已签收,签收人是:【本人】"

            //            },
            //			{
            //				"time": "2016-06-25 15:31",
            //				"context": "【陕西陇县公司】的派件员【西城业务员】正在派件"
            //			},
            //			{
            //				"time": "2016-06-25 14:11",
            //				"context": "快件已到达【陕西陇县公司】"
            //			},
            //			{
            //				"time": "2016-06-25 09:08",
            //				"context": "由【陕西宝鸡公司】发往【陕西陇县公司】"
            //			},
            //			{
            //				"time": "2016-06-24 14:08",
            //				"context": "由【陕西西安中转部】发往【陕西宝鸡公司】"
            //			},
            //			{
            //				"time": "2016-06-22 13:23",
            //				"context": "由【山东临沂公司】发往【陕西西安中转部】"
            //			},
            //			{
            //				"time": "2016-06-21 23:02",
            //				"context": "【江苏常熟公司】正在进行【装袋】扫描"
            //			},
            //			{
            //				"time": "2016-06-21 23:02",
            //				"context": "由【江苏常熟公司】发往【江苏江阴航空部】"
            //			},
            //			{
            //				"time": "2016-06-21 18:30",
            //				"context": "【江苏常熟公司】的收件员【严继东】已收件"
            //			},
            //			{
            //				"time": "2016-06-21 16:41",
            //				"context": "【江苏常熟公司】的收件员【凌明】已收件"
            //			}
            //		],
            //                "possibleExpList": [//当auto查询失败的时候,返回此信息,成功时不返回
            //                                    //用户表示该单号可能属于那些快递物流公司
            //                        {
            //                                 "simpleName": "shunfeng",//快递公司简称
            //                                 "expName": "顺丰速运"
            //                         }
            //                 ],
            //		"expTextName": "申通快递"//快递公司名
            //	}
            //}

            //失败数据
            //{ "showapi_res_code":0,"showapi_res_error":"","showapi_res_body":{ "ret_code":-1,"flag":false,"msg":"未知的快递公司名,请调用快递公司查询或快递公司列表接口"} }
            #endregion

            if (string.IsNullOrWhiteSpace(com))
            {
                return LitJson.JsonMapper.ToJson(new Models.ResultJson() { status = 0, msg = "公司编号不能为空" });
            }
            if (string.IsNullOrWhiteSpace(nu))
            {
                return LitJson.JsonMapper.ToJson(new Models.ResultJson() { status = 0, msg = "快递号不能为空" });
            }

            //判断是否已经被签收
            Models.SysExpressSearch searchModel = FindEntity("{\"MailNo\":\"" + nu + "\",\"ExpSpellName\":\"" + com + "\"}");
            if (searchModel != null && searchModel.Status == 4) //已被签收，返回数据库里的数据
            {
                return SearchDataBase(searchModel);
            }

            //查询是否有缓存
            object obj = CacheHelper.Get("no" + nu);
            if (obj != null) //有缓存
            {
                return obj.ToString();
            }
            else //没有缓存
            {
                string result = GetHttpUrl(com, nu); //请求阿里物流接口
                if (result.Contains("showapi_res_code"))
                {
                    JsonData jd = JsonMapper.ToObject(result);//字符串转换成json格式
                    int respCode = Utils.ObjToInt(jd["showapi_res_code"], 1);//查询结果 0:成功 其他为失败 
                    string respMsg = Utils.ObjectToStr(jd["showapi_res_error"]);//平台返回的错误信息
                    if (respCode == 0)//成功返回数据
                    {
                        //写入数据库
                        JsonData bodyJD = jd["showapi_res_body"];//body数据
                        int retCode = Utils.ObjToInt(bodyJD["ret_code"], 1);//查询结果 0:成功 其他为失败 
                        bool flag = Utils.StrToBool(bodyJD["flag"].ToString(), false);//物流信息是否获取成功
                        if (retCode == 0 && flag) //接口查询成功
                        {
                            int status = Utils.ObjToInt(bodyJD["status"], 1);//物流信息是否获取成功
                            string mailNo = Utils.ObjectToStr(bodyJD["mailNo"]);//快递单号
                            string update = Utils.ObjectToStr(bodyJD["update"]);//数据最后查询的时间
                            string updateStr = Utils.ObjectToStr(bodyJD["updateStr"]);//数据最后更新的时间
                            string expSpellName = Utils.ObjectToStr(bodyJD["expSpellName"]);//快递字母简称
                            string expTextName = Utils.ObjectToStr(bodyJD["expTextName"]);//快递公司名

                            string tel = string.Empty;
                            if (((IDictionary)bodyJD).Contains("tel"))
                            {
                                tel = Utils.ObjectToStr(bodyJD["tel"]);//快递电话
                            }

                            JsonData dataList = bodyJD["data"];//快递单号

                            string newResult = InserExpressSearch(mailNo, expSpellName, expTextName, update, updateStr, flag, status, tel, dataList);
                            return newResult;
                        }
                        else
                        {
                            string msg = Utils.ObjectToStr(bodyJD["msg"]);//错误信息
                            return LitJson.JsonMapper.ToJson(new Models.ResultJson() { status = 0, msg = msg });
                        }
                    }
                    return LitJson.JsonMapper.ToJson(new Models.ResultJson() { status = 0, msg = "请求失败" + respMsg });
                }
                return LitJson.JsonMapper.ToJson(new Models.ResultJson() { status = 0, msg = "阿里云快递查询失败：账户余额不足" });
            }
        }

        /// <summary>
        /// 添加快递查询到数据库
        /// </summary>
        /// <param name="mailNo"></param>
        /// <param name="expSpellName"></param>
        /// <param name="expTextName"></param>
        /// <param name="update"></param>
        /// <param name="updateStr"></param>
        /// <param name="dataList"></param>
        private string InserExpressSearch(string mailNo, string expSpellName, string expTextName, string update, string updateStr, bool flag, int status, string tel, JsonData dataList)
        {
            //是否已经查询
            Models.SysExpressSearch searchModel = FindEntity("{\"MailNo\":\"" + mailNo + "\",\"ExpSpellName\":\"" + expSpellName + "\"}");
            if (searchModel == null)//第一次查询
            {
                //添加查询记录
                searchModel = new Models.SysExpressSearch();
                searchModel.MailNo = mailNo;
                searchModel.ExpSpellName = expSpellName;
                searchModel.ExpTextName = expTextName;
                searchModel.Tel = tel;
                searchModel.LastQueryTime = update;
                searchModel.UpdateStr = updateStr;
                searchModel.Flag = flag;
                searchModel.Status = status;

                Insert(searchModel);
            }
            else //修改
            {
                searchModel.Flag = flag;
                searchModel.Status = status;
                searchModel.LastQueryTime = update;
                searchModel.UpdateStr = updateStr;
                UpdateForm(searchModel);
            }
            List<Models.SysExpressSearchInfo> infoList = GetListInfo("{\"MailNo\":\"" + mailNo + "\"}");


            //添加详细信息
            for (int i = 0; i < dataList.Count; i++)
            {
                int x = 0;
                foreach (Models.SysExpressSearchInfo info in infoList)
                {
                    if (info.mailNo == mailNo && info.AddTime == Utils.ObjectToDateTime(dataList[i]["time"]) && info.context == Utils.ObjectToStr(dataList[i]["context"]))
                    {
                        x++;
                        break;
                    }
                }
                if (x == 0)
                {
                    Models.SysExpressSearchInfo infoModel = new Models.SysExpressSearchInfo();
                    infoModel.mailNo = mailNo;
                    infoModel.AddTime = Utils.ObjectToDateTime(dataList[i]["time"]);
                    infoModel.context = Utils.ObjectToStr(dataList[i]["context"]);
                    infoModel.SysExpressSearchId = searchModel.SysExpressSearchId;
                    InsertInfo(infoModel);
                    infoList.Add(infoModel);
                }
                x = 0;
            }
            searchModel.InfoList = infoList;
            return SearchDataBase(searchModel);
        }
        #endregion

        #region 从数据库查询最新的数据
        /// <summary>
        /// 从数据库查询最新的数据
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private string SearchDataBase(Models.SysExpressSearch searchModel)
        {
            string stringJson = LitJson.JsonMapper.ToJson(new Models.ResultJson() { status = 1, msg = "请求成功" ,info= searchModel });
            CacheHelper.Insert("no" + searchModel.MailNo, stringJson, 120);//添加到缓存 过期时间2个小时
            return stringJson;
        }
        #endregion

        #region 请求接口数据
        /// <summary>
        /// 请求接口数据
        /// </summary>
        /// <param name="com"></param>
        /// <param name="nu"></param>
        /// <returns></returns>
        private string GetHttpUrl(string com, string nu)
        {
            string querys = "com=" + com + "&nu=" + nu;
            string bodys = "";
            string url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            //Console.WriteLine(httpResponse.StatusCode);
            //Console.WriteLine(httpResponse.Method);
            //Console.WriteLine(httpResponse.Headers);
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            string result = reader.ReadToEnd();//返回结果
            return result;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        #endregion

    }
}