using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tests.Model
{
    public abstract class TestModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Indicates if the lifecycle callback method is called
        /// </summary>
        public bool CallbackCalled
        {
            protected set;
            get;
        }

        /// <summary>
        /// The datetime when the callback is called
        /// </summary>
        public DateTime CallbackCalledAt { get; set; }

        /// <summary>
        /// The datetime when the model is saved
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime SavedAt { get; set; }
    }
}
