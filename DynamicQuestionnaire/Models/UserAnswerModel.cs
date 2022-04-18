using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Models
{
    public class UserAnswerModel
    {
        public Guid UserID { get; set; }
        public Guid QuestionID { get; set; }
        public string QuestionTyping { get; set; }
        public int AnswerNum { get; set; }
        public string Answer { get; set; }
    }
}