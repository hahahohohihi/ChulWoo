using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChulWoo.Models
{ 
    public class Employee
    {
        // 키값(직원테이블) 키값(직원테이블)
        public int ID { get; set; }

        // 키값(계약정보) 키값(계약정보)
        public virtual ICollection<Contract> Contracts { get; set; }

        // 키값(인사) 키값(인사)
        public virtual ICollection<Personnel> Personnels { get; set; }

        // 키값(퇴직정보) 키값(퇴직정보)
        public int? ResignID { get; set; }
        public virtual Resign Resign { get; set; }

        // 부서 부서(베트남)
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string DepartmentVn { get; set; }

        // 부서 부서(한글)
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string DepartmentKr { get; set; }

        // 직원이름 직원이름
        [Required]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Name { get; set; }

        // 직원번호 직원번호
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string EmployeeNo { get; set; }

        // 은행계좌번호 은행계좌번호
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string BankAccount { get; set; }

        // 지점 지점
        public string BankLocation { get; set; }

        // 세금번호 세금번호
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string TexNo { get; set; }

        // 직책 직책
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string JobPosition { get; set; }

        // 성별 성별
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Sex { get; set; }

        // 생년월일 생년월일
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        // 주민등록번호 주민등록번호
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string RegistrationNo { get; set; }

        // 발급일 발급일
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RegistrationDate { get; set; }

        // 발급처 발급처
        public string RegistrationPosition { get; set; }

        // 전화번호1 전화번호1
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Tel1 { get; set; }

        // 전화번호2 전화번호2
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Tel2 { get; set; }

        // 이메일 이메일
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Email { get; set; }

        // 주소 직원주소
        public string Adress { get; set; }

        // 민족 민족
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string People { get; set; }

        // 종교 종교
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Religion { get; set; }

        // 국적 국적
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Country { get; set; }

        // 학력 학력
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string EducationLevel { get; set; }

        // 전공 전공(베트남)
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string MajorVn { get; set; }

        // 전공 전공(한글)
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string MajorKr { get; set; }

        // 혼인현황 혼인현황
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Marriage { get; set; }

        // 18살이하 부양가족수 18살이하 부양가족수
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string DependentChild { get; set; }

        // 노동연령을넘은 부양가족수 노동연령을넘은 부양가족수
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string DependentParents { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}

