using DotNexus.Core.Enums;

namespace DotNexus.Core.Tokens.Models
{
    public class TokenRegister : Token
    {
        public override string Type => "token";
        public override int TypeId => (int)TokenType.Register;

        public double Supply { get; set; }
    }
}