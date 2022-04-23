using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Models
{
    public class QuestionModel
    {
        public Guid QuestionID { get; set; }
        public Guid QuestionnaireID { get; set; }
        public string QuestionCategory { get; set; }
        public string QuestionTyping { get; set; }
        public string QuestionName { get; set; }
        public bool QuestionRequired { get; set; }
        public string QuestionAnswer { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid? CommonQuestionID { get; set; }
        public bool IsCreated { get; set; }
        public bool IsUpdated { get; set; }
        public bool IsDeleted { get; set; }
    }
}