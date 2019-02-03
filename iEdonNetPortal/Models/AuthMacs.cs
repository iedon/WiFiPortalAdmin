using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iEdonNetPortal.Models
{
    public partial class AuthMacs
    {
        [DisplayName("用户")]
        public uint Uid { get; set; }
        [DisplayName("用户名")]
        [Required]
        public Accounts U { get; set; }
        [DisplayName("记录编号")]
        [Required]
        public ulong Id { get; set; }
        [DisplayName("流量")]
        [Required]
        public ulong Traffic { get; set; }
        [DisplayName("MAC")]
        [Required]
        public string Mac { get; set; }
        [DisplayName("活跃时间")]
        [Required]
        public DateTime Date { get; set; }
    }
}
