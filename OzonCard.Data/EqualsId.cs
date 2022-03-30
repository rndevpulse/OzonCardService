
namespace OzonCard.Data
{
    public class EqualsId<T>
    {
        public virtual Guid Id { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj?.GetType() != GetType()) return false;
            return Id == ((EqualsId<T>)obj).Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
