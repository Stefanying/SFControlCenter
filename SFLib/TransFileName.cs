using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SFLib
{
    public class TransFileName
    {
        static void Sample()
        {
            string hex = Char2HexFileName("abc.flv");
            string tmp = Hex2CharFileName(hex);

            hex = Char2HexFileName("你好.flv");
            tmp = Hex2CharFileName(hex);
        }
        public unsafe static string Char2HexFileName(string fileName)
        {
            string ret = fileName;
            byte[] char2hex = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66 };
            string fileExt = Path.GetExtension(fileName);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] tmp = encoding.GetBytes(fileNameWithoutExt);
            fixed (byte* p_src = &tmp[0])
            {
                byte[] des = new byte[2 * tmp.Length];
                // char2hex
                for (int i = 0; i < tmp.Length; ++i)
                {
                    byte ch = p_src[i];
                    des[2 * i] = char2hex[(ch & 0xf0) >> 4];
                    des[2 * i + 1] = char2hex[ch & 0x0f];
                }
                ret = encoding.GetString(des);
                ret += fileExt;
            }
            return ret;
        }

        public unsafe static string Hex2CharFileName(string hex)
        {
            string ret = hex;

            byte[] char2hex = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66 };
            Dictionary<byte, byte> hex2char = new Dictionary<byte, byte>();
            for (byte i = 0; i < 16; ++i)
                hex2char[char2hex[i]] = i;

            string fileExt = Path.GetExtension(hex);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(hex);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] tmp = encoding.GetBytes(fileNameWithoutExt);
            fixed (byte* p_src = &tmp[0])
            {
                byte[] des = new byte[tmp.Length / 2];
                // hex2char
                for (int i = 0; i < des.Length; ++i)
                {
                    byte up = (byte)(hex2char[p_src[2 * i]] << 4);
                    byte low = (hex2char[p_src[2 * i + 1]]);
                    des[i] = (byte)(up | low);
                }
                ret = encoding.GetString(des);
                ret += fileExt;
            }
            return ret;
        }
    }
}
