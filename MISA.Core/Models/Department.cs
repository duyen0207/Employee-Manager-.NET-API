namespace MISA.Core.Models
{
    /// <summary>
    /// Phòng ban
    /// </summary>
    /// CreatedBy: LTBDUYEN (21/06/2022)
    public class Department:BaseEntity
    {
        #region Constructor
        
        public Department()
        {
            this.DepartmentId = Guid.NewGuid();
        }



        #endregion

        #region Properties

        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }

        
        #endregion

    }
}
