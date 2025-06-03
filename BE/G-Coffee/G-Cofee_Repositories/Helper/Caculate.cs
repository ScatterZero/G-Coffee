using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Helper
{
    public class Caculate
    {
        private readonly Random _random;

        public Caculate()
        {
            _random = new Random();
        }

        public string GenerateEan13Barcode()
        {
            char[] digits = new char[13];

            // Tạo 12 chữ số ngẫu nhiên
            for (int i = 0; i < 12; i++)
            {
                digits[i] = (char)('0' + _random.Next(0, 10));
            }

            // Tính checksum
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = digits[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }
            int checksum = (10 - (sum % 10)) % 10;
            digits[12] = (char)('0' + checksum);

            return new string(digits);
        }
    }
}
