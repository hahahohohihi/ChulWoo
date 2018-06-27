using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChulWoo.Models
{
    public class Company
    {
        // 키값(회사) 키값(회사)
        public int ID { get; set; }

        // 회사이름 회사이름
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Name { get; set; }

        // 회사주소 회사주소
        public string Address { get; set; }

        // 회사전화번호 회사전화번호
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Tel { get; set; }

        // 회사텍스코드 회사텍스코드
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Texcode { get; set; }
    }
}