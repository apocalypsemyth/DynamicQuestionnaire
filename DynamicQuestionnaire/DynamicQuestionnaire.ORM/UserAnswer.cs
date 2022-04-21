namespace DynamicQuestionnaire.DynamicQuestionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserAnswer
    {
        [Key]
        [Column(Order = 0)]
        public Guid QuestionnaireID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid UserID { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid QuestionID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string QuestionTyping { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AnswerNum { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(500)]
        public string Answer { get; set; }
    }
}
