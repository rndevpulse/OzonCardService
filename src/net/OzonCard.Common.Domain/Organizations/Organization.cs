using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Organizations;

public class Organization : AggregateRoot
{
    private readonly ICollection<Member> _members = new List<Member>();
    private readonly ICollection<Program> _programs = new List<Program>();
    private readonly ICollection<Category> _categories = new List<Category>();
    public string Name { get; set; }
    public string Login { get; private set; }
    public string Password { get; private set; }
    public IEnumerable<Member> Members => _members;
    public IEnumerable<Program> Programs => _programs;
    public IEnumerable<Category> Categories => _categories;

    

    public Organization(Guid id, string name, string login, string password) : base(id)
    {
        Name = name;
        Login = login;
        Password = password;
    }

    public Member AddOrUpdateMember(Guid id, string name)
    {
        var member = _members.FirstOrDefault(x => x.UserId == id);
        if (member == null)
        {
            member = new Member(id);
            _members.Add(member);
        }
        member.Update(name);
        return member;
    }

    public void RemoveMember(string name)
    {
        var member = _members.FirstOrDefault(x => x.Name == Name);
        if (member != null)
            _members.Remove(member);
    }

    public void UpdateCategories(IEnumerable<Category> categories)
    {
        var updated = new List<Guid>();
        foreach (var category in categories)
        {
            var item = _categories.FirstOrDefault(x => x.Id == category.Id);
            if (item == null)
                _categories.Add(category);
            else
            {
                item.Name = category.Name;
                item.IsActive = category.IsActive;
            }
            updated.Add(category.Id);
        }
        foreach (var category in _categories.Where(x => updated.Contains(x.Id)))
            category.IsActive = false;
    }
    
    
    public void UpdatePrograms(IEnumerable<Program> programs)
    {
        var updated = new List<Guid>();
        foreach (var program in programs)
        {
            var item = _programs.FirstOrDefault(x => x.Id == program.Id);
            if (item == null)
                _programs.Add(program);
            else
            {
                item.Name = program.Name;
                item.IsActive = program.IsActive;
                foreach (var wallet in program.Wallets)
                    item.AddOrUpdateWallet(wallet);
            }
            updated.Add(program.Id);
        }
        foreach (var program in _programs.Where(x => updated.Contains(x.Id)))
            program.IsActive = false;
    }
}