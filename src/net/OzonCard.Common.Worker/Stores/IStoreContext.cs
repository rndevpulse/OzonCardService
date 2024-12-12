using OzonCard.Common.Core;

namespace OzonCard.Common.Worker.Stores;

public interface IStoreContext :  IEntityStoreWithId<Guid>, IDisposable;