using DotNexus.Core.Enums;

namespace DotNexus.Core.Tokens.Models
{
    public class TokenAccount : Token
    {
        public override string Type => "account";
        public override int TypeId => (int)TokenType.Account;
    }
}