using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Instructor")]
        public int InstID { get; set; }
        public Instructor Instructor { get; set; }
    }
}
