using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.api.dtos.Response
{
    public class StudentResDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public float DiemCC { get; set; }

        public float DiemGiuaKy { get; set; }

        public float DiemCuoiKy { get; set; }

        public float DiemTongKet { get; set; }

        public string XepLoai { get; set; }
    }

}