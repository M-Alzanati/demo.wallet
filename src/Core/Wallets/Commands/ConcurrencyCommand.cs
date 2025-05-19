using System;

namespace Application.Wallets.Commands
{
    public abstract class ConcurrencyCommand
    {
        public string RowVersion { get; set; }

        public byte[] GetRowVersionBytes() => Convert.FromBase64String(RowVersion);
    }
}
