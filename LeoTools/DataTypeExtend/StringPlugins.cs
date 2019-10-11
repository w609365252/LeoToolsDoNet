using System;
using System.Text;
using System.Text.RegularExpressions;

namespace LeoTools.DataTypeExtend
{
    /// <summary>
    /// String扩展方法类
    /// </summary>
    
    public static class StringPlugins
    {
        /// <summary>
        /// 拼音区编码数组
        /// </summary>
        private static int[] PhoneticCode = new int[]
            {
                -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
                -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
                -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
                -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
                -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
                -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
                -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
                -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
                -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
                -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
                -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
                -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
                -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
                -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
                -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
                -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
                -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
                -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
                -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
                -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
                -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
                -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
                -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
                -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
                -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
                -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
                -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
                -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
                -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
                -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
                -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
                -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
                -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
            };

        /// <summary>
        /// 拼音数组
        /// </summary>
        private static string[] Phonetic = new string[]
            {
                "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
                "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
                "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
                "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
                "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
                "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
                "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
                "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
                "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
                "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
                "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
                "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
                "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
                "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
                "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
                "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
                "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
                "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
                "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
                "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
                "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
                "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
                "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
                "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
                "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
                "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
                "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
                "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
                "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
                "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
                "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
                "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
                "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
           };

        /// <summary>
        /// 判断指定的字符串是 null 还是 System.String.Empty
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>true或false</returns>
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        /// <summary>
        /// 判断指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>true或false</returns>
        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// 是否为空字符串, 如果字符串为null, 异常
        /// </summary>
        /// <param name="str">元字符串</param>
        /// <returns>是否是空字符串</returns>
        
        public static bool IsNull(this string str) => str.Length <= 0;

        /// <summary>
        /// 返回从左边开始，指定长度的字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取的字符串</returns>
        public static string LeftSubstring(this string str, int length) => (str.Length <= length) ? str : str.Substring(0, length);

        /// <summary>
        /// 返回从右边开始，指定长度的字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取的字符串</returns>
        public static string RightSubstring(this string str, int length) => (str.Length <= length) ? str : str.Substring(str.Length - length);

        /// <summary>
        /// 两个字符串是否相等(效率最高的方式,此方法不处理字符串为null的情况，否则异常)
        /// </summary>
        /// <param name="str1">第一个字符串</param>
        /// <param name="str2">第二个字符串</param>
        /// <param name="com">是否忽略大小写，默认忽略</param>
        /// <returns>是否相等</returns>
        public static bool _Equals(this string str1, string str2, bool com = true) => string.Compare(str1, str2, com) == 0;

        /// <summary>
        /// 是否为空字符串，不是判断是不是null，如果字符串为null，会引发异常
        /// </summary>
        /// <param name="str">元字符串</param>
        /// <returns>是否是空字符串</returns>
        public static bool IsWhite(this string str) => str.Length == 0;

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string FristToUpper(this string str) => str.IsNullOrWhiteSpace() ? string.Empty : str[0].ToString().ToUpper() + str.Substring(1);

        /// <summary>
        /// 字符串是否为纯数字
        /// </summary>
        /// <param name="s">元字符串</param>
        /// <returns>是否是数字</returns>
        
        public static bool IsNumber(this string s) => new Regex("^[0-9]*$").IsMatch(s);

        /// <summary>
        /// 字符串是不是小数
        /// </summary>
        /// <param name="s">元字符串</param>
        /// <returns></returns>
        
        public static bool IsDecimal(this string s) => new Regex(@"^\d+\.\d+$").IsMatch(s);

        /// <summary>
        /// 字符串是不是邮箱
        /// </summary>
        /// <param name="email">邮箱字符串</param>
        /// <returns>是否有效</returns>
        public static bool CheckEmail(this string email)
        {
            Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            return r.IsMatch(email);
        }

        /// <summary>
        /// 最后一个字母转大写
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string LastToUpper(this string str) => str.IsNullOrWhiteSpace() ? string.Empty : str.Substring(0, str.Length - 1) + str[str.Length - 1].ToString().ToUpper();

        /// <summary>
        /// 身份证验证, 支持15和18位身份证
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns></returns>
        public static bool CheckIDCard(this string id)
        {
            return id.Length == 18 ? id.CheckIDCard18() : id.Length == 15 ? id.CheckIDCard15() : false;
        }

