using NPOI.SS.Formula;
using NPOI.SS.UserModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace ADMReestructuracion.Common.Extensions
{
    public static class SystemExtensions
    {
        /// <summary>
        /// Devuelve el valor de un Enum
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TResult GetValue<TResult>(this IConvertible source)
            where TResult : IConvertible
        {
            return (TResult)source;
        }

        /// <summary>
        /// Devuelve el valor del enum
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static short GetValue(this IConvertible source)
        {
            return (short)source;
        }


        /// <summary>
        /// Cantidad de enums
        /// </summary>
        /// <param name="soure"></param>
        /// <returns></returns>
        public static int Count(this Enum soure)
        {
            return Enum.GetNames(soure.GetType()).Length;
        }


        /// <summary>
        /// Obtiene en nombre del valor
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetName(this Enum value, bool? lowercase = null)
        {
            var result = Convert.ToString(value);

            if (lowercase.HasValue)
            {
                if (lowercase.Value)
                {
                    result = result?.ToLower();
                }
                else
                {
                    result = result?.ToUpper();
                }
            }

            return result;
        }


        /// <summary>
        /// devuelve la fecha inicio
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// devuelve la fecha fin
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        /// <summary>
        /// transforma la entero a fecha
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string date)
        {
            if (!string.IsNullOrWhiteSpace(date))
            {
                if (DateTime.TryParse(date, CultureInfo.GetCultureInfo("es"), DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }

            return default;
        }

        /// <summary>
        /// Genra una cadena aleatoria con una cantidad específica
        /// </summary>
        /// <param name="me"></param>
        /// /// <param name="length"></param>
        /// <returns></returns>
        public static string GeneratePassword(this object me, int length = 8)
        {
            me?.ToString();

            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%*";
            var random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(chars);
        }

        public static string DecodeToken(this string token)
        {
            try
            {
                var hash = token.FromBase64String();
                var code = hash.Substring(32);
                code = new string(code.Reverse().ToArray());

                return code;
            }
            catch
            {
                return default;
            }
        }


        public static string EncodeToken(this object data)
        {
            try
            {
                var code = new string($"{data}".Reverse().ToArray());
                return ($"{Guid.NewGuid()}".Replace("-", "") + code).ToBase64String();
            }
            catch
            {
                return default;
            }
        }


        public static string ToBase64String(this object data)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes($"{data}");
                return Convert.ToBase64String(plainTextBytes);
            }
            catch
            {
                return default;
            }
        }


        public static string FromBase64String(this string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return default;
            }

        }

        public static Attachment ToAttachment(this byte[] file, string name)
        {
            if (file != null && file.Length > 0)
            {
                return new MemoryStream(file).ToAttachment(name);
            }

            return default;
        }

        public static Attachment ToAttachment(this Stream file, string name)
        {
            return new Attachment(file, name);
        }

        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static object GetValue(this ICell cell)
        {
            object result = null;

            try
            {
                if (cell != null)
                {
                    if (cell.CellType == CellType.Formula)
                    {
                        var wb = cell.Sheet.Workbook;

                        if (wb != null)
                        {
                            BaseFormulaEvaluator.EvaluateAllFormulaCells(wb);
                        }
                    }

                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            result = DateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue : cell.NumericCellValue;
                            break;
                        case CellType.String:
                            result = $"{cell.StringCellValue}";
                            break;
                        case CellType.Blank:
                            result = string.Empty;
                            break;
                        case CellType.Boolean:
                            result = cell.BooleanCellValue;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
