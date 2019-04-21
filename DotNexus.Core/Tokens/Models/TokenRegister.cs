namespace DotNexus.Core.Tokens.Models
{
    public class TokenRegister : Token
    {
        public override string Type => "token";

        public double Supply { get; set; }
    }
}