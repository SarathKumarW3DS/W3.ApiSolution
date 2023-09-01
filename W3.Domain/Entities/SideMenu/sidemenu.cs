using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W3.Domain.Entities.SideMenu
{
    public class sidemenu
    {
        [Key]
        public int MenuId { get; set; }
        public string Role { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int SortNummber { get; set; }
        public int isActive { get; set; }
        public ICollection<SubMenu>SubMenu{get;set;}
    }
}
