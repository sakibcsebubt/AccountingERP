using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace MVC.ERPWEB.ApiCommonClasses
{
    public class WebProcessAccess
    {

        public static async Task GetApiToken()
        {

            var jwtoken1 = new AppCustomClasses.JwtTokenUser() { UserId = "user001", Password = "12345678", Expirhour = 6 };
            var result = await AppBasicInfo.GerpApiClient.PostAsJsonAsync("Jwt", jwtoken1);
            result.EnsureSuccessStatusCode();
            if (result.IsSuccessStatusCode)
            {
                try
                {
                    string tokenjson1 = await result.Content.ReadAsStringAsync();
                    AppBasicInfo.ApiToken = JsonConvert.DeserializeObject<AppCustomClasses.JwtTokenInfo>(tokenjson1);
                }
                catch (Exception exp)
                {
                    WebProcessAccess.ShowCatchErrorMessage("ApiToken: 001", exp);
                }
            }
        }



        public static async Task<string> GetGerpAppJsonData(ApiAccessParms pap1, string dbName1)
        {
            try
            {
                if (AppBasicInfo.ApiToken == null)
                {
                    AppBasicInfo.ConnectApi();
                    await WebProcessAccess.GetApiToken();
                }
                pap1.DbName = dbName1;
                pap1.UserID = AppBasicInfo.usrid1a;
                pap1.UserPwd = AppBasicInfo.usrpass1a;
                AppBasicInfo.DatabaseErrorInfoList = null;
                string JsonDs1 = "";
                string sJSONResponse = JsonConvert.SerializeObject(pap1, Formatting.Indented);
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(sJSONResponse);
                var data1 = Convert.ToBase64String(plainTextBytes);
                AppBasicInfo.GerpApiClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AppBasicInfo.ApiToken.tokenstr);
                //var result = await McAppGenInfo.McApiClient.PostAsJsonAsync("dbRequest", data1);

                var result = await AppBasicInfo.GerpApiClient.PostAsJsonAsync("dbAsyncRequest", data1);

                //result.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                {
                    JsonDs1 = await result.Content.ReadAsStringAsync();
                    JsonDs1 = JsonDs1.TrimStart('"').TrimEnd('"');
                    JsonDs1 = AppCustomFunctions.StrDecompress(JsonDs1.ToString());
                }
                if (JsonDs1.Trim().Length <= 20)
                {
                    AppBasicInfo.DatabaseErrorInfoList = new List<AppCustomClasses.DatabaseErrorInfo>();
                    AppBasicInfo.DatabaseErrorInfoList.Add(new AppCustomClasses.DatabaseErrorInfo
                    {
                        errornumber = 0,
                        errorseverity = 0,
                        errorstate = 0,
                        process_id = "",
                        errorline = 0,
                        errormessage = "Unknown Error Occurred",
                        errorprocedure = ""
                    });
                    return null;
                }
                if (JsonDs1.Substring(0, 20).Contains("\"ErrorTable\": ["))
                {
                    AppBasicInfo.DatabaseErrorInfoList = AppCustomFunctions.JsonStringToList<AppCustomClasses.DatabaseErrorInfo>(JsonDs1, "ErrorTable");
                    return null;
                }
                return JsonDs1;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static void ShowCatchErrorMessage(string errcod1, Exception exp)
        {
            throw exp;
        }
    }
}
