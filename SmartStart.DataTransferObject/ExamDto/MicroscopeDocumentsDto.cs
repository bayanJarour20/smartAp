using Microsoft.AspNetCore.Http;
using SmartStart.DataTransferObject.QuestionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.ExamDto
{
    public class SectionsMicroscopeDocumentsDto
    {
        /// <summary>
        /// Exam id
        /// </summary>
        public Guid Id { get; set; }
        public IEnumerable<ExamQuestionDocumnetsDto> Sections { get; set; } = new List<ExamQuestionDocumnetsDto>();
    }

    public class MicroscopeDocumentsDto : ExamDto
    {
        public IEnumerable<ExamDocumentDto> Documents { get; set; }
        public IEnumerable<ExamQuestionDocumnetsDto> Sections { get; set; } = new List<ExamQuestionDocumnetsDto>();
    }

    public class ExamQuestionDocumnetsDto : QuestionTagsDto
    {
        public IEnumerable<QuestionDocumentNote> Documents { get; set; } = new List<QuestionDocumentNote>();
    }

    public class QuestionDocumentNote
    {
        /// <summary>
        /// QuestionDocumnet id
        /// </summary>
        public Guid SectionImageId { get; set; }
        public string Note { get; set; }
        public string Path { get; set; }
        public IFormFile File { get; set; }
    }
}
