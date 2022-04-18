namespace DynamicQuestionnaire.DynamicQuestionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Typing
    {
        public Guid TypingID { get; set; }

        [Required]
        [StringLength(50)]
        public string TypingName { get; set; }
    }
}
