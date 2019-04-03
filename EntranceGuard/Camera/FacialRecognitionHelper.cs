namespace EntranceGuard
{
    using Baidu.Aip.Face;
    using System.Linq;

    class FacialRecognitionHelper
    {
        // 设置APPID/AK/SK
        public string APP_ID = "你的 App ID";
        public string API_KEY = "你的 Api Key";
        public string SECRET_KEY = "你的 Secret Key";
        public Baidu.Aip.Face.Face client;

        public FacialRecognitionHelper()
        {
            APP_ID = "15700137";
            API_KEY = "xNEfNoAZCr78tX2Wv4WfeRDy";
            SECRET_KEY = "V0vM0hhw0n4hcVVGFvGYMSBVzHr5klDx";
            client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
        }


        public Newtonsoft.Json.Linq.JObject SearchFace(string mat, string groupIdList = "test1,test", string imageType = "BASE64", System.Collections.Generic.Dictionary<string, object> options = null)
        {
            /*
            if(options == null)
            {
                options = new System.Collections.Generic.Dictionary<string, object>{
                    {"quality_control", "HIGH"}, //图片质量控制 NONE: 不进行控制 LOW:较低的质量要求 NORMAL: 一般的质量要求 HIGH: 较高的质量要求 默认 NONE
                    {"liveness_control", "LOW"}, //活体检测控制 NONE: 不进行控制 LOW:较低的活体要求(高通过率 低攻击拒绝率) NORMAL: 一般的活体要求(平衡的攻击拒绝率, 通过率) HIGH: 较高的活体要求(高攻击拒绝率 低通过率) 默认NONE
                };
              
            }
            */
            var result = client.Search(mat, imageType, groupIdList, options);
            //FacadeTool.Debug(result);
            return result;
        }
    }
}
