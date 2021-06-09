using Application.Common.Interfaces;
using System;

namespace Infrastructure.Services
{
    public class MachineDateService : ICurrentDateService
    {
        public DateTime CurrentDate => DateTime.Now;
    }
}
