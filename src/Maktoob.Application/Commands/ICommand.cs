using Maktoob.CrossCuttingConcerns.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Application.Commands
{
    public interface ICommand<TResult> where TResult : GResult
    {
    }
}
