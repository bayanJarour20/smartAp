using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Main
{
    public class QuestionTag : BaseEntity<Guid>
    {
        [Required]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        [Required]
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
