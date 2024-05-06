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

        member.Name = name;
        return member;
    }

    public void RemoveMember(Guid id)
    {
        var member = _members.FirstOrDefault(x => x.UserId == id);
        if (member != null)
            _members.Remove(member);
    }

    public Category UpdateCategory(Guid id, string name, bool isActive)
    {
        var item = _categories.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            item = new Category(id)
            {
                Name = name,
                IsActive = isActive
            };
            _categories.Add(item);
        }
        else
        {
            item.Name = name;
            item.IsActive = isActive;
        }
        return item;
    }
    
    
    public void UpdatePrograms(Guid id, string name, bool isActive, Guid walletId, string walletType)
    {
        var program = _programs.FirstOrDefault(x => x.Id == id);
        if (program == null)
        {
            program = new Program(id)
            {
                Name = name,
                IsActive = isActive,
            };
            _programs.Add(program);
        }
        else
        {
            program.Name = name;
            program.IsActive = isActive;
        }
        program.AddOrUpdateWallet(new Wallet(walletId, name, "", walletType));
    }
}