        /// <summary>
        /// 18位身份证验证
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns></returns>
        public static bool CheckIDCard18(this string id)
        {
            if (id.IsNullOrWhiteSpace()) throw new Exception("身份证号码不能为空");
            if (long.TryParse(id.Remove(17), out long n) == false || n < Math.Pow(10, 16) || long.TryParse(id.Replace('x', '0').Replace('X', '0'), out n) == false) return false;//数字验证
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(id.Remove(2)) == -1) return false;//省份验证
            string birth = id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false) return false;//生日验证
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++) sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != id.Substring(17, 1).ToLower()) return false;//校验码验证
            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 15位身份证验证
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <returns></returns>
        public static bool CheckIDCard15(this string id)
        {
            if (id.IsNullOrWhiteSpace()) throw new Exception("身份证号码不能为空");
            if (long.TryParse(id, out long n) == false || n < Math.Pow(10, 14)) return false;//数字验证
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(id.Remove(2)) == -1) return false;//省份验证
            string birth = id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false) return false;//生日验证
            return true;//符合15位身份证标准
        }

        /// <summary>
        /// 验证电话号码，带区号，中间用-隔开
        /// </summary>
        /// <param name="strTelephone">电话号码，带区号，区号和号码用-隔开</param>
        /// <returns>是否是合法电话</returns>
        public static bool IsTelephone(this string strTelephone)
        {
            if (strTelephone.IsNullOrWhiteSpace()) throw new Exception("电话号码不能为空");
            return Regex.IsMatch(strTelephone, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="strHandset">手机号</param>
        /// <returns>是否是手机号</returns>
        public static bool IsPhone(this string strHandset)
        {
            if (strHandset.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(strHandset, @"^[1][3,4,5,7,8]\d{9}$");
        }

        /// <summary>
        /// 验证邮编
        /// </summary>
        /// <param name="strPostalcode">邮编</param>
        /// <returns>是否是邮编</returns>
        public static bool IsPostalcode(this string strPostalcode)
        {
            if (strPostalcode.IsNullOrWhiteSpace()) throw new Exception("邮编不能为空");
            return Regex.IsMatch(strPostalcode, @"^\d{6}$");
        }

        /// <summary>
        /// 汉字转换成全拼
        /// </summary>
        /// <param name="chstr">汉字字符串</param>
        /// <returns>转换后的拼音字符串</returns>
        public static string ConvertTo(this string chstr)
        {
            Regex reg = new Regex("^[\u4e00-\u9fa5]$"); //验证是否输入汉字
            byte[] arr = new byte[2];
            string pystr = string.Empty;
            int asc = 0, M1 = 0, M2 = 0;
            char[] mChar = chstr.ToCharArray();         //获取汉字对应的字符数组
            for (int j = 0; j < mChar.Length; j++)
            {
                if (reg.IsMatch(mChar[j].ToString()))  //如果输入的是汉字
                {
                    arr = Encoding.Default.GetBytes(mChar[j].ToString());
                    M1 = arr[0];
                    M2 = arr[1];
                    asc = M1 * 256 + M2 - 65536;
                    if (asc > 0 && asc < 160)
                    {
                        pystr += mChar[j];
                    }
                    else
                    {
                        switch (asc)
                        {
                            case -9254:
                                pystr += "Zhen"; break;
                            case -8985:
                                pystr += "Qian"; break;
                            case -5463:
                                pystr += "Jia"; break;
                            case -8274:
                                pystr += "Ge"; break;
                            case -5448:
                                pystr += "Ga"; break;
                            case -5447:
                                pystr += "La"; break;
                            case -4649:
                                pystr += "Chen"; break;
                            case -5436:
                                pystr += "Mao"; break;
                            case -5213:
                                pystr += "Mao"; break;
                            case -3597:
                                pystr += "Die"; break;
                            case -5659:
                                pystr += "Tian"; break;
                            default:
                                pystr = GetPhonetic(pystr, asc);
                                break;
                        }
                    }
                }
                else
                {
                    pystr += mChar[j].ToString();
                }
            }
            return pystr;
        }

        /// <summary>
        /// 验证是不是有效的车牌号
        /// </summary>
        /// <param name="vehicleNumber">源字符串</param>
        /// <returns></returns>
        
        public static bool IsLicensePlate(this string vehicleNumber)
        {
            bool result = false;
            if (vehicleNumber.Length == 7 || vehicleNumber.Length == 8)
            {
                string express = @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领]{1}[A-Z]{1}[A-Z0-9]{5,6}[挂学警港澳]{0,1}$";
                result = Regex.IsMatch(vehicleNumber, express);
            }
            return result;
        }

        private static string GetPhonetic(string pystr, int asc)
        {
            for (int i = (PhoneticCode.Length - 1); i >= 0; i--)
            {
                if (PhoneticCode[i] <= asc) //判断汉字的拼音区编码是否在指定范围内
                {
                    pystr += Phonetic[i];   //如果不超出范围则获取对应的拼音
                    break;
                }
            }
            return pystr;
        }

        #region 字符串转其它类型方法
        /// <summary>
        /// 字符串转Int16，转换失败返回默认值
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="default">转换失败返回的默认值，默认为0</param>
        /// <returns>源字符串的Int16形式</returns>
        public static int ToInt16(this string str, short @default = 0) => Int16.TryParse(str, out short item) ? item : @default;

        /// <summary>
        /// 字符串转Int32，转换失败返回默认值
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="default">转换失败返回的默认值，默认为0</param>
        /// <returns>源字符串的Int32形式</returns>
        public static int ToInt32(this string str, int @default = 0) => Int32.TryParse(str, out int item) ? item : @default;

        /// <summary>
        /// 字符串转Int64，转换失败返回默认值
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="default">转换失败返回的默认值，默认为0</param>
        /// <returns>源字符串的Int64形式</returns>
        public static long ToInt64(this string str, long @default = 0) => Int64.TryParse(str, out long item) ? item : @default;

        /// <summary>
        /// 字符串转double，转换失败返回默认值
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="default">转换失败返回的默认值，默认为0.00</param>
        /// <returns>源字符串的double形式</returns>
        public static double ToDouble(this string str, double @default = 0.00) => double.TryParse(str, out double item) ? item : @default;

        /// <summary>
        /// 字符串转float，转换失败返回默认值
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="default">转换失败返回的默认值，默认为0.00</param>
        /// <returns>源字符串的float形式</returns>
        public static float ToFloat(this string str, float @default = 0.00f) => float.TryParse(str, out float item) ? item : @default;

        /// <summary>
        /// 字符串转decimal, 转换失败返回默认值
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="default">转换失败返回的默认值，默认为0.00</param>
        /// <returns>源字符串的decimal形式</returns>
        public static decimal ToDecimal(this string str, decimal @default = 0.00M) => decimal.TryParse(str, out decimal item) ? item : @default;

        /// <summary>
        /// 字符串转DateTime，转换失败返回默认值
        /// </summary> 
        /// <param name="str">源字符串</param>
        /// <param name="default">转换失败返回的默认值，默认为DateTime.MinValue</param>
        /// <returns>源字符串的DateTime形式</returns>
        public static DateTime ToDateTime(this string str, DateTime? @default = null) => DateTime.TryParse(str, out DateTime item) ? item : @default == null ? DateTime.MinValue : (DateTime)@default;

        /// <summary>
        /// 时间戳字符串转DateTime
        /// </summary>
        /// <param name="timeStamp">时间戳字符串</param>
        /// <returns>DateTime对象</returns>
        public static DateTime StampToDateTime(this string timeStamp)
        {
            DateTime dateTimeStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }
        #endregion

        /// <summary>
        /// 获得字节长度, 一个中文是两个字节, 字母和数字是一个字节
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="isTrim">是否去掉前后的空格再进行计算, 默认不去掉</param>
        /// <returns>字节长度</returns>
        /// <exception cref="NullReferenceException">字符串是null的异常</exception>
        public static int GetBytesLength(this string str, bool isTrim = false)
        {
            if (isTrim) str = str.Trim();
            return Encoding.Default.GetBytes(str).Length;
        }


        /// <summary>
        /// 将html文本转化为 文本内容方法TextNoHTML
        /// </summary>
        /// <param name="Htmlstring">HTML文本值</param>
        /// <returns></returns>
        public static string TextNoHTML(this string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            //替换掉 < 和 > 标记
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Replace("\r", "");
            Htmlstring = Htmlstring.Replace("\n", "");
            //返回去掉html标记的字符串
            return Htmlstring;
        }

    }
}