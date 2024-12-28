using System;

namespace AP.Data;

public interface ICommonEntity
{
    long Id { get; set; }

    Guid RowGuid { get; set; }

    Guid CreatedBy { get; set; }
    DateTimeOffset DateCreated { get; set; }
    
    Guid? ModifiedBy { get; set; }
    DateTimeOffset? DateModified { get; set; }

    bool CanBePurged { get; set; }
}