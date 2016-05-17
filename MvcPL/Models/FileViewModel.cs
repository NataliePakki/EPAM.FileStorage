using System;
using System.ComponentModel.DataAnnotations;

namespace MvcPL.Models {
        public class FileViewModel {
        public int Id { get; set; }
        [Display(Name = "File name")]
        public string Name { get; set; }
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Display(Name = "Date created")]
        public DateTime CreationDate { get; set; }
        public bool IsPublic { get; set; }
        public int UserId { get; set; }

        }
}