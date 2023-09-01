using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W3.Domain.Entities.SideMenu
{
    public class SubMenu
    {
        [Key]
        public int Id { get; set; }
        public string Role { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int SortNummber { get; set; }
        public int isActive { get; set; }
        [ForeignKey("sidemenu")]
        public int MenuId { get; set; }
        public sidemenu sidemenu { get; set; }
    }
}
