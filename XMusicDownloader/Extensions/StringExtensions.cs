using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace XMusicDownloader.Extensions
{
    public static class StringExtensions
    {
        static StringExtensions()
        {
            StringExtensions.ResetParsers(false);
        }

        #region 1.Intercept ：截取字符串

        /// <summary>
        /// 获取字符串中指定字符串之后的字符串
        /// </summary>
        /// <param name="str">要截取的原字符串</param>
        /// <param name="afterWhat">截取的依据</param>
        /// <returns>
        /// 返回截取到的字符串。
        /// 如果无任何匹配，则返回 null；
        /// </returns>
        public static string GetAfter(this string str, string afterWhat)
        {
            int index = str.IndexOf(afterWhat);
            if (index == -1) return null;

            index += str.Length;
            return str.Substring(index);
        }

        /// <summary>
        /// 获取字符串中指定字符串的最后一个匹配之后的字符串
        /// </summary>
        /// <param name="str">要截取的原字符串</param>
        /// <param name="afterWhat">截取的依据</param>
        /// <returns>
        /// 返回截取到的字符串。
        /// 如果无任何匹配，则返回 null；
        /// </returns>
        public static string GetLastAfter(this string str, string afterWhat)
        {
            int index = str.LastIndexOf(afterWhat);
            if (index == -1) return null;

            index += str.Length;
            return str.Substring(index);
        }

        /// <summary>
        /// 获取字符串中指定字符串之前的字符串
        /// </summary>
        /// <param name="str">要截取的原字符串</param>
        /// <param name="beforeWhat">截取的依据</param>
        /// <returns>
        /// 返回截取到的字符串。
        /// 如果无任何匹配，则返回 null；
        /// </returns>
        public static string GetBefore(this string str, string beforeWhat)
        {
            int index = str.IndexOf(beforeWhat);
            return str.Substring(0, index);
        }

        /// <summary>
        /// 获取字符串中指定字符串最后一个匹配之前的字符串
        /// </summary>
        /// <param name="str">要截取的原字符串</param>
        /// <param name="beforeWhat">截取的依据</param>
        /// <returns>
        /// 返回截取到的字符串。
        /// 如果无任何匹配，则返回 null；
        /// </returns>
        public static string GetLastBefore(this string str, string beforeWhat)
        {
            int index = str.LastIndexOf(beforeWhat);
            return str.Substring(0, index);
        }

        /// <summary>
        /// 获取字符串中指定的两个字符串之间的字符串内容
        /// </summary>
        /// <param name="str">要截取的原字符串</param>
        /// <param name="from">
        /// 截取时作为依据的起始字符串
        /// 如果 from == ""，从零位置开始截取
        /// </param>
        /// <param name="to">
        /// 截取时作为依据的终止字符串
        /// 如果 to == "", 一直截取到最后一个字符
        /// </param>
        /// <returns>
        /// 返回截取到的字符串
        /// </returns>
        public static string GetBetween(this string str, string from, string to)
        {
            if (from == null || to == null)
            {
                throw new ArgumentException("参数 from 与 to，都不能为 null");
            }
            int iStart, iEnd;
            if (from == string.Empty)
                iStart = 0;
            else
                iStart = str.IndexOf(from) + from.Length;
            if (to == string.Empty)
                iEnd = str.Length;
            else
                iEnd = str.IndexOf(to);
            return str.Substring(iStart, iEnd - iStart);
        }

        #endregion

        #region 2.Regex ：正则操作

        /// <summary>
        /// 判断字符串是否与给定模式相匹配
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="pattern">要匹配的模式</param>
        /// <returns>
        /// 返回是否匹配
        /// </returns>
        public static bool IsMatch(this string str, string pattern)
        {
            if (str == null) return false;

            return System.Text.RegularExpressions.Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 查找字符串中与指定模式的所有匹配
        /// </summary>
        /// <param name="str">要匹配的字符串</param>
        /// <param name="pattern">进行匹配的正则表达式</param>
        /// <returns>
        /// 返回所有匹配，包括全局匹配和子匹配，匹配到的文本
        /// </returns>
        public static string[] FindAll(this string str, string pattern)
        {
            if (str == null) return null;

            Match m = System.Text.RegularExpressions.Regex.Match(str, pattern);
            return m.Groups.OfType<Group>().Select(g => g.Value).ToArray();
        }

        #endregion

        #region 3.Fill ：填充

        #region 3.1.Center ：居中填充

        /// <summary>
        /// 使用空格对文本进行居中填充
        /// </summary>
        /// <param name="str">被居中填充的文本</param>
        /// <param name="totalWidth">填充后的总字符数</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string Center(this string str, int totalWidth)
        {
            return Center(str, totalWidth, ' ');
        }

        /// <summary>
        /// 使用指定字符对文本进行居中填充
        /// </summary>
        /// <param name="str">被居中填充的文本</param>
        /// <param name="totalWidth">填充后的总字符数</param>
        /// <param name="fillWith">填充时使用的字符</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string Center(this string str, int totalWidth, char fillWith)
        {
            int strlen = str.Length;
            if (strlen >= totalWidth)
            {
                return str;
            }
            else
            {
                int rightLen = (totalWidth - strlen) / 2;
                int leftLen = totalWidth - strlen - rightLen;
                return fillWith.ToString().Repeat(leftLen) +
                    str + fillWith.ToString().Repeat(rightLen);
            }
        }

        #endregion

        #region 3.2.PadLeftEx ：定宽左填充

        /// <summary>
        /// 按系统默认字符编码对文本进行定宽左填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">要填充到的字节长度</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string PadLeftEx(this string str, int totalByteCount)
        {
            return PadLeftEx(str, totalByteCount, Encoding.Default.BodyName);
        }

        /// <summary>
        /// 按指定字符编码对文本进行定宽左填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">要填充到的字节长度</param>
        /// <param name="encodingName">用于在填充过程中进行文本解析的字符编码</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string PadLeftEx(this string str, int totalByteCount, string encodingName)
        {
            Encoding coding = Encoding.GetEncoding(encodingName);
            int width = coding.GetByteCount(str);
            //总字节数减去原字符串多占的字节数，就是应该添加的空格数
            int padLen = totalByteCount - width;
            if (padLen <= 0)
                return str;
            else
                return str.PadLeft(padLen);
        }

        /// <summary>
        /// 按系统默认字符编码对文本使用指定的填充符进行定宽左填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">要填充到的字节长度</param>
        /// <param name="fillWith">填充符</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string PadLeftEx(this string str, int totalByteCount, char fillWith)
        {
            return PadLeftEx(str, totalByteCount, fillWith, Encoding.Default.BodyName);
        }

        /// <summary>
        /// 按指定字符编码对文本使用指定的填充符进行定宽左填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">要填充到的字节长度</param>
        /// <param name="fillWith">填充符</param>
        /// <param name="encodingName">用于在填充过程中进行文本解析的字符编码</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string PadLeftEx(this string str, int totalByteCount,
            char fillWith, string encodingName)
        {
            Encoding coding = Encoding.GetEncoding(encodingName);
            int fillWithWidth = coding.GetByteCount(new char[] { fillWith });
            int width = coding.GetByteCount(str);
            //总字节数减去原字符串多占的字节数，再除以填充字符的占的字节数，
            //就是应该添加的空格数【因为有时候是双字节的填充符，比如中文】
            int padLen = (totalByteCount - width) / fillWithWidth;
            if (padLen <= 0)
                return str;
            else
                return str.PadLeft(padLen, fillWith);
        }

        #endregion

        #region 3.3.CenterEx ：定宽居中填充

        /// <summary>
        /// 按系统默认字符编码对文本进行定宽居中填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="totalByteCount"></param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string CenterEx(this string str, int totalByteCount)
        {
            return CenterEx(str, totalByteCount, Encoding.Default.BodyName);
        }

        /// <summary>
        /// 按指定的字符编码对文本进行定宽居中填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要居中填充的字符串</param>
        /// <param name="totalByteCount">填充后的总字节数</param>
        /// <param name="encodingName">用于在填充过程中进行文本解析的字符编码</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string CenterEx(this string str, int totalByteCount, string encodingName)
        {
            Encoding coding = Encoding.GetEncoding(encodingName);
            int width = coding.GetByteCount(str);
            //总字节数减去原字符串多占的字节数，就是应该添加的空格数
            int padLen = totalByteCount - width;
            if (padLen < 0) return str;
            int padRight = padLen / 2;
            int padLeft = padLen - padRight;
            return " ".Repeat(padLeft) + str + " ".Repeat(padRight);
        }

        /// <summary>
        /// 按系统默认字符编码对文本使用指定的填充符进行定宽居中填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">填充后得到的结果包含的总字节数</param>
        /// <param name="fillWith">填充符</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string CenterEx(this string str, int totalByteCount, char fillWith)
        {
            return CenterEx(str, totalByteCount, fillWith, Encoding.Default.BodyName);
        }

        /// <summary>
        /// 按指定的字符编码对文本使用指定的填充符进行定宽居中填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">填充后得到的文本需达到的总字节数</param>
        /// <param name="fillWith">填充符</param>
        /// <param name="encodingName">用于在填充过程中进行文本解析的字符编码</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string CenterEx(this string str, int totalByteCount,
            char fillWith, string encodingName)
        {
            Encoding coding = Encoding.GetEncoding(encodingName);
            int fillWithWidth = coding.GetByteCount(new char[] { fillWith });
            string fillStr = fillWith.ToString();
            int width = coding.GetByteCount(str);
            //总字节数减去原字符串多占的字节数，就是应该添加的空格数
            int padLen = (totalByteCount - width) / fillWithWidth;
            if (padLen < 0) return str;
            int padRight = padLen / 2;
            int padLeft = padLen - padRight;
            return fillStr.Repeat(padLeft) + str + fillStr.Repeat(padRight);
        }

        #endregion

        #region 3.4.PadRight ： 定宽右填充

        /// <summary>
        /// 按系统默认字符编码对文本进行定宽右填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">要填充到的字节长度</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string PadRightEx(this string str, int totalByteCount)
        {
            return PadRightEx(str, totalByteCount, Encoding.Default.BodyName);
        }

        /// <summary>
        /// 按指定字符编码对文本进行定宽右填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">要填充到的字节长度</param>
        /// <param name="encodingName">用于在填充过程中进行文本解析的字符编码</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string PadRightEx(this string str, int totalByteCount, string encodingName)
        {
            Encoding coding = Encoding.GetEncoding(encodingName);
            int width = coding.GetByteCount(str);
            //总字节数减去原字符串多占的字节数，就是应该添加的空格数
            int padLen = totalByteCount - width;
            if (padLen <= 0)
                return str;
            else
                return str.PadRight(padLen);
        }

        /// <summary>
        /// 按系统默认字符编码对文本使用指定的填充符进行定宽右填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">要填充到的字节长度</param>
        /// <param name="fillWith">填充符</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string PadRightEx(this string str, int totalByteCount, char fillWith)
        {
            return PadRightEx(str, totalByteCount, fillWith, Encoding.Default.BodyName);
        }

        /// <summary>
        /// 按指定字符编码对文本使用指定的填充符进行定宽右填充（以半角字符宽度为1单位宽度）
        /// </summary>
        /// <param name="str">要填充的文本</param>
        /// <param name="totalByteCount">要填充到的字节长度</param>
        /// <param name="fillWith">填充符</param>
        /// <param name="encodingName">用于在填充过程中进行文本解析的字符编码</param>
        /// <returns>
        /// 返回填充后的文本
        /// </returns>
        public static string PadRightEx(this string str, int totalByteCount,
            char fillWith, string encodingName)
        {
            Encoding coding = Encoding.GetEncoding(encodingName);
            int fillWithWidth = coding.GetByteCount(new char[] { fillWith });
            int width = coding.GetByteCount(str);
            //总字节数减去原字符串多占的字节数，再除以填充字符的占的字节数，
            //就是应该添加的空格数【因为有时候是双字节的填充符，比如中文】
            int padLen = (totalByteCount - width) / fillWithWidth;
            if (padLen <= 0)
                return str;
            else
                return str.PadRight(padLen, fillWith);
        }

        #endregion

        #endregion

        #region 4.Repeat ：复制字符串

        /// <summary>
        /// 取得字符串的指定次重复后的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="times">要重复的次数</param>
        /// <returns>
        /// 返回复制了指定次的字符串
        /// </returns>
        public static string Repeat(this string str, int times)
        {
            if (times < 0)
                throw new ArgumentException("参数 times 不能小于0.");

            if (str == null)
                throw new ArgumentException("要复制的字符串不能为 null.");

            if (str == string.Empty) return string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= times; i++)
            {
                sb.Append(str);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 取得字符串的指定次重复后的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="totalByteCount">要重复到的字符宽度</param>
        /// <returns>
        /// 返回复制了指定次的字符串
        /// </returns>
        public static string RepeatEx(this string str, int totalByteCount)
        {
            return StringExtensions.RepeatEx(str, totalByteCount, Encoding.Default.BodyName);
        }

        /// <summary>
        /// 取得字符串的指定次重复后的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="totalByteCount">要重复到的字符宽度</param>
        /// <param name="encodingName">用于在复制过程中进行文本解析的字符编码</param>
        /// <returns>
        /// 返回复制了指定次的字符串
        /// </returns>
        public static string RepeatEx(this string str, int totalByteCount, string encodingName)
        {
            if (totalByteCount < 0)
                throw new ArgumentException("参数 times 不能小于0.");

            if (str == null)
                throw new ArgumentException("要复制的字符串不能为 null.");

            if (str == string.Empty) return string.Empty;

            Encoding coding = Encoding.GetEncoding(encodingName);
            int len = coding.GetByteCount(str);
            int times = totalByteCount / len;
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= times; i++)
            {
                sb.Append(str);
            }
            return sb.ToString();
        }

        #endregion

        #region 5.Parser ：从字符串或表式文本提取对象

        #region Parsers : Common Parsers and Parsers Maker

        public delegate bool TryParse<T>(string valueStr, out T value);

        public static readonly Dictionary<Type, object> Parsers =
            new Dictionary<Type, object>();

        /// <summary>
        /// <see cref="char"/> 类型字符串解析器
        /// </summary>
        public static TryParse<char> CharParser =
            (string valueStr, out char value) => Char.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="bool"/> 类型字符串解析器
        /// </summary>
        public static TryParse<bool> BooleanParser =
            (string valueStr, out bool value) => Boolean.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="byte"/> 类型字符串解析器
        /// </summary>
        public static TryParse<byte> ByteParser =
            (string valueStr, out byte value) => Byte.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="sbyte"/> 类型字符串解析器
        /// </summary>
        public static TryParse<sbyte> SByteParser =
            (string valueStr, out sbyte value) => SByte.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="short"/> 类型字符串解析器
        /// </summary>
        public static TryParse<short> Int16Parser =
            (string valueStr, out short value) => Int16.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="int"/> 类型字符串解析器
        /// </summary>
        public static TryParse<int> Int32Parser =
            (string valueStr, out int value) => Int32.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="long"/> 类型字符串解析器
        /// </summary>
        public static TryParse<long> Int64Parser =
            (string valueStr, out long value) => Int64.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="ushort"/> 类型字符串解析器
        /// </summary>
        public static TryParse<ushort> UInt16Parser =
            (string valueStr, out ushort value) => UInt16.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="uint"/> 类型字符串解析器
        /// </summary>
        public static TryParse<uint> UInt32Parser =
            (string valueStr, out uint value) => UInt32.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="ulong"/> 类型字符串解析器
        /// </summary>
        public static TryParse<ulong> UInt64Parser =
            (string valueStr, out ulong value) => UInt64.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="float"/> 类型字符串解析器
        /// </summary>
        public static TryParse<float> SingleParser =
            (string valueStr, out float value) => Single.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="double"/> 类型字符串解析器
        /// </summary>
        public static TryParse<double> DoubleParser =
            (string valueStr, out double value) => Double.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="decimal"/> 类型字符串解析器
        /// </summary>
        public static TryParse<decimal> DecimalParser =
            (string valueStr, out decimal value) => Decimal.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="DateTime"/> 类型字符串解析器
        /// </summary>
        public static TryParse<DateTime> DateTimeParser =
            (string valueStr, out DateTime value) => DateTime.TryParse(valueStr, out value);

        /// <summary>
        /// <see cref="TimeSpan"/> 类型字符串解析器
        /// </summary>
        public static TryParse<TimeSpan> TimeSpanParser =
            (string valueStr, out TimeSpan value) => TimeSpan.TryParse(valueStr, out value);

        /// <summary>
        /// 获取指定枚举类型的字符串解析器
        /// </summary>
        /// <typeparam name="TEnum">给定枚举类型</typeparam>
        /// <exception cref="ArgumentException">
        /// 要求 <typeparamref name="TEnum"/> 是一个具体的枚举类型
        /// </exception>
        /// <remarks>
        /// <para>这里之所以不用约束条件限制死，是为了内部方法如 </para>
        /// <para><see cref="StringExtensions.VParse{TValue}(string, TryParse{TValue}, bool)"/> 和</para>
        /// <see cref="StringExtensions.VParseTable{TValue}(string, bool, TryParse{TValue}, bool)"/>
        /// 调用的方便
        /// </remarks>
        /// <returns>
        /// 如果指定的类型参数确实是一个枚举类型，就返回它的字符串解析器；否则，报错
        /// </returns>
        public static TryParse<TEnum> GetEnumParser<TEnum>()
            where TEnum : struct
        {
            if (typeof(TEnum).IsEnum)
                return (string valueStr, out TEnum value) =>
                    Enum.TryParse(valueStr, out value);
            else
                throw new ArgumentException("必须是枚举类型", nameof(TEnum));
        }

        /// <summary>
        /// 重置解析器容器
        /// </summary>
        /// <param name="onlyCommonParser">仅重置常用的类型解析器</param>
        public static void ResetParsers(bool onlyCommonParser)
        {
            if (!onlyCommonParser) StringExtensions.Parsers.Clear();
            StringExtensions.Parsers[typeof(char)] = StringExtensions.CharParser;
            StringExtensions.Parsers[typeof(bool)] = StringExtensions.BooleanParser;
            StringExtensions.Parsers[typeof(byte)] = StringExtensions.ByteParser;
            StringExtensions.Parsers[typeof(sbyte)] = StringExtensions.SByteParser;
            StringExtensions.Parsers[typeof(short)] = StringExtensions.Int16Parser;
            StringExtensions.Parsers[typeof(int)] = StringExtensions.Int32Parser;
            StringExtensions.Parsers[typeof(long)] = StringExtensions.Int64Parser;
            StringExtensions.Parsers[typeof(ushort)] = StringExtensions.UInt16Parser;
            StringExtensions.Parsers[typeof(uint)] = StringExtensions.UInt32Parser;
            StringExtensions.Parsers[typeof(ulong)] = StringExtensions.UInt64Parser;
            StringExtensions.Parsers[typeof(float)] = StringExtensions.SingleParser;
            StringExtensions.Parsers[typeof(double)] = StringExtensions.DoubleParser;
            StringExtensions.Parsers[typeof(decimal)] = StringExtensions.DecimalParser;
            StringExtensions.Parsers[typeof(DateTime)] = StringExtensions.DateTimeParser;
            StringExtensions.Parsers[typeof(TimeSpan)] = StringExtensions.TimeSpanParser;
        }

        /// <summary>
        /// 通过反射获取指定类型的 bool TryParse(string s, out T value) 静态方法
        /// </summary>
        /// <typeparam name="T">要获取方法的类型</typeparam>
        /// <returns>
        /// 如果找到这个方法，就返回它；找不到，就报错
        /// </returns>
        private static TryParse<T> GetParser<T>()
        {
            Type t = typeof(T);

            MethodInfo miTryParse = t.GetMethod("TryParse", BindingFlags.Public |
                BindingFlags.Static, null, CallingConventions.Any,
                new Type[] { typeof(string), typeof(T).MakeByRefType() }, null);
            if (miTryParse == null)
                throw new Exception("类型[" + t.FullName +
                    "]没有[bool TryParse(string s, out " + t.Name +
                    " value)]静态方法，无法完成解析");

            return Delegate.CreateDelegate(typeof(TryParse<T>), miTryParse)
                as TryParse<T>;
        }

        #endregion

        /// <summary>
        /// 将字符串，使用指定的解析器方法（如果有提供），解析成可空的目标类型对象
        /// </summary>
        /// <typeparam name="TValue">要解析成的目标类型的可空类型所包装的类型</typeparam>
        /// <param name="valueStr">要解析的字符串</param>
        /// <param name="parser">
        /// 字符串解析器方法
        /// <para>当某类型的解析器被传入调用一次，该解析器会被缓存；</para>
        /// <para>解析器是与解析的目标类型绑定的，下次再调用，不需要再传此参数；</para>
        /// <para>如果再次传入，会视情况（是否相等）覆盖上次调用时所用的解析器</para>
        /// <para>当您未指定解析器方法时，将尝试取得目标类型的固有解析器，如果没找到，将会报错</para>
        /// <para>手传解析器，比目标类型原生解析器具有更高的优先级</para>
        /// <para>本扩展方法类 StringEx，在加载之初，已经初始化了如下常用类型的字符串解析器：</para>
        /// <para>char/bool/byte/sbyte/short/int/long/ushort/uint/ulong/float/double/decimal/DateTime/TimeSpan</para>
        /// <para>另外对于枚举类型，也会自动生成 <typeparamref name="TValue"/> 类型的解析器 Enum.TryParse </para>
        /// </param>
        /// <param name="temp">解析器是否是临时解析器，如果是，则不会被保存</param>
        /// <returns>
        /// 解析成功，返回解析出来的值类型对象；失败，则返回 null
        /// </returns>
        /// <remarks>
        /// <para>如果提供了解析器，本方法通过给定的解析器，解析给定的字符器到目标类型</para>
        /// <para>否则，本方法尝试通过目标类型的 bool TryParse(string s, out TRefer value) 静态方法来实现功能</para>
        /// <para>如果指定的目标类型，不包含此种签名的静态方法，则会报错</para>
        /// </remarks>
        public static TValue? VParse<TValue>(this string valueStr,
            TryParse<TValue> parser = null, bool temp = false)
            where TValue : struct
        {
            Type t = typeof(TValue);

            object proc;
            if (parser == null)
            {
                if (StringExtensions.Parsers.ContainsKey(t))
                    proc = StringExtensions.Parsers[t];
                else
                {
                    if (t.IsEnum)
                        proc = StringExtensions.GetEnumParser<TValue>();
                    else
                        proc = StringExtensions.GetParser<TValue>();
                }
            }
            else
                proc = parser;

            if (!temp &&
                (!StringExtensions.Parsers.ContainsKey(t) ||
                StringExtensions.Parsers[t] != proc))
                StringExtensions.Parsers.Add(t, proc);

            TryParse<TValue> tryParse = proc as TryParse<TValue>;
            if (tryParse(valueStr, out TValue value))
                return value;
            else
                return null;
        }

        /// <summary>
        /// 将字符串，使用指定的解析器方法（如果有提供），解析成目标类型对象
        /// </summary>
        /// <typeparam name="TRefer">要解析成的目标类型</typeparam>
        /// <param name="valueStr">要解析的字符串</param>
        /// <param name="parser">
        /// 字符串解析器方法
        /// <para>当某类型的解析器被传入调用一次，该解析器会被缓存；</para>
        /// <para>解析器是与解析的目标类型绑定的，下次再调用，不需要再传此参数；</para>
        /// <para>如果再次传入，会视情况（是否相等）覆盖上次调用时所用的解析器</para>
        /// <para>当您未指定解析器方法时，将尝试取得目标类型的固有解析器，如果没找到，将会报错</para>
        /// <para>手传解析器，比目标类型原生解析器具有更高的优先级</para>
        /// <para>本扩展方法类 StringEx，在加载之初，已经初始化了如下常用类型的字符串解析器：</para>
        /// <para>char/bool/byte/sbyte/short/int/long/ushort/uint/ulong/float/double/decimal/DateTime/TimeSpan</para>
        /// </param>
        /// <param name="temp">解析器是否是临时解析器，如果是，则不会被保存</param>
        /// <returns>
        /// 解析成功，则返回解析到的对象；失败，则返回 null
        /// </returns>
        /// <remarks>
        /// <para>如果提供了解析器，本方法通过给定的解析器，解析给定的字符器到目标类型</para>
        /// <para>否则，本方法尝试通过目标类型的 bool TryParse(string s, out TRefer value) 静态方法来实现功能</para>
        /// <para>如果指定的目标类型，不包含此种签名的静态方法，则会报错</para>
        /// </remarks>
        public static TRefer RParse<TRefer>(this string valueStr,
            TryParse<TRefer> parser = null, bool temp = false)
            where TRefer : class
        {
            Type t = typeof(TRefer);

            object proc;
            if (parser == null)
            {
                if (StringExtensions.Parsers.ContainsKey(t))
                    proc = StringExtensions.Parsers[t];
                else
                    proc = StringExtensions.GetParser<TRefer>();
            }
            else
            {
                proc = parser;
            }

            if (!temp &&
                (!StringExtensions.Parsers.ContainsKey(t) ||
                StringExtensions.Parsers[t] != proc))
                StringExtensions.Parsers.Add(t, proc);

            TryParse<TRefer> tryParse = proc as TryParse<TRefer>;
            tryParse(valueStr, out TRefer value);
            return value;
        }

        /// <summary>
        /// 将表式文本，使用指定的解析器方法（如果有提供），解析成可空的目标类型对象数组
        /// </summary>
        /// <typeparam name="TValue">要解析成的目标类型</typeparam>
        /// <param name="tableText">要解析的表式文本</param>
        /// <param name="noWhiteLines">是否抛弃空白行</param>
        /// <param name="parser">
        /// 表式文本解析器方法
        /// <para>当某类型的解析器被传入调用一次，该解析器会被缓存，除非指定它是临时的；</para>
        /// <para>解析器是与解析的目标类型绑定的，下次再调用，不需要再传此参数；</para>
        /// <para>如果再次传入，会视情况（是否相等）覆盖上次调用时所用的解析器</para>
        /// <para>当您未指定解析器方法时，将尝试取得目标类型的固有解析器，如果没找到，将会报错</para>
        /// <para>手传解析器，比目标类型原生解析器具有更高的优先级</para>
        /// <para>本扩展方法类 StringEx，在加载之初，已经初始化了如下常用类型的字符串解析器：</para>
        /// <para>char/bool/byte/sbyte/short/int/long/ushort/uint/ulong/float/double/decimal/DateTime/TimeSpan</para>
        /// <para>另外对于枚举类型，也会自动生成 <typeparamref name="TValue"/> 类型的解析器 Enum.TryParse </para>
        /// </param>
        /// <param name="temp">解析器是否是临时解析器，如果是，则不会被保存</param>
        /// <returns>
        /// 解析成功，则返回解析到的对象；失败，则返回 null
        /// </returns>
        /// <remarks>
        /// <para>如果提供了解析器，本方法通过给定的解析器，解析给定的表式文本到目标类型</para>
        /// <para>否则，本方法尝试通过目标类型的 bool TryParse(string s, out TRefer value) 静态方法来实现功能</para>
        /// <para>如果指定的目标类型，不包含此种签名的静态方法，则会报错</para>
        /// </remarks>
        public static TValue?[] VParseTable<TValue>(
            this string tableText, bool noWhiteLines = true,
            TryParse<TValue> parser = null, bool temp = false)
            where TValue : struct
        {
            Type t = typeof(TValue);

            object proc;
            if (parser == null)
            {
                if (StringExtensions.Parsers.ContainsKey(t))
                    proc = StringExtensions.Parsers[t];
                else
                {
                    if (t.IsEnum)
                        proc = StringExtensions.GetEnumParser<TValue>();
                    else
                        proc = StringExtensions.GetParser<TValue>();
                }
            }
            else
                proc = parser;

            if (!temp &&
                (!StringExtensions.Parsers.ContainsKey(t) ||
                StringExtensions.Parsers[t] != proc))
                StringExtensions.Parsers.Add(t, proc);

            TryParse<TValue> tryParse = proc as TryParse<TValue>;
            List<TValue?> values = new List<TValue?>();
            foreach (string line in tableText.GetLines(noWhiteLines))
            {
                if (tryParse(line, out TValue value))
                    values.Add(value);
                else
                    values.Add(null);
            }

            return values.ToArray();
        }

        /// <summary>
        /// 将表式文本，使用指定的解析器方法（如果有提供），解析成目标类型对象数组
        /// </summary>
        /// <typeparam name="TRefer">要解析成的目标类型</typeparam>
        /// <param name="tableText">要解析的表式文本</param>
        /// <param name="noWhiteLines">是否抛弃空白行</param>
        /// <param name="parser">
        /// 表式文本解析器方法
        /// <para>当某类型的解析器被传入调用一次，该解析器会被缓存，除非指定它是临时的；</para>
        /// <para>解析器是与解析的目标类型绑定的，下次再调用，不需要再传此参数；</para>
        /// <para>如果再次传入，会视情况（是否相等）覆盖上次调用时所用的解析器</para>
        /// <para>当您未指定解析器方法时，将尝试取得目标类型的固有解析器，如果没找到，将会报错</para>
        /// <para>手传解析器，比目标类型原生解析器具有更高的优先级</para>
        /// <para>本扩展方法类 <see cref="StringEx"/>，在加载之初，已经初始化了如下常用类型的字符串解析器：</para>
        /// <para>char/bool/byte/sbyte/short/int/long/ushort/uint/ulong/float/double/decimal/DateTime/TimeSpan</para>
        /// </param>
        /// <param name="temp">解析器是否是临时解析器，如果是，则不会被保存</param>
        /// <returns>
        /// 解析成功，则返回解析到的对象；失败，则返回 null
        /// </returns>
        /// <remarks>
        /// <para>如果提供了解析器，本方法通过给定的解析器，解析给定的表式文本到目标类型</para>
        /// <para>否则，本方法尝试通过目标类型的 bool TryParse(string s, out TRefer value) 静态方法来实现功能</para>
        /// <para>如果指定的目标类型，不包含此种签名的静态方法，则会报错</para>
        /// </remarks>
        public static TRefer[] RParseTable<TRefer>(
            this string tableText, bool noWhiteLines = true,
            TryParse<TRefer> parser = null, bool temp = false)
            where TRefer : class
        {
            Type t = typeof(TRefer);

            object proc;
            if (parser == null)
            {
                if (StringExtensions.Parsers.ContainsKey(t))
                    proc = StringExtensions.Parsers[t];
                else
                    proc = StringExtensions.GetParser<TRefer>();
            }
            else
            {
                proc = parser;
            }

            if (!temp &&
                (!StringExtensions.Parsers.ContainsKey(t) ||
                StringExtensions.Parsers[t] != proc))
                StringExtensions.Parsers.Add(t, proc);

            TryParse<TRefer> tryParse = proc as TryParse<TRefer>;

            return tableText.GetLines(noWhiteLines)
                .Select(line =>
                {
                    tryParse(line, out TRefer value);
                    return value;
                }).ToArray();
        }

        #endregion

        #region Z.Unclassified ：未分类

        /// <summary>
        /// 获取指定字符器的字节宽度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetWidth(this string str)
        {
            Encoding coding = Encoding.Default;
            return coding.GetByteCount(str);
        }

        /// <summary>
        /// 使用指定格式化器格式化值表
        /// </summary>
        /// <param name="format">格式化器</param>
        /// <param name="values">值表</param>
        /// <returns>
        /// 返回格式化后的字符串
        /// </returns>
        public static string FormatWith(this string format, params object[] values)
        {
            return string.Format(format, values);
        }

        /// <summary>
        /// 从指定字符器获取行
        /// </summary>
        /// <param name="text">指定的字符器</param>
        /// <param name="noWhiteLines">是否抛弃空白行</param>
        /// <returns></returns>
        public static string[] GetLines(this string text, bool noWhiteLines = false)
        {
            if (noWhiteLines)
                return text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                    .Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            else
                return text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        #endregion

    }
}
