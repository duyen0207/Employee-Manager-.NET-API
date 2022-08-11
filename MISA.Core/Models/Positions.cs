namespace MISA.Core.Models
{
    /// <summary>
    /// Chức vụ
    /// </summary>
    /// Created by: LTBDUYEN (21/06/2022)
    public class Positions:BaseEntity
    {
        #region Constructor
        public Positions()
        {
            this.PositionId = Guid.NewGuid();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid PositionId { get; set; }

        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public string PositionName { get; set; }

        
        #endregion
    }
}
