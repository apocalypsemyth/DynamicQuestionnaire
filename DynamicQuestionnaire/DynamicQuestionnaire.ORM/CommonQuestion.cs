namespace DynamicQuestionnaire.DynamicQuestionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CommonQuestion
    {
        public Guid CommonQuestionID { get; set; }

        [Required]
        [StringLength(50)]
        public string CommonQuestionName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
