using Newtonsoft.Json;
using System.Data;
using System.IO.Compression;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace MVC.ERPWEB.ApiCommonClasses
{
    [Serializable]
    public static class AppCustomFunctions
    {
        public static string RemoveExtraSpaces(string str)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            return regex.Replace(str, " ");
        }
        public static string ExprToValue(string cExpr)
        {
            // Calculator
            string mExpr1 = cExpr.Trim().Replace(",", "");
            mExpr1 = mExpr1.Replace("/", " div ");
            XmlDocument xmlDoc = new XmlDocument();
            XPathNavigator xPathNavigator = xmlDoc.CreateNavigator();
            mExpr1 = xPathNavigator.Evaluate(mExpr1).ToString();
            return mExpr1;
        }
        public static List<T> JsonStringToList<T>(this string JsonDs1, string partName1) //where T : class, new()
        {
            dynamic obj = JsonConvert.DeserializeObject(JsonDs1);
            List<T> lstCopy = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(obj[partName1]));
            return lstCopy;
        }
        public static String ListToJsonString<T>(this List<T> list1, string partName1)
        {
            string JsonStr1 = JsonConvert.SerializeObject(list1);
            JsonStr1 = "{\"" + partName1 + "\":" + JsonStr1 + "}";
            return JsonStr1;
        }
        public static string Text2Value(string InputValue)
        {
            // Calculator
            string OutputValue = "0.00";
            #region javascript calculat

            Type scriptType = Type.GetTypeFromCLSID(Guid.Parse("0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC"));
            dynamic obj = Activator.CreateInstance(scriptType, false);
            obj.Language = "Javascript";
            string str = null;
            try
            {
                dynamic res = obj.Eval(InputValue);
                str = Convert.ToString(res);
                //this.txtbFResult.Text = this.txtResult.Text + "=" + str;
                OutputValue = str;
            }
            catch (Exception)
            {
                return OutputValue;
                //throw;
            }
            #endregion
            return OutputValue;
        }

        public static string Left(string host, int index)
        {
            return host.Substring(0, index);

        }
        public static string Right(string host, int index)
        {
            return host.Substring(host.Length - index);
        }
        public static string Num2Word(double XX1, int Index)
        {
            Index = (Index == 0 ? 1 : Index);
            string[] X1 = new string[101];
            string[] Y1 = new string[6];
            string[] Z1 = new string[3];

            X1[0] = "Zero ";
            X1[1] = "One ";
            X1[2] = "Two ";
            X1[3] = "Three ";
            X1[4] = "Four ";
            X1[5] = "Five ";
            X1[6] = "Six ";
            X1[7] = "Seven ";
            X1[8] = "Eight ";
            X1[9] = "Nine ";
            X1[10] = "Ten ";
            X1[11] = "Eleven ";
            X1[12] = "Twelve ";
            X1[13] = "Thirteen ";
            X1[14] = "Fourteen ";
            X1[15] = "Fifteen ";
            X1[16] = "Sixteen ";
            X1[17] = "Seventeen ";
            X1[18] = "Eighteen ";
            X1[19] = "Nineteen ";
            X1[20] = "Twenty ";
            X1[30] = "Thirty ";
            X1[40] = "Forty ";
            X1[50] = "Fifty ";
            X1[60] = "Sixty ";
            X1[70] = "Seventy ";
            X1[80] = "Eighty ";
            X1[90] = "Ninety ";

            for (int J1 = 20; J1 <= 90; J1 = J1 + 10)
                for (int I1 = 1; I1 <= 9; I1++)
                    X1[J1 + I1] = X1[J1] + X1[I1];

            Y1[1] = "Hundred ";
            Y1[2] = "Thousand ";
            Y1[3] = (Index >= 3 ? "Million " : "Lac ");
            Y1[4] = (Index >= 3 ? "Billion " : "Crore ");
            Y1[5] = "Trillion ";
            Z1[1] = "Minus ";
            Z1[2] = "Zero ";
            long N_1 = System.Convert.ToInt64(Math.Floor(XX1));
            string N_2 = XX1.ToString();
            while (!(N_2.Length == 0))
            {
                if (N_2.Substring(0, 1) == ".")
                    break;
                N_2 = N_2.Substring(1);
            }
            N_2 = (N_2.Length == 0 ? " " : N_2);
            switch (Index)
            {
                case 1:
                case 3:
                    N_2 = ((N_2.Substring(0, 1) == ".") ? ((string)(N_2.Substring(1) + "00000")).Substring(0, 5) : "00000");
                    break;
                case 2:
                case 4:
                    N_2 = ((N_2.Substring(0, 1) == ".") ? ((string)(N_2.Substring(1) + "00000")).Substring(0, 2) : "00");
                    break;
            }
            string S_GN = (Math.Sign(N_1) == -1 ? Z1[1] : "");
            string Z1_ER = (N_1 == 0 ? Z1[2] : "");
            string N_O = Right("00000000000000000" + Math.Abs(N_1).ToString(), 17);
            string[] L = new string[100];
            switch (Index)
            {
                case 1:
                case 2:
                    L[0] = "";
                    L[1] = ((Convert.ToInt32(N_O.Substring(0, 1)) == 0) ? "" : X1[int.Parse(N_O.Substring(0, 1))] + Y1[1]);
                    L[2] = ((Convert.ToInt32(N_O.Substring(1, 2)) == 0) ? "" : X1[int.Parse(N_O.Substring(1, 2))] + Y1[4]);
                    L[3] = ((Convert.ToInt32(N_O.Substring(3, 2)) == 0) ? "" : X1[int.Parse(N_O.Substring(3, 2))] + Y1[3]);
                    L[4] = ((Convert.ToInt32(N_O.Substring(5, 2)) == 0) ? "" : X1[int.Parse(N_O.Substring(5, 2))] + Y1[2]);
                    L[5] = ((Convert.ToInt32(N_O.Substring(7, 1)) == 0) ? "" : X1[int.Parse(N_O.Substring(7, 1))] + Y1[1]);
                    L[6] = ((Convert.ToInt32(N_O.Substring(8, 2)) == 0) ? ((Convert.ToInt32(N_O.Substring(0, 10))) == 0 ? "" : Y1[4]) : X1[int.Parse(N_O.Substring(8, 2))] + Y1[4]);
                    L[7] = ((Convert.ToInt32(N_O.Substring(10, 2)) == 0) ? "" : X1[int.Parse(N_O.Substring(10, 2))] + Y1[3]);
                    L[8] = ((Convert.ToInt32(N_O.Substring(12, 2)) == 0) ? "" : X1[int.Parse(N_O.Substring(12, 2))] + Y1[2]);
                    L[9] = ((Convert.ToInt32(N_O.Substring(14, 1)) == 0) ? "" : X1[int.Parse(N_O.Substring(14, 1))] + Y1[1]);
                    L[10] = (Convert.ToInt32(N_O.Substring(15, 2)) == 0) ? "" : X1[int.Parse(N_O.Substring(15, 2))];
                    break;
                case 3:
                case 4:
                    L[0] = ((Convert.ToInt32(N_O.Substring(0, 2)) == 0) ? "" : X1[int.Parse(N_O.Substring(0, 2))] + Y1[2]);
                    L[1] = ((Convert.ToInt32(N_O.Substring(2, 1)) == 0) ? "" : X1[int.Parse(N_O.Substring(2, 1))] + Y1[1]);
                    L[2] = ((Convert.ToInt32(N_O.Substring(3, 2)) == 0) ? ((Convert.ToInt32(N_O.Substring(2, 1)) == 0) ? "" : Y1[5]) : X1[int.Parse(N_O.Substring(3, 2))] + Y1[5]);
                    L[3] = ((Convert.ToInt32(N_O.Substring(5, 1)) == 0) ? "" : X1[int.Parse(N_O.Substring(5, 1))] + Y1[1]);
                    L[4] = ((Convert.ToInt32(N_O.Substring(6, 2)) == 0) ? ((Convert.ToInt32(N_O.Substring(5, 1)) == 0) ? "" : Y1[4]) : X1[int.Parse(N_O.Substring(6, 2))] + Y1[4]);
                    L[5] = ((Convert.ToInt32(N_O.Substring(8, 1)) == 0) ? "" : X1[int.Parse(N_O.Substring(8, 1))] + Y1[1]);
                    L[6] = ((Convert.ToInt32(N_O.Substring(9, 2)) == 0) ? ((Convert.ToInt32(N_O.Substring(8, 1)) == 0) ? "" : Y1[3]) : X1[int.Parse(N_O.Substring(9, 2))] + Y1[3]);
                    L[7] = ((Convert.ToInt32(N_O.Substring(11, 1)) == 0) ? "" : X1[int.Parse(N_O.Substring(11, 1))] + Y1[1]);
                    L[8] = ((Convert.ToInt32(N_O.Substring(12, 2)) == 0) ? ((Convert.ToInt32(N_O.Substring(11, 1)) == 0) ? "" : Y1[2]) : X1[int.Parse(N_O.Substring(12, 2))] + Y1[2]);
                    L[9] = ((Convert.ToInt32(N_O.Substring(14, 1)) == 0) ? "" : X1[int.Parse(N_O.Substring(14, 1))] + Y1[1]);
                    L[10] = (Convert.ToInt32(N_O.Substring(15, 2)) == 0) ? "" : X1[int.Parse(N_O.Substring(15, 2))];
                    break;
            }
            string O = S_GN + Z1_ER + L[0] + L[1] + L[2] + L[3] + L[4] + L[5] + L[6] + L[7] + L[8] + L[9] + L[10];
            string[] M = new string[100];
            string Q_ = "";
            string P = "";

            switch (Index)
            {
                case 1:
                case 3:
                    M[1] = ((Convert.ToInt32(N_2) >= 1) ? X1[int.Parse(N_2.Substring(0, 1))] : "");
                    M[2] = ((Convert.ToInt32(N_2) >= 1 & Convert.ToInt32(N_2.Substring(1)) >= 1) ? X1[int.Parse(N_2.Substring(1, 1))] : "");
                    M[3] = ((Convert.ToInt32(N_2) >= 1 & Convert.ToInt32(N_2.Substring(2)) >= 1) ? X1[int.Parse(N_2.Substring(2, 1))] : "");
                    M[4] = ((Convert.ToInt32(N_2) >= 1 & Convert.ToInt32(N_2.Substring(3 - 1)) >= 1) ? X1[int.Parse(N_2.Substring(3, 1))] : "");
                    M[5] = ((Convert.ToInt32(N_2) >= 1 & Convert.ToInt32(N_2.Substring(4)) >= 1) ? X1[Convert.ToInt32(N_2.Substring(4, 1))] : "");
                    M[6] = ((Convert.ToInt32(N_2) > 0) ? "Point " : "");
                    P = M[6] + M[1] + M[2] + M[3] + M[4] + M[5];
                    Q_ = O + P;
                    break;
                case 2:
                    M[1] = ((Convert.ToInt32(N_2) >= 1) ? X1[int.Parse(N_2)] : "");
                    M[6] = ((Convert.ToInt32(N_2) > 0) ? "And Paisa " : "");
                    P = M[6] + M[1];
                    Q_ = "( Taka " + O + P + "Only )";
                    break;
                case 4:
                    M[1] = ((Convert.ToInt32(N_2) >= 1) ? X1[int.Parse(N_2)] : "");
                    M[6] = ((Convert.ToInt32(N_2) > 0) ? "And Cent " : "");
                    P = M[6] + M[1];
                    Q_ = "( Dollar " + O + P + "Only )";
                    break;
            }
            return Q_;
        }

        //--------------------------------------------------------------------------------------------------------
        public static string BanglaComa(double AA) // Bangla Coma
        {
            string[] A = new string[21];
            A[1] = ((Math.Sign(AA) >= 0) ? "" : "-");
            A[2] = Math.Abs(AA).ToString("###0.00");
            A[3] = Math.Abs(AA).ToString("###0.000");
            A[3] = ((double.Parse(A[3]) - (double.Parse(A[2])))).ToString();
            A[2] = A[2] + ((double.Parse(A[3]) >= 0.005) ? 0.01 : 0);
            A[2] = Left(A[2], A[2].Length - 1);
            A[4] = ((string)(string.Empty.PadLeft(24) + A[2])).Substring(((string)(string.Empty.PadLeft(24) + A[2])).Length - 24);
            A[5] = A[4].Substring(0, 2);
            A[6] = A[4].Substring(2, 2);
            A[7] = A[4].Substring(4, 3);
            A[8] = A[4].Substring(7, 2);
            A[9] = A[4].Substring(9, 2);
            A[10] = A[4].Substring(11, 3);
            A[11] = A[4].Substring(14, 2);
            A[12] = A[4].Substring(16, 2);
            A[13] = A[4].Substring(18, 3);
            A[14] = A[5] + "," + A[6] + "," + A[7] + "," + A[8] + "," + A[9] + "," + A[10] + "," + A[11] + "," + A[12] + "," + A[13];
            A[14] = A[14].Trim();

            while (A[14].Substring(0, 1) == ",")
            {
                A[14] = A[14].Substring(1, A[14].Length - 1);
                A[14] = A[14].Trim();
            }
            A[15] = A[14] + A[4].Substring(21, 3);
            A[16] = ((string)(string.Empty.PadLeft(24) + A[15])).Substring(((string)(string.Empty.PadLeft(24) + A[15])).Length - 24) + " ";
            A[17] = ((A[1] != "") ? "(" : "") + A[16].Trim() + ((A[1] != "") ? ")" : "");
            return A[17];
        }
        //-------------------------------------------------------------------------------------------------------       

        public static string XmlSerialize(object dataToSerialize)
        {
            if (dataToSerialize == null) return null;

            using (StringWriter stringwriter = new System.IO.StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }

        public static T XmlDeserialize<T>(string xmlText)
        {
            if (string.IsNullOrWhiteSpace(xmlText)) return default(T);

            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        public static string EncodePassword(string originalPassword)
        {
            byte[] originalBytes;
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }

        public static string UnicodeNumEng2Ban(string input1 = "")
        {
            //char[] ar1 = {'0','1','2','3','4','5','6','7','8','9' };
            char[] ar2 = { '০', '১', '২', '৩', '৪', '৫', '৬', '৭', '৮', '৯' };
            char[] ar3 = input1.ToCharArray();
            string output1 = "";
            foreach (char item in ar3)
            {
                int i1 = -1;
                bool isInt = int.TryParse(item.ToString(), out i1);
                output1 += (isInt && i1 >= 0 ? ar2[i1].ToString() : item.ToString());
            }

            return output1;
        }
        public static string UnicodeNumBan2Eng(string input1 = "")
        {
            char[] ar3 = input1.ToCharArray();
            string output1 = "";
            foreach (char item in ar3)
            {
                string item1 = item.ToString();
                switch (item1)
                {
                    case "০": output1 += "0"; break;
                    case "১": output1 += "1"; break;
                    case "২": output1 += "2"; break;
                    case "৩": output1 += "3"; break;
                    case "৪": output1 += "4"; break;
                    case "৫": output1 += "5"; break;
                    case "৬": output1 += "6"; break;
                    case "৭": output1 += "7"; break;
                    case "৮": output1 += "8"; break;
                    case "৯": output1 += "9"; break;
                    default: output1 += item1; break;
                }
            }
            return output1;
        }

        public static string Eng2BanMonthsDays()
        {
            string[] EngMonthse = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            string[] EndMonthsb = { "জানুয়ারী", "ফেব্রুয়ারী", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই ", "আগষ্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর" };

            string[] EngSMonthse = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string[] EndSMonthsb = { "জানুঃ", "ফেব্রু", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই ", "আগষ্ট", "সেপ্টেঃ", "অক্টোঃ", "নভেঃ", "ডিসেঃ" };

            string[] BanMonthse = { "Boishakh", "Joishtho", "Asharh", "Srabon", "Bhadro", "Ashshin", "Kartik", "Ogrohaeon", "Poush", "Magh", "Falgun", "Choitro" };
            string[] BanMonthsb = { "বৈশাখ", "জৈষ্ঠ্য", "আষাঢ়", "শ্রাবণ", "ভাদ্র", "আশ্বিন", "কার্তিক", "অগ্রাহায়ন", "পৌষ", "মাঘ", "ফাল্গুন" };

            string[] EngDays = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            string[] BanDays = { "শনিবার", "রবিবার", "সোমবার", "মঙ্গলবার", "বুধবার", "বৃহস্পতিবার", "শুক্রবার" };

            string[] EngsDays = { "Sat", "Sun", "Mon", "Tue", "Wed", "Thu", "Fri" };
            string[] BansDays = { "শনি", "রবি", "সোম", "মঙ্গল", "বুধ", "বৃহস্পতি", "শুক্র" };

            // January       February       March   April   May     June    July    August  September   October     November        December
            // জানুয়ারী        ফেব্রুয়ারী       মার্চ     এপ্রিল        মে       জুন      জুলাই     আগষ্ট        সেপ্টেম্বর      অক্টোবর       নভেম্বর       ডিসেম্বর

            // বৈশাখ  জৈষ্ঠ্য      আষাঢ়     শ্রাবণ        ভাদ্র     আশ্বিন        কার্তিক        অগ্রাহায়ন      পৌষ      মাঘ      ফাল্গুন

            //Saturday      Sunday      Monday      Tuesday     Wednesday       Thursday, Firday
            // শনিবার         রবিবার         সোমবার   মঙ্গলবার  বুধবার    বৃহস্পতিবার শুক্রবার
            // Boishakh            Joishtho            Asharh Srabon    Bhadro  Ashshin Kartik  Ogrohaeon   Poush   Magh    Falgun  Choitro
            // PM = 12-2= দুপুর, 3-4 = অপরাহ্ন, 5 = বিকেল, 6-7, সন্ধ্যা, 8-11: রাত, AM = পূর্বাহ্ণ
            return "";
        }
        public static List<string> EngBanCalculator(string input1 = "0")
        {
            input1 = input1.Trim();
            string BanNum1 = "০১২৩৪৫৬৭৮৯";
            bool isBan1 = false;
            char[] arr1 = input1.ToCharArray();
            foreach (var item in arr1)
            {
                if (BanNum1.Contains(item.ToString()))
                {
                    isBan1 = true;
                    break;
                }
            }
            string EngNum1 = (isBan1 ? AppCustomFunctions.UnicodeNumBan2Eng(input1) : input1);
            string output1 = AppCustomFunctions.Text2Value(EngNum1);
            if (isBan1)
            {
                input1 = AppCustomFunctions.UnicodeNumEng2Ban(input1);
                output1 = AppCustomFunctions.UnicodeNumEng2Ban(output1);
            }

            var lst1 = new List<string>();
            lst1.Add(input1);
            lst1.Add(output1);
            return lst1;
        }
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static string DateToBangla(string enNumInp)
        {
            //    enNumInp = this.txtboxInput.Text.Trim();  //12-February-2018 // পূর্বাহ্ন  অপরাহ্ন
            char[] bnNum = { '০', '১', '২', '৩', '৪', '৫', '৬', '৭', '৮', '৯' };
            string[] bn = { "জানু", "ফেব্রু", "মার্চ", "এপ্রি", "মে", "জুন", "জুলা", "আগ", "সেপ্টে", "অক্টো", "নভে", "ডিসে" };
            string[] bnFull = { "জানুয়ারি", "ফেব্রুয়ারি", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "আগস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর" };
            string[] enFull = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            //   string[] en = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string banNumOutput = "";

            foreach (var c in enNumInp.ToArray())
            {
                char c1 = c;
                for (int i = 0; i <= 9; i++)
                {
                    if (c1.ToString() == i.ToString())
                    {
                        c1 = bnNum[i];
                        break;
                    }
                }
                banNumOutput += c1.ToString();  // 
            }
            for (int i = 0; i < 12; i++)
            {
                if (banNumOutput.Contains(enFull[i]))
                {
                    banNumOutput = banNumOutput.Replace(enFull[i], bnFull[i].ToString());
                    break;
                }
                else if (banNumOutput.Contains(enFull[i].Substring(0, 3)))
                {
                    banNumOutput = banNumOutput.Replace(enFull[i].Substring(0, 3), bn[i]);
                    break;
                }
            }
            //   this.lblShow.Content = banNumOutput;

            banNumOutput = banNumOutput.Replace("AM", "পূর্বাহ্ন").Replace("PM", "অপরাহ্ন");

            return banNumOutput;
        }


        public static string StrCompress(string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return Convert.ToBase64String(mso.ToArray());
            }
        }

        public static string StrDecompress(string s)
        {
            var bytes = Convert.FromBase64String(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }


        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static void ObjectToObject(object source, object destination)
        {
            // Purpose : Use reflection to set property values of objects that share the same property names.
            Type s = source.GetType();
            Type d = destination.GetType();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            var objSourceProperties = s.GetProperties(flags);
            var objDestinationProperties = d.GetProperties(flags);

            var propertyNames = objSourceProperties.Select(c => c.Name).ToList();

            foreach (var properties in objDestinationProperties.Where(properties => propertyNames.Contains(properties.Name)))
            {
                try
                {
                    PropertyInfo piSource = source.GetType().GetProperty(properties.Name);

                    properties.SetValue(destination, piSource.GetValue(source, null), null);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }
        public static List<T> CopyList<T>(this List<T> lst)
        {
            List<T> lstCopy = new List<T>();

            foreach (var item in lst)
            {
                var instanceOfT = Activator.CreateInstance<T>();
                ObjectToObject(item, instanceOfT);
                lstCopy.Add(instanceOfT);
            }
            return lstCopy;
        }

        public static string GetDatabaseName(string ComCod1)
        {
            string dbName1 = "DLRSLSINF_0000SYSDB";
            switch (ComCod1)
            {
                case "4105": dbName1 = "DLRSLSINF_4105DHKDB"; break;
                case "4111": dbName1 = "DLRSLSINF_4111CTGDB"; break;
                case "4116": dbName1 = "DLRSLSINF_4116CMLDB"; break;
                case "4121": dbName1 = "DLRSLSINF_4121NOADB"; break;
                case "4126": dbName1 = "DLRSLSINF_4126RAJDB"; break;
                case "4131": dbName1 = "DLRSLSINF_4131BGRDB"; break;
                case "4136": dbName1 = "DLRSLSINF_4136BRSDB"; break;
                case "4141": dbName1 = "DLRSLSINF_4141KHLDB"; break;
                case "4146": dbName1 = "DLRSLSINF_4146SYLDB"; break;
                case "4151": dbName1 = "DLRSLSINF_4151DINDB"; break;
                case "4156": dbName1 = "DLRSLSINF_4156PABDB"; break;
                case "4161": dbName1 = "DLRSLSINF_4161PATDB"; break;
                case "4166": dbName1 = "DLRSLSINF_4166JSRDB"; break;
                case "4171": dbName1 = "DLRSLSINF_4171MYMDB"; break;
                case "4176": dbName1 = "DLRSLSINF_4176JAMDB"; break;
                case "4181": dbName1 = "DLRSLSINF_4181TNGDB"; break;
                case "4186": dbName1 = "DLRSLSINF_4186RNPDB"; break;
                case "4191": dbName1 = "DLRSLSINF_4191FRPDB"; break;
                case "4198": dbName1 = "DLRSLSINF_4198DRADB"; break;
            }
            return dbName1;
        }
        public static async Task<bool> GetTerminalConfiguration()
        {
            AppBasicInfo.TerminalComponentList1.Clear();
            AppBasicInfo.TerminalComponentList1 = await AppCustomFunctions.GenerateTerminalConfiguration();
            string Terminalsdes1 = "";
            var tlist1 = AppBasicInfo.TerminalComponentList1.FindAll(x => x.grp == "BaseBoard" && x.cnm == "SerialNumber").ToList();
            if (tlist1.Count > 0)
                Terminalsdes1 = new String(tlist1[0].cds.Trim().ToCharArray().Reverse().ToArray());
            string Terminalsdes2 = "";
            var tlist2 = AppBasicInfo.TerminalComponentList1.FindAll(x => x.grp == "Processor" && x.cnm == "ProcessorId").ToList();
            if (tlist2.Count > 0)
                Terminalsdes2 = new String(tlist2[0].cds.Trim().ToCharArray().Reverse().ToArray());

            string Terminalsdes3 = "";
            var tlist3 = AppBasicInfo.TerminalComponentList1.FindAll(x => x.grp == "OperatingSystem" && x.cnm == "OSDriveVolumeSerialNumber").ToList();
            if (tlist3.Count > 0)
                Terminalsdes3 = new String(tlist3[0].cds.Trim().ToCharArray().Reverse().ToArray());

            AppBasicInfo.AppTerminalSlNo = Terminalsdes1.Trim() + "-" + Terminalsdes2.Trim() + "-" + Terminalsdes3.Trim();

            var tlist4 = AppBasicInfo.TerminalComponentList1.FindAll(x => x.grp == "Processor" && x.cnm == "SystemName").ToList();
            if (tlist4.Count > 0)
                AppBasicInfo.AppTerminalName = tlist4[0].cds.Trim();

            AppBasicInfo.AppTerminalsDes = "";

            foreach (var item in AppBasicInfo.TerminalComponentList1)
                item.tsl = AppBasicInfo.AppTerminalSlNo;

            return await Task.FromResult(true);
        }
        public static async Task<List<AppCustomClasses.TerminalComponent>> GenerateTerminalConfiguration()
        {
            /*
            // Hardware Information     Win32_BaseBoard,        Win32_BIOS,     Win32_Processor
            // Operating System         Win32_OperatingSystem
            // Disk Drives              Win32_DiskDrive,        Win32_LogicalDisk
            // Physical Memory          Win32_PhysicalMemory
            // Network Adapter          Win32_NetworkAdapter
            */
            /*
             * TerminalComponent
               public string tsl { get; set; }         // Terminal Serial No.
            public string sln { get; set; }         // Component Serial No.
            public string grp { get; set; }         // Group Name
            public string cnm { get; set; }         // Component Name
            public string cds { get; set; }         // Component Description
             
             */
            var TerminalComponentList1 = new List<AppCustomClasses.TerminalComponent>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_BaseBoard");
            foreach (ManagementObject share in searcher.Get())
            {
                foreach (PropertyData PC in share.Properties)
                {
                    if ((PC.Name.Equals("SerialNumber") || PC.Name.Equals("Version")) && PC.Value != null && PC.Value.ToString().Trim().Length > 0)
                    {
                        var item = new AppCustomClasses.TerminalComponent() { grp = "BaseBoard", cnm = PC.Name, cds = PC.Value.ToString().Trim() };
                        TerminalComponentList1.Add(item);
                    }
                }
            }

            searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject share in searcher.Get())
            {
                foreach (PropertyData PC in share.Properties)
                {
                    if ((PC.Name.Equals("ProcessorId") || PC.Name.Equals("SystemName")) && PC.Value != null && PC.Value.ToString().Trim().Length > 0)
                    {
                        var item = new AppCustomClasses.TerminalComponent() { grp = "Processor", cnm = PC.Name, cds = PC.Value.ToString().Trim() };
                        TerminalComponentList1.Add(item);
                    }
                }
            }

            searcher = new ManagementObjectSearcher("select * from Win32_BIOS");
            foreach (ManagementObject share in searcher.Get())
            {
                foreach (PropertyData PC in share.Properties)
                {
                    if ((PC.Name.Equals("SerialNumber") || PC.Name.Equals("Version")) && PC.Value != null && PC.Value.ToString().Trim().Length > 0)
                    {
                        var item = new AppCustomClasses.TerminalComponent() { grp = "BIOS", cnm = PC.Name, cds = PC.Value.ToString().Trim() };
                        TerminalComponentList1.Add(item);
                    }
                }
            }

            searcher = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
            foreach (ManagementObject share in searcher.Get())
            {
                string Name1 = " ", Value1 = " ";
                foreach (PropertyData PC in share.Properties)
                {
                    if ((PC.Name.Equals("Capacity") || PC.Name.Equals("SerialNumber") || PC.Name.Equals("Speed"))
                        && PC.Value != null && PC.Value.ToString().Trim().Length > 0)
                    {
                        Name1 += PC.Name + "|";
                        Value1 += PC.Value.ToString().Trim() + "|";
                        //var item = new AppCustomClasses.TerminalComponent() { grp = "PhysicalMemory", cnm = PC.Name, cds = PC.Value.ToString().Trim() };
                        //TerminalComponentList1.Add(item);
                    }
                }
                Name1 = Name1.Substring(0, Name1.Length - 1).Trim();
                Value1 = Value1.Substring(0, Value1.Length - 1).Trim();
                if (Name1.Length > 0)
                {
                    var item2 = new AppCustomClasses.TerminalComponent() { grp = "PhysicalMemory", cnm = Name1, cds = Value1 };
                    TerminalComponentList1.Add(item2);
                }
            }

            searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject share in searcher.Get())
            {
                string Name1 = " ", Value1 = " ";
                foreach (PropertyData PC in share.Properties)
                {
                    if ((PC.Name.Equals("Model") || PC.Name.Equals("InterfaceType") || PC.Name.Equals("SerialNumber")
                        || PC.Name.Equals("Size")) && PC.Value != null && PC.Value.ToString().Trim().Length > 0)
                    {
                        //if (PC.Name.Equals("InterfaceType") && PC.Value.ToString().Equals("USB")) // If USB drive information not required
                        //    break;
                        Name1 += PC.Name + "|";
                        Value1 += PC.Value.ToString().Trim() + "|";
                        //var item = new AppCustomClasses.TerminalComponent() { grp = "DiskDrive", cnm = PC.Name, cds = PC.Value.ToString().Trim() };
                        //TerminalComponentList1.Add(item);
                    }
                }
                Name1 = Name1.Substring(0, Name1.Length - 1).Trim();
                Value1 = Value1.Substring(0, Value1.Length - 1).Trim();
                if (Name1.Length > 0)
                {
                    var item2 = new AppCustomClasses.TerminalComponent() { grp = "DiskDrive", cnm = Name1, cds = Value1 };
                    TerminalComponentList1.Add(item2);
                }
            }
            string SystemDir1 = Path.GetPathRoot(Environment.SystemDirectory);
            string SystemDrive1 = Path.GetPathRoot(Environment.SystemDirectory).Split(':')[0] + ":";
            var itemOSDrive1 = new AppCustomClasses.TerminalComponent();
            var itemSystemDir1 = new AppCustomClasses.TerminalComponent() { grp = "OperatingSystem", cnm = "SystemDirectory", cds = Environment.SystemDirectory };

            searcher = new ManagementObjectSearcher("select * from Win32_LogicalDisk");
            foreach (ManagementObject share in searcher.Get())
            {
                string Name1 = " ", Value1 = " ";
                foreach (PropertyData PC in share.Properties)
                {
                    if ((PC.Name.Equals("DeviceID") || PC.Name.Equals("Size") || PC.Name.Equals("VolumeSerialNumber"))
                        && PC.Value != null && PC.Value.ToString().Trim().Length > 0)
                    {
                        Name1 += PC.Name + "|";
                        Value1 += PC.Value.ToString().Trim() + "|";
                        //var item = new AppCustomClasses.TerminalComponent() { grp = "LogicalDisk", cnm = PC.Name, cds = PC.Value.ToString().Trim() };
                        //this.TerminalComponentList1.Add(item);
                    }
                }
                Name1 = Name1.Substring(0, Name1.Length - 1).Trim();
                Value1 = Value1.Substring(0, Value1.Length - 1).Trim();
                if (Name1.Length > 0)
                {
                    var item2 = new AppCustomClasses.TerminalComponent() { grp = "LogicalDisk", cnm = Name1, cds = Value1 };
                    TerminalComponentList1.Add(item2);
                }

                if (Value1.Contains(SystemDrive1))
                {
                    itemOSDrive1.grp = "OperatingSystem";
                    itemOSDrive1.cnm = "OSDriveVolumeSerialNumber";// Name1;
                    itemOSDrive1.cds = Value1.Split('|')[2];
                }

            }
            searcher = new ManagementObjectSearcher("select * from Win32_NetworkAdapter");
            foreach (ManagementObject share in searcher.Get())
            {
                string Name1 = " ", Value1 = " ";
                string GUID1 = "", MACAddress1 = "", NetConnectionID1 = "";
                foreach (PropertyData PC in share.Properties)
                {
                    if ((PC.Name.Equals("GUID") || PC.Name.Equals("MACAddress") || PC.Name.Equals("NetConnectionID"))
                        && PC.Value != null && PC.Value.ToString().Trim().Length > 0)
                    {
                        Name1 += PC.Name + "|";
                        Value1 += PC.Value.ToString().Trim() + "|";
                        switch (PC.Name)
                        {
                            case "GUID": GUID1 = PC.Value.ToString().Trim(); break;
                            case "MACAddress": MACAddress1 = PC.Value.ToString().Trim(); break;
                            case "NetConnectionID": NetConnectionID1 = PC.Value.ToString().Trim(); break;
                        }
                        //var item = new AppCustomClasses.TerminalComponent() { grp = "NetworkAdapter", cnm = PC.Name, cds = PC.Value.ToString().Trim() };
                        //TerminalComponentList1.Add(item);
                    }
                }
                if (GUID1.Length > 0 && MACAddress1.Length > 0 && NetConnectionID1.Length > 0)
                {
                    //var itemn1 = new AppCustomClasses.TerminalComponent() { grp = "NetworkAdapter", cnm = "GUID", cds = GUID1 };
                    //TerminalComponentList1.Add(itemn1);
                    //var itemn2 = new AppCustomClasses.TerminalComponent() { grp = "NetworkAdapter", cnm = "MACAddress", cds = MACAddress1 };
                    //TerminalComponentList1.Add(itemn2);
                    //var itemn3 = new AppCustomClasses.TerminalComponent() { grp = "NetworkAdapter", cnm = "NetConnectionID", cds = NetConnectionID1 };
                    //TerminalComponentList1.Add(itemn3);
                    Name1 = Name1.Substring(0, Name1.Length - 1).Trim();
                    Value1 = Value1.Substring(0, Value1.Length - 1).Trim();
                    if (Name1.Length > 0)
                    {
                        var item2 = new AppCustomClasses.TerminalComponent() { grp = "NetworkAdapter", cnm = Name1, cds = Value1 };
                        TerminalComponentList1.Add(item2);
                    }
                }
            }
            searcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject share in searcher.Get())
            {
                foreach (PropertyData PC in share.Properties)
                {
                    if ((PC.Name.Equals("Caption") || PC.Name.Equals("Version") || PC.Name.Equals("SerialNumber"))
                        && PC.Value != null && PC.Value.ToString().Trim().Length > 0)
                    {
                        var item = new AppCustomClasses.TerminalComponent() { grp = "OperatingSystem", cnm = PC.Name, cds = PC.Value.ToString().Trim() };
                        TerminalComponentList1.Add(item);
                    }
                }
            }
            TerminalComponentList1.Add(itemOSDrive1);
            TerminalComponentList1.Add(itemSystemDir1);

            int slnum1 = 1;
            foreach (var item in TerminalComponentList1)
            {
                item.tsl = "";// "41011009";// "CBWS41011009";
                item.sln = slnum1.ToString("00");
                slnum1++;
            }
            //return TerminalComponentList1;
            return await Task.FromResult(TerminalComponentList1);
        }
        public static List<string> SeparateStringToList(string str1, int strlen1)
        {
            List<string> slist = new List<string>();
            int startLength = 0; // start string length

            for (int i = 0; i < str1.Length; i++)
            {
                int slen = str1.Substring(startLength, str1.Length - startLength).Length; // new string length after item add to the list
                if (slen > strlen1) //  IndexOutOfRangeException resolving
                {
                    var parts = str1.Substring(startLength, strlen1).Split(' '); // string to array 
                    var item1 = string.Join(" ", parts.Take(parts.Length - 1));  // string in n character
                    slist.Add(item1); // my string add to the list
                    startLength += item1.Length; // start string length changes after add to the list
                }
                else
                {
                    var parts1 = str1.Substring(startLength, slen).Split(' ');
                    var item1 = string.Join(" ", parts1.Take(parts1.Length + 1));
                    slist.Add(item1);
                    return slist;
                }
            }
            return slist;
        }

        public static List<string> SeparateStringToList2(string ArrString, int position)
        {
            int total, modulas, loopCount, lastIndex = 0;
            List<string> result = new List<string>();
            var r = ArrString.Split(',').ToList();
            total = r.Count();
            loopCount = total / position;
            modulas = total % position;
            for (int i = 0; i < loopCount; i++)
            {
                var str = "";
                for (int j = 0; j < position; j++)
                {
                    str = str + r[lastIndex] + ",";
                    lastIndex = lastIndex + 1;
                }
                var lastComma = str.LastIndexOf(',');
                str = str.Remove(lastComma, 1).Insert(lastComma, "");
                if (str.Length > 0)
                    result.Add(str);
            }
            string tempStr = "";
            for (int i = 0; i < modulas; i++)
            {
                int index = total - (i + 1);
                tempStr = tempStr + r[index] + ",";
            }
            var lastComma2 = tempStr.LastIndexOf(',');
            if (lastComma2 > 0)
            {
                tempStr = tempStr.Remove(lastComma2, 1).Insert(lastComma2, "");
                if (tempStr.Length > 0)
                    result.Add(tempStr);
            }
            return result;
        }

    }
}
