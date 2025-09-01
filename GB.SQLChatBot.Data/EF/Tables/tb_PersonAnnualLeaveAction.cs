using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GB.SQLChatBot.Data.EF.Tables
{

    [Table("tb_PersonAnnualLeaveAction")]
    public partial class tb_PersonAnnualLeaveAction
    {
        [Key]
        public int PersonAnnualLeaveActionRef { get; set; }

        public int PersonRef { get; set; }

        public int ActionTypeRef { get; set; }

        public int? LeaveRequestRef { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Duration { get; set; }

        public int CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}