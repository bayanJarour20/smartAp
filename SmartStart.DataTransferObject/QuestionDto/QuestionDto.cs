using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.QuestionDto
{
    public class QuestionDto
    {
        /// <summary>
        /// Question id
        /// </summary>
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Hint { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public AnswerTypes AnswerType { get; set; }
        public bool IsCorrected { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
