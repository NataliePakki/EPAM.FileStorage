using System;
using System.ComponentModel.DataAnnotations;

namespace MvcPL.Models {
        public class FileViewModel {
        public int Id { get; set; }
        [Display(Name = "File name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Size")]
        public String Size { get; set; }
        [Display(Name = "User name")]
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        [Display(Name = "Date created")]
        public DateTime CreationDate { get; set; }
        public bool IsShared { get; set; }
        public int UserId { get; set; }
    }
}