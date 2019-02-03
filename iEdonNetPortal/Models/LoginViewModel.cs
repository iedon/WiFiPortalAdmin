using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iEdonNetPortal.Models
{
    public class LoginViewModel
    {
        // 用户名
        [DisplayName("用户名")]
        [Required]
        public string Username { get; set; }
        // 密码
        [DisplayName("密码")]
        [Required]
        public string Password { get; set; }
    }
}
