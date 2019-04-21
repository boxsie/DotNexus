﻿using System.Threading.Tasks;

namespace Boxsie.Wrapplication.Storage.Stores
{
    public class EncryptedTextStore : TextStore
    {
        private readonly string _key;

        public EncryptedTextStore(string key)
        {
            _key = key;
        }

        public override async Task WriteAsync(string filePath, string content)
        {
            var encryptedText = await BxUtils.EncryptTextAsync(content, _key);

            await base.WriteAsync(filePath, encryptedText);
        }

        public override async Task<string> ReadAsync(string filePath)
        {
            var encryptedText = await base.ReadAsync(filePath);

            return await BxUtils.DecryptTextAsync(encryptedText, _key);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}