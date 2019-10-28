namespace Assignment01
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            Attendances = new HashSet<Attendance>();
            Registers = new HashSet<Register>();
        }

        public int Id { get; set; }

        [Required]
        public string studentForname { get; set; }

        [Required]
        public string studentSurname { get; set; }

        [Required]
        public string studentNumber { get; set; }

        [Required]
        public string studentEmail { get; set; }

        public string studentTelNum { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Register> Registers { get; set; }
    }
}
