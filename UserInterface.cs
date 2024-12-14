using Spectre.Console;
using System.Linq;

namespace GymApp
{
    public class UserInterface
    {
        private Administration<MembershipType> membershipRepository = new Administration<MembershipType>();
        private Administration<Member> memberRepository = new Administration<Member>();
        private List<Member> listOfMembers;

        public UserInterface()
        {
            membershipRepository.Add(new MembershipType { Name = "Basic", Price = 100, DurationInDays = 30 });
            membershipRepository.Add(new MembershipType { Name = "Premium", Price = 200, DurationInDays = 60 });
            membershipRepository.Add(new MembershipType { Name = "Gold", Price = 300, DurationInDays = 90 });

            listOfMembers = memberRepository.GetAll().ToList();
        }

        public void PrintMenu()
        {
            while (true)
            {
                Console.Clear();

                var options = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .HighlightStyle(new Style(foreground: Color.Yellow))
                    .AddChoices(new[] {
                        "Add member", "Show active members", "Update member", "Remove member", "Exit",
                    }));

                switch (options)
                {
                    case "Add member":
                        AddMember();
                        break;
                    case "Show active members":
                        ShowMembers();
                        break;
                    case "Update member":
                        UpdateMember();
                        break;
                    case "Remove member":
                        RemoveMember();
                        break;
                    case "Exit":
                        return;
                }
            }
        }

        private void ShowMembers()
        {
            foreach (var member in listOfMembers)
            {
                AnsiConsole.MarkupLine($"[bold]{member.Name}[/] - {member.Age} years old ({member.MembershipType.Name} membership)");
            }
            Console.ReadKey();
        }
        private void RemoveMember()
        {
            if (!listOfMembers.Any())
            {
                AnsiConsole.MarkupLine("[red]No members to remove[/]");
                Console.ReadKey();
                return;
            }

            var memberName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select member to remove")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .HighlightStyle(new Style(foreground: Color.Yellow))
                    .AddChoices(listOfMembers.Select(member => member.Name).ToArray()));

            var memberToRemove = listOfMembers.FirstOrDefault(member => member.Name == memberName);
            if (memberToRemove != null)
            {
                memberRepository.Remove(member => member.Name == memberToRemove.Name);
                listOfMembers.Remove(memberToRemove);
            }
        }
        private void UpdateMember()
        {
            if (!listOfMembers.Any())
            {
                AnsiConsole.MarkupLine("[red]No members to update[/]");
                Console.ReadKey();
                return;
            }

            var memberName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select member to update")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .HighlightStyle(new Style(foreground: Color.Yellow))
                    .AddChoices(listOfMembers.Select(member => member.Name).ToArray()));

            var memberToUpdate = listOfMembers.FirstOrDefault(member => member.Name == memberName);

            if (memberToUpdate != null)
            {
                var updatedName = AnsiConsole.Ask<string>("Enter new name:");
                var updatedAge = AnsiConsole.Ask<int>("Enter new age:");

                var listOfMembershipTypes = membershipRepository.GetAll().ToList();

                var newMembershipTypeName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select new membership type")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .HighlightStyle(new Style(foreground: Color.Yellow))
                        .AddChoices(listOfMembershipTypes.Select(membership => membership.Name).ToArray()));

                var newMembershipType = listOfMembershipTypes.FirstOrDefault(membership => membership.Name == newMembershipTypeName);

                memberRepository.Update(new Member(updatedName, updatedAge, newMembershipType), member => member.Name == memberToUpdate.Name);
                memberToUpdate.Name = updatedName;
                memberToUpdate.Age = updatedAge;
                memberToUpdate.MembershipType = newMembershipType;
            }
        }
        private void AddMember()
        {
            string name = AnsiConsole.Ask<string>("Enter your name:");
            int age = AnsiConsole.Ask<int>("Enter your age:");

            var listOfMembershipTypes = membershipRepository.GetAll().ToList();

            var membershipTypeName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select membership type")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .HighlightStyle(new Style(foreground: Color.Yellow))
                    .AddChoices(listOfMembershipTypes.Select(membership => membership.Name).ToArray()));

            var selectedMembershipType = listOfMembershipTypes.FirstOrDefault(membership => membership.Name == membershipTypeName);

            var newMember = new Member(name, age, selectedMembershipType);
            memberRepository.Add(newMember);
            listOfMembers.Add(newMember);
        }
    }
}
