using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Model
{
    public class Position : BaseEntity
    {
        #region Property
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid PositionId { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string PositionName { get; set; }
        #endregion
    }
}
