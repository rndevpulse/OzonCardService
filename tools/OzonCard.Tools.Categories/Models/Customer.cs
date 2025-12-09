namespace OzonCard.Tools.Categories.Models;

public class Customer : IAddon
{
    public string Name { get; private set; } = "";
    public string Card { get; private set; } = "";
    public IEnumerable<string> Categoties { get; private set; }
    
    public IEnumerable<IAddon> Load(string line)
    {
        var str = line.Split(';');
        var result = new List<IAddon>();
        try
        {
            var categories = str[2]
                .Replace("\"","")
                .Trim()
                .Split(',')
                .Select(x=>x.Trim())
                .ToList();
            foreach (var card in str[1].Split(','))
            {
                if (ReadCardCellValue(card) is not {} value)
                    continue;
                result.Add(new Customer()
                {
                    Card = value,
                    Name = str.First(),
                    Categoties = categories
                });
            }
            return result;
        }
        catch (Exception e)
        {
            throw new Exception($"exeption in: {line}\n{e.Message}");
        }
       
    }
    
    string? ReadCardCellValue(string value)
    {
        return value.Length switch
        {
            0 => null,
            8 => value,
            > 8 => null,
            _ => "00000000".Remove(0, value.Length) + value
        };
    }
}