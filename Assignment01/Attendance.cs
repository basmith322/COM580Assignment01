namespace Assignment01
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attendance")]
    public partial class Attendance
    {
        public int Id { get; set; }

        [Required]
        public string attendanceStatus { get; set; }

        public int eventID { get; set; }

        public int studentID { get; set; }

        public virtual LearningEvent LearningEvent { get; set; }

        public virtual Student Student { get; set; }
    }
}
