using System;
using System.Collections.Generic;
using System.Linq;

namespace Miracle.Api.Services.Helpers
{
    /// <summary>
    /// AutoMapper ın görevini üstlenir.
    /// </summary>
    public class DataHelper
    {
        /// <summary>
        /// FieldBinder metodundan gelen hataları tutar.
        /// </summary>
        public List<Exception> Errors { get; set; }
        /// <summary>
        /// FieldBinder metodunda hata olup olmadığını belirtir.
        /// </summary>
        public bool HasErrors { get; set; }
        public DataHelper()
        {
            HasErrors = false;
            Errors = new List<Exception>();
        }
        public bool FieldBinder(object source, object destination, List<string> ignoredFields = null)
        {
            if (source == null || destination == null)
            {
                throw new NullReferenceException();
            }

            if (ignoredFields == null)
                ignoredFields = new List<string>();

            var sfields = source.GetType().GetProperties();
            var dfields = destination.GetType().GetProperties();

            int sFieldsCount = sfields.Length;
            for (int i = 0; i < sFieldsCount; i++)
            {
                try
                {
                    var sfieldName = sfields[i].Name;
                    if (ignoredFields.Exists(s => s == sfieldName))
                        continue;

                    var sval = sfields.FirstOrDefault(s => s.Name == sfieldName);
                    if (sval == null)
                        continue;

                    var dval = dfields.FirstOrDefault(s => s.Name == sfieldName);
                    if (dval == null)
                        continue;

                    var val = sval.GetValue(source);

                    dval.SetValue(destination, val);
                }
                catch (Exception ex)
                {
                    HasErrors = true;
                    Errors.Add(ex);
                }
            }

            return !HasErrors;
        }
    }
}
