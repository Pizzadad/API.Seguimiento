using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ByteSizeLib;

namespace Common
{
    /// <summary>
    /// Atributo para las DTO que indican un tamaño máximo para los datos en byte array.
    /// </summary>
    public class LengthAttribute : ValidationAttribute
    {
        readonly int length;
        /// <summary>
        /// Atributo para las DTO que indican un tamaño máximo para los datos en byte array.
        /// </summary>
        public LengthAttribute(int kbPermitidos)
        {
            this.length = kbPermitidos;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            return ((byte[])value).Length <= ByteSize.FromKiloBytes(length).Bytes;
        }
    }
}
