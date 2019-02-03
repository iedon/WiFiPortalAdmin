using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iEdonNetPortal.Models
{
    public partial class Logs
    {
        [DisplayName("用户")]
        public uint Uid { get; set; }
        [DisplayName("用户名")]
        [Required]
        public Accounts U { get; set; }
        [DisplayName("日志编号")]
        [Required]
        public ulong Id { get; set; }
        [DisplayName("动作类型")]
        [Required]
        public byte Action { get; set; }
        public enum EnumLogAction
        {
            [Display(Name = "未定义")]
            undefined,
            [Display(Name = "登录")]
            login,
            [Display(Name = "注销")]
            logout,
            [Display(Name = "计费")]
            billing,
            [Display(Name = "注册")]
            register
        }
        [DisplayName("日志内容")]
        public string Log { get; set; }
        [DisplayName("操作日期")]
        [Required]
        public DateTime Date { get; set; }
    }
}
