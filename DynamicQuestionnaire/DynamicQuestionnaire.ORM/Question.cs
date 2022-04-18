namespace DynamicQuestionnaire.DynamicQuestionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Question
    {
        public Guid QuestionID { get; set; }

        public Guid QuestionnaireID { get; set; }

        [Required]
        [StringLength(50)]
        public string QuestionCategory { get; set; }

        [Required]
        [StringLength(50)]
        public string QuestionTyping { get; set; }

        [Required]
        [StringLength(50)]
        public string QuestionName { get; set; }

        public bool QuestionRequired { get; set; }

        [Required]
        [StringLength(500)]
        public string QuestionAnswer { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }
    }
}
