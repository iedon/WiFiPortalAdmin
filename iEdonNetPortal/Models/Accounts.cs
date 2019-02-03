using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iEdonNetPortal.Models
{
    public partial class Accounts
    {
        public Accounts()
        {
            AuthMacs = new HashSet<AuthMacs>();
            Logs = new HashSet<Logs>();
        }
        [DisplayName("用户编号")]
        [Required]
        public uint Uid { get; set; }
        [DisplayName("用户名")]
        [Required]
        public string User { get; set; }
        [DisplayName("密码")]
        [Required]
        public string Password { get; set; }
        [DisplayName("姓名")]
        [Required]
        public string Name { get; set; }
        [DisplayName("在线")]
        [Required]
        public ushort Online { get; set; }
        public enum EnumAccountOnline
        {
            [Display(Name = "离线")]
            loggedout,
            [Display(Name = "在线")]
            loggedin
        }
        [DisplayName("点数")]
        [Required]
        public uint Credit { get; set; }
        [DisplayName("邮箱")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [DisplayName("活跃时间")]
        [DataType(DataType.DateTime)]
        public DateTime? Lastlogin { get; set; }
        [DisplayName("最后IP")]
        public string Lastip { get; set; }

        public ICollection<AuthMacs> AuthMacs { get; set; }
        public ICollection<Logs> Logs { get; set; }
    }
}
