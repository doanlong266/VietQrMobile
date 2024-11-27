using SQLite;

namespace VietQrMobile.Models
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]
        public string AccountNo { get; set; }

        [MaxLength(100)]
        public string AccountName { get; set; }

        [MaxLength(6)]
        public int AcqId { get; set; }
    }
}
