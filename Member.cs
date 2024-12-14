using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp
{
    public class Member
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public MembershipType MembershipType { get; set; }

        public Member(string name, int age, MembershipType membershipType)
        {
            Name = name;
            Age = age;
            MembershipType = membershipType;
        }
    }
}
