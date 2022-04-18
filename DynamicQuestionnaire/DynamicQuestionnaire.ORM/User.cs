namespace DynamicQuestionnaire.DynamicQuestionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public Guid UserID { get; set; }

        public Guid QuestionnaireID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(24)]
        public string Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public int Age { get; set; }

        public DateTime AnswerDate { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }
    }
}
