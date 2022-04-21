using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Models
{
    public class UserModel
    {
        public Guid UserID { get; set; }
        public Guid QuestionnaireID { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime AnswerDate { get; set; }
    }
}