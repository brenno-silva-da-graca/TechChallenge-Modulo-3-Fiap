using System.Text.RegularExpressions;

namespace Domain.Repositories
{
    public static class ContatoRepositoryDomain
    {
        public static bool EmailIsOk(string email)
        {
            return Regex.IsMatch(email, "([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})");
        }

        public static bool TelefoneIsOk(string telefone)
        {
            return Regex.IsMatch(telefone, "(\\d{2}\\d{8,9})|(\\(\\d{2}\\)\\d{8,9})|(\\(\\d{2}\\)\\d{4,5}\\-\\d{4})|(\\d{2}\\d{4,5}\\-\\d{4})|(\\d{2}\\-\\d{8,9})");
        }

        public static int GetDDDFromStringTelefone(string telefone)
        {
            var DDD = 0;
            if (telefone.Substring(0, 1) == "(")
            {
                DDD = Int32.Parse(telefone.Substring(1, 2));
            }
            else
            {
                DDD = Int32.Parse(telefone.Substring(0, 2));
            }
            return DDD;
        }
    }
}
