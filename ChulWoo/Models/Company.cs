using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ChulWoo.LocalResource;

namespace ChulWoo.Models
{
    public class Company
    {
        // 키값(회사) 키값(회사)
        public int ID { get; set; }

        // 진행프로젝트
        public virtual ICollection<Project> Projects { get; set; }

        // 판매한 제품들
        public virtual ICollection<MaterialBuy> MaterialBuys { get; set; }

        // 회사이름 회사이름
        [Display(Name = "CompanyName", ResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Name { get; set; }

        // 회사주소 회사주소
        [Display(Name = "Adress", ResourceType = typeof(Resource))]
        public string Address { get; set; }

        // 회사전화번호 회사전화번호
        [Display(Name = "Tel", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Tel { get; set; }

        // 회사텍스코드 회사텍스코드
        [Display(Name = "Texcode", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Texcode { get; set; }

        // 은행계좌번호 은행계좌번호
        [Display(Name = "BankAccount", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string BankAccount { get; set; }

        // 지점 지점
        [Display(Name = "BankLocation", ResourceType = typeof(Resource))]
        public string BankLocation { get; set; }
    }
}