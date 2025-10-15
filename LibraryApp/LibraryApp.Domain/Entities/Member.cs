using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<BorrowedItem>? BorrowedItems { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipEndDate => MembershipStartDate.AddDays(10);
    }
}
