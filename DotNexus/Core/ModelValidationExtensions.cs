using System;
using DotNexus.Account.Models;
using DotNexus.Assets.Models;
using DotNexus.Core.Enums;

namespace DotNexus.Core
{
    public static class ModelValidationExtensions
    {
        public static void Validate(this NexusUser user, UserValidationMode mode)
        {
            if (user == null)
                throw new ArgumentException("User is required");

            switch (mode)
            {
                case UserValidationMode.Create:
                    if (string.IsNullOrWhiteSpace(user.Username))
                        throw new ArgumentException("Username is required");

                    if (string.IsNullOrWhiteSpace(user.Password))
                        throw new ArgumentException("Password is required");

                    if (user.Pin.ToString().Length < 4)
                        throw new ArgumentException("PIN must be greater than 3 digits");
                    break;
                case UserValidationMode.Authenticate:
                    if (user.GenesisId == null)
                        throw new ArgumentException("A session key is required");

                    if (string.IsNullOrWhiteSpace(user.GenesisId?.Session))
                        throw new ArgumentException("A session key is required to create an asset");

                    if (user.Pin.ToString().Length < 4)
                        throw new ArgumentException("PIN must be greater than 3 digits");
                    break;
                case UserValidationMode.Lookup:
                    if (string.IsNullOrWhiteSpace(user.Username) && string.IsNullOrWhiteSpace(user?.GenesisId.Genesis))
                        throw new ArgumentException("A valid genesis ID or username is required");
                    break;
            }
        }

        public static void Validate(this Asset asset)
        {
            if (asset == null)
                throw new ArgumentException("Asset is required");

            if (string.IsNullOrWhiteSpace(asset.Name) && string.IsNullOrWhiteSpace(asset.Data))
                throw new ArgumentException("Name and/or data is required");
        }

        public static void Validate(this Token token)
        {
            if (token == null)
                throw new ArgumentException("Token is required");

            if (string.IsNullOrWhiteSpace(token.Name) && string.IsNullOrWhiteSpace(token.Address))
                throw new ArgumentException("Name and/or address is required");
        }
    }
}