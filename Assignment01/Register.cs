namespace Assignment01
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Register")]
    public partial class Register
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime regDate { get; set; }

        public int studentID { get; set; }

        public int moduleID { get; set; }

        public virtual Module Module { get; set; }

        public virtual Student Student { get; set; }
    }
}
