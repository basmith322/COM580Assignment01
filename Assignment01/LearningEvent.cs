namespace Assignment01
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LearningEvent")]
    public partial class LearningEvent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LearningEvent()
        {
            Attendances = new HashSet<Attendance>();
        }

        public int Id { get; set; }

        [Required]
        public string eventType { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime eventDateTime { get; set; }

        public int eventDuration { get; set; }

        public int moduleID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }

        public virtual Module Module { get; set; }
    }
}
