using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CustomerApi.Core.SharedKernel;

public  abstract class BaseEvent : INotification
{
    public string MessageType { get; protected init; }

    public Guid AggregateId { get; protected init; }

    public DateTime OccurreedOn { get; private init; } = DateTime.Now;
}